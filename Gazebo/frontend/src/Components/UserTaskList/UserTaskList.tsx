export {};

// // src/components/ActionList.tsx
// import React, { useState, useEffect } from 'react';
// import api from '../../utils/api'; // Import the Axios instance
// import { EventTaskDto } from '../../interfaces';

// const ActionList: React.FC = () => {
//   const [actions, setActions] = useState<EventTaskDto[]>([]);

//   useEffect(() => {
//     const fetchActions = async () => {
//       try {
//         const userId = '1'; 
//         const response = await api.get<EventTaskDto[]>(`/api/event-task/${userId}/all-tasks`);
//         const { data } = response;
//         setActions(data); 
//       } catch (error) {
//         console.error('Error fetching action list:', error);
//       }
//     };

//     fetchActions();
//   }, []); // Empty dependency array for componentDidMount behavior

//   return (
//     <div className="action-list-container">
//       <h1>Action List</h1>
//       <ul>
//         {actions.map((action) => (
//           <li key={action.taskId}>
//             {action.taskName} - {action.status}
//           </li>
//         ))}
//       </ul>
//     </div>
//   );
// };

// export default ActionList;
