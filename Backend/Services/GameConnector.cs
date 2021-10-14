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

using Communication;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Backend
{
    public class GameConnector : GameRequests.GameRequestsBase
    {
        private readonly ILogger _logger;

        public GameConnector(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GameConnector>();
        }

        public override Task<Question> GetNextQuestion(GameData request, ServerCallContext context)
        {
            Answer[] answers = {
                new Answer { Answer_ = "Bad One 1", Description = "Not the best one", Ok = false },
                new Answer { Answer_ = "Bad One 2", Description = "You can do better", Ok = false },
                new Answer { Answer_ = "Bad One 3", Description = "Are you that stupid", Ok = false },
                new Answer { Answer_ = "Yay!", Description = "W/e", Ok = true }
            };
            var question = new Question
            {
                Message = "This is a test question.",
                Hint = "The shortest answer is the best one.",
                QuestionType = Question.Types.QuestionType.Abcd
            };
            question.Answers.Add(answers);


            return Task.FromResult(question);
        }
    }
}
