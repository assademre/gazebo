import React, { useEffect, useState } from "react";
import { fetchTasksAPI, getEventByEventIdAPI } from "../api";
import "./GetTasks.css";
import { useNavigate } from "react-router-dom";
import statusOptions from "../helpers/statusOptions";
import currencySymbols from "../helpers/currencySymbols";
import Layout from "../NavigationBar/Layout";

function GetTasks() {
  const [events, setEvents] = useState([]);
  const [sortConfig, setSortConfig] = useState({ key: null, direction: null });
  const [eventNames, setEventNames] = useState({});

  const navigate = useNavigate();

  const handleBack = () => {
    navigate('/main-page');
  };

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const userId = localStorage.getItem('userId');
      const eventsData = await fetchTasksAPI(userId);
      setEvents(eventsData);
      
      const eventIds = eventsData.map(event => event.eventId);
      for (const eventId of eventIds) {
        const eventName = await fetchEventName(eventId);
        setEventNames(prevState => ({
          ...prevState,
          [eventId]: eventName
        }));
      }
    } catch (error) {
      console.error('Error fetching data:', error);
    }
  };

  const handleSort = (key) => {
    let direction = 'asc';
    if (sortConfig.key === key && sortConfig.direction === 'asc') {
      direction = 'desc';
    }
    setSortConfig({ key, direction });
  };

  const fetchEventName = async (eventId) => {
    try {
      const event = await getEventByEventIdAPI(eventId);
      return event.eventName;
    } catch (error) {
      console.error('Error fetching event name:', error);
      return '';
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

  const sortedTasks = [...events].sort((a, b) => {
    if (sortConfig.key && a[sortConfig.key] && b[sortConfig.key]) {
      if (a[sortConfig.key] < b[sortConfig.key]) {
        return sortConfig.direction === 'asc' ? -1 : 1;
      }
      if (a[sortConfig.key] > b[sortConfig.key]) {
        return sortConfig.direction === 'asc' ? 1 : -1;
      }
    }
    return 0;
  });

  return (
    <Layout>
        <div className="get-events-page">

<h2>My Tasks</h2>
<table className="tasks-table">
  <thead>
    <tr>
      <th onClick={() => handleSort('taskName')}>
        Task Name {sortConfig.key === 'taskName' && (
          sortConfig.direction === 'asc' ? '▼' : '▲'
        )}
      </th>
      <th onClick={() => handleSort('taskDate')}>
        Due Date {sortConfig.key === 'taskDate' && (
          sortConfig.direction === 'asc' ? '▼' : '▲'
        )}
      </th>
      <th>Status</th>
      <th onClick={() => handleSort('budget')}>
        Budget {sortConfig.key === 'budget' && (
          sortConfig.direction === 'asc' ? '▼' : '▲'
        )}
      </th>
      <th onClick={() => handleSort('eventId')}>
        Event Name {sortConfig.key === 'eventId' && (
          sortConfig.direction === 'asc' ? '▼' : '▲'
        )}
      </th>
    </tr>
  </thead>
  <tbody>
    {sortedTasks.map(task => (
      <tr key={task.taskId}>
        <td>{task.taskName}</td>
        <td>{formatISODate(task.taskDate)}</td>
        <td>{getStatusLabel(task.status)}</td>
        <td>{task.budget}{getCurrencyLabel(task.currency)}</td>
        <td>{eventNames[task.eventId] || 'Loading...'}</td>
      </tr>
    ))}
  </tbody>
</table>

<button className="button" onClick={handleBack}>Back to Main Page</button>

</div>
    </Layout>
  );
}
  
export default GetTasks;