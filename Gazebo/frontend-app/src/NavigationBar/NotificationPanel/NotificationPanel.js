import React from 'react';
import "./NotificationPanel.css";

const NotificationPanel = () => {
    const notifications = [
        { id: 1, message: "Notification 1" },
        { id: 2, message: "Notification 2" },
        { id: 3, message: "Notification 3" },
    ];

    return (
        <div className="notification-panel">
            <div className="notification-header">
            </div>
            <div className="notifications-list">
                {notifications.map(notification => (
                    <div key={notification.id} className="notification-item">
                        <p>{notification.message}</p>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default NotificationPanel;
