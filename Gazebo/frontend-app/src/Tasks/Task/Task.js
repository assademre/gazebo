import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getTaskByTaskIdAPI, updateTaskAPI } from './../../api';
import { format } from 'date-fns';
import statusOptions from '../../helpers/statusOptions';
import "./Task.css";
import Layout from '../../NavigationBar/Layout';

function Task() {
  const { taskId } = useParams();
  const [task, setTask] = useState(null);
  const [editMode, setEditMode] = useState(false);
  const [updatedTask, setUpdatedTask] = useState({});
  const navigate = useNavigate();

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const taskData = await getTaskByTaskIdAPI(taskId);
      setTask(taskData);
      setUpdatedTask({ ...taskData });
    } catch (error) {
      console.error('Error fetching task data:', error);
      navigate('/get-tasks');
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
      const updatedTaskData = { ...updatedTask, updatedDate: currentDate };
      await updateTaskAPI(updatedTaskData);
      setTask(updatedTaskData);
      setEditMode(false);
    } catch (error) {
      console.error('Error updating task:', error);
    }
  };

  const handleCancel = () => {
    setUpdatedTask({ ...task });
    setEditMode(false);
  };

  if (!task) {
    return <div>Loading...</div>;
  }

  const formattedDate = format(new Date(task.taskDate), 'dd-MM-yyyy');

  return (
    <Layout>
      
    <div className="task-page-container">
      <div className="task-details">
        <div className="task-detail-item">
          <strong>Task Name:</strong> {editMode ? (
            <input
              type="text"
              value={updatedTask.taskName}
              onChange={(e) => setUpdatedTask({ ...updatedTask, taskName: e.target.value })}
            />
          ) : (
            <span>{task.taskName}</span>
          )}
        </div>
        <div className="task-detail-item">
          <strong>Task Date:</strong> {editMode ? (
            <input
              type="date"
              value={updatedTask.taskDate || formattedDate}
              onChange={(e) => setUpdatedTask({ ...updatedTask, taskDate: e.target.value })}
            />
          ) : (
            <span>{formattedDate}</span>
          )}
        </div>
        <div className="event-detail-item">
          <strong>Status:</strong> {editMode ? (
            <select
              value={updatedTask.status}
              onChange={(e) => setUpdatedTask({ ...updatedTask, status: e.target.value })}
            >
              {statusOptions.map(option => (
                <option key={option.value} value={option.value}>{option.label}</option>
              ))}
            </select>
          ) : (
            <span>{getStatusLabel(task.status)}</span>
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
    </Layout>
  );
}

export default Task;
