import react from "react";
import "./Chat.css";

const Bubble = ({ message }) => {
  return (
    <div
      className={`chat-message-bubble ${
        message.myMessage ? "my-message" : "other-messages"
      }`}
    >
      <div>
        <div className="chat-message-author">{message.from}</div>
        <div className="chat-message">{message.message}</div>
        <div className="chat-message-time">
          {new Date(message.date).getHours() +
            ":" +
            new Date(message.date).getMinutes()}
        </div>
      </div>
    </div>
  );
};

export default Bubble;
