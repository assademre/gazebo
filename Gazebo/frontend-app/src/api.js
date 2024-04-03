import axios from "axios";

const api = axios.create({
  headers: {
    "Content-Type": "application/json",
  },
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

export const fetchTasksAPI = async (userId) => {
try {
    const response = await api.get(`/api/event-task/${userId}/all-tasks`);
    return response.data;
} catch (error) {
    throw error.response.data || error.message;
}
};

export const getUsernameAPI = async (userId) => {
    try {
        const response = await api.get(`/api/user/${userId}/profile`);
        return response.data;
    } catch (error) {
        throw error.response.data || error.message;
    }
    };

export default api;

