import React, { useEffect, useState } from "react";
import axios from "axios";
import { getUsername, getRole, getUserId } from "./utils";
import AddBook from "./AddBook";
import DeleteBook from "./DeleteBook";
import NotDoneBooks from "./NotDoneBooks";
import DoneBooks from "./DoneBooks";
import GetWeeklyBook from "./GetWeeklyBook";
import SetWeeklyBook from "./SetWeeklyBook";
const Profile = () => {
  const token = localStorage.getItem("token");
  var userRole = getRole();
  var userUsername = getUsername();
  //---------------------------
  const [styleDelete, changeDelete] = useState("hide");
  const showDelete = () => {
    changeDelete("show");
  };

  //------podaci-o-korisniku--
  const [user, setUser] = useState({});
  var isUser = true;
  if (userRole == "user") {
    isUser = true;
  } else if (userRole == "admin") {
    isUser = false;
  }
  const ShowInfo = () => {
    axios
      .get("https://localhost:7119/User/GetUser/" + userUsername, {
        headers: { Authorization: `Bearer ${token}` },
      })
      .then((res) => {
        //console.log(res.data);
        setUser({
          nn: res.data.name,
          sn: res.data.surname,
          un: res.data.username,
        });
      });
  };

  const deleteProfile = (e) => {
    var usr = e.target.usrname.value;
    var passs = e.target.pass.value;
    axios
      .delete(
        "https://localhost:7119/User/DeleteProfile/" + usr + "/" + passs,
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      )
      .then((res) => {
        window.location = "/";
      })
      .catch((err) => console.log(err.message));
  };
  useEffect(() => {
    ShowInfo();
  });
  const [getIsbn, setGetIsbn] = useState("");
  const [al, setAl] = useState(false);
  const [al2, setAl2] = useState(false);
  //-------------------------------------------------------------------
  const getISBN = () => {
    axios
      .get("https://localhost:7119/Book/GetBookForThisWeek/")
      .then((res) => {
        if (res.status !== 202) {
          setGetIsbn(res.data.isbn);
          setAl2(false);
        } else setAl2(true);
      })
      .catch((err) => console.log(err.message));
  };
  const addBookToList = () => {
    console.log(getIsbn);
    axios
      .put(
        "https://localhost:7119/User/AddBookInMyList/" +
          userUsername +
          "/" +
          getIsbn
      )
      .then((res) => {
        if (res.status === 202) setAl(true);
      })
      .catch((err) => console.log(err.message));
  };
  useEffect(() => {
    getISBN();
  });
  return isUser ? (
    <div className="c-profile">
      <div className="d-grid">
        <br />
        <button type="submit" className="btn btn-success" onClick={showDelete}>
          DELETE
        </button>
        <br />
        <div className={styleDelete}>
          <form onSubmit={(e) => deleteProfile(e)}>
            <h3>Confirm</h3>
            <div className="mb-3">
              <label>Username</label>
              <input
                id="usrname"
                type="text"
                className="form-control"
                placeholder="Username"
              />
            </div>
            <div className="mb-3">
              <label>Password</label>
              <input
                id="pass"
                type="password"
                className="form-control"
                placeholder="Enter password"
              />
            </div>
            <div className="d-grid">
              <button type="submit" className="btn btn-success">
                Submit
              </button>
            </div>
          </form>
        </div>
      </div>
      <h5>First name:{user.nn}</h5>
      <h5>Last name:{user.sn}</h5>
      <h5>Username:{user.un}</h5>
      <br />
      {al2 === false ? (
        <>
          <GetWeeklyBook />
          {al === false ? (
            <>
              <p>Have you read this week's book?</p>
              <button className="btn btn-success" onClick={addBookToList}>
                Yes, add it to my list
              </button>
            </>
          ) : (
            <div class="alert alert-danger" role="alert">
              You already added this book!
            </div>
          )}
        </>
      ) : (
        <div class="alert alert-danger" role="alert">
          Weekly book not assigned yet!
        </div>
      )}
    </div>
  ) : (
    <div className="scroll-profile">
      <div
        style={{
          display: "flex",
          justifyContent: "space-between",
        }}
      >
        <AddBook />
        <DeleteBook />
      </div>
      <br />
      <div style={{ display: "flex", flexDirection: "row", gap: "2rem" }}>
        <div className="scroll-neki2">
          <NotDoneBooks />
        </div>
        <div className="scroll-neki2">
          <DoneBooks />
        </div>
      </div>
      <br />
      <div style={{ display: "flex", flexDirection: "row", gap: "2rem" }}>
        <div className="scroll-neki">
          <GetWeeklyBook />
          {al2 === false ? (
            <></>
          ) : (
            <div class="alert alert-danger" role="alert">
              Weekly book not assigned yet!
            </div>
          )}
        </div>

        <SetWeeklyBook />
      </div>
    </div>
  );
};
export default Profile;
