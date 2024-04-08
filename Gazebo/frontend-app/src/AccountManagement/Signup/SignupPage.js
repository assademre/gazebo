import React from 'react';
import SignupForm from './Signupform';
import './SignupPage.css'

const SignupPage = () => {
  const handleSubmit = (formData) => {
    console.log('Form data:', formData);
  };

  return (
    <div className='signup-body'>
      <SignupForm onSubmit={handleSubmit} />
    </div>
  );
};

export default SignupPage;
