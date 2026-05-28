import axiosConfig from '../../../core/axiosConfig';

export const getProjectsByUser = async (userId) => {
  const response = await axiosConfig.get(`/projects/users/${userId}`);
  return response.data;
};

export const getMyProjects = async () => {
  const response = await axiosConfig.get('/projects/mine');
  return response.data;
};

export const createProject = async (dto) => {
  const response = await axiosConfig.post('/projects', dto);
  return response.data;
};

export const updateProject = async (id, dto) => {
  const response = await axiosConfig.put(`/projects/${id}`, dto);
  return response.data;
};

export const deleteProject = async (id) => {
  const response = await axiosConfig.delete(`/projects/${id}`);
  return response.data;
};
