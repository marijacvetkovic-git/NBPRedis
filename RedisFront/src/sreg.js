import React from "react";
import axios from "axios";
const SignUp = () => {
  const addUser = (e) => {
    e.preventDefault();
    var name = e.target.name.value;
    var sname = e.target.sname.value;
    var usernam = e.target.usernam.value;
    var pass = e.target.pass.value;
    var uloga = "user";
    const user = {
      username: usernam,
      name: name,
      surname: sname,
      password: pass,
      role: uloga,
    };
    const config = {
      headers: {
        "Access-Control-Allow-Origin": "*",
        "Content-Type": "application/json",
      },
    };
    axios
      .post("https://localhost:7119/User/SignUp/", user, config)
      .then((res) => {
        console.log(res.data);
      })
      .catch((err) => console.log(err.message));
  };

  return (
    <form onSubmit={(e) => addUser(e)}>
      <h3>Sign Up</h3>
      <div className="mb-3">
        <label>First name</label>
        <input
          id="name"
          type="text"
          className="form-control"
          placeholder="First name"
        />
      </div>
      <div className="mb-3">
        <label>Last name</label>
        <input
          id="sname"
          type="text"
          className="form-control"
          placeholder="Last name"
        />
      </div>
      <div className="mb-3">
        <label>Username</label>
        <input
          id="usernam"
          type="text"
          className="form-control"
          placeholder="Enter username"
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
          Sign Up
        </button>
      </div>
      <p className="forgot-password text-right">
        Already registered <a href="/sign-in">sign in?</a>
      </p>
    </form>
  );
};

export default SignUp;
