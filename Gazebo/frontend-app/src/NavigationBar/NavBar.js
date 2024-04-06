import React from 'react';
import { Link } from 'react-router-dom';
import image from "../logo.png";
import "./NavBar.css";
import Logout from '../Logout/Logout';

const NavBar = () => {
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
        </nav>
    );
};

export default NavBar;