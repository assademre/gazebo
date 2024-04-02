import React from 'react'
import Card from '../Card/Card';
import { EventSearch, TaskSearch } from '../../events';
import { DateModify } from '../../Helpers/dateParser';
import { CurrencyConverter } from '../../Helpers/currencyHelper';
import { StatusConverter } from '../../Helpers/statusConverter';


interface Props {
  searchResults: TaskSearch[];
}

const CardList: React.FC<Props> = ({searchResults}: Props): JSX.Element => {
  return <>
  {searchResults.length > 0 ? (
      searchResults.map((result) => {
        return <Card cardName={result.taskName}
        dueDate={DateModify.parseDateString(result.taskDate)}
        taskStatus={StatusConverter.getStatus(result.status)}
        budget={`${CurrencyConverter.getCurrencySymbol(result.currency)}${result.budget}`}
        eventName={result.eventName}/>;
      })
    ) : (
      <h1> No results </h1>
    )}
  
  </>
}

export default CardList