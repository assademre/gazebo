import React from 'react'
import "./Card.css";
import { TaskSearch } from '../../events';

type Props = {
    cardName : string;
    dueDate : string;
    taskStatus: string;
    budget: string;
    eventName: string;
}

const Card: React.FC<Props> = ({cardName, dueDate, taskStatus, budget, eventName}: Props): JSX.Element => {
  return (
  <div className="card">
    <img
    src="https://htmlcolorcodes.com/assets/images/colors/turquoise-color-solid-background-1920x1080.png"
    alt='eventLogo'/>
    <div className='details'>
        <h2>{cardName}</h2>
        <p>{dueDate}</p>
        <p>{taskStatus}</p>
        <p>{budget}</p>
        <p>{eventName}</p>
        {/* <p>{searchResult.createdDate}</p> */}
    </div>
    {/* <p className='info'>
        Lorem ipsum dolor, sit amet consectetur adipisicing elit. Repudiandae eveniet facere earum sed nam. Laboriosam dolorum at nam, modi necessitatibus 
        debitis natus nobis id. Voluptas ut voluptate voluptates ab suscipit.
    </p> */}
  </div>);  
}

export default Card 