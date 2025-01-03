import React from 'react';
import './PageLayout.css';

const PageLayout = ({ children }) => {
  return (
    <div className="page-container">
      <div className="page-content">
        {children}
      </div>
    </div>
  );
};

export default PageLayout; 