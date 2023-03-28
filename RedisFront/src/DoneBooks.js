import axios from "axios";
import { useEffect, useState } from "react";

const DoneBooks = () => {
  const token = localStorage.getItem("token");
  const [nbooks, setNbooks] = useState([]);
  useEffect(() => {
    axios
      .get("https://localhost:7119/Book/GetDoneBooks", {
        headers: { Authorization: `Bearer ${token}` },
      })
      .then((res) => {
        setNbooks(res.data);
      })
      .catch((err) => console.log(err.message));
  }, []);
  return (
    <div>
      <h4>Books that have been done so far:</h4>
      {nbooks.map((nb) => (
        <p key={nb.isbn}>
          {nb.naslov} - {nb.autor} (ISBN-{nb.isbn}),
        </p>
      ))}
    </div>
  );
};
export default DoneBooks;
