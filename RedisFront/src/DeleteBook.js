import axios from "axios";
import { useState, useEffect } from "react";
const DeleteBook = () => {
  const token = localStorage.getItem("token");
  const [al, setAl] = useState(false);
  const [isbnValue, setIsbnValue] = useState();
  const deleteBook = () => {
    const isbnValue = document.getElementById("deleteIsbn").value;
    console.log(isbnValue);
    axios
      .delete("https://localhost:7119/Book/DeleteBook/" + isbnValue, {
        headers: { Authorization: `Bearer ${token}` },
      })
      .then((res) => {
        if (res.status === 202) {
          setAl(true);
        } else {
          setAl(false);
          window.location.reload();
        }
      })
      .catch((err) => console.log(err.message));
  };

  return (
    <div>
      <h3>DELETE BOOK</h3>
      <div className="mb-3">
        <label>ISBN</label>
        <input
          id="deleteIsbn"
          type="text"
          className="form-control"
          placeholder="ISBN"
        />
      </div>
      <div className="d-grid">
        <button type="button" onClick={deleteBook} className="btn btn-success">
          Delete
        </button>
        {al === true ? (
          <div class="alert alert-danger" role="alert">
            This book doesn't exist!
          </div>
        ) : (
          <></>
        )}
      </div>
    </div>
  );
};
export default DeleteBook;
