import React from "react";
import "../node_modules/bootstrap/dist/css/bootstrap.min.css";
import "./App.css";
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import Login from "./login.js";
import SignUp from "./sreg.js";
import Profile from "./Profile.js";
import YourBooks from "./YourBooks";
import { getRole, getUsername, getUserId } from "./utils";
import Chat from "./chat/Chat.js";
import "./App.css";
import "bootstrap/dist/css/bootstrap.min.css";

var role = getRole();
var token = localStorage.getItem("token");
const logout = () => {
  localStorage.removeItem("token");
  window.location = "/";
};
function App() {
  return (
    <Router>
      <div className="App">
        <nav className="navbar navbar-expand-lg navbar-light fixed-top">
          <div className="container">
            <Link className="navbar-brand" to={"/sign-in"}>
              ReadersCult
            </Link>
            <div className="collapse navbar-collapse" id="navbarTogglerDemo02">
              <ul className="navbar-nav ml-auto">
                <li className="nav-item">
                  <Link className="nav-link" to={"/sign-in"}>
                    Sign in
                  </Link>
                </li>
                {token === null ? (
                  <li className="nav-item">
                    <Link className="nav-link" to={"/sign-up"}>
                      Sign up
                    </Link>
                  </li>
                ) : (
                  <li className="=" nav-item>
                    <button
                      type="submit"
                      className="btn btn-success"
                      onClick={logout}
                    >
                      LogOut
                    </button>
                  </li>
                )}

                {token !== null && role !== "admin" ? (
                  <>
                    <li className="nav-item">
                      <Link className="nav-link" to={"/profile"}>
                        Profile
                      </Link>
                    </li>
                    <li className="nav-item">
                      <Link className="nav-link" to={"/mybooks"}>
                        My books
                      </Link>
                    </li>
                    <li className="nav-item">
                      <Link className="nav-link" to={"/chat"}>
                        Chat room
                      </Link>
                    </li>
                  </>
                ) : (
                  <></>
                )}
              </ul>
            </div>
          </div>
        </nav>
        <div className="auth-wrapper">
          <div className="auth-inner">
            <Routes>
              <Route path="/profile" element={<Profile />} />
              <Route path="/mybooks" element={<YourBooks />} />
              <Route exact path="/" element={<Login />} />
              <Route path="/sign-in" element={<Login />} />
              <Route path="/sign-up" element={<SignUp />} />
              <Route path="/chat" element={<Chat />} />
            </Routes>
          </div>
        </div>
      </div>
    </Router>
  );
}
export default App;
