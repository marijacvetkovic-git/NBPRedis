import React, { useState, useEffect } from "react";
import axios from "axios";
import { getUsername, getRole, getUserId } from "./utils";
const YourBooks = () => {
  const [books, setBooks] = useState([]);
  const userID = getUserId();
  const username = getUsername();
  const [hasBooks, setHasBooks] = useState(false);
  useEffect(() => {
    axios
      .get("https://localhost:7119/User/GetAllBooksOfUser/" + username)
      .then((res) => {
        if (res.status != 202) {
          setHasBooks(true);
          setBooks(res.data);
        } else {
          setHasBooks(false);
        }
      });
  }, []);
  const RateBook = (e, isbn) => {
    var rate = e.target.rateNum.value;
    console.log(isbn, rate);
    axios
      .put(
        "https://localhost:7119/User/AddRateBook/" +
          username +
          "/" +
          isbn +
          "/" +
          rate
      )
      .then()
      .catch((err) => {
        console.log(err.message);
      });
  };
  return (
    <>
      <h5>Your Books:</h5>
      <div style={{overflow:"auto",height:"600px",width:"1000px"}}> 
      <div
        style={{
          display: "flex",
          flexDirection: "row",
          gap: "2rem",
          maxWidth: "1000px",
          flexWrap: "wrap"
        }}
      >
        {hasBooks === true ? (
          books.map((b) => {
            return (
              <>
                <div className="card" key={b.isbn} style ={{width:"300px",height:"100%"}}>
                  <div className="card-body">
                    <h5 className="card-title">{b.naslov}</h5>
                    <h6 className="card-subtitle mb-2 text-muted">{b.autor}</h6>
                    <p className="card-text">
                      Genre: {b.zanr}
                      <br />
                      Synopsis:{b.ukratko}
                      <br />
                      Prosecna ocena:{b.prosecna}
                    </p>
                    {b.ocena == null ? (
                      <>
                        <form onSubmit={(e) => RateBook(e, b.isbn)}>
                          <button className="btn btn-success">
                            Rate this book
                          </button>
                          <input
                            id="rateNum"
                            type="text"
                            className="form-control"
                            placeholder="rateNumber"
                          />
                        </form>
                      </>
                    ) : (
                      <>You rated this book with: {b.ocena}</>
                    )}
                  </div>
                </div>
              </>
            );
          })
        ) : (
          <p>Trenutno nemate procitane knjige. Procitajte neku :)</p>
        )}
      </div>
      </div>
    </>
  );
};
export default YourBooks;
