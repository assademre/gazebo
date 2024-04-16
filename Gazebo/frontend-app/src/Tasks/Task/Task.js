import React, { useState, useEffect } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { getTaskByTaskIdAPI } from '../../api';
import { format } from 'date-fns';
import "./Task.css";
import Layout from '../../NavigationBar/Layout';

function Task() {
  const { taskId } = useParams();
  const [task, setTask] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const taskData = await getTaskByTaskIdAPI(taskId);
      setTask(taskData);
    } catch (error) {
      console.error('Error fetching task data:', error);
      navigate('/get-tasks');
    }
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
            <strong>Task Name:</strong> <span>{task.taskName}</span>
          </div>
          <div className="task-detail-item">
            <strong>Task Date:</strong> <span>{formattedDate}</span>
          </div>
          <div className="task-detail-item">
            <strong>Status:</strong> <span>{task.status}</span>
          </div>
          <Link to={`/edit-task/${taskId}`}>
            <button>Edit</button>
          </Link>
        </div>
      </div>
    </Layout>
  );
}

export default Task;
