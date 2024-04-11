import React, { useState } from "react";
import { BrowserRouter as Router, Route, Routes, Navigate } from "react-router-dom";
import MainPage from "./MainPage/MainPage";
import EditEvent from "./Events/Event/EditEvent";
import EditTask from "./Tasks/Task/EditTask";
import CreateEvent from "./Events/CreateEvent/CreateEvent";
import CreateTask from "./Tasks/CreateTask/CreateTask";
import GetEvents from "./Events/GetEvents/GetEvents";
import GetTasks from "./Tasks/GetTasks/GetTasks";
import Login from "./AccountManagement/LoginPage/Login";
import SignupPage from "./AccountManagement/Signup/SignupPage";
import Logout from "./AccountManagement/Logout/Logout";
import Splash from "./AccountManagement/SplashScreen/Splash";

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
          <Route path="/" element={<Splash />} />
          <Route path="/signup" element={<SignupPage setSignupSuccessMessage={setSignupSuccessMessage} />} />
          <Route path="/login" element={<Login onLoginSuccess={handleLoginSuccess} />} />
          <Route path="/logout" element={<Logout />} />
          <Route path="/main-page" element={isLoggedIn ? <MainPage /> : <Login onLoginSuccess={handleLoginSuccess} />} />
          <Route path="/create-event" element={<CreateEvent />} />
          <Route path="/create-task" element={<CreateTask />} />
          <Route path="/get-events" element={<GetEvents />} />
          <Route path="/get-tasks" element={<GetTasks />} />
          <Route path="/edit-event/:eventId" element={<EditEvent />} />
          <Route path="/edit-task/:taskId" element={<EditTask />} />
          <Route path="*" element={<Navigate to="/login" />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
