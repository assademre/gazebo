import axios from "axios";

const api = axios.create({
  headers: {
    "Content-Type": "application/json"
  }
});

const getToken = () => {
  return localStorage.getItem('token');
};

api.interceptors.request.use(config => {
  const token = getToken();
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
}, error => {
  return Promise.reject(error);
});

export const createEventAPI = async (data) => {
  try {
    const response = await api.post("/api/event", data);
    return response.data;
  } catch (error) {
    throw error.response.data || error.message;
  }
};

export const createTaskAPI = async (data) => {
    try {
      const response = await api.post("/api/eventtask", data);
      return response.data;
    } catch (error) {
      throw error.response.data || error.message;
    }
  };

export const fetchTasksAPI = async () => {
try {
    const response = await api.get(`/api/eventtask/alltasks`);
    return response.data;
} catch (error) {
    throw error.response.data || error.message;
    }
  };

export const getUsernameAPI = async () => {
    try {
        const response = await api.get(`/api/user/profile`);
        return response.data;
    } catch (error) {
        throw error.response.data || error.message;
    }
  };

export const getUsersAPI = async () => {
  try {
      const response = await api.get(`/api/user/userlist`);
      return response.data;
  } catch (error) {
      throw error.response.data || error.message;
  }
};

export const setUserAsAdminAPI = async (eventId, userId) => {
  try {
      const response = await api.put(`/api/event/${eventId}/${userId}/admin`);
      return response.data;
  } catch (error) {
      throw error.response.data || error.message;
  }
};

export const getEventByEventIdAPI = async (eventId) => {
  try {
      const response = await api.get(`/api/event/${eventId}/event`);
      return response.data;
  } catch (error) {
      throw error.response.data || error.message;
  }
};

export const getTaskByTaskIdAPI = async (taskId) => {
  try {
      const response = await api.get(`/api/eventtask/${taskId}/task`);
      return response.data;
  } catch (error) {
      throw error.response.data || error.message;
  }
};

export const getEventByUserIdAPI = async () => {
  try {
      const response = await api.get(`/api/event/createdevents`);
      return response.data;
  } catch (error) {
      throw error.response.data || error.message;
  }
};

export const getAdminEventByUserIdAPI = async () => {
  try {
      const response = await api.get(`/api/event/myevents`);
      return response.data;
  } catch (error) {
      throw error.response.data || error.message;
  }
};

export const updateEventAPI = async (data) => {
  try {
    const response = await api.put('/api/event', data);
    return response.data;
  } catch (error) {
    throw new Error(error.response.data.message || 'Failed to update');
  }
};

export const updateTaskAPI = async (data) => {
  try {
    const response = await api.put('/api/eventtask', data);
    return response.data;
  } catch (error) {
    throw new Error(error.response.data.message || 'Failed to update');
  }
};

//Notification API Calls
export const updateNotificationAPI = async (notificationId) => {
  try {
    const response = await api.put(`/api/notification`, notificationId);
    return response.data;
  } catch (error) {
    throw new Error(error.response.data.message || 'Failed while updating notification');
  }
};

export const getNotificationsAPI = async () => {
  try {
    const response = await api.get(`/api/notification`);
    return response.data;
  } catch (error) {
    throw new Error(error.response.data.message || 'Failed while getting notifications');
  }
};

// Comments API Calls

export const addCommentAPI = async (comment) => {
  try {
    const response = await api.post(`/api/comment`, comment);
    return response.data;
  } catch (error) {
    throw new Error(error.response.data.message || 'Failed while adding the comment');
  }
};

export const getCommentsAPI = async (postGroupTypeId, postGroupId, pageNumber, pageSize) => {
  try {
    const response = await api.get(`/api/comment/postgrouptypeid=${postGroupTypeId}&postgroupid=${postGroupId}&pagenumber=${pageNumber}&pagesize=${pageSize}`);
    return response.data;
  } catch (error) {
    throw new Error(error.response.data.message || 'Failed while getting comments');
  }
};

// Friendship API Calls

export const getFriendsAPI = async () => {
  try {
    const response = await api.get(`/api/friendship`);
    return response.data;
  } catch (error) {
    throw new Error(error.response.data.message || 'Failed while getting friends');
  }
};

export const getFriendshipRequestAPI = async () => {
  try {
    const response = await api.get(`/api/friendship/friendshiprequests`);
    return response.data;
  } catch (error) {
    throw new Error(error.response.data.message || 'Failed while getting friendship requests');
  }
};

export const sendFriendshipRequestAPI = async (receiverId) => {
  try {
    const response = await api.put(`/api/friendship/request/${receiverId}`);
    return response.data;
  } catch (error) {
    throw new Error(error.response.data.message || 'Failed while sending a friendship request');
  }
};

export const respondFriendshipRequestAPI = async (senderId, responseId) => {
  try {
    const response = await api.put(`/api/friendship/response/${senderId}/${responseId}`);
    return response.data;
  } catch (error) {
    throw new Error(error.response.data.message || 'Failed while responding to a friendship request');
  }
};

export const deleteFriendAPI = async (friendId) => {
  try {
    const response = await api.delete(`/api/friendship/${friendId}`);
    return response.data;
  } catch (error) {
    throw new Error(error.response.data.message || 'Failed while deleting a friend');
  }
};

// User Access API Calls
export const checkUsernameAvailabilityAPI = async (username) => {
  try {
    const response = await api.get(`/api/useraccess/usernameavailability/${username}`);
    return response.data;
  } catch (error) {
    throw new Error(error.response.data.message || 'Username is taken');
  }
};

export const signupAPI = async (formData) => {
  try {
    const response = await api.post('/api/useraccess/signup', formData);
    return response.data;
  } catch (error) {
    throw new Error(error.response.data.message || 'Failed to signup');
  }
};

export const loginAPI = async (username, password) => {
  try {
      const response = await api.post(`/api/useraccess/login`, {username: username, password: password});
      return response;
  } catch (error) {
      throw error.response.data || error.message;
  }
};

export const logoutAPI = async () => {
  try {
    const response = await api.post('/api/useraccess/logout');
    return response.data;
  } catch (error) {
    throw new Error(error.response.data.message || 'Failed to logout');
  }
};

export default api;