import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { getEventByEventIdAPI } from '../../api';
import statusOptions from '../../helpers/statusOptions';
import { format } from 'date-fns';
import "./Event.css";
import Layout from '../../NavigationBar/Layout';

function Event() {
  const {t} = useTranslation();
  const { eventId } = useParams();
  const [event, setEvent] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    fetchData();
  }, []);

  const getStatusLabel = (statusValue) => {
    const statusOption = statusOptions.find(option => option.value === statusValue);
    return statusOption ? statusOption.label : 'Unknown';
  };

  const fetchData = async () => {
    try {
      const eventData = await getEventByEventIdAPI(eventId);
      setEvent(eventData);
    } catch (error) {
      console.error('Error fetching event data:', error);
      navigate('/get-events');
    }
  };

  if (!event) {
    return <div>{t('loading')}</div>;
  }

  const formattedDate = format(new Date(event.eventDate), 'dd-MM-yyyy');

  return (
    <Layout>
      <div className="event-page-container">
        <div className="event-details">
          <div className="event-detail-item">
            <strong>{t('eventName')}:</strong> <span>{event.eventName}</span>
          </div>
          <div className="event-detail-item">
            <strong>{t('budget')}:</strong> <span>{event.budget}</span>
          </div>
          <div className="event-detail-item">
            <strong>{t('place')}:</strong> <span>{event.place}</span>
          </div>
          <div className="event-detail-item">
            <strong>{t('eventDate')}:</strong> <span>{formattedDate}</span>
          </div>
          <div className="event-detail-item">
            <strong>{t('status')}:</strong> <span>{t(getStatusLabel(event.status))}</span>
          </div>
          <Link to={`/edit-event/${eventId}`}>
            <button>{t('edit')}</button>
          </Link>
        </div>
      </div>
    </Layout>
  );
}

export default Event;
