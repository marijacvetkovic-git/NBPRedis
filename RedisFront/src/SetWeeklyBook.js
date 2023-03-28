import axios from "axios";
import { useEffect, useState } from "react";
const SetWeeklyBook = () => {
  const token = localStorage.getItem("token");
  const setWB = () => {
    const isbnValue = document.getElementById("SetIsbn").value;
    console.log(isbnValue);
    axios
      .put("https://localhost:7119/Book/SetBookForThisWeek/" + isbnValue, {
        headers: { Authorization: `Bearer ${token}` },
      })
      .then(window.location.reload())
      .catch((err) => console.log(err.message));
  };
  return (
    <div>
      <h4>Assign this week's reading:</h4>
      <div className="mb-3">
        <label>ISBN</label>
        <input
          id="SetIsbn"
          type="text"
          className="form-control"
          placeholder="ISBN"
        />
      </div>
      <div className="d-grid">
        <button onClick={setWB} className="btn btn-success">
          Dodeli
        </button>
      </div>
    </div>
  );
};
export default SetWeeklyBook;
