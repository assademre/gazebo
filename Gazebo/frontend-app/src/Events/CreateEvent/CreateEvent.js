import React, { Fragment, useState } from "react";
import { useTranslation } from 'react-i18next';
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { createEventAPI } from "./../../api";
import "./CreateEvent.css";
import { useNavigate } from "react-router-dom";
import Layout from "../../NavigationBar/Layout";
import currencySymbols from "../../helpers/currencySymbols";

function CreateEvent() {
  const [t] = useTranslation();
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
      console.log(result);
      navigate('/get-events');
    } catch (error) {
      console.log(error);
      alert(error);
    }
  }

  return (
    <Layout>
      <div className="container">
      <div className="title">{t('createANewEvent')}</div>
      <div className="label">{t('eventType')}</div>
      <select className="select-field" value={eventType} onChange={(e) => setEventType(e.target.value)}>
        <option value="Default">{t('default')}</option>
        <option value="Birthday">{t('birthday')}</option>
      </select>

      <div className="label">{t('eventName')}</div>
      <input type="text" className="input-field" value={eventName} onChange={(e) => setEventName(e.target.value)} />

      <div className="label">{t('budget')}</div>
      <input type="number" className="input-field" value={budget} onChange={(e) => setBudget(parseInt(e.target.value))} />

      <select className="select-field" value={currency} onChange={(e) => setCurrency(e.target.value)}>
          <option value="">{t('selectCurrency')}</option>
          {currencySymbols.map((option) => (
            <option key={option.value} value={option.value}>{option.label}</option>
          ))}
        </select>

      <div className="label">{t('place')}</div>
      <input type="text" className="input-field" value={place} onChange={(e) => setPlace(e.target.value)} />

      <div className="label">{t('eventDueDate')}</div>
        <DatePicker selected={eventDate} onChange={(date) => setEventDate(date)} />
        <div className="button-container">
        <button className="button" onClick={handleBack}>{t('backToMainPage')}</button>
        <button className="button" onClick={() => handleSave()}>{t('createEvent')}</button>
        </div>
    </div>
    </Layout>
  );
}

export default CreateEvent;