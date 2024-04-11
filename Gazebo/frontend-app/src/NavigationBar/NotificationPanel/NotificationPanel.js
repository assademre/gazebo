import React, { useState, useEffect } from 'react';
import "./NotificationPanel.css";
import { getNotificationsAPI, updateNotificationAPI  } from '../../api';

const NotificationPanel = ({ setNotifications: updateNotifications }) => {
    const [notifications, setNotificationsLocal] = useState([]);

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
        console.log('Notification clicked:', notificationId);
        updateNotificationAPI(notificationId)
            .then(() => {
                setNotificationsLocal(prevNotifications => prevNotifications.filter(notification => notification.notificationId !== notificationId));
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
                        <a key={notification.notificationId} href="#" className="notification-item" onClick={() => handleNotificationClick(notification.notificationId)}>
                            <p>{notification.body}</p>
                        </a>
                    ))
                ) : (
                    <p>No new notification</p>
                )}
            </div>
        </div>
    );
};

export default NotificationPanel;
