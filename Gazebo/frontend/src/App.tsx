import React, { ChangeEvent, SyntheticEvent, useState } from 'react';
import logo from './logo.svg';
import './App.css';
import CardList from './Components/CardList/CardList';
import Search from './Components/Search/Search';
import Login from './Components/LoginPage/Login';
import { TaskSearch } from './events';
import { getTasksByUserId } from './api';

function App() {
  const [search, setSearch] = useState<number>();
  const [searchResult, setSearchResult] = useState<TaskSearch[]>([]);
  const [serverError, setServerError] = useState<string>("");

  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    setSearch(parseInt(e.target.value, 10));
    console.log(e);
  };

  const onClick = async (e: SyntheticEvent) => {
    const result = await getTasksByUserId(search);
    if(typeof result === "string") {
        setServerError(result);
    }
    else if(Array.isArray(result.data)){
      setSearchResult(result.data)
  }
  console.log(searchResult)
};

  return (
    <div className="App">
      <Search onClick={onClick} search={search} handleChange={handleChange}/>
      {serverError && <h1>{serverError}</h1>}
      <CardList searchResults={searchResult}/>
      {/* <Login/> */}
    </div>
  );
}

export default App;
