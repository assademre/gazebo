import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { Link, useNavigate } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBell, faUserCircle } from '@fortawesome/free-solid-svg-icons';
import NotificationPanel from './NotificationPanel/NotificationPanel';
import image from "../logo.png";
import { getNotificationsAPI } from '../api';
import "./NavBar.css";
import i18n from '../locales/i18n';

const NavBar = () => {
    const { t } = useTranslation();
    const navigate = useNavigate();
    const [showNotifications, setShowNotifications] = useState(false);
    const [notifications, setNotifications] = useState([]);
    const [selectedLanguage, setSelectedLanguage] = useState(
        localStorage.getItem('language') || 'en'
    );
    const [username, setUsername] = useState('');
    const [showProfileDropdown, setShowProfileDropdown] = useState(false);

    useEffect(() => {
        getNotifications();
        const storedUsername = localStorage.getItem('username');
        if (storedUsername) {
            setUsername(storedUsername);
        }
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

    const toggleProfileDropdown = () => {
        setShowProfileDropdown(!showProfileDropdown);
    };

    const handleLogout = () => {
        navigate('/logout');
    };

    return (
        <nav className='navbar-container'>
            <Link to="/main-page">
                <img src={image} className='logo' alt="logo" />
            </Link>
            <ul className='nav__links'>
                <li><Link to="/get-events">{t('myEvents')}</Link></li>
                <li><Link to="/get-tasks">{t('myTasks')}</Link></li>
            </ul>
            <div className="language-selector">
                <select value={selectedLanguage} onChange={(e) => handleLanguageChange(e.target.value)}>
                    <option value="en">{t('english')}</option>
                    <option value="de">{t('german')}</option>
                    <option value="tr">{t('turkish')}</option>
                    <option value="pl">{t('polish')}</option>
                </select>
            </div>
            <div className="profile-notification-container">
                <div className="notification-bell" onClick={toggleNotifications}>
                    <FontAwesomeIcon icon={faBell} />
                    {showNotifications && <NotificationPanel notifications={notifications} />}
                    {notifications.length > 0 && <div className="notification-badge">{notifications.length}</div>}
                </div>
                <div className="profile-section" onClick={toggleProfileDropdown}>
                    <FontAwesomeIcon icon={faUserCircle} className="profile-icon" />
                    <span className="username">{username}</span>
                    {showProfileDropdown && (
                        <div className="profile-dropdown">
                            <button onClick={handleLogout}>{t('logout')}</button>
                        </div>
                    )}
                </div>
            </div>
        </nav>
    );
};

export default NavBar;
