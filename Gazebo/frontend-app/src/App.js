import React, { useState } from "react";
import { BrowserRouter as Router, Route, Routes, Navigate } from "react-router-dom";
import MainPage from "./MainPage/MainPage";
import CreateTask from "./CreateTask/CreateTask";
import CreateEvent from "./CreateEvent/CreateEvent";
import GetEvents from "./GetEvents/GetEvents";
import GetTasks from "./GetTasks/GetTasks";
import Login from "./LoginPage/Login";

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  const handleLoginSuccess = () => {
    setIsLoggedIn(true);
  };

  return (
    <Router>
      <div>
        <Routes>
          {isLoggedIn ? (
            <>
              <Route path="/" element={<MainPage />} />
              <Route path="/create-event" element={<CreateEvent />} />
              <Route path="/create-task" element={<CreateTask />} />
              <Route path="/get-events" element={<GetEvents />} />
              <Route path="/get-tasks" element={<GetTasks />} />
              <Route path="/login" element={<Login />} />
            </>
          ) : (
            <Route
              path="/"
              element={<Login onLoginSuccess={handleLoginSuccess} />}
            />
          )}
          <Route path="/*" element={<Navigate to="/" replace />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
