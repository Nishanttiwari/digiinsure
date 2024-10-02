import logging
from urllib.parse import parse_qs
import time

import azure.functions as func

from message_util import MessageUtil

ACCOUNT_SID = "ACcf7c7b28e043207673c709a20c6714db"
AUTH_TOKEN = "917c6c332b1895ad8f168ae36a74f152"
message_sending_util = MessageUtil(account_sid=ACCOUNT_SID, auth_token=AUTH_TOKEN)

app = func.FunctionApp(http_auth_level=func.AuthLevel.ANONYMOUS)


@app.route(route="http_example")
def http_example(req: func.HttpRequest) -> func.HttpResponse:
    logging.info("Python HTTP trigger function processed a request.")
    _body_byte = req.get_body().decode('utf-8')
    _body = parse_qs(_body_byte)
    _text_message = _body['Body'][0].lower()
    _from = _body['From'][0]
    _name = _body['ProfileName'][0]
    _param = {}
    _param["name"] = _name

    logging.info(f"Obtained text message {_text_message} from {_from}")

    if _text_message == "hi":
        message_sending_util.send_message_from_intent(to=_from, intent_message="greeting")
    if _text_message == "nr2 2th":
        message_sending_util.send_message_from_intent(to=_from, intent_message="postcode_verfiied")
    if _text_message == "fund_value_past1":
        message_sending_util.send_message_from_intent(to=_from, intent_message="fund_value_past1")
        time.sleep(4)
        message_sending_util.send_message_from_intent(to=_from, intent_message="fund_value_past2")
    if _text_message == "fund_value_past2":
        message_sending_util.send_message_from_intent(to=_from, intent_message="fund_value_past2")
        
    if _text_message == "yes":
        message_sending_util.send_message_from_intent(to=_from, intent_message="resolve_yes")
    if _text_message == "no, thank you":
        message_sending_util.send_message_from_intent(to=_from, intent_message="resolve_yes")
    if _text_message == "i have more queries":
        message_sending_util.send_message_from_intent(to=_from, intent_message="more_yes")
    if _text_message == "fund_allocation_1":
        message_sending_util.send_message_from_intent(to=_from, intent_message="fund_allocation_1")
        time.sleep(4)
        message_sending_util.send_message_from_intent(to=_from, intent_message="fund_value_past2")
    if _text_message == "estd_fund_value_1":
        message_sending_util.send_message_from_intent(to=_from, intent_message="estd_fund_value_1")
    
    
    if _text_message == "hello":
        message_sending_util.send_message_from_intent(to=_from, intent_message="hi", param=_param)
    if _text_message == "bye":
        message_sending_util.send_message_from_intent(to=_from, intent_message="bye")

    return func.HttpResponse(status_code=200)
