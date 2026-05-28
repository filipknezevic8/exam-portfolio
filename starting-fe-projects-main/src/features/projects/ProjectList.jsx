import React from 'react';
import './projects.scss';

const statusLabels = {
  0: 'U pripremi',
  1: 'U realizaciji',
  2: 'Kompletiran',
  3: 'Arhiviran'
};

const isVisibleForUsers = (status) => {
  const value = typeof status === 'number' ? status : Number(status);
  return value === 1 || value === 2;
};

const getStatusLabel = (status) => {
  const value = typeof status === 'number' ? status : Number(status);
  return statusLabels[value] || statusLabels[status] || 'Nepoznat status';
};

const ProjectList = ({ projects = [], onSelectProject, selectedProjectId, showAllStatuses = true }) => {
  const visibleProjects = showAllStatuses
    ? projects
    : projects.filter((project) => isVisibleForUsers(project.status));

  return (
    <div className="project-list">
      {visibleProjects.map((project) => (
        <div
          key={project.id}
          className={`project-card ${selectedProjectId === project.id ? 'selected' : ''}`}
        >
          <div className="project-card-header">
            <h3 className="project-name">{project.name}</h3>
            {onSelectProject && (
              <button
                className="btn btn-primary btn-sm"
                onClick={() => onSelectProject(project.id)}
              >
                Izmeni
              </button>
            )}
          </div>

          <p className="project-description">{project.description}</p>

          <div className="project-meta">
            <span className="project-status">Status: {getStatusLabel(project.status)}</span>
            <span>Započet: {new Date(project.startedAt).toLocaleDateString()}</span>
            {project.completedAt && (
              <span>Završen: {new Date(project.completedAt).toLocaleDateString()}</span>
            )}
          </div>
        </div>
      ))}
    </div>
  );
};

export default ProjectList;
