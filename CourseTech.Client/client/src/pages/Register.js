import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import { useAuth } from '../contexts/AuthContext';
import PageLayout from '../components/PageLayout';
import './Register.css';
import { FaEye, FaEyeSlash} from 'react-icons/fa';
import welcomeImage from '../assets/images/welcome.svg';

const schema = yup.object().shape({
  firstName: yup
    .string()
    .min(2, 'First name must be at least 2 characters')
    .required('First name is required'),
  lastName: yup
    .string()
    .min(2, 'Last name must be at least 2 characters')
    .required('Last name is required'),
  email: yup
    .string()
    .email('Please enter a valid email address')
    .required('Email is required'),
  password: yup
    .string()
    .min(6, 'Password must be at least 6 characters')
    .matches(/(?=.*[0-9])/, 'Password must contain at least one number')
    .required('Password is required'),
  confirmPassword: yup
    .string()
    .oneOf([yup.ref('password'), null], 'Passwords do not match')
    .required('Please confirm your password'),
});

const Register = () => {
  const { register: registerUser } = useAuth();
  const navigate = useNavigate();
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const [submitError, setSubmitError] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  const {
    register,
    handleSubmit,
    formState: { errors, dirtyFields, isValid },
    watch,
  } = useForm({
    resolver: yupResolver(schema),
    mode: 'onChange',
  });

  const watchedFields = watch();

  const getInputStatus = (fieldName) => {
    if (!dirtyFields[fieldName]) return '';
    if (errors[fieldName]) return 'error';
    return 'success';
  };

  const onSubmit = async (data) => {
    try {
      setIsLoading(true);
      setSubmitError('');
      const success = await registerUser({
        firstName: data.firstName,
        lastName: data.lastName,
        email: data.email,
        password: data.password,
      });
      if (success) {
        navigate('/');
      }
    } catch (error) {
      setSubmitError(error.message || 'Failed to register. Please try again.');
    } finally {
      setIsLoading(false);
    }
  };

  const togglePasswordVisibility = (field) => {
    if (field === 'password') {
      setShowPassword(!showPassword);
    } else {
      setShowConfirmPassword(!showConfirmPassword);
    }
  };

  return (
    <PageLayout>
      <div className="register-container">
        <div className="register-box">
          <div className="register-left">
            <div className="welcome-text">
              <h2>Create Your Account</h2>
              <p>Join our learning community today</p>
            </div>

            <form className="register-form" onSubmit={handleSubmit(onSubmit)} noValidate>
              {submitError && (
                <div className="submit-error">
                  <div className="error-message">{submitError}</div>
                </div>
              )}

              <div className="form-group">
                <label htmlFor="firstName">First Name</label>
                <input
                  type="text"
                  id="firstName"
                  {...register('firstName')}
                  className={getInputStatus('firstName')}
                  placeholder="Enter your first name"
                />
                {errors.firstName && (
                  <div className="error-message">{errors.firstName.message}</div>
                )}
              </div>

              <div className="form-group">
                <label htmlFor="lastName">Last Name</label>
                <input
                  type="text"
                  id="lastName"
                  {...register('lastName')}
                  className={getInputStatus('lastName')}
                  placeholder="Enter your last name"
                />
                {errors.lastName && (
                  <div className="error-message">{errors.lastName.message}</div>
                )}
              </div>

              <div className="form-group">
                <label htmlFor="email">Email</label>
                <input
                  type="email"
                  id="email"
                  {...register('email')}
                  className={getInputStatus('email')}
                  placeholder="Enter your email"
                />
                {errors.email && (
                  <div className="error-message">{errors.email.message}</div>
                )}
              </div>

              <div className="form-group">
                <label htmlFor="password">Password</label>
                <div className="password-input">
                  <input
                    type={showPassword ? 'text' : 'password'}
                    id="password"
                    {...register('password')}
                    className={getInputStatus('password')}
                    placeholder="Create a password"
                  />
                  <button
                    type="button"
                    className="show-password"
                    onClick={() => togglePasswordVisibility('password')}
                  >
                    {showPassword ? <FaEyeSlash /> : <FaEye />}
                  </button>
                </div>
                {errors.password && (
                  <div className="error-message">{errors.password.message}</div>
                )}
              </div>

              <div className="form-group">
                <label htmlFor="confirmPassword">Confirm Password</label>
                <div className="password-input">
                  <input
                    type={showConfirmPassword ? 'text' : 'password'}
                    id="confirmPassword"
                    {...register('confirmPassword')}
                    className={getInputStatus('confirmPassword')}
                    placeholder="Confirm your password"
                  />
                  <button
                    type="button"
                    className="show-password"
                    onClick={() => togglePasswordVisibility('confirm')}
                  >
                    {showConfirmPassword ? <FaEyeSlash /> : <FaEye />}
                  </button>
                </div>
                {errors.confirmPassword && (
                  <div className="error-message">
                    {errors.confirmPassword.message}
                  </div>
                )}
              </div>

              <button
                type="submit"
                className="register-btn"
                disabled={isLoading || !isValid}
              >
                {isLoading ? 'Creating Account...' : 'Create Account'}
              </button>
            </form>
          </div>

          <div className="vertical-divider"></div>

          <div className="register-right">
            <img
              src={welcomeImage}
              alt="Welcome illustration"
              className="welcome-image"
            />
            <p>Already have an account?</p>
            <Link to="/login" className="login-link">
              Sign In
            </Link>
          </div>
        </div>
      </div>
    </PageLayout>
  );
};

export default Register; 