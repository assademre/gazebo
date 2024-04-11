import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBell } from '@fortawesome/free-solid-svg-icons';
import NotificationPanel from './NotificationPanel/NotificationPanel';
import image from "../logo.png";
import { getNotificationsAPI } from '../api';
import "./NavBar.css";

const NavBar = () => {
    const [showNotifications, setShowNotifications] = useState(false);
    const [notifications, setNotifications] = useState([]);

    useEffect(() => {
        getNotifications();
    }, []);

    const getNotifications = () => {
        getNotificationsAPI()
            .then(data => {
                const unreadNotifications = data.filter(notification => !notification.isRead);
                setNotifications(unreadNotifications);
            })
            .catch(error => {
                console.error('Error fetching notifications:', error);
            });
    };

    const toggleNotifications = () => {
        setShowNotifications(!showNotifications);
    };

    return (
        <nav className='navbar-container'>
            <Link to="/main-page">
                <img src={image} className='logo' alt="logo" />
            </Link>
            <ul className='nav__links'>
                <li><Link to="/get-events">My Events</Link></li>
                <li><Link to="/get-tasks">My Tasks</Link></li>
                <li><Link to="/logout">Logout</Link></li>
            </ul>
            <div className="notification-bell" onClick={toggleNotifications}>
                <FontAwesomeIcon icon={faBell} />
                {showNotifications && <NotificationPanel notifications={notifications} />}
                {notifications.length > 0 && <div className="notification-badge">{notifications.length}</div>}
            </div>
        </nav>
    );
};

export default NavBar;