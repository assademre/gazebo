export interface EventSearch {
    currency: string;
    eventName: string;
    budget: number;
    place: string;
    status: string;
    createdDate: string;
    updatedDate: string;
    eventDate: string;
  }

  export interface TaskSearch {
    eventId: number;
    taskName: string;
    currency: string;
    eventName: string;
    budget: number;
    place: string;
    status: string;
    ownerId: number;
    createdDate: string;
    updatedDate: string;
    taskDate: string;
  }