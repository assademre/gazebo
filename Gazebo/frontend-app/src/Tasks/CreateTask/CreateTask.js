import React, { Fragment, useState, useEffect } from "react";
import { createTaskAPI, getEventByUserIdAPI } from "./../../api";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import "./CreateTask.css";
import { useNavigate } from "react-router-dom";
import Layout from "./../../NavigationBar/Layout";
import currencySymbols from "./../../helpers/currencySymbols";

function CreateTask() {
  const [ownerId, setOwnerId] = useState(() => localStorage.getItem('userId') ?? 0);
  const [eventId, setEventId] = useState('');
  const [taskName, setTaskName] = useState('');
  const [budget, setBudget] = useState(0);
  const [currency, setCurrency] = useState('');
  const [place, setPlace] = useState('');
  const [status, setStatus] = useState('NotStarted');
  const [eventOptions, setEventOptions] = useState([]);
  const [taskDate, setTaskDate] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    fetchEventOptions();
  }, []);

  const fetchEventOptions = async () => {
    try {
      const events = await getEventByUserIdAPI();
      const options = events.map(event => ({
        label: event.eventName,
        value: event.eventId
      }));
      setEventOptions(options);
    } catch (error) {
      console.error("Error fetching event options:", error);
    }
  };

  const handleBack = () => {
    navigate('/main-page');
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
      taskDate: taskDate.toISOString(),
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

        <div className="label">Event</div>
        <select className="select-field" value={eventId} onChange={(e) => setEventId(e.target.value)}>
          <option value="">Select Event</option>
          {eventOptions.map((option) => (
            <option key={option.value} value={option.value}>{option.label}</option>
          ))}
        </select>

        <div className="label">Task Name</div>
        <input type="text" className="input-field" value={taskName} onChange={(e) => setTaskName(e.target.value)} />

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

        {/* <div className="label">Status</div>
        <select className="select-field" value={status} onChange={(e) => setStatus(e.target.value)}>
          <option value="">Select Status</option>
          {statusOptions.map((option) => (
            <option key={option.value} value={option.value}>{option.label}</option>
          ))}
        </select>  */}

      <div className="label">Task Due Date</div>
        <DatePicker selected={taskDate} onChange={(date) => setTaskDate(date)} />

        <button className="button" onClick={handleBack}>Back to Main Page</button>

        <button className="button" onClick={() => handleSave()}>Create Task</button>
      </div>
    </Layout>
  );
}

export default CreateTask;
