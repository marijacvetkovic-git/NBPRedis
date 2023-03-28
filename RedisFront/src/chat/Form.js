import { TextField } from "@mui/material";
import { getUsername } from "../utils.js";
import { useRef } from "react";
import { v4 as uuidv4 } from "uuid";

import "./Chat.css";

const Form = ({ sendMessage }) => {
  var username = getUsername();
  const textFieldRef = useRef(null);

  const onSendMessage = () => {
    const msgText = textFieldRef.current?.value;
    if (!username || !msgText) return;

    const guid = uuidv4();
    const newMessage = {
      id: guid,
      from: username,
      date: new Date(),
      message: msgText,
      room: "groupChat",
    };

    sendMessage(newMessage);

    textFieldRef.current.value = "";
  };

  return (
    <div className="chat-form">
      <TextField
        size="small"
        fullWidth
        placeholder="Write a message ABOUT BOOKS"
        inputRef={textFieldRef}
        color="success"
        onKeyPress={(ev) => {
          if (ev.key === "Enter") {
            onSendMessage();
            ev.preventDefault();
          }
        }}
      />
    </div>
  );
};

export default Form;
