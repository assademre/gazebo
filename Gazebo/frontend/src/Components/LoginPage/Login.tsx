import React, { useState } from 'react';
import './Login.css'; 
import logo from '../assets/logo.png';

const Login: React.FC = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  
  const handleLogin = () => {
    
    console.log('Logging in with:', { username, password });
  };

  return (
    <div className="login-container">
      <div className="logo-container">
        <img src={logo} alt="Logo" className="logo-image" />
      </div>
      <h1>Login</h1>
      <form onSubmit={(e) => { e.preventDefault(); handleLogin(); }}>
        <input
          type="text"
          placeholder="Username"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          required
          title="Username cannot be empty"
        />
        <input
          type="password"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
          title="Password cannot be empty"
        />
        <button type="submit">Login</button>
      </form>
      <div className="signup-link">
        <span className="signup-text">Sign Up</span>
      </div>
      <div className="forgot-password">
        <span className="forgot-password-text">Forgot password?</span>
      </div>
    </div>
  );
};

export default Login;
