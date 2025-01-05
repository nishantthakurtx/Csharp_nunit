import React from 'react';
import { Link } from 'react-router-dom';
import { FiGlobe } from 'react-icons/fi';
import '../styles/Footer.css';

const Footer = () => {
  const currentYear = new Date().getFullYear();

  return (
    <footer className="footer">
      <div className="footerTop">
        <div className="footerContent">
          <div className="footerColumns">
            <div className="footerColumn">
              <h4>CourseTech Business</h4>
              <ul>
                <li><Link to="/business">Teach on CourseTech</Link></li>
                <li><Link to="/business/teams">Get the app</Link></li>
                <li><Link to="/business/enterprise">About us</Link></li>
                <li><Link to="/business/contact">Contact us</Link></li>
              </ul>
            </div>

            <div className="footerColumn">
              <h4>Careers</h4>
              <ul>
                <li><Link to="/careers">Careers</Link></li>
                <li><Link to="/blog">Blog</Link></li>
                <li><Link to="/help">Help and Support</Link></li>
                <li><Link to="/affiliate">Affiliate</Link></li>
                <li><Link to="/investors">Investors</Link></li>
              </ul>
            </div>

            <div className="footerColumn">
              <h4>Terms</h4>
              <ul>
                <li><Link to="/terms">Terms</Link></li>
                <li><Link to="/privacy">Privacy policy</Link></li>
                <li><Link to="/cookie-policy">Cookie settings</Link></li>
                <li><Link to="/sitemap">Sitemap</Link></li>
                <li><Link to="/accessibility">Accessibility statement</Link></li>
              </ul>
            </div>
          </div>
        </div>
      </div>

      <div className="footerBottom">
        <div className="footerBottomContent">
          <div className="footerLogo">
            <Link to="/">CourseTech</Link>
          </div>
          <div className="footerCopyright">
            <p>&copy; {currentYear} CourseTech, Inc.</p>
          </div>
          <div className="footerLanguage">
            <button className="languageButton">
              <FiGlobe />
              <span>English</span>
            </button>
          </div>
        </div>
      </div>
    </footer>
  );
};

export default Footer; 