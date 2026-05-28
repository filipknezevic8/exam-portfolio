import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { registerUser } from './services/userService';
import '../../core/global.scss';
import './users.scss';

const Register = () => {
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { register, handleSubmit, formState: { errors } } = useForm();

  const onSubmit = async (data) => {
    setError('');
    setLoading(true);

    try {
      await registerUser(data.username, data.email, data.password, data.name, data.surname);
      navigate('/login');
    } catch (err) {
      setError(err.response?.data?.message || 'Greška pri registraciji. Pokušaj ponovo.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-container">
      <form onSubmit={handleSubmit(onSubmit)} className="auth-form">
        <h2>Registracija</h2>
        
        {error && <div className="error-message">{error}</div>}
        
        <div className="form-group">
          <label>Email:</label>
          <input 
            type="email" 
            placeholder="Unesite email"
            {...register('email', { 
              required: 'Email je obavezan',
              pattern: {
                value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                message: 'Neispravna email adresa'
              }
            })}
          />
          {errors.email && <span className="error-message">{errors.email.message}</span>}
        </div>

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
          <label>Ime:</label>
          <input 
            type="text" 
            placeholder="Unesite ime"
            {...register('name', { required: 'Ime je obavezno' })}
          />
          {errors.name && <span className="error-message">{errors.name.message}</span>}
        </div>

        <div className="form-group">
          <label>Prezime:</label>
          <input 
            type="text" 
            placeholder="Unesite prezime"
            {...register('surname', { required: 'Prezime je obavezno' })}
          />
          {errors.surname && <span className="error-message">{errors.surname.message}</span>}
        </div>
        
        <div className="form-group">
          <label>Lozinka:</label>
          <input 
            type="password" 
            placeholder="Unesite lozinku (min. 8 karaktera)"
            {...register('password', { 
              required: 'Lozinka je obavezna',
              minLength: {
                value: 8,
                message: 'Lozinka mora imati najmanje 8 karaktera'
              },
              validate: {
                hasDigit: value => /\d/.test(value) || 'Lozinka mora sadržati bar jednu cifru',
                hasLowercase: value => /[a-z]/.test(value) || 'Lozinka mora sadržati bar jedno malo slovo',
                hasUppercase: value => /[A-Z]/.test(value) || 'Lozinka mora sadržati bar jedno veliko slovo',
                hasSpecialChar: value => /[^a-zA-Z0-9]/.test(value) || 'Lozinka mora sadržati bar jedan specijalni karakter'
              }
            })}
          />
          {errors.password && <span className="error-message">{errors.password.message}</span>}
        </div>
        
        <button 
          type="submit" 
          disabled={loading}
          className="btn btn-success"
        >
          {loading ? 'Registracija u toku...' : 'Registruj se'}
        </button>
        
        <div className="form-actions">
          <p>
            Već imaš nalog? <Link to="/login" style={{color:"#007bff"}}>Prijavi se</Link>
          </p>
        </div>
      </form>
    </div>
  );
};

export default Register;
