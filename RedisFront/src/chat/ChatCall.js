import * as signalR from "@microsoft/signalr";
import { useRef } from "react";

const useChatCall = (messageListener) => {
  var url = "https://localhost:7119/ChatHub";
  const connection = useRef(
    new signalR.HubConnectionBuilder()
      .withUrl(url, {
        withCredentials: true,
      })
      .build()
  );

  connection.current.on("message", (data) => {
    const msg = {
      id: data.id,
      from: data.from,
      date: data.date,
      message: data.message,
      room: data.room,
    };
    messageListener(msg);
  });
  if (connection.current.state === "Disconnected") {
    connection.current.start();
  }

  const sendMessage = (message) => {
    const messageString = JSON.stringify(message);
    connection.current.invoke("SendMessage", messageString);
  };

  const closeConnection = () => {
    if (connection.current.state === "Connected") {
      connection.current.stop();
    }
  };
  return { sendMessage, closeConnection };
};

export default useChatCall;
