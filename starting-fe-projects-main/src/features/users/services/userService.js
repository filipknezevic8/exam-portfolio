import axiosConfig from '../../../core/axiosConfig';

export const loginUser = async (username, password) => {
  const response = await axiosConfig.post('/auth/login', {
    username,
    password
  });
  return response.data;
};

export const registerUser = async (username, email, password, name, surname) => {
  const response = await axiosConfig.post('/auth/register', {
    email,
    username,
    password,
    name,
    surname
  });
  return response.data;
};

export const getUserProfile = async () => {
  const response = await axiosConfig.get('/auth/profile');
  return response.data;
};

export const getUsers = async (page, pageSize) => {
  const response = await axiosConfig.get(`/users?page=${page}&pageSize=${pageSize}`);
  return response.data;
};
