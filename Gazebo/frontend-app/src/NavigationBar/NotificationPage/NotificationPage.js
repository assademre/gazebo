import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { getNotificationsAPI } from '../../api';
import './NotificationPage.css';
import Layout from '../Layout';

const NotificationPage = () => {
  const { t } = useTranslation();
  const [notifications, setNotifications] = useState([]);

  useEffect(() => {
    getNotificationsAPI()
      .then(data => {
        setNotifications(data);
      })
      .catch(error => {
        console.error('Error fetching notifications:', error);
      });
  }, []);

  const groupNotificationsByDate = () => {
    const groupedNotifications = {};
    notifications.forEach(notification => {
      const date = new Date(notification.createdDate);
      const dateString = date.toDateString();
      const today = new Date();
      const yesterday = new Date(today);
      yesterday.setDate(today.getDate() - 1);
      let formattedDate = dateString;
      if (date.toDateString() === today.toDateString()) {
        formattedDate = t('today');
      } else if (date.toDateString() === yesterday.toDateString()) {
        formattedDate = t('yesterday');
      }
      if (groupedNotifications[formattedDate]) {
        groupedNotifications[formattedDate].push(notification);
      } else {
        groupedNotifications[formattedDate] = [notification];
      }
    });
    return Object.entries(groupedNotifications).reverse();
  };

  return (
    <Layout>
        <div className="notification-page">
      <h1>{t('Notifications')}</h1>
      {groupNotificationsByDate().map(([date, notifications]) => (
        <div key={date} className="notification-group">
          <h2 className="notification-group-date">{date}</h2>
          <ul className="notification-list">
            {notifications.map(notification => (
              <li key={notification.id} className="notification-item">
                {notification.body}
              </li>
            ))}
          </ul>
        </div>
      ))}
    </div>
    </Layout>
  );
};

export default NotificationPage;
