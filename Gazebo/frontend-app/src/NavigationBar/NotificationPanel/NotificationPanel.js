import React, { useState, useEffect } from 'react';
import { getNotificationsAPI, updateNotificationAPI } from '../../api';
import { useNavigate } from 'react-router-dom';
import './NotificationPanel.css';

const NotificationPanel = ({ setNotifications: updateNotifications }) => {
    const [notifications, setNotificationsLocal] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        getNotificationsAPI()
            .then(data => {
                const unreadNotifications = data.filter(notification => !notification.isRead);
                setNotificationsLocal(unreadNotifications);
                updateNotifications(unreadNotifications);
            })
            .catch(error => {
                console.error('Error fetching notifications:', error);
            });
    }, [updateNotifications]);

    const handleNotificationClick = (notificationId) => {
        updateNotificationAPI(notificationId)
            .then(() => {
                navigate('/notifications');
            })
            .catch(error => {
                console.error('Error updating notification status:', error);
            });
    };

    return (
        <div className="notification-panel">
            <div className="notification-header"></div>
            <div className="notifications-list">
                {notifications.length > 0 ? (
                    notifications.map(notification => (
                        <div key={notification.notificationId} className="notification-item" onClick={() => handleNotificationClick(notification.notificationId)}>
                            <p>{notification.body}</p>
                        </div>
                    ))
                ) : (
                    <p onClick={() => navigate('/notifications')}>No new notification</p>
                )}
            </div>
        </div>
    );
};

export default NotificationPanel;
