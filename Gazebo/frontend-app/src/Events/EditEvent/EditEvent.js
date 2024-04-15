import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getEventByEventIdAPI, updateEventAPI } from '../../api';
import { format } from 'date-fns';
import statusOptions from '../../helpers/statusOptions';
import "./EditEvent.css";
import Layout from '../../NavigationBar/Layout';

function EditEvent() {
  const { eventId } = useParams();
  const navigate = useNavigate();

  const [event, setEvent] = useState(null);
  const [updatedEvent, setUpdatedEvent] = useState({});

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const eventData = await getEventByEventIdAPI(eventId);
      console.log("Fetched Event Data:", eventData.eventDate);
      setEvent(eventData);
      setUpdatedEvent({ ...eventData });
    } catch (error) {
      console.error('Error fetching event data:', error);
      navigate('/get-events');
    }
  };

  // const getStatusLabel = (statusValue) => {
  //   const statusOption = statusOptions.find(option => option.value === statusValue);
  //   return statusOption ? statusOption.label : 'Unknown';
  // };

  const handleSave = async () => {
    try {
      const currentDate = new Date().toISOString();
      const updatedEventData = { ...updatedEvent, updatedDate: currentDate };
      await updateEventAPI(updatedEventData);
      setEvent(updatedEventData);
      navigate('/get-events');
    } catch (error) {
      console.error('Error updating event:', error);
    }
  };

  const handleCancel = () => {
    navigate(-1);
  };

  if (!event) {
    return <div>Loading...</div>;
  }

  return (
    <Layout>
      <div className="edit-event-page-container">
        <div className="edit-event-details">
          <div className="edit-event-detail-item">
            <strong>Event Name:</strong> 
            <input
              type="text"
              value={updatedEvent.eventName || ''}
              onChange={(e) => setUpdatedEvent({ ...updatedEvent, eventName: e.target.value })}
            />
          </div>
          <div className="edit-event-detail-item">
          <strong>Event Date:</strong> 
          <input
            type="date"
            value={updatedEvent.eventDate ? format(new Date(updatedEvent.eventDate), 'yyyy-MM-dd') : ''}
            onChange={(e) => setUpdatedEvent({ ...updatedEvent, eventDate: e.target.value })}
          />
          </div>
          <div className="edit-event-detail-item">
            <strong>Status:</strong> 
            <select
              value={updatedEvent.status || ''}
              onChange={(e) => setUpdatedEvent({ ...updatedEvent, status: e.target.value })}
            >
              {statusOptions.map(option => (
                <option key={option.value} value={option.value}>{option.label}</option>
              ))}
            </select>
          </div>
          <div className="button-container">
            <button onClick={handleSave}>Save</button>
            <button onClick={handleCancel}>Cancel</button>
          </div>
        </div>
      </div>
    </Layout>
  );
}

export default EditEvent;
