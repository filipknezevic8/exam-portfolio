import React, { useState, useContext } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import UserContext from '../../UserContext';
import { loginUser } from './services/userService';
import '../../core/global.scss';
import './users.scss';

const Login = () => {
  const { setUser } = useContext(UserContext);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { register, handleSubmit, formState: { errors } } = useForm();

  const onSubmit = async (data) => {
    setError('');
    setLoading(true);

    try {
      const token = await loginUser(data.username, data.password);
      
      localStorage.setItem('token', token);
      const payload = JSON.parse(atob(token.split('.')[1]));
      setUser(payload);

      const role = payload.role || payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      if (role === 'User') {
        navigate('/users');
      } else {
        navigate('/profile');
      }
    } catch (err) {
      if(err.response.status === 401) {
        setError('Neispravno korisničko ime ili lozinka.');
      } else {
        setError('Greška na serveru. Proveri Network tab ili upotrebi debugger.');
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-container">
      <form onSubmit={handleSubmit(onSubmit)} className="auth-form">
        <h2>Prijava</h2>
        
        {error && <div className="error-message">{error}</div>}
        
        <div className="form-group">
          <label>Korisničko ime:</label>
          <input 
            type="text" 
            placeholder="Unesite korisničko ime"
            {...register('username', { required: 'Korisničko ime je obavezno' })}
          />
          {errors.username && <span className="error-message">{errors.username.message}</span>}
        </div>
        
        <div className="form-group">
          <label>Lozinka:</label>
          <input 
            type="password" 
            placeholder="Unesite lozinku"
            {...register('password', { required: 'Lozinka je obavezna' })}
          />
          {errors.password && <span className="error-message">{errors.password.message}</span>}
        </div>
        
        <button 
          type="submit" 
          disabled={loading}
          className="btn btn-primary"
        >
          {loading ? 'Prijava u toku...' : 'Prijavi se'}
        </button>
        
        <div className="form-actions">
          <p>
            Nemaš nalog? <Link to="/register" style={{color:"#007bff"}}>Registruj se</Link>
          </p>
        </div>
      </form>
    </div>
  );
};

export default Login;
