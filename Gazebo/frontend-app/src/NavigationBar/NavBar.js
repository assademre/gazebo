import React from 'react';
import { Link } from 'react-router-dom';
import image from "../logo.png";
import "./NavBar.css";

const NavBar = () => {
    return (
        <nav className='navbar-container'>
            <Link to="/main-page">
                <img src={image} className='logo' alt="logo" />
            </Link>
            <ul className='nav__links'>
                <li><Link to="/get-events">My Events</Link></li>
                <li><Link to="/get-tasks">My Tasks</Link></li>
            </ul>
        </nav>
    );
};

export default NavBar;