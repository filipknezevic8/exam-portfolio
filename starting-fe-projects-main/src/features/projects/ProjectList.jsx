import React from 'react';
import './projects.scss';

const statusLabels = {
  0: 'U pripremi',
  1: 'U realizaciji',
  2: 'Kompletiran',
  3: 'Arhiviran'
};

const getStatusValue = (status) => {
  return typeof status === 'number' ? status : Number(status);
};

const getStatusLabel = (status) => {
  const value = getStatusValue(status);
  return statusLabels[value] || 'Nepoznat status';
};

const canShowToUser = (status) => {
  const value = getStatusValue(status);
  return value === 1 || value === 2;
};

const ProjectList = ({
  projects = [],
  onSelectProject,
  selectedProjectId,
  onStartProject,
  onConcludeProject,
  onReopenProject,
  showAllStatuses = true,
  actionsDisabled = false
}) => {
  const visibleProjects = showAllStatuses ? projects : projects.filter((project) => canShowToUser(project.status));

  const renderActions = (project) => {
    const status = getStatusValue(project.status);
    const actions = [];

    if (onSelectProject && status !== 2) {
      actions.push(
        <button
          key="edit"
          type="button"
          className="btn btn-primary btn-sm"
          onClick={() => onSelectProject(project.id)}
          disabled={actionsDisabled}
        >
          Izmeni
        </button>
      );
    }

    if (onStartProject && status === 0) {
      actions.push(
        <button
          key="start"
          type="button"
          className="btn btn-success btn-sm"
          onClick={() => onStartProject(project.id)}
          disabled={actionsDisabled}
        >
          Započni
        </button>
      );
    }

    if (onConcludeProject && status === 1) {
      actions.push(
        <button
          key="conclude"
          type="button"
          className="btn btn-warning btn-sm"
          onClick={() => onConcludeProject(project.id)}
          disabled={actionsDisabled}
        >
          Zaključi
        </button>
      );
    }

    if (onReopenProject && status === 2) {
      actions.push(
        <button
          key="reopen"
          type="button"
          className="btn btn-secondary btn-sm"
          onClick={() => onReopenProject(project.id)}
          disabled={actionsDisabled}
        >
          Vrati u pripremu
        </button>
      );
    }

    if (actions.length === 0) {
      return null;
    }

    return <div className="project-actions">{actions}</div>;
  };

  return (
    <div className="project-list">
      {visibleProjects.map((project) => (
        <div
          key={project.id}
          className={`project-card ${selectedProjectId === project.id ? 'selected' : ''}`}
        >
          <div className="project-card-header">
            <h3 className="project-name">{project.name}</h3>
            {renderActions(project)}
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
