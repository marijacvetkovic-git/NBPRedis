import React, { Component } from "react";
import axios from "axios";

const Login = () => {
  const loginUser = (e) => {
    e.preventDefault();
    //const token = localStorage.getItem("token");
    var usr = e.target.usrname.value;
    var passs = e.target.pass.value;
    // const config = {
    //   headers: {
    //     "Access-Control-Allow-Origin": "*",
    //     "Content-Type": "application/json",
    //   },
    // };
    axios
      .get("https://localhost:7119/User/LogIn/" + usr + "/" + passs)
      .then((res) => {
        console.log(res.data);

        if (res.data.token != undefined) {
          localStorage.setItem("token", res.data.token);
          //window.location.reload();
        } else console.log("nema sifra bato");

        window.location = "/Profile";

        //return res.data.token;
      })
      .catch((err) => console.log(err.message));
  };

  return (
    <form onSubmit={(e) => loginUser(e)}>
      <h3>Sign In</h3>
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
  );
};
export default Login;
