import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { fetchTasksAPI, getUsernameAPI, getEventByEventIdAPI } from "../api";
import "./MainPage.css";

function MainPage() {
  const [events, setEvents] = useState([]);
  const [sortConfig, setSortConfig] = useState({ key: null, direction: null });
  const [user, setUsername] = useState("");
  const [eventNames, setEventNames] = useState({});

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const userId = 1;
      const eventsData = await fetchTasksAPI(userId);
      setEvents(eventsData);

      const usernameData = await getUsernameAPI(userId);
      setUsername(usernameData.name);

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

  const fetchEventName = async (eventId) => {
    try {
      const eventName = await getEventByEventIdAPI(eventId);
      return eventName;
    } catch (error) {
      console.error('Error fetching event name:', error);
      return '';
    }
  };

  const handleSort = (key) => {
    let direction = 'asc';
    if (sortConfig.key === key && sortConfig.direction === 'asc') {
      direction = 'desc';
    }
    setSortConfig({ key, direction });
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
    <div className="main-page">
      <div className="welcome-message">
        Welcome <span className="name">{user}</span>
      </div>

      <div className="button-container">
        <Link to="/create-task" className="button">Create Task</Link>
        <Link to="/create-event" className="button">Create Event</Link>
      </div>

      <h2>My Upcoming Tasks</h2>
      <table className="events-table">
        <thead>
          <tr>
            <th onClick={() => handleSort('taskName')}>
              Task Name {sortConfig.key === 'taskName' && (
                sortConfig.direction === 'asc' ? '▼' : '▲'
              )}
            </th>
            <th onClick={() => handleSort('taskDate')}>
              Task Date {sortConfig.key === 'taskDate' && (
                sortConfig.direction === 'asc' ? '▼' : '▲'
              )}
            </th>
            <th onClick={() => handleSort('status')}>
              Status {sortConfig.key === 'status' && (
                sortConfig.direction === 'asc' ? '▼' : '▲'
              )}
            </th>
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
              <td>{task.status}</td>
              <td>{task.budget}</td>
              <td>{eventNames[task.eventId] || 'Loading...'}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default MainPage;
