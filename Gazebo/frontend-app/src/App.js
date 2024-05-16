import React, { useState } from "react";
import { BrowserRouter as Router, Route, Routes, Navigate } from "react-router-dom";
import MainPage from "./MainPage/MainPage";
import EditEvent from "./Events/EditEvent/EditEvent";
import EditTask from "./Tasks/EditTask/EditTask";
import Task from "./Tasks/Task/Task"
import Event from "./Events/Event/Event";
import CreateEvent from "./Events/CreateEvent/CreateEvent";
import CreateTask from "./Tasks/CreateTask/CreateTask";
import GetEvents from "./Events/GetEvents/GetEvents";
import GetTasks from "./Tasks/GetTasks/GetTasks";
import Login from "./AccountManagement/LoginPage/Login";
import SignupPage from "./AccountManagement/Signup/SignupPage";
import Logout from "./AccountManagement/Logout/Logout";
import Splash from "./AccountManagement/SplashScreen/Splash";
import NotificationPage from "./NavigationBar/NotificationPage/NotificationPage";
import FriendshipPage from "./Friendship/FriendshipPage";

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
          <Route path="/main-page" element={<MainPage />} />
          <Route path="/create-event" element={<CreateEvent />} />
          <Route path="/create-task" element={<CreateTask />} />
          <Route path="/get-events" element={<GetEvents />} />
          <Route path="/get-tasks" element={<GetTasks />} />
          <Route path="/event/:eventId" element={<Event />} />
          <Route path="/task/:taskId" element={<Task />} />
          <Route path="/edit-event/:eventId" element={<EditEvent />} />
          <Route path="/edit-task/:taskId" element={<EditTask />} />
          <Route path="/notifications" element={<NotificationPage />} />
          <Route path="/friendship" element={<FriendshipPage />} />
          <Route path="*" element={<Navigate to="/login" />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
