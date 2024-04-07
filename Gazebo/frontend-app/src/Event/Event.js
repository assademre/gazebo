import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getEventByEventIdAPI, updateEventAPI } from '../api';
import { format } from 'date-fns';
import statusOptions from '../helpers/statusOptions';
import "./Event.css";

function Event() {
  const { eventId } = useParams();
  const [event, setEvent] = useState(null);
  const [editMode, setEditMode] = useState(false);
  const [updatedEvent, setUpdatedEvent] = useState({});
  const navigate = useNavigate();

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const eventData = await getEventByEventIdAPI(eventId);
      setEvent(eventData);
      setUpdatedEvent({ ...eventData });
    } catch (error) {
      console.error('Error fetching event data:', error);
      navigate('/get-events');
    }
  };

  const getStatusLabel = (statusValue) => {
    const statusOption = statusOptions.find(option => option.value === statusValue);
    return statusOption ? statusOption.label : 'Unknown';
  };

  const handleEdit = () => {
    setEditMode(true);
  };

  const handleSave = async () => {
    try {
      const currentDate = new Date().toISOString();
      const updatedEventData = { ...updatedEvent, updatedDate: currentDate };
      await updateEventAPI(updatedEventData);
      setEvent(updatedEventData);
      setEditMode(false);
    } catch (error) {
      console.error('Error updating event:', error);
    }
  };

  const handleCancel = () => {
    setUpdatedEvent({ ...event });
    setEditMode(false);
  };

  if (!event) {
    return <div>Loading...</div>;
  }

  const formattedDate = format(new Date(event.eventDate), 'dd-MM-yyyy');

  return (
    <div className="event-page-container">
      <div className="event-details">
        <div className="event-detail-item">
          <strong>Event Name:</strong> {editMode ? (
            <input
              type="text"
              value={updatedEvent.eventName}
              onChange={(e) => setUpdatedEvent({ ...updatedEvent, eventName: e.target.value })}
            />
          ) : (
            <span>{event.eventName}</span>
          )}
        </div>
        <div className="event-detail-item">
          <strong>Event Date:</strong> {editMode ? (
            <input
              type="date"
              value={updatedEvent.eventDate || formattedDate}
              onChange={(e) => setUpdatedEvent({ ...updatedEvent, eventDate: e.target.value })}
            />
          ) : (
            <span>{formattedDate}</span>
          )}
        </div>
        <div className="event-detail-item">
          <strong>Status:</strong> {editMode ? (
            <select
              value={updatedEvent.status}
              onChange={(e) => setUpdatedEvent({ ...updatedEvent, status: e.target.value })}
            >
              {statusOptions.map(option => (
                <option key={option.value} value={option.value}>{option.label}</option>
              ))}
            </select>
          ) : (
            <span>{getStatusLabel(event.status)}</span>
          )}
        </div>
        {editMode ? (
          <>
            <button onClick={handleSave}>Save</button>
            <button onClick={handleCancel}>Cancel</button>
          </>
        ) : (
          <button onClick={handleEdit}>Edit</button>
        )}
      </div>
    </div>
  );
}

export default Event;
