import React, { useState } from "react";
import { BrowserRouter as Router, Route, Routes, Navigate } from "react-router-dom";
import MainPage from "./MainPage/MainPage";
import Event from "./Events/Event/Event";
import CreateEvent from "./Events/CreateEvent/CreateEvent";
import CreateTask from "./Tasks/CreateTask/CreateTask";
import GetEvents from "./Events/GetEvents/GetEvents";
import GetTasks from "./Tasks/GetTasks/GetTasks";
import Login from "./LoginPage/Login";
import SignupPage from "./Signup/SignupPage";
import Logout from "./Logout/Logout";

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [signupSuccessMessage, setSignupSuccessMessage] = useState("");

  const handleLoginSuccess = () => {
    setIsLoggedIn(true);
  };

  return (
    <Router>
      <div>
        {signupSuccessMessage && <div>{signupSuccessMessage}</div>}
        <Routes>
          <Route path="/signup" element={<SignupPage setSignupSuccessMessage={setSignupSuccessMessage} />} />
          <Route path="/main-page" element={isLoggedIn ? <MainPage /> : <Login onLoginSuccess={handleLoginSuccess} />} />
          <Route path="/create-event" element={<CreateEvent />} />
          <Route path="/create-task" element={<CreateTask />} />
          <Route path="/get-events" element={<GetEvents />} />
          <Route path="/get-tasks" element={<GetTasks />} />
          <Route path="/login" element={<Login onLoginSuccess={handleLoginSuccess} />} />
          <Route path="/logout" element={<Logout />} />
          <Route path="/event/:eventId" element={<Event />} /> // Add this route for event detail
          <Route path="*" element={<Navigate to="/signup" />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
