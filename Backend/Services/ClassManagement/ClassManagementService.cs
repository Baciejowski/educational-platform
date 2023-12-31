﻿using System;
using System.Linq;
using Backend.Controllers.APIs.Models;
using Backend.Models;
using Backend.Services.EmailProvider;
using Backend.Services.EmailProvider.Models;
using Backend.Services.TeacherManagement;

namespace Backend.Services.ClassManagement
{
    public class ClassManagementService : IClassManagementService
    {
        private readonly ITeacherManagementService _teacherManagementService;
        protected readonly DataContext Context;
        private readonly IMailService _mailService;

        public ClassManagementService(IMailService mailService, ITeacherManagementService teacherManagementService, DataContext context)
        {
            _mailService = mailService;
            _teacherManagementService = teacherManagementService;
            Context = context;
        }


        public void SendGameInvitationToStudents(ClassesGameInvitationViewModel classesGameInvitationViewModel, string authName)
        {
            var teacher = _teacherManagementService.GetTeacher(authName);
            var classItem = teacher.Classes
                .FirstOrDefault(item => item.ClassID == classesGameInvitationViewModel.ClassId);
            var studentsList = classItem?.Students;
            var topicItem = teacher.Topics
                .FirstOrDefault(topicItem => topicItem.TopicID == classesGameInvitationViewModel.TopicId);
            var scenarioItem = topicItem?.Scenarios
                .FirstOrDefault(scenarioItem => scenarioItem.ScenarioID == classesGameInvitationViewModel.ScenarioId);

            foreach (var student in studentsList)
            {
                var gameId =
                    $"{classesGameInvitationViewModel.ClassId}-{student.StudentID}-{teacher.TeacherID}-{classesGameInvitationViewModel.TopicId}-{classesGameInvitationViewModel.ScenarioId}-{classesGameInvitationViewModel.StartGame}-{classesGameInvitationViewModel.EndGame}";

                var code = String.Format("{0:X}", gameId.GetHashCode());
                var request = new GameInvitationRequest
                {
                    ToEmail = student.Email,
                    UserName = student.FirstName,
                    Code = code,
                    Topic = topicItem.TopicName,
                    Scenario = scenarioItem.Name,
                    Url = scenarioItem.Url,
                    StartDate = classesGameInvitationViewModel.StartGame.ToString("yyyy-MM-dd HH:mm"),
                    EndDate = classesGameInvitationViewModel.EndGame.ToString("yyyy-MM-dd HH:mm")
                };


                Context.Sessions.Add(new Session
                {
                    Student = student,
                    Scenario = scenarioItem,
                    StartGame = classesGameInvitationViewModel.StartGame,
                    EndGame = classesGameInvitationViewModel.EndGame,
                    RandomTest = classesGameInvitationViewModel.RandomTest,
                    AiCategorization = classesGameInvitationViewModel.AiCategorization,
                    Code = code
                });
                _ = _mailService.SendGameInvitationRequestAsync(request);
            }
            Context.SaveChanges();
        }
    }
}