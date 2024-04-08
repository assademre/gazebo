import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { signupAPI } from '../../api';

const SignupForm = () => {
  const [formData, setFormData] = useState({
    username: '',
    password: '',
    name: '',
    surname: '',
    email: '',
    phoneNumber: ''
  });
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);
    try {
      await signupAPI(formData);
      setIsLoading(false);
      navigate('/login', { state: { successMessage: 'Signup successful! Redirecting to login page...' } });
    } catch (error) {
      setIsLoading(false);
      setError(error.message);
      console.error('Signup Error:', error);
    }
  };

  return (
    <div>
      <h2>Signup</h2>
      <form className='signup-form' onSubmit={handleSubmit}>
        <div>
          <label>Username:</label>
          <input type="text" name="username" value={formData.username} onChange={handleChange} required />
        </div>
        <div>
          <label>Password:</label>
          <input type="password" name="password" value={formData.password} onChange={handleChange} required />
        </div>
        <div>
          <label>Name:</label>
          <input type="text" name="name" value={formData.name} onChange={handleChange} required />
        </div>
        <div>
          <label>Surname:</label>
          <input type="text" name="surname" value={formData.surname} onChange={handleChange} required />
        </div>
        <div>
          <label>Email:</label>
          <input type="email" name="email" value={formData.email} onChange={handleChange} required />
        </div>
        <div>
          <label>Phone Number:</label>
          <input type="tel" name="phoneNumber" value={formData.phoneNumber} onChange={handleChange} required />
        </div>
        <button type="submit" disabled={isLoading}>{isLoading ? 'Signing up...' : 'Signup'}</button>
      </form>
      <Link className='login-link' to="/login">Already have an account? Login</Link>
      {error && <div className="error-message">{error}</div>}
    </div>
  );
};

export default SignupForm;
