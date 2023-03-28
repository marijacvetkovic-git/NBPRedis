import axios from "axios";
import { useEffect, useState } from "react";

const NotDoneBooks = () => {
  const token = localStorage.getItem("token");
  const [nbooks, setNbooks] = useState([]);
  useEffect(() => {
    axios
      .get("https://localhost:7119/Book/GetNotDoneBooks", {
        headers: { Authorization: `Bearer ${token}` },
      })
      .then((res) => {
        setNbooks(res.data);
      })
      .catch((err) => console.log(err.message));
  }, []);
  return (
    <div>
      <h4>Books that haven't been done:</h4>
      {nbooks.map((nb) => (
        <p key={nb.isbn}>
          {nb.naslov} - {nb.autor} (ISBN-{nb.isbn}),
        </p>
      ))}
    </div>
  );
};
export default NotDoneBooks;
