import React, { useContext } from 'react';
import { Link } from 'react-router-dom';
import UserContext from '../../UserContext';
import '../../core/global.scss';
import './layout.scss';

const Home = () => {
  const { user } = useContext(UserContext);

  return (
    <div className="home-container">
      <h1>Dobrodošli!</h1>
      
      {user ? (
        <div>
          <p className="welcome-text">
            Zdravo, <strong>{user.sub}</strong>! 
            Uspešno si prijavljen/a.
          </p>
        </div>
      ) : (
        <div>
          <p className="welcome-text">
            Prijavi se da pristupiš funkcionalnostima.
          </p>
        </div>
      )}
      
      <div className="features-box">
        <h2>Postojeće funkcionalnosti:</h2>
        <ul>
          <li>
            Autentifikacija
            <ul>
              <li>Registracija novih korisnika, prijava i odjava</li>
              <li>Čitanje username i role u Header komponenti</li>
            </ul>
          </li>
          <li>Pregled registrovanih korisnika</li>
          <li>
            Upravljanje projektima
            <ul>
              <li>Pregled projekata odabranog korisnika</li>
              <li>Dodavanje, izmena i brisanje projekata prijavljenog korisnika</li>
            </ul>
          </li>
        </ul>
        <b>Prijavi se sa "alice" i "Alice123!" da vidiš sve funkcije</b>
      </div>
    </div>
  );
};

export default Home;
