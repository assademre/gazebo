export {};

export class StatusConverter {
    private static statuses: Record<string, string> = {
      NotStarted: 'Not Started',
      InProgress: 'In Progress',
      Completed: 'Completed',
      Cancelled: 'Cancelled',
    };
  
    static getStatus(status: string): string {
      const code = status.toUpperCase();
      
      if (this.statuses.hasOwnProperty(status)) {
        return this.statuses[status];
      } else {
        return status;
      }
    }
  }
  