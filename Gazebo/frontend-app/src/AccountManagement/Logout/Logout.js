import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { logoutAPI } from '../../api';

function Logout() {
  const navigate = useNavigate();

  useEffect(() => {
    const handleLogout = async () => {
      try {
        await logoutAPI();
        localStorage.removeItem('token');
        localStorage.removeItem('userId');
        localStorage.removeItem('username');
        navigate('/login');
      } catch (error) {
        console.error('Logout failed', error)
      }
    };

    handleLogout();
  }, [navigate]);

  return null;
}

export default Logout;
