import axios from 'axios';

const axiosConfig = axios.create({
  baseURL: 'http://localhost:5231/api/'
});

axiosConfig.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

axiosConfig.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response && error.response.status === 401) {
      console.log('Niste autorizovani, molimo prijavite se ponovo.');
    }
    if (error.code === 'ERR_NETWORK') {
      console.log('Server nije dostupan. Proveri da li server radi ili da li je dobar URL naveden.');
    }
    return Promise.reject(error);
  }
);

export default axiosConfig;