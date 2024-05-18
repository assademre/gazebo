import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { getProfileAPI, updateProfileAPI } from '../api'; 
import Layout from '../NavigationBar/Layout';

const ProfilePage = () => {
  const { userId } = useParams();
  const {t} = useTranslation();
  const [profile, setProfile] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    phoneNumber: '',
    bio: ''
  });

  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const profileData = await getProfileAPI(userId);
        setProfile(profileData);
        setFormData({
          name: profileData.name,
          email: profileData.email,
          phoneNumber: profileData.phoneNumber,
          bio: profileData.bio
        });
        setLoading(false);
      } catch (error) {
        setError(error.message);
        setLoading(false);
      }
    };

    fetchProfile();
  }, [userId]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const updatedProfile = await updateProfileAPI(formData);
      setProfile(updatedProfile);
      alert('Profile updated successfully!');
    } catch (error) {
      setError(error.message);
    }
  };

  if (loading) return <div>{t("loading")}</div>;
  if (error) return <div>{t("error")} {error}</div>;

  return (
    <Layout>
        <div>
      <h1>{t("profilePage")}</h1>
      {profile && (
        <div>
          <h2>{profile.name}</h2>
          <p>{profile.email}</p>
          <p>{profile.phoneNumber}</p>
          <p>{profile.bio}</p>
        </div>
      )}
      <form onSubmit={handleSubmit}>
        <div>
          <label>{t("name")}</label>
          <input
            type="text"
            name="name"
            value={formData.name}
            onChange={handleChange}
          />
        </div>
        <div>
          <label>{t("email")}</label>
          <input
            type="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
          />
        </div>
        <div>
          <label>{t("phoneNumber")}</label>
          <input
            type="text"
            name="phoneNumber"
            value={formData.phoneNumber}
            onChange={handleChange}
          />
        </div>
        <div>
          <label>{t("bio")}</label>
          <textarea
            name="bio"
            value={formData.bio}
            onChange={handleChange}
          />
        </div>
        <button type="submit">{t("updateProfile")}</button>
      </form>
    </div>
    </Layout>
  );
};

export default ProfilePage;
