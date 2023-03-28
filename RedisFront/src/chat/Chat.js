import Form from "./Form";
import List from "./List";
import useChatCall from "./ChatCall";
import { getUsername } from "../utils";
import { useEffect, useState } from "react";

const Chat = () => {
  var username = getUsername();
  const [messages, setChatMessages] = useState([]);

  const onReceiveMessage = (message) => {
    const msg = {
      id: message.id,
      from: message.from,
      date: message.date,
      message: message.message,
      room: message.room,
      myMessage: message.from === username,
    };
    setChatMessages((state) => {
      if (state.some((m) => m.id === msg.id)) {
        return state;
      }

      return [msg, ...state];
    });
  };
  const { sendMessage, closeConnection } = useChatCall(onReceiveMessage);
  useEffect(() => {
    return closeConnection;
  }, []);

  return (
    <div className="chat-container">
      <h2>Chatroom</h2>
      <List chatMessages={messages} />
      <Form sendMessage={sendMessage} />
    </div>
  );
};

export default Chat;
