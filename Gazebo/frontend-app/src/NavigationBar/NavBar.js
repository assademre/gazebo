import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBell } from '@fortawesome/free-solid-svg-icons';
import NotificationPanel from './NotificationPanel/NotificationPanel';
import image from "../logo.png";
import "./NavBar.css";

const NavBar = () => {
    const [showNotifications, setShowNotifications] = useState(false);

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
                
            </div>
            {showNotifications && <NotificationPanel />}
        </nav>
    );
};

export default NavBar;
