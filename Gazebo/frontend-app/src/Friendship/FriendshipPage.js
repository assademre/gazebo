import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { getFriendsAPI, getFriendshipRequestAPI, respondFriendshipRequestAPI, deleteFriendAPI } from '../api';
import './FriendshipPage.css';
import Layout from '../NavigationBar/Layout';

function FriendshipPage() {
  const {t} = useTranslation();
  const [friends, setFriends] = useState([]);
  const [friendshipRequests, setFriendshipRequests] = useState([]);
  const [error, setError] = useState('');

  useEffect(() => {
    fetchFriends();
    fetchFriendshipRequests();
  }, []);

  const fetchFriends = async () => {
    try {
      const friendsData = await getFriendsAPI();
      setFriends(friendsData);
    } catch (error) {
      setError(error.message);
    }
  };

  const fetchFriendshipRequests = async () => {
    try {
      const requestsData = await getFriendshipRequestAPI();
      setFriendshipRequests(requestsData);
    } catch (error) {
      setError(error.message);
    }
  };

  const handleRespondRequest = async (senderId, responseId) => {
    try {
      await respondFriendshipRequestAPI(senderId, responseId);
      fetchFriends();
      fetchFriendshipRequests();
    } catch (error) {
      setError(error.message);
    }
  };

  const handleDeleteFriend = async (friendId) => {
    try {
      await deleteFriendAPI(friendId);
      fetchFriends();
    } catch (error) {
      setError(error.message);
    }
  };

  return (
    <Layout>
        <div className="friendship-page">
      <h1>{t("friendshipPage")}</h1>
      {error && <p className="error">{error}</p>}
      <div className="friendship-requests">
        <h2>{t("friendshipRequests")}</h2>
        {friendshipRequests.length === 0 ? (
          <p>{t("noFriendshipRequests")}</p>
        ) : (
          friendshipRequests.map((request) => (
            <div key={request.id} className="request">
              <p>{request.friendName} {t("sentYouFriendshipRequest")}</p>
              <button onClick={() => handleRespondRequest(request.friendId, 2)}>{t("accept")}</button>
              <button onClick={() => handleRespondRequest(request.friendId, 3)}>{t("decline")}</button>
            </div>
          ))
        )}
      </div>
      <div className="friends-list">
        <h2>{t("friends")}</h2>
        {friends.length === 0 ? (
          <p>{t("noFriends")}</p>
        ) : (
          friends.map((friend) => (
            <div key={friend.id} className="friend">
              <p>{friend.friendName}</p>
              <button onClick={() => handleDeleteFriend(friend.id)}>{t("delete")}</button>
            </div>
          ))
        )}
      </div>
    </div>
    </Layout>
  );
}

export default FriendshipPage;
