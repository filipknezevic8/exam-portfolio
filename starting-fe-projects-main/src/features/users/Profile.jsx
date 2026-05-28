import React, { useContext, useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import UserContext from '../../UserContext';
import { getUserProfile } from './services/userService';
import '../../core/global.scss';
import './users.scss';

const Profile = () => {
  const { user } = useContext(UserContext);
  const navigate = useNavigate();
  const [profile, setProfile] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    if (!user) {
      navigate('/login');
      return;
    }

    const fetchProfile = async () => {
      try {
        const data = await getUserProfile();
        setProfile(data);
      } catch (err) {
        setError('Nije moguće učitati profil');
      } finally {
        setLoading(false);
      }
    };

    fetchProfile();
  }, [user, navigate]);

  if (!user) return null;
  if (loading) return <div className="profile-container"><p>Učitavanje...</p></div>;
  if (error) return <div className="profile-container"><p className="error-message">{error}</p></div>;
  if (!profile) return null;

  const fields = [
    { label: 'Korisničko ime', value: profile.username },
    { label: 'Email', value: profile.email },
    { label: 'Ime', value: profile.name },
    { label: 'Prezime', value: profile.surname },
    { label: 'Uloga', value: profile.role }
  ];

  return (
    <div className="profile-container">
      <div className="profile-card">
        <h2>Profil korisnika</h2>
        
        <div className="profile-fields">
          {fields.map(field => field.value && (
            <div key={field.label}>
              <strong>{field.label}:</strong>
              <span>{field.value}</span>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default Profile;
