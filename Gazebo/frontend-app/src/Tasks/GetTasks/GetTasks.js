import React, { useEffect, useState } from "react";
import { fetchTasksAPI } from "./../../api";
import "./GetTasks.css";
import { Link, useNavigate } from "react-router-dom";
import statusOptions from "./../../helpers/statusOptions";
import currencySymbols from "./../../helpers/currencySymbols";
import Layout from "./../../NavigationBar/Layout";

function GetTasks() {
  const [events, setEvents] = useState([]);
  const [sortConfig, setSortConfig] = useState({ key: null, direction: null });

  const navigate = useNavigate();

  const handleBack = () => {
    navigate('/main-page');
  };

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const eventsData = await fetchTasksAPI();
      setEvents(eventsData);
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
        <div className="task-table-container">
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
                <th></th>
              </tr>
            </thead>
            </table>
            <div className="task-table-body">
            <table className="tasks-table">
            <tbody>
              {sortedTasks.map(task => (
                <tr key={task.taskId}>
                  <td><Link className="task" to={`/task/${task.taskId}`}>{task.taskName}</Link></td>
                  <td>{formatISODate(task.taskDate)}</td>
                  <td>{getStatusLabel(task.status)}</td>
                  <td>{task.budget}{getCurrencyLabel(task.currency)}</td>
                  <td>{task.eventName || 'Loading...'}</td>
                  <td><button onClick={() => navigate(`/edit-task/${task.taskId}`)}>Edit</button></td>
                </tr>
              ))}
            </tbody>
          </table>
          </div>
        </div>
        <button className="button" onClick={handleBack}>Back to Main Page</button>
      </div>
    </Layout>
  );
}

export default GetTasks;
