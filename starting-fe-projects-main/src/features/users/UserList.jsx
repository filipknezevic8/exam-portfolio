import React, { useState, useEffect, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import UserContext from '../../UserContext';
import { getUsers } from './services/userService';
import { getProjectsByUser } from '../projects/services/projectService';
import ProjectList from '../projects/ProjectList';
import './users.scss';

const UserList = () => {
  const { user } = useContext(UserContext);
  const navigate = useNavigate();
  const [users, setUsers] = useState([]);
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [totalCount, setTotalCount] = useState(0);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const [selectedUserId, setSelectedUserId] = useState(null);
  const [projects, setProjects] = useState(null);
  const [projectsLoading, setProjectsLoading] = useState(false);
  const [projectsError, setProjectsError] = useState('');

  const totalPages = Math.ceil(totalCount / pageSize);

  useEffect(() => {
    if (!user) {
      navigate('/login');
      return;
    }

    const fetchUsers = async () => {
      setLoading(true);
      setError('');
      try {
        const data = await getUsers(page, pageSize);
        setUsers(data.items);
        setTotalCount(data.totalCount);
      } catch (err) {
        setError('Greška pri učitavanju korisnika.');
      } finally {
        setLoading(false);
      }
    };

    fetchUsers();
  }, [page, pageSize, user, navigate]);

  const handlePageSizeChange = (e) => {
    setPageSize(Number(e.target.value));
    setPage(1);
  };

  const handleShowProjects = async (userId) => {
    setSelectedUserId(userId);
    setProjectsLoading(true);
    setProjectsError('');
    setProjects(null);
    try {
      const data = await getProjectsByUser(userId);
      setProjects(data);
    } catch (err) {
      setProjectsError('Došlo je do greške pri učitavanju projekata.');
    } finally {
      setProjectsLoading(false);
    }
  };

  return (
    <div className="user-page-wrapper">
      <div className="user-list-container">
        <div className="user-list-controls">
          <label>Veličina stranice:</label>
          <select value={pageSize} onChange={handlePageSizeChange}>
            <option value={5}>5</option>
            <option value={10}>10</option>
            <option value={20}>20</option>
          </select>
          <button className="btn btn-sm" disabled={page <= 1} onClick={() => setPage(page - 1)}>
            &lt;
          </button>
          <span>{page} / {totalPages || 1}</span>
          <button className="btn btn-sm" disabled={page >= totalPages} onClick={() => setPage(page + 1)}>
            &gt;
          </button>
        </div>
        <hr></hr>
        <div className="user-list">
          {loading && <p>Učitavanje...</p>}
          {error && <p className="error-message">{error}</p>}
          {!loading && !error && users.map((u, index) => (
            <div key={index} className={`user-row ${selectedUserId === u.id ? 'active' : ''}`}>
              <span>{u.name} {u.surname}</span>
              <button className="btn btn-sm" onClick={() => handleShowProjects(u.id)}>
                Projekti
              </button>
            </div>
          ))}
        </div>
      </div>

      <div className="project-panel">
        {projectsLoading && <p>Učitavanje projekata...</p>}
        {projectsError && <p className="error-message">{projectsError}</p>}
        {!projectsLoading && !projectsError && projects && projects.length === 0 && (
          <p>Korisnik nema aktivnih projekata.</p>
        )}
        {!projectsLoading && !projectsError && projects && projects.length > 0 && (
          <ProjectList projects={projects} />
        )}
      </div>
    </div>
  );
};

export default UserList;
