import json
import logging
from json_templates import JsonTemplates
from twilio.rest import Client


class MessageUtil:

    def __init__(self, account_sid, auth_token):
        self.__from_number = "whatsapp:+14155238886"
        self.__conversation_map_json = json.load(open("conversation.json"))
        self.client = Client(account_sid, auth_token)
        self.json_templates = JsonTemplates()

    def send_message_from_intent(self, intent_message, to, param=None):
        response = self.__conversation_map_json.get(intent_message)
        if not response:
            logging.error(f"no conversation defined for intent {intent_message}")
            return
        content_sid = response.get("content_sid")
        text_message = response.get("text")
        image = response.get("image")
        content_sid_with_content_variable = response.get("content_variable")
        # logging.info(f"Sending message to {to},content_sid: {content_sid},msg: {text_message},image_url: {image}, content_var_json: {content_sid_with_content_variable}")

        content_var_json = None
        if content_sid_with_content_variable:
            self.json_templates.loads(json.dumps(content_sid_with_content_variable))
            generate_response = self.json_templates.generate(param if param else {})
            if generate_response[0]:
                content_var_json = generate_response[1]
            else:
                content_var_json={}

        return self._send_message(
            to=to,
            content_sid=content_sid,
            image_url=image,
            content_var_json=content_var_json,
            msg=text_message
        )

    def _send_message(self, to, content_sid, msg, image_url, content_var_json):
        logging.info(f"Sending message to {to},content_sid: {content_sid},msg: {msg},image_url: {image_url}, content_var_json: {content_var_json}")
        if content_var_json and content_sid:
            return self._send_message_with_content_variable(
                content_sid=content_sid, content_json=content_var_json,
                to=to
            )
        elif content_sid:
            return self._send_template_message(content_sid, to)
        elif image_url:
            return self._send_media(to=to, media_url=image_url)
        
        return self._send_text_message(to, msg)

    def _send_text_message(self, to, msg):
        msg = self.client.messages.create(
            to=to,
            from_=self.__from_number,
            body=msg
        )
        logging.info(f"Sent message: {msg} to {to}")
        return msg

    def _send_media(self, to, media_url):
        msg = self.client.messages.create(
            to=to,
            from_=self.__from_number,
            media_url=media_url
        )
        logging.info(f"Sent message {msg} to {to}")

    def _send_template_message(self, content_sid, to):
        msg = self.client.messages.create(
            content_sid=content_sid,
            to=to,
            from_=self.__from_number,
        )
        logging.info(f"Sent message: {msg} to {to}")

    def _send_message_with_content_variable(self, content_sid, content_json, to):
        msg = self.client.messages.create(
            content_sid=content_sid,
            to=to,
            from_=self.__from_number,
            content_variables=json.dumps(content_json)
        )
        logging.info(f"Sent message: {msg} to {to}")
