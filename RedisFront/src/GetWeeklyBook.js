import axios from "axios";
import { useEffect, useState } from "react";

const GetWeeklyBook = () => {
  const token = localStorage.getItem("token");
  const [nbooks, setNbooks] = useState([]);
  useEffect(() => {
    axios
      .get("https://localhost:7119/Book/GetBookForThisWeek", {
        headers: { Authorization: `Bearer ${token}` },
      })
      .then((res) => {
        setNbooks(res.data);
      })
      .catch((err) => console.log(err.message));
  }, []);
  return (
    <div>
      <h4>This week's reading:</h4>
      <p>
        {nbooks.naslov} - {nbooks.autor}
      </p>
    </div>
  );
};
export default GetWeeklyBook;
