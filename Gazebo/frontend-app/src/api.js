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
      const response = await api.post("/api/event-task", data);
      return response.data;
    } catch (error) {
      throw error.response.data || error.message;
    }
  };

export const fetchTasksAPI = async () => {
try {
    const response = await api.get(`/api/event-task/all-tasks`);
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
      const response = await api.get(`/api/user/user-list`);
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
      const response = await api.get(`/api/event-task/${taskId}/task`);
      return response.data;
  } catch (error) {
      throw error.response.data || error.message;
  }
};

export const getEventByUserIdAPI = async () => {
  try {
      const response = await api.get(`/api/event/created-events`);
      return response.data;
  } catch (error) {
      throw error.response.data || error.message;
  }
};

export const getAdminEventByUserIdAPI = async () => {
  try {
      const response = await api.get(`/api/event/my-events`);
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
    const response = await api.put('/api/event-task', data);
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

// User Access API Calls
export const checkUsernameAvailabilityAPI = async (username) => {
  try {
    const response = await api.get(`/api/user-access/username-availability/${username}`);
    return response.data;
  } catch (error) {
    throw new Error(error.response.data.message || 'Username is taken');
  }
};

export const signupAPI = async (formData) => {
  try {
    const response = await api.post('/api/user-access/signup', formData);
    return response.data;
  } catch (error) {
    throw new Error(error.response.data.message || 'Failed to signup');
  }
};

export const loginAPI = async (username, password) => {
  try {
      const response = await api.post(`/api/user-access/login`, {username: username, password: password});
      return response;
  } catch (error) {
      throw error.response.data || error.message;
  }
};

export const logoutAPI = async () => {
  try {
    const response = await api.post('/api/user-access/logout');
    return response.data;
  } catch (error) {
    throw new Error(error.response.data.message || 'Failed to logout');
  }
};

export default api;