from pydantic import BaseModel
from typing import List, Optional
from evaluation.evaluator import *
from fastapi import FastAPI
from inference import qg_prediction

app = FastAPI()

"""Classes for /difficulty endpoint
"""


class QuestionIn(BaseModel):
    """
    Failure check in case of boolean answers
    """
    QuestionID: int
    Content: Optional[str] = 'Default question'
    CorrectAnswer: Optional[str] = ''
    QuestionType: int


class QuestionsIn(BaseModel):
    ScenarioID: Optional[int]
    Questions: List[QuestionIn]


class QuestionOutput(BaseModel):
    QuestionID: int
    DifficultyLevel: int


class QuestionsOutput(BaseModel):
    ScenarioID: int
    Questions: List[QuestionOutput]


"""Endpoint /difficulty processed below
"""


@app.post("/difficulty", response_model=QuestionsOutput)
def process_output(user_request_in: QuestionsIn):
    questions = [(question_item.QuestionID,
                  question_item.Content,
                  question_item.CorrectAnswer,
                  question_item.QuestionType) for question_item in user_request_in.Questions]
    processed = assign_difficulty_level(questions)
    return {
        "ScenarioID": user_request_in.ScenarioID,
        "Questions": [
            {
                "QuestionID": q[0],
                "DifficultyLevel": q[1]
            } for q in processed
        ]
    }


"""Classes for endpoint /generateQuestions
"""


class AnswerIn(BaseModel):
    AnswerID: int
    Answer: str


class ContextIn(BaseModel):
    ScenarioID: int
    Answers: Optional[List[AnswerIn]] = []
    Text: str


class AiGeneratedQuestion(BaseModel):
    Content: str
    CorrectAnswer: str
    DifficultyLevel: int


class QuestionsOut(BaseModel):
    ScenarioID: int
    Questions: List[AiGeneratedQuestion]


@app.post("/generateQuestions", response_model=QuestionsOut)
def process_generated(user_request_in: ContextIn):
    generator = qg_prediction.QuestionGenerator()
    if user_request_in.Answers:
        answers = [answer.Answer for answer in user_request_in.Answers]
        generated = generator.generate(user_request_in.Text, answers)

    else:
        generated = generator.generate(user_request_in.Text)
    # sort both by id
    generated.sort(key=lambda x: x[0])
    lvl_assigned = assign_difficulty_level(generated)
    lvl_assigned.sort(key=lambda x: x[0])
    result = [(qg[1], qg[2], lvled[1]) for qg, lvled in zip(generated, lvl_assigned)]

    return {
        "ScenarioID": user_request_in.ScenarioID,
        "Questions": [
            {
                "Content": q[0],
                "CorrectAnswer": q[1],
                "DifficultyLevel": q[2]
            } for q in result
        ]
    }
