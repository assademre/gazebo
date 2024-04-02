import axios from 'axios';
import { EventSearch, TaskSearch } from './events';

interface EventResponse {
  data: EventSearch[];
}

export const getEvents = async (userId: number | undefined) => {
  try {
    const data = await axios.get<EventResponse>(
      `/api/event/${userId}/created-events`
    );
    return data;
  }
  catch(error) {
    if(axios.isAxiosError(error)) {
      console.log("error message: ", error.message);
      return error.message
    }
    else {
      console.log("unexpected error: ", error)
      return "Unexpected error";
    }
  }
};

export const getTasksByUserId = async (userId: number | undefined) => {
  try {
    const data = await axios.get<TaskSearch[]>(
      `/api/event-task/${userId}/all-tasks`
    );
    return data;
  }
  catch(error) {
    if(axios.isAxiosError(error)) {
      console.log("error message: ", error.message);
      return error.message
    }
    else {
      console.log("unexpected error: ", error)
      return "Unexpected error";
    }
  }
};