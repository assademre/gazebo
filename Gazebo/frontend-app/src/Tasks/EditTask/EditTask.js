import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getTaskByTaskIdAPI, updateTaskAPI } from '../../api';
import { format } from 'date-fns';
import statusOptions from '../../helpers/statusOptions';
import "./EditTask.css";
import Layout from '../../NavigationBar/Layout';

function EditTask() {
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
      navigate('/get-tasks')
    } catch (error) {
      console.error('Error updating task:', error);
    }
  };

  const handleCancel = () => {
    navigate(-1);
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
        <strong>Task Name:</strong> 
            <input
              type="text"
              value={updatedTask.taskName || ''}
              onChange={(e) => setUpdatedTask({ ...updatedTask, taskName: e.target.value })}
            />
          </div>
          <div className="edit-task-detail-item">
          <strong>Task Date:</strong> 
          <input
            type="date"
            value={updatedTask.taskDate ? format(new Date(updatedTask.taskDate), 'yyyy-MM-dd') : ''}
            onChange={(e) => setUpdatedTask({ ...updatedTask, taskDate: e.target.value })}
          />
          </div>
        <div className="event-detail-item">
          <strong>Status:</strong>
            <select
              value={updatedTask.status}
              onChange={(e) => setUpdatedTask({ ...updatedTask, status: e.target.value })}
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

export default EditTask;
