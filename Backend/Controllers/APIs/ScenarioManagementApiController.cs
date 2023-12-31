﻿using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers.APIs
{
    [ApiController]
    [Route("api/scenarios")]
    public class ScenarioManagementApiController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<ScenarioManagementApiController> _logger;

        public ScenarioManagementApiController(DataContext dataContext, ILogger<ScenarioManagementApiController> logger)
        {
            _dataContext = dataContext;
            _logger = logger;

        }

        [HttpHead]
        [Route("/api/scenarios/{id}")]
        [Authorize]
        public IActionResult Head([FromRoute][Required] int id, bool includeQuestions, bool includeAnswers)
        {
            if (includeAnswers && !includeQuestions) return BadRequest();

            Teacher teacher = _dataContext.ResolveOrCreateUser(HttpContext.User);
            if (teacher == null) return Unauthorized();

            Scenario scenario = _dataContext.Scenarios.FirstOrDefault(s => s.ScenarioID == id);
            if (scenario == null) return NotFound();

            return Ok();
        }

        [HttpGet]
        [Route("/api/scenarios/{id}")]
        [Authorize]
        public IActionResult Get([FromRoute][Required] int id, bool includeQuestions, bool includeAnswers)
        {
            if (includeAnswers && !includeQuestions) return BadRequest();

            Teacher teacher = _dataContext.ResolveOrCreateUser(HttpContext.User);
            if (teacher == null) return Unauthorized();

            Scenario scenario = _dataContext.Scenarios.FirstOrDefault(s => s.ScenarioID == id);
            if (scenario == null) return NotFound();

            if (includeQuestions)
            {
                _dataContext.Entry(scenario).Collection(s => s.Questions).Load();
                foreach (Question question in scenario.Questions)
                {
                    if (includeAnswers)
                    {
                        _dataContext.Entry(question).Collection(q => q.ABCDAnswers).Load();
                    }
                    else
                        question.BooleanAnswer = null;
                }
            }
            JsonSerializerSettings settings = new JsonSerializerSettings();
            IgnoreJsonAttributesResolver resolver = new IgnoreJsonAttributesResolver();
            resolver.PropertiesToSkip = new Dictionary<string, HashSet<string>> {
                {"Question", new HashSet<string>() { "Scenarios", "AIRespresentation"} },
                {"Answer", new HashSet<string>() {"Question"} },
                {"Scenario", new HashSet<string>() {"Topic", "Games", "Session", "AIRespresentation"} },
            };
            settings.ContractResolver = resolver;

            return new ObjectResult(JsonConvert.SerializeObject(scenario, settings));
        }

        [AcceptVerbs("COPY")]
        [Route("/api/scenarios/{id}")]
        [Authorize]
        public IActionResult Copy([FromRoute][Required] int id)
        {
            Teacher teacher = _dataContext.ResolveOrCreateUser(HttpContext.User);
            if (teacher == null) return Unauthorized();

            Scenario scenario = _dataContext.Scenarios
                .Include(s => s.Topic)
                    .ThenInclude(topic => topic.Teacher)
                .Include(s => s.Questions)
                    .ThenInclude(q => q.ABCDAnswers)
                .AsNoTracking()
                .FirstOrDefault(s => s.ScenarioID == id);
            if (scenario == null) return NotFound();
            if (scenario.Topic.Teacher.TeacherID!=(teacher.TeacherID)) return Unauthorized();

            scenario.ScenarioID = 0;
            scenario.Name += " (Copy)";
            foreach (Question q in scenario.Questions)
            {
                q.QuestionID = 0;
                foreach (Answer a in q.ABCDAnswers)
                {
                    a.AnswerID = 0;
                }
            }

            scenario.Topic = _dataContext.Scenarios.Include(s => s.Topic).FirstOrDefault(s => s.ScenarioID == id).Topic;
            _dataContext.Add(scenario);

            try
            {
                _dataContext.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                return StatusCode(400, e.Message + " -> " + e.InnerException.Message);
            }
            return Ok();
        }

        [HttpPost]
        [Route("/api/scenarios/{id}/media")]
        public IActionResult UploadFile(IFormCollection upload, [FromRoute][Required] int id)
        {
            List<string> filesContents = new List<string>();
            foreach (var formFile in upload.Files)
            {
                if (formFile.Length > 0)
                {
                    using (var reader = new StreamReader(formFile.OpenReadStream()))
                    {
                        filesContents.Add(reader.ReadToEnd());
                    }
                }
            }
            CommunicationWithAI.TokenizeAndAddToRequestsQueue(filesContents, id);
            Task.Run(() => CommunicationWithAI.SendRequests());

            return Ok();
        }


        [HttpDelete]
        [Authorize]
        public IActionResult Delete(int id)
        {
            Teacher teacher = _dataContext.ResolveOrCreateUser(HttpContext.User);
            if (teacher == null) return Unauthorized();

            Scenario scenario = _dataContext.Scenarios.FirstOrDefault(s => s.ScenarioID == id);
            if (scenario == null) return NotFound();

            _dataContext.Entry(scenario).Reference(s => s.Topic).Load();
            _dataContext.Entry(scenario.Topic).Reference(t => t.Teacher).Load();
            if (!scenario.Topic.Teacher.Equals(teacher)) return Unauthorized();

            _dataContext.Remove(scenario);
            try
            {
                _dataContext.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                return StatusCode(400, e.Message + " -> " + e.InnerException.Message);
            }
            return Ok();
        }
    }
}
