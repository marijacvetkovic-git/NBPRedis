import react from "react";
import ChatMessageBubble from "./Bubble";
import "./Chat.css";

const List = ({ chatMessages }) => {
  return (
    <div className="chat-message-list">
      {chatMessages.map((m) => (
        <ChatMessageBubble key={m.id} message={m} />
      ))}
    </div>
  );
};

export default List;
