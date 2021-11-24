from evaluation.evaluator import *
#from inference.qg_prediction import QuestionGenerator
import os
from API import models
import uvicorn

test_path = "resources/WorldWarII.json"
sample_path = r"C:\Users\HP.LAPTOP-QI9IOVLF\OneDrive\Pulpit\sample.txt"


def run():
    uvicorn.run(models.app, host="0.0.0.0", port=8000)


if __name__ == '__main__':
    run()
