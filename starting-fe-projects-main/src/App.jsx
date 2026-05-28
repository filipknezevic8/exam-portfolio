import React, { useState } from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import UserContext from './UserContext';
import Header from './core/layout/Header';
import Home from './core/layout/Home';
import Login from './features/users/Login';
import Register from './features/users/Register';
import Profile from './features/users/Profile';
import UserList from './features/users/UserList';
import MyProjects from './features/projects/MyProjects';
import './core/global.scss';

const App = () => {
  let token = localStorage.getItem('token');
  if(token) token = JSON.parse(atob(token.split('.')[1]));
  const [user, setUser] = useState(token);

  return (
    <UserContext.Provider value={{ user, setUser }}>
      <BrowserRouter>
        <Header />
        <div className="container">
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            <Route path="/profile" element={<Profile />} />
            <Route path="/users" element={<UserList />} />
            <Route path="/my-projects" element={<MyProjects />} />
          </Routes>
        </div>
      </BrowserRouter>
    </UserContext.Provider>
  );
}

export default App;
