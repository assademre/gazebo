import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { useTranslation } from 'react-i18next';
import { fetchTasksAPI, getUsernameAPI } from "../api";
import "./MainPage.css";
import statusOptions from "../helpers/statusOptions";
import currencySymbols from "../helpers/currencySymbols";
import Layout from "../NavigationBar/Layout";

function MainPage() {
  const { t } = useTranslation();
  const [events, setEvents] = useState([]);
  const [sortConfig, setSortConfig] = useState({ key: 'taskDate', direction: 'asc' });
  const [user, setUsername] = useState("");

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const eventsData = await fetchTasksAPI();
      setEvents(eventsData);

      const usernameData = await getUsernameAPI();
      setUsername(usernameData.name);
    } catch (error) {
      console.error('Error fetching data:', error);
    }
  };

  const getStatusLabel = (statusValue) => {
    const statusOption = statusOptions.find(option => option.value === statusValue);
    return statusOption ? statusOption.label : 'Unknown';
  };

  const getCurrencyLabel = (currency) => {
    const currencySymbol = currencySymbols.find(symbol => symbol.value === currency);
    return currencySymbol ? currencySymbol.label : "currency";
  };

  const formatISODate = (dateString) => {
    const dateObj = new Date(dateString);
    const day = dateObj.getDate().toString().padStart(2, '0');
    const month = (dateObj.getMonth() + 1).toString().padStart(2, '0');
    const year = dateObj.getFullYear();
    return `${day}-${month}-${year}`;
  };

  const today = new Date();
  const futureDate = new Date();
  futureDate.setDate(today.getDate() + 7);

  const sortedTasks = [...events].sort((a, b) => {
    if (sortConfig.key === 'taskDate') {
      const dateA = new Date(a.taskDate);
      const dateB = new Date(b.taskDate);
      if (dateA < dateB) {
        return sortConfig.direction === 'asc' ? -1 : 1;
      }
      if (dateA > dateB) {
        return sortConfig.direction === 'asc' ? 1 : -1;
      }
    }
    return 0;
  });

  const filteredTasks = sortedTasks.filter(task => {
    const taskDate = new Date(task.taskDate);
    console.log(today);
    console.log(futureDate);
    return taskDate >= today && taskDate < futureDate && !['cancelled', 'completed'].includes(task.status.toLowerCase());
  });

  return (
    <Layout>
      <div className="main-page">
        <div className="welcome-message">
        {console.log('Translated welcomeMessage:', t('welcomeMessage'))}
          {t('welcomeMessage')} <span className="name">{user}</span>
        </div>

        <div className="button-container">
          <Link to="/create-event" className="button">{t('createEvent')}</Link>
          <Link to="/create-task" className="button">{t('createTask')}</Link>
        </div>
        
        <h2>{t('myUpcomingTasks')}</h2>
        <div className="main-page-table-container">
          <table className="main-page-table">
            <thead>
              <tr>
                <th>{t('taskName')}</th>
                <th>{t('dueDate')}</th>
                <th>{t('status')} </th>
                <th>{t('budget')}</th>
                <th>{t('eventName')} </th>
              </tr>
            </thead>
            </table>
            <div className="main-page-table-body">
            <table className="main-page-table">
            <tbody>
              {filteredTasks.map(task => (
                <tr key={task.taskId}>
                  <td>{task.taskName}</td>
                  <td>{formatISODate(task.taskDate)}</td>
                  <td>{t(getStatusLabel(task.status))}</td>
                  <td>{task.budget}{getCurrencyLabel(task.currency)}</td>
                  <td>{task.eventName || 'Loading...'}</td>
                </tr>
              ))}
            </tbody>
          </table>
          </div>
        </div>
      </div>
    </Layout>
  );
}

export default MainPage;
