import React, { useState, useEffect, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import UserContext from '../../UserContext';
import { getMyProjects, createProject, updateProject, deleteProject } from './services/projectService';
import ProjectList from './ProjectList';
import './projects.scss';

const MyProjects = () => {
  const { user } = useContext(UserContext);
  const navigate = useNavigate();

  const [projects, setProjects] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const [editingProjectId, setEditingProjectId] = useState(null);
  const [isCreating, setIsCreating] = useState(false);
  const [saving, setSaving] = useState(false);
  const [formError, setFormError] = useState('');

  const { register, handleSubmit, reset, formState: { errors } } = useForm();

  useEffect(() => {
    if (!user) {
      navigate('/login');
      return;
    }
    fetchProjects();
  }, [user, navigate]);

  const fetchProjects = async () => {
    setLoading(true);
    setError('');
    try {
      const data = await getMyProjects();
      setProjects(data);
    } catch (err) {
      setError('Greška pri učitavanju projekata.');
    } finally {
      setLoading(false);
    }
  };

  const handleSelectProject = (projectId) => {
    const project = projects.find(p => p.id === projectId);
    if (!project) return;
    setIsCreating(false);
    setEditingProjectId(projectId);
    setFormError('');
    reset({
      name: project.name,
      description: project.description,
      startedAt: project.startedAt ? project.startedAt.substring(0, 16) : ''
    });
  };

  const handleNewProject = () => {
    setEditingProjectId(null);
    setIsCreating(true);
    setFormError('');
    reset({ name: '', description: '', startedAt: '' });
  };

  const onSubmit = async (data) => {
    setSaving(true);
    setFormError('');
    try {
      const dto = {
        name: data.name,
        description: data.description,
        startedAt: data.startedAt ? new Date(data.startedAt).toISOString() : null
      };

      if (isCreating) {
        await createProject(dto);
      } else {
        await updateProject(editingProjectId, dto);
      }

      await fetchProjects();
      setEditingProjectId(null);
      setIsCreating(false);
      reset({ name: '', description: '', startedAt: '' });
    } catch (err) {
      setFormError('Greška pri čuvanju projekta.');
    } finally {
      setSaving(false);
    }
  };

  const handleDelete = async () => {
    setSaving(true);
    setFormError('');
    try {
      await deleteProject(editingProjectId);
      await fetchProjects();
      setEditingProjectId(null);
      setIsCreating(false);
      reset({ name: '', description: '', startedAt: '' });
    } catch (err) {
      setFormError('Greška pri brisanju projekta.');
    } finally {
      setSaving(false);
    }
  };

  const showForm = isCreating || editingProjectId;

  return (
    <div className="my-projects-wrapper">
      <div className="my-projects-left">
        <div className="my-projects-controls">
          <button className="btn btn-success" onClick={handleNewProject}>
            Dodaj novi projekat
          </button>
        </div>
        {loading && <p>Učitavanje...</p>}
        {error && <p className="error-message">{error}</p>}
        {!loading && !error && projects.length === 0 && (
          <p style={{ padding: '20px' }}>Nemate projekata.</p>
        )}
        {!loading && !error && projects.length > 0 && (
          <ProjectList projects={projects} onSelectProject={handleSelectProject} />
        )}
      </div>

      <div className="my-projects-right">
        {showForm && (
          <form onSubmit={handleSubmit(onSubmit)} className="project-form">
            <h2>{isCreating ? 'Novi projekat' : 'Izmena projekta'}</h2>

            {formError && <div className="error-message">{formError}</div>}

            <div className="form-group">
              <label>Naziv:</label>
              <input
                type="text"
                placeholder="Unesite naziv projekta"
                {...register('name', { required: 'Naziv je obavezan' })}
              />
              {errors.name && <span className="error-message">{errors.name.message}</span>}
            </div>

            <div className="form-group">
              <label>Opis:</label>
              <textarea
                placeholder="Unesite opis projekta"
                rows={4}
                {...register('description')}
              />
            </div>

            <div className="form-group">
              <label>Datum početka:</label>
              <input
                type="datetime-local"
                {...register('startedAt')}
              />
            </div>

            <div className="form-actions-row">
              <button type="submit" className="btn btn-primary" disabled={saving}>
                {saving ? 'Čuvanje...' : 'Sačuvaj'}
              </button>
              {!isCreating && editingProjectId && (
                <button
                  type="button"
                  className="btn btn-danger"
                  disabled={saving}
                  onClick={handleDelete}
                >
                  Obriši
                </button>
              )}
            </div>
          </form>
        )}
      </div>
    </div>
  );
};

export default MyProjects;
