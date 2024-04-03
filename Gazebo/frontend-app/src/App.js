import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import MainPage from "./MainPage/MainPage";
import CreateTask from "./CreateTask/CreateTask";
import CreateEvent from "./CreateEvent/CreateEvent";

function App() {
  return (
    <Router>
      <div>
        <Routes>
          <Route path="/" element={<MainPage />} />
          <Route path="/create-event" element={<CreateEvent />} />
          <Route path="/create-task" element={<CreateTask />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;