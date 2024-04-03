import React, { useEffect, useState } from "react";
import { getEventByUserIdAPI } from "../api";
import "./GetEvents.css";
import { useNavigate } from "react-router-dom";
import statusOptions from "../helpers/statusOptions";
import currencySymbols from "../helpers/currencySymbols";

function GetEvents() {
  const [events, setEvents] = useState([]);
  const [sortConfig, setSortConfig] = useState({ key: null, direction: null });

  const navigate = useNavigate();

  const handleBack = () => {
    navigate(-1);
  };

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const userId = 1;
      const eventsData = await getEventByUserIdAPI(userId);
      setEvents(eventsData);
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
    <div className="get-events-page">

      <h2>My Events</h2>
      <table className="tasks-table">
        <thead>
          <tr>
            <th onClick={() => handleSort('eventName')}>
              Event Name {sortConfig.key === 'eventName' && (
                sortConfig.direction === 'asc' ? '▼' : '▲'
              )}
            </th>
            <th onClick={() => handleSort('eventDate')}>
              Event Date {sortConfig.key === 'eventDate' && (
                sortConfig.direction === 'asc' ? '▼' : '▲'
              )}
            </th>
            <th>Status</th>
            <th onClick={() => handleSort('budget')}>
              Budget {sortConfig.key === 'budget' && (
                sortConfig.direction === 'asc' ? '▼' : '▲'
              )}
            </th>
            <th>Event Type</th>
          </tr>
        </thead>
        <tbody>
          {sortedTasks.map(event => (
            <tr key={event.eventId}>
              <td>{event.eventName}</td>
              <td>{formatISODate(event.eventDate)}</td>
              <td>{getStatusLabel(event.status)}</td>
              <td>{event.budget}{getCurrencyLabel(event.currency)}</td>
              <td>{event.eventType}</td>
            </tr>
          ))}
        </tbody>
      </table>

      <button className="back-to-main-button" onClick={handleBack}>Back to Main Page</button>
      
    </div>
  );
}
  
export default GetEvents;