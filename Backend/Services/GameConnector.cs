#region Copyright notice and license

// Copyright 2019 The gRPC Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Threading.Tasks;
using Gameplay;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Backend.Services
{
    public class GameConnector : GameRequests.GameRequestsBase
    {
        private readonly ILogger _logger;
        private Random random = new Random();

        public GameConnector(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GameConnector>();
        }

        public override Task<QuestionResponse> PrepareNextQuestion(QuestionRequest request, ServerCallContext context)
        {
            // Question.Types.Answer[] answers = {
            //     new Question.Types.Answer { AnswerID = 1, Content = "Bad One 1", Correct = false, Argumentation = "Not the best one"},
            //     new Question.Types.Answer { AnswerID = 2, Content = "Bad One 2", Correct = false, Argumentation = "You can do better"},
            //     new Question.Types.Answer { AnswerID = 3, Content = "Bad One 3", Correct = false, Argumentation = "Are you that stupid"},
            //     new Question.Types.Answer { AnswerID = 4, Content = "Yay!", Correct = true, Argumentation = "W/e"}
            // };
            var question = new QuestionResponse();
            // {(
            //     QuestionID = 1,
            //     Content = "This is a test question.",
            //     Hint = "The shortest answer is the best one.",
            //     Difficulty = (uint) (random.Next() % 5 + 1),
            //     QuestionType = Question.Types.QuestionType.Abcd,
            // };
            // question.Answers.Add(answers);


            return Task.FromResult(question);
        }
    }
}
