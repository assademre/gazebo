import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './Splash.css';
import logo from "../../logo_black.png";

const Splash = () => {
    const navigate = useNavigate();
    const [text, setText] = useState('');
    const textContent = `Nestled in a bustling city, "The Serene Bean" cafe offers cozy ambiance, aromatic coffee, and delicious pastries. Patrons enjoy quiet moments or lively conversations amidst the hustle and bustle. It's a sanctuary where worries fade and friendships flourish over steaming cups of coffee.`;

    const handleLoginClick = () => {
        navigate('/login');
    };

    const handleSignupClick = () => {
        navigate('/signup');
    };

    useEffect(() => {
        let index = 0;
        const interval = setInterval(() => {
            if (index <= textContent.length) {
                setText(textContent.substring(0, index));
                index++;
            } else {
                clearInterval(interval);
            }
        }, 25);

        return () => clearInterval(interval);
    }, []);

    return (
        <div className="splash-wrapper">
            <div className="left-column">
                <div className="splash-content">
                    <p className='splash-text'>{text}</p>
                </div>
            </div>
            <div className="right-column">
                <img src={logo} alt="Logo" className="logo-splash" />
                <div className="splash-button-container">
                    <button className="splash-login-button" onClick={handleLoginClick}>Login</button>
                    <button className="splash-signup-button" onClick={handleSignupClick}>Signup</button>
                </div>
            </div>
        </div>
    );
};

export default Splash;
