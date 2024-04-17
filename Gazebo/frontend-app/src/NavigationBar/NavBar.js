import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBell } from '@fortawesome/free-solid-svg-icons';
import NotificationPanel from './NotificationPanel/NotificationPanel';
import image from "../logo.png";
import { getNotificationsAPI } from '../api';
import "./NavBar.css";
import i18n from '../locales/i18n';

const NavBar = () => {
    const { t } = useTranslation();
    const [showNotifications, setShowNotifications] = useState(false);
    const [notifications, setNotifications] = useState([]);
    const [selectedLanguage, setSelectedLanguage] = useState(
        localStorage.getItem('language') || 'en'
    );

    useEffect(() => {
        getNotifications();
    }, []);

    useEffect(() => {
        localStorage.setItem('language', selectedLanguage);
    }, [selectedLanguage]);

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

    const handleLanguageChange = (language) => {
        i18n.changeLanguage(language);
        setSelectedLanguage(language);
    };
    

    return (
        <nav className='navbar-container'>
            <Link to="/main-page">
                <img src={image} className='logo' alt="logo" />
            </Link>
            <ul className='nav__links'>
                <li><Link to="/get-events">{t('myEvents')}</Link></li>
                <li><Link to="/get-tasks">{t('myTasks')}</Link></li>
                <li><Link to="/logout">{t('logout')}</Link></li>
            </ul>
            <div className="language-selector">
                <select value={selectedLanguage} onChange={(e) => handleLanguageChange(e.target.value)}>
                    <option value="en">{t('english')}</option>
                    <option value="de">{t('german')}</option>
                    <option value="tr">{t('turkish')}</option>
                    <option value="pl">{t('polish')}</option>
                </select>
            </div>
            <div className="notification-bell" onClick={toggleNotifications}>
                <FontAwesomeIcon icon={faBell} />
                {showNotifications && <NotificationPanel notifications={notifications} />}
                {notifications.length > 0 && <div className="notification-badge">{notifications.length}</div>}
            </div>
        </nav>
    );
};

export default NavBar;
