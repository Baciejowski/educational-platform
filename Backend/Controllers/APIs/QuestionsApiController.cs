using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace Backend.Controllers.Api
{
    [ApiController]
    [Route("api/questions")]
    public class QuestionsApiController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<QuestionsApiController> _logger;

        public QuestionsApiController(DataContext dataContext, ILogger<QuestionsApiController> logger)
        {
            _dataContext = dataContext;
            _logger = logger;

        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] Question question, [Required] int scenarioID)
        {
            Teacher teacher = _dataContext.ResolveOrCreateUser(HttpContext.User);
            if (teacher == null) return Unauthorized();

            Scenario scenario = _dataContext.Scenarios.FirstOrDefault(s => s.ScenarioID == scenarioID);
            if (scenario == null) return NotFound();

            _dataContext.Entry(scenario).Reference(s => s.Topic).Load();
            _dataContext.Entry(scenario.Topic).Reference(topic => topic.Teacher).Load();
            if (!teacher.Equals(scenario.Topic.Teacher)) return Unauthorized();

            _dataContext.Add(question);
            question.Scenarios = new List<Scenario> { scenario };


            try
            {
                _dataContext.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                return StatusCode(400, e.Message + " -> " + e.InnerException.Message);
            }

            return StatusCode(201, question.QuestionID);

        }


        [HttpPut]
        [Authorize]
        public IActionResult Put([FromBody] Question question)
        {
            Teacher teacher = _dataContext.ResolveOrCreateUser(HttpContext.User);
            if (teacher == null) return Unauthorized();

            Question original = _dataContext.Questions.Include(q => q.ABCDAnswers).FirstOrDefault(q => q.QuestionID == question.QuestionID);
            if (original == null)
            {
                return NotFound();
            }
            else
            {
                _dataContext.Entry(original).Collection(o => o.Scenarios).Load();
                bool owner = false;
                foreach (Scenario scenario in original.Scenarios)
                {
                    _dataContext.Entry(scenario).Reference(s => s.Topic).Load();
                    _dataContext.Entry(scenario.Topic).Reference(topic => topic.Teacher).Load();
                    if (scenario.Topic.Teacher.Equals(teacher))
                    {
                        owner = true;
                        break;
                    }
                }
                if (!owner) return Unauthorized();

                IList<int> idsToRemove = new List<int>();
                foreach (Answer originalAns in original.ABCDAnswers)
                {
                    var newAns = question.ABCDAnswers.FirstOrDefault(a => a.AnswerID == originalAns.AnswerID);
                    if (newAns == null)
                        idsToRemove.Add(originalAns.AnswerID);
                    else
                    {
                        if (!string.Equals(newAns.Argumentation, originalAns.Argumentation))
                            originalAns.Argumentation = newAns.Argumentation;
                        if (!string.Equals(newAns.Content, originalAns.Content))
                            originalAns.Argumentation = newAns.Argumentation;
                        if (newAns.Correct != originalAns.Correct)
                            originalAns.Correct = newAns.Correct;
                    }
                }
                foreach (int id in idsToRemove)
                    _dataContext.Remove(original.ABCDAnswers.First(a => a.AnswerID == id));
                foreach (Answer answer in question.ABCDAnswers)
                    if (answer.AnswerID == 0)
                    {
                        _dataContext.Add(answer);
                        original.ABCDAnswers.Add(answer);
                    }

                if (original.Difficulty != question.Difficulty)
                    original.Difficulty = question.Difficulty;
                if (original.AiDifficulty != question.AiDifficulty)
                    original.AiDifficulty = question.AiDifficulty;
                if (!string.Equals(original.Content, question.Content))
                    original.Content = question.Content;
                if (!string.Equals(original.Hint, question.Hint))
                    original.Hint = question.Hint;
                if (!original.QuestionType.Equals(question.QuestionType))
                    original.QuestionType = question.QuestionType;
                if (!bool.Equals(original.BooleanAnswer, question.BooleanAnswer))
                    original.BooleanAnswer = question.BooleanAnswer;
                if (original.IsImportant != question.IsImportant)
                    original.IsImportant = question.IsImportant;
                if (original.IsObligatory != question.IsObligatory)
                    original.IsObligatory = question.IsObligatory;

                _dataContext.Update(original);
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

        [HttpDelete]
        [Authorize]
        public IActionResult Delete(int id)
        {
            Teacher teacher = _dataContext.ResolveOrCreateUser(HttpContext.User);
            if (teacher == null) return Unauthorized();

            Question question = _dataContext.Questions.FirstOrDefault(q => q.QuestionID == id);
            if (question == null) return NotFound();

            _dataContext.Entry(question).Collection(o => o.Scenarios).Load();
            bool owner = false;
            foreach (Scenario scenario in question.Scenarios)
            {
                _dataContext.Entry(scenario).Reference(s => s.Topic).Load();
                _dataContext.Entry(scenario.Topic).Reference(topic => topic.Teacher).Load();
                if (scenario.Topic.Teacher.Equals(teacher))
                {
                    owner = true;
                    break;
                }
            }
            if (!owner) return Unauthorized();

            _dataContext.RemoveRange(question.ABCDAnswers);
            _dataContext.Remove(question);

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
