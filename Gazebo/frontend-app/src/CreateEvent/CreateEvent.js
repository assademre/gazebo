import React, { Fragment, useState } from "react";
import { createEventAPI } from "../api";
import statusOptions from "../helpers/statusOptions";
import "./CreateEvent.css";
import { useNavigate } from "react-router-dom";

function CreateEvent() {
  const [createrId, setCreaterId] = useState(1);
  const [eventType, setEventType] = useState('Default');
  const [eventName, setEventName] = useState('');
  const [budget, setBudget] = useState(0);
  const [currency, setCurrency] = useState('');
  const [place, setPlace] = useState('');
  const [status, setStatus] = useState('NotStarted');

  const navigate = useNavigate();

  const handleBack = () => {
    navigate(-1);
  };

  const handleSave = async () => {
    const data = {
      createrId: createrId,
      eventType: eventType,
      eventName: eventName,
      budget: budget,
      currency: currency,
      place: place,
      status: status,
      createdDate: new Date().toISOString(),
      updatedDate: new Date().toISOString(),
      eventDate: new Date().toISOString(),
    };

    try {
      const result = await createEventAPI(data);
      alert(result);
    } catch (error) {
      alert(error);
    }
  }

  return (
    <div className="container">
      <div className="title">Create a new Event</div>
      <div className="label">Event Type</div>
      <select className="select-field" value={eventType} onChange={(e) => setEventType(e.target.value)}>
        <option value="Default">Default</option>
        <option value="Birthday">Birthday</option>
      </select>

      <div className="label">Event Name</div>
      <input type="text" className="input-field" value={eventName} onChange={(e) => setEventName(e.target.value)} />

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

      <button className="button" onClick={() => handleSave()}>Create Event</button>

      <button className="back-to-main-button" onClick={handleBack}>Back to Main Page</button>
    </div>
  );
}

export default CreateEvent;