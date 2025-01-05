import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { FiUser, FiMail, FiLock } from 'react-icons/fi';
import styles from '../styles/Auth.module.css';
import { toast } from 'react-toastify';

const Register = () => {
  const navigate = useNavigate();
  const { register } = useAuth();
  const [isLoading, setIsLoading] = useState(false);
  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    confirmPassword: ''
  });

  const [passwordValidation, setPasswordValidation] = useState({
    minLength: false,
    hasNumber: false,
    hasUpperCase: false,
    hasLowerCase: false,
    hasSpecialChar: false
  });

  const validatePassword = (password) => {
    return {
      minLength: password.length >= 6,
      hasNumber: /\d/.test(password),
      hasUpperCase: /[A-Z]/.test(password),
      hasLowerCase: /[a-z]/.test(password),
      hasSpecialChar: /[!@#$%^&*(),.?":{}|<>]/.test(password)
    };
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    
    if (name === 'password') {
      setPasswordValidation(validatePassword(value));
      setFormData(prev => ({
        ...prev,
        [name]: value
      }));
    } else {
      setFormData(prev => ({
        ...prev,
        [name]: value
      }));
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);

    // Validate all fields
    if (!formData.firstName || !formData.lastName || !formData.email || !formData.password) {
      toast.error('Please fill in all required fields');
      setIsLoading(false);
      return;
    }

    // Validate password requirements
    const validation = validatePassword(formData.password);
    if (!Object.values(validation).every(Boolean)) {
      toast.error('Password does not meet all requirements');
      setIsLoading(false);
      return;
    }

    // Validate password match
    if (formData.password !== formData.confirmPassword) {
      toast.error('Passwords do not match');
      setIsLoading(false);
      return;
    }

    try {
      await register(formData);
      toast.success('Registration successful!');
      navigate('/');
    } catch (error) {
      toast.error(error?.message || 'Failed to register');
      console.error('Registration error:', error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className={styles.authContainer}>
      <div className={styles.authForm}>
        <h2>Create Account</h2>
        <form onSubmit={handleSubmit}>
          <div className={styles.formGroup}>
            <label>
              <FiUser className={styles.inputIcon} />
              First Name
            </label>
            <input
              type="text"
              name="firstName"
              value={formData.firstName}
              onChange={handleInputChange}
              placeholder="Enter your first name"
              disabled={isLoading}
            />
          </div>

          <div className={styles.formGroup}>
            <label>
              <FiUser className={styles.inputIcon} />
              Last Name
            </label>
            <input
              type="text"
              name="lastName"
              value={formData.lastName}
              onChange={handleInputChange}
              placeholder="Enter your last name"
              disabled={isLoading}
            />
          </div>

          <div className={styles.formGroup}>
            <label>
              <FiMail className={styles.inputIcon} />
              Email
            </label>
            <input
              type="email"
              name="email"
              value={formData.email}
              onChange={handleInputChange}
              placeholder="Enter your email"
              disabled={isLoading}
            />
          </div>

          <div className={styles.formGroup}>
            <label>
              <FiLock className={styles.inputIcon} />
              Password
            </label>
            <input
              type="password"
              name="password"
              value={formData.password}
              onChange={handleInputChange}
              placeholder="Enter your password"
              disabled={isLoading}
            />
            {formData.password && (
              <div className={styles.passwordRequirements}>
                <p className={styles.requirementTitle}>Password must have:</p>
                <ul>
                  <li className={passwordValidation.minLength ? styles.valid : styles.invalid}>
                    At least 6 characters
                  </li>
                  <li className={passwordValidation.hasNumber ? styles.valid : styles.invalid}>
                    At least one number
                  </li>
                  <li className={passwordValidation.hasUpperCase ? styles.valid : styles.invalid}>
                    At least one uppercase letter
                  </li>
                  <li className={passwordValidation.hasLowerCase ? styles.valid : styles.invalid}>
                    At least one lowercase letter
                  </li>
                  <li className={passwordValidation.hasSpecialChar ? styles.valid : styles.invalid}>
                    At least one special character (!@#$%^&*(),.?":{}|&lt;&gt;)
                  </li>
                </ul>
              </div>
            )}
          </div>

          <div className={styles.formGroup}>
            <label>
              <FiLock className={styles.inputIcon} />
              Confirm Password
            </label>
            <input
              type="password"
              name="confirmPassword"
              value={formData.confirmPassword}
              onChange={handleInputChange}
              placeholder="Confirm your password"
              disabled={isLoading}
            />
            {formData.confirmPassword && formData.password !== formData.confirmPassword && (
              <p className={styles.errorText}>Passwords do not match</p>
            )}
          </div>

          <button 
            type="submit" 
            className={styles.submitButton}
            disabled={
              isLoading || 
              !Object.values(passwordValidation).every(Boolean) || 
              formData.password !== formData.confirmPassword
            }
          >
            {isLoading ? 'Creating Account...' : 'Create Account'}
          </button>
        </form>
      </div>
    </div>
  );
};

export default Register; 