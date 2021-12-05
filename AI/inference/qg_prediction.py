import re
import statistics

import spacy
import torch
from transformers import T5Tokenizer, AutoModelForSeq2SeqLM

"""Uploading the language model only once in order to avoid memory overuse
"""
nlp = spacy.load("en_core_web_sm")


def get_noun_phrases(paragraph):
    """Helper function extracting noun chunks as possible answer candidates from context
    :param paragraph: one paragraph of context (max length of 490 tokens)
    :return: a dictionary with noun phrases as keys and paragraph as value
    """
    chunks = nlp(paragraph).noun_chunks
    candidates = []
    result = {}
    for np in chunks:
        candidates.append(np.text)
    lengths = [len(cand) for cand in candidates]
    mean = statistics.median_high(lengths)
    for n in candidates:
        if len(n) > mean:
            result[n] = paragraph
    return result


def _make_dict(question, answer):
    """Helper function for presenting the information in human-readable form
    :param question: generated question string
    :param answer: answer candidate string
    :return:
    """
    return {"Question": question, "Answer": answer}


class QuestionGenerator:
    def __init__(self):
        self.model = AutoModelForSeq2SeqLM.from_pretrained("Kithogue/T5_Question_Generation")
        self.answer_token = "<answer_token>"
        self.context_token = "<context_token>"
        self.tokenizer = T5Tokenizer.from_pretrained('t5-base', use_fast=False)
        self.tokenizer.add_special_tokens(
            {'additional_special_tokens': [self.answer_token, self.context_token]}
        )
        self.model.resize_token_embeddings(len(self.tokenizer))
        self.max_sequence_length = 512
        self.device = torch.device("cuda" if torch.cuda.is_available() else "cpu")

    def generate(self, context):
        inputs, answers = self.encode_inputs(context)
        generated_questions = self.generate_questions_from_inputs(inputs)
        message = f"{len(generated_questions)} questions do not match {len(answers)} answers"
        assert len(generated_questions) == len(answers), message
        qa_list = self._get_all_qa_pairs(generated_questions, answers)
        return qa_list

    def encode_inputs(self, text):
        inputs = []
        answers = []
        segments = self._split_into_segments(text)
        candidates = [get_noun_phrases(par) for par in segments]
        for candidate in candidates:
            answer_candidates = list(candidate.keys())
            context = candidate[answer_candidates[0]]
            processed_contexts, processed_answers = self._prepare_model_inputs(answer_candidates, context)
            inputs.extend(processed_contexts)
            answers.extend(processed_answers)
        return inputs, answers

    def generate_questions_from_inputs(self, model_inputs):
        return [self._generate_question(model_input) for model_input in model_inputs]

    def _split_into_segments(self, context):
        max_segment_length = 490  # a bit shorter than possible max sequence length
        paragraphs = context.split("\n")
        tokenized = [self.tokenizer(p)["input_ids"] for p in paragraphs if len(p) > 0]
        segments = []
        while len(tokenized) > 0:
            segment = []
            while len(segment) < max_segment_length and len(tokenized) > 0:
                paragraph = tokenized.pop(0)
                segment.extend(paragraph)
            segments.append(segment)
        return [self.tokenizer.decode(s) for s in segments]

    def _prepare_model_inputs(self, answer_candidates, text):
        inputs = []
        answers = []
        for answer_candidate in answer_candidates:
            context_and_answer = "{} {} {} {}".format(
                self.context_token, text, self.answer_token, answer_candidate
            )
            inputs.append(context_and_answer)
            answers.append(answer_candidate)

        return inputs, answers

    def _generate_question(self, model_input):
        self.model.eval()
        encoded_input = self._encode_model_input(model_input)
        with torch.no_grad():
            output = self.model.generate(input_ids=encoded_input["input_ids"])
        return self.tokenizer.decode(output[0], skip_special_tokens=True)

    def _encode_model_input(self, model_input):
        return self.tokenizer(
            model_input,
            padding='max_length',
            max_length=self.max_sequence_length,
            truncation=True,
            return_tensors='pt',
        ).to(self.device)

    def _get_all_qa_pairs(self, generated_questions, answers):
        qa_list = []
        for i in range(len(generated_questions)):
            qa = _make_dict(
                generated_questions[i].split("?")[0] + "?", answers[i]
            )
            qa_list.append(qa)
        return qa_list
