from pydantic import BaseModel
from typing import List, Optional
from evaluation.evaluator import *
import uvicorn
from fastapi import FastAPI

app = FastAPI()


class QuestionIn(BaseModel):
    QuestionID: int
    Content: Optional[str] = 'Default question'
    CorrectAnswer: str
    QuestionType: Optional[int] = 0


class QuestionsIn(BaseModel):
    ScenarioID: Optional[int]
    Questions: List[QuestionIn]


class QuestionOutput(BaseModel):
    QuestionID: int
    DifficultyLevel: int


class QuestionsOutput(BaseModel):
    ScenarioID: int
    Questions: List[QuestionOutput]


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

