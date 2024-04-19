import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { getTaskByTaskIdAPI } from '../../api';
import statusOptions from '../../helpers/statusOptions';
import { format } from 'date-fns';
import "./Task.css";
import Layout from '../../NavigationBar/Layout';

function Task() {
  const { t } = useTranslation();
  const { taskId } = useParams();
  const [task, setTask] = useState(null);
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
      const taskData = await getTaskByTaskIdAPI(taskId);
      setTask(taskData);
    } catch (error) {
      console.error('Error fetching task data:', error);
      navigate('/get-tasks');
    }
  };

  if (!task) {
    return <div>{t('loading')}</div>;
  }

  const formattedDate = format(new Date(task.taskDate), 'dd-MM-yyyy');

  return (
    <Layout>
      <div className="task-page-container">
        <div className="task-details">
          <div className="task-detail-item">
            <strong>{t('taskName')}:</strong> <span>{task.taskName}</span>
          </div>
          <div className="task-detail-item">
            <strong>{t('eventName')}:</strong>
            <span>
              <Link to={`/event/${task.eventId}`} style={{ color: 'black' }}>{task.eventName}</Link>
            </span>
          </div>
          <div className="task-detail-item">
            <strong>{t('taskDate')}:</strong> <span>{formattedDate}</span>
          </div>
          <div className="task-detail-item">
            <strong>{t('status')}:</strong> <span>{t(getStatusLabel(task.status))}</span>
          </div>
          <div className="button-container">
            <Link to={`/edit-task/${taskId}`}>
              <button>{t('edit')}</button>
            </Link>
            <button onClick={() => navigate(-1)}>{t('cancel')}</button>
          </div>
        </div>
      </div>
    </Layout>
  );
}

export default Task;
