import spacy
from spacy.lang.en import English
import wordfreq
import json
import os

"""A module assigns difficulty levels in range [1, 5]
to both generated and predefined questions,
basing on the type of question
and word frequency of the answer tokens,
as well as presence of named entities and numerical values in the answer
"""

nlp = spacy.load("en_core_web_sm")
lang_model = English()


def process_predefined(path_to_object):
    result = []
    with open(path_to_object, 'r', encoding='utf-8') as questions:
        data = json.load(questions)
        scenario_id = data['ScenarioID']
        for question in data['Questions']:
            q_id = question['QuestionID']
            q_content = question['Content']
            a_content = question['CorrectAnswer']
            q_type = question['QuestionType']
            result.append((q_id, q_content, a_content, q_type))
    return scenario_id, result


def get_question_type(qa_tuple):
    """A function defines a question type
    Special questions requiring a full answer usually start with a question word or a preposition,
    therefore we tokenize the question and get a PoS-tag of the first token
    we also assume that the question could be open or close.
    :param qa_tuple: question, answer, isOpen
    :returns integer value, the base of the heuristic function
    """
    # think of a simpler rule-based approach looking for question words in question tokens,
    # covering the questions of type: [General Question + Why?].
    doc = nlp(qa_tuple[0])
    if doc[0].pos_ == "PRON" or doc[0].pos_ == "DET" or doc[0].pos_ == "ADP":
        if qa_tuple[2]:
            return 2
        else:
            return 1
    else:
        return 0


def tokenize(answer):
    """Helper method which utilizes Spacy tokenizer to split the text into tokens
    stripping it of punctuation.
    :param answer: str
    :return: list of string tokens
    """
    tokenizer = lang_model.tokenizer
    tokens = tokenizer(answer)
    return [str(token) for token in tokens if not token.is_punct]


def get_frequency_coefficient(answer: str) -> float:
    """Tokenizes the answer, calculates the inverse frequency (1 - frequency, how high is the probability
    NOT to encounter the token in the corpus)
    for each token, calculates max inverse frequency for the answer (meaning the rarest token)
    and adds it to 1 to get a frequency coefficient
    on encounter of an out-of-vocabulary token, immediately returns 2
    :param answer: answer string
    :return: floating point number between 1 and 2
    """
    # counter check out
    answer_tokens = tokenize(answer)
    word_freq_tokens = []
    for token in answer_tokens:
        if wordfreq.word_frequency(token, 'en') == 0.0:
            return 2.0
        else:
            inverse_frequency = 1 - wordfreq.word_frequency(token, 'en')
            word_freq_tokens.append(inverse_frequency)
    return 1 + max(word_freq_tokens)


def is_named_entity(answer: str) -> float:
    """Checks if the answer contains named entities which have to be remembered precisely
    :param answer: answer string
    :return: floating point number, either 1.0 or 1.5
    """
    doc = nlp(answer)
    if doc.ents:
        return 1.5
    else:
        return 1


def is_number(answer: str) -> float:
    """Checks if the question contains a numerical token, which is harder to memorize
    :param answer: answer string
    :return: floating point number, either 1.0 or 1.5
    """
    for token in tokenize(answer):
        if token.isnumeric():
            return 1.5
    return 1


def get_question_complexity(question_id: int, answer: str, q_type: int):
    """Applies the heuristic function to each question to get the complexity score
    f(question) = q_type * rarity score * NER coefficient * numerical coefficient,
    range of the function is [1, 9], where 1 is assigned to boolean questions, and 9 - to open questions,
    where answers contain out-of-vocabulary and numerical tokens along with named entities
    :param question_id: int id of the question
    :param answer: answer string
    :param q_type: type of question (0 - boolean, 1 - close, 2 - open)
    :return: a tuple of question, answer and assigned score
    """
    if q_type == 0:
        score = 1
    else:
        score = q_type * get_frequency_coefficient(answer) * is_named_entity(
            answer) * is_number(answer)
    return question_id, score


def assign_difficulty_level(all_qa):
    """Questions are sorted by complexity score in ascending order and separated into five batches;
    we assume that only a small part of the questions could be considered difficult,
    therefore the batch size is increased by one, if the number of questions is not a multiple of 5.
    The batch with the maximum level of difficulty is always equal to or smaller than the size of the other batches.
    The level of difficulty is calculated as follows: index / batch + 1
    :param all_qa: list of either generated or predefined questions
    :return: list of questions with a difficulty level assigned
    """
    # iterate over enumerate / zip
    qa_complexity = [get_question_complexity(question_id=qa[0], answer=qa[2], q_type=qa[3]) for qa in all_qa]
    qa_complexity.sort(key=lambda x: x[1])
    if len(qa_complexity) % 5 == 0:
        batch = len(qa_complexity) / 5
    else:
        batch = len(qa_complexity) / 5 + 1
    result = []
    for i in range(len(qa_complexity)):
        level = int(i / batch) + 1
        result_tuple = (qa_complexity[i][0], level)
        result.append(result_tuple)
    return result


def convert_to_json(questions, path, scenario_id):
    """Helper method serializing the questions with difficulty level to a json
    :param scenario_id: int, id of the scenario
    :param questions: list of processed questions
    :param path: relative path to the output directory
    """
    inner_list = []
    for q in questions:
        q_dict = {'QuestionID': q[0], 'DifficultyLevel': q[1]}
        inner_list.append(q_dict)
    result = {'Scenario_ID': scenario_id, 'Questions': inner_list}
    filename = os.path.join(path, 'output.json')
    with open(filename, 'w', encoding='utf-8') as f:
        json.dump(result, f)
