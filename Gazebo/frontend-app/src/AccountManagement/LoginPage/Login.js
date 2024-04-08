import React, { useState } from 'react';
import { loginAPI } from '../../api';
import { useNavigate } from 'react-router-dom';
import './Login.css';

const Login = ({ onLoginSuccess }) => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();
        setLoading(true);
        setError('');
        try {
            console.log('Logging in...');
            const userData = await loginAPI(username, password);
            const token = userData.data.token;
            const userId = userData.data.userId;
            localStorage.setItem('token', token);
            localStorage.setItem('userId', userId);
            onLoginSuccess();
            navigate('/main-page');
        } catch (error) {
            setError(error.message);
            console.error('Login Error:', error);
        }
        setUsername('');
        setPassword('');
        setLoading(false);
    };

    return (
        <div className="login-container">
            <form className="login-form" onSubmit={handleLogin}>
                <h2>Login</h2>
                {error && <div className="error-message">{error}</div>}
                <div className="input-group">
                    <label>Username</label>
                    <input type="text" value={username} onChange={(e) => setUsername(e.target.value)} required />
                </div>
                <div className="input-group">
                    <label>Password</label>
                    <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} required />
                </div>
                <button type="submit" disabled={loading}>
                    {loading ? 'Logging in...' : 'Login'}
                </button>
                <a href="/signup" className="signup-link">New member?</a>
            </form>
        </div>
    );
};

export default Login;
