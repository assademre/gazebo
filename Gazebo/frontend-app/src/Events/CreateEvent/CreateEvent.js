import React, { Fragment, useState } from "react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { createEventAPI } from "./../../api";
import "./CreateEvent.css";
import { useNavigate } from "react-router-dom";
import Layout from "../../NavigationBar/Layout";
import currencySymbols from "../../helpers/currencySymbols";

function CreateEvent() {
  const [createrId, setCreaterId] = useState(() => localStorage.getItem('userId') ?? 0);
  const [eventType, setEventType] = useState('Default');
  const [eventName, setEventName] = useState('');
  const [budget, setBudget] = useState(0);
  const [currency, setCurrency] = useState('');
  const [place, setPlace] = useState('');
  const [status, setStatus] = useState('NotStarted');
  const [eventDate, setEventDate] = useState('');

  const navigate = useNavigate();

  const handleBack = () => {
    navigate('/main-page');
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
      eventDate: eventDate.toISOString(),
    };

    try {
      const result = await createEventAPI(data);
      alert(result);
    } catch (error) {
      alert(error);
    }
  }

  return (
    <Layout>
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

      <select className="select-field" value={currency} onChange={(e) => setCurrency(e.target.value)}>
          <option value="">Select Currency</option>
          {currencySymbols.map((option) => (
            <option key={option.value} value={option.value}>{option.label}</option>
          ))}
        </select>

      <div className="label">Place</div>
      <input type="text" className="input-field" value={place} onChange={(e) => setPlace(e.target.value)} />

      <div className="label">Event Due Date</div>
        <DatePicker selected={eventDate} onChange={(date) => setEventDate(date)} />

      <button className="button" onClick={handleBack}>Back to Main Page</button>

      <button className="button" onClick={() => handleSave()}>Create Event</button>
    </div>
    </Layout>
  );
}

export default CreateEvent;