import React from 'react';
import { useNavigate } from 'react-router-dom';
import './Splash.css';
import logo from "../../logo_black.png";

const Splash = () => {
    const navigate = useNavigate();

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
                <p className='text'>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce ut urna vel eros euismod malesuada. Nulla facilisi. Integer tempus elit nec sapien convallis, sed faucibus felis bibendum. Suspendisse potenti. Nunc ut libero a justo scelerisque posuere. Sed malesuada sapien at ipsum elementum vestibulum. Nam auctor odio eget velit venenatis fermentum. Integer pretium elit vitae nisi vehicula, a bibendum arcu bibendum. Morbi pulvinar velit nec urna tincidunt, eget eleifend erat varius. Nam vitae libero et justo luctus tristique. Nulla sollicitudin risus
</p>
                <div className="button-container">
                    <button className="login-button" onClick={handleLoginClick}>Login</button>
                    <button className="signup-button" onClick={handleSignupClick}>Signup</button>
                </div>
            </div>
        </div>
    );
};

export default Splash;
