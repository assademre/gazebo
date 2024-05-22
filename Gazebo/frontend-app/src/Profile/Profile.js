import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { getProfileAPI, updateProfileAPI, sendFriendshipRequestAPI, deleteFriendAPI } from '../api';
import Layout from '../NavigationBar/Layout';
import './Profile.css';

const ProfilePage = () => {
  const { userId } = useParams();
  const { t } = useTranslation();
  const storedUserId = localStorage.getItem('userId');
  const [profile, setProfile] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [editMode, setEditMode] = useState(false);
  const [formData, setFormData] = useState({
    username: '',
    name: '',
    surname: '',
    email: '',
    phoneNumber: '',
    dateOfBirth: '',
    bio: ''
  });
  const [requestSent, setRequestSent] = useState(false);
  const [friendshipStatus, setFriendshipStatus] = useState(null);

  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const profileData = await getProfileAPI(userId);
        setProfile(profileData);
        setFormData({
          username: profileData.username,
          name: profileData.name,
          surname: profileData.surname,
          email: profileData.email,
          phoneNumber: profileData.phoneNumber,
          dateOfBirth: profileData.dateOfBirth,
          bio: profileData.bio
        });
        setLoading(false);
        if (profileData.friendshipStatus) {
          setFriendshipStatus(profileData.friendshipStatus);
        }
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
      setEditMode(false);
      alert('Profile updated successfully!');
    } catch (error) {
      setError(error.message);
    }
  };

  const handleSendFriendshipRequest = async () => {
    try {
      await sendFriendshipRequestAPI(userId);
      setRequestSent(true);
      alert('Friendship request sent successfully!');
    } catch (error) {
      setError(error.message);
    }
  };

  const handleRemoveFriend = async () => {
    try {
      await deleteFriendAPI(userId);
      setFriendshipStatus(null);
      alert('Friend removed successfully!');
    } catch (error) {
      setError(error.message);
    }
  };

  if (loading) return <div className="loading">{t("loading")}</div>;
  if (error) return <div className="error">{t("error")} {error}</div>;

  return (
    <Layout>
      <div className="profile-container">
        <h1>{t("profilePage")}</h1>
        {profile && (
          <div className="profile-info">
            <h2>{profile.username}</h2>
            <p>{t("name")}: {profile.name}</p>
            <p>{t("surname")}: {profile.surname}</p>
            <p>{t("email")}: {profile.email}</p>
            <p>{t("phoneNumber")}: {profile.phoneNumber}</p>
            <p>{t("bio")}: {profile.bio}</p>
          </div>
        )}
        {storedUserId === userId && !editMode && (
          <button onClick={() => setEditMode(true)}>{t("updateProfile")}</button>
        )}
        {storedUserId !== userId && friendshipStatus !== null && (
          friendshipStatus === 1 ? (
            <button disabled>{t("friendshipRequestPending")}</button>
          ) : friendshipStatus === 2 ? (
            <button onClick={handleRemoveFriend}>{t("removeFriend")}</button>
          ) : (
            <button onClick={handleSendFriendshipRequest}>{t("sendFriendshipRequest")}</button>
          )
        )}
        {editMode && (
          <form className="profile-form" onSubmit={handleSubmit}>
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
            <button type="submit">{t("saveChanges")}</button>
            <button type="button" onClick={() => setEditMode(false)}>{t("cancel")}</button>
          </form>
        )}
      </div>
    </Layout>
  );
};

export default ProfilePage;
