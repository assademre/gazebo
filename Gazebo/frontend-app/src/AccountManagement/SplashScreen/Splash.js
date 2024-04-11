import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './Splash.css';
import logo from "../../logo_black.png";

const Splash = () => {
    const navigate = useNavigate();
    const [typedText, setTypedText] = useState('');
    const fullText = `Nestled in a bustling city, "The Serene Bean" cafe offers cozy ambiance, aromatic coffee, and delicious pastries. Patrons enjoy quiet moments or lively conversations amidst the hustle and bustle. It's a sanctuary where worries fade and friendships flourish over steaming cups of coffee.`;
    const [index, setIndex] = useState(0);

    useEffect(() => {
        const typingInterval = setInterval(() => {
            if (index < fullText.length) {
                setTypedText((prev) => prev + fullText[index]);
                setIndex((prev) => prev + 1);
            }
        }, 20);

        return () => clearInterval(typingInterval);
    }, [index, fullText]);

    const handleLoginClick = () => {
        navigate('/login');
    };

    const handleSignupClick = () => {
        navigate('/signup');
    };

    return (
        <div className="splash-screen">
            <img src={logo} alt="Logo" className="logo-splash" />
            <div className="splash-content">
                <p className='text typing-animation'>{typedText}</p>
                <div className="button-container">
                    <button className="login-button" onClick={handleLoginClick}>Login</button>
                    <button className="signup-button" onClick={handleSignupClick}>Signup</button>
                </div>
            </div>
        </div>
    );
};

export default Splash;
