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
using Backend.Analysis_module;
using Gameplay;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Backend.Services
{
    public class GameConnector : GameplayMessages.GameplayMessagesBase
    {
        private readonly DataContext _context;
        private readonly ILogger _logger;
        private Random _random = new Random();
        private readonly IAnalysisModuleService _analysisModuleService;

        public GameConnector(ILoggerFactory loggerFactory, IAnalysisModuleService analysisModuleService, DataContext context)
        {
            _analysisModuleService = analysisModuleService;
            _context = context;
            _logger = loggerFactory.CreateLogger<GameConnector>();
        }

        public override Task<StartGameResponse> StartNewSession(StartGameRequest request, ServerCallContext context)
        {
            return Task.FromResult(_analysisModuleService.StartNewSession(request, _context));
        }

        public override Task<QuestionResponse> PrepareNextQuestion(QuestionRequest request, ServerCallContext context)
        {
            return Task.FromResult(_analysisModuleService.PrepareNextQuestion(request));
        }

        public override Task<Empty> UpdateStudentsAnswers(StudentAnswerRequest request, ServerCallContext context)
        {
            return Task.FromResult(_analysisModuleService.UpdateStudentsAnswers(request));
        }

        public override Task<Empty> FinishGame(EndGameRequest request, ServerCallContext context)
        {
            return Task.FromResult(_analysisModuleService.EndGame(request, _context));
        }
    }
}