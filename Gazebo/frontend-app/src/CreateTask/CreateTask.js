import React, { Fragment, useState } from "react";
import { createTaskAPI } from "../api";
import statusOptions from "../helpers/statusOptions";
import "./CreateTask.css";
import { useNavigate } from "react-router-dom";
import Layout from "../NavigationBar/Layout";

function CreateTask() {
  const [ownerId, setOwnerId] = useState('');
  const [eventId, setEventId] = useState('');
  const [taskName, setTaskName] = useState('');
  const [budget, setBudget] = useState(0);
  const [currency, setCurrency] = useState('');
  const [place, setPlace] = useState('');
  const [status, setStatus] = useState('NotStarted');


  const navigate = useNavigate();

  const handleBack = () => {
    navigate('/');
  };

  const handleSave = async () => {
    const data = {
      ownerId: ownerId,
      eventId: eventId,
      taskName: taskName,
      budget: budget,
      currency: currency,
      place: place,
      status: status,
      createdDate: new Date().toISOString(),
      updatedDate: new Date().toISOString(),
      taskDate: new Date().toISOString(),
    };

    try {
      const result = await createTaskAPI(data);
      alert(result);
    } catch (error) {
      alert(error);
    }
  }

  return (
    <Layout>
      <div className="container">
      <div>Create a new Task</div>

      <div className="label">Owner Id</div>
      <input type="text" className="input-field" value={ownerId} onChange={(e) => setOwnerId(e.target.value)} />

      <div className="label">Event Id</div>
      <input type="text" className="input-field" value={eventId} onChange={(e) => setEventId(e.target.value)} />
    
      <div className="label">Task Name</div>
      <input type="text" className="input-field" value={taskName} onChange={(e) => setTaskName(e.target.value)} />

      <div className="label">Budget</div>
      <input type="number" className="input-field" value={budget} onChange={(e) => setBudget(parseInt(e.target.value))} />

      <div className="label">Currency</div>
      <input type="text" className="input-field" value={currency} onChange={(e) => setCurrency(e.target.value)} />

      <div className="label">Place</div>
      <input type="text" className="input-field" value={place} onChange={(e) => setPlace(e.target.value)} />

      {/* <div className="label">Status</div>
      <select className="select-field" value={status} onChange={(e) => setStatus(e.target.value)}>
        <option value="">Select Status</option>
        {statusOptions.map((option) => (
          <option key={option.value} value={option.value}>{option.label}</option>
        ))}
      </select> */}

      <button className="button" onClick={handleBack}>Back to Main Page</button>

      <button className="button" onClick={() => handleSave()}>Create Task</button>
    </div>
    </Layout>
  );
}

export default CreateTask;