import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import { FaEye, FaEyeSlash } from 'react-icons/fa';
import { useAuth } from '../contexts/AuthContext';
import './Login.css';

const schema = yup.object().shape({
  email: yup
    .string()
    .email('Please enter a valid email address')
    .required('Email is required')
    .matches(
      /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
      'Please enter a valid email address'
    ),
  password: yup
    .string()
    .min(6, 'Password must be at least 6 characters')
    .matches(
      /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*#?&]{6,}$/,
      'Password must contain at least one letter and one number'
    )
    .required('Password is required'),
});

const Login = () => {
  const navigate = useNavigate();
  const { login } = useAuth();
  const [showPassword, setShowPassword] = useState(false);
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
    return errors[fieldName] ? 'error' : 'success';
  };

  const onSubmit = async (data) => {
    if (!isValid) return;
    
    setIsLoading(true);
    setSubmitError('');

    try {
      await login(data);
      navigate('/');
    } catch (error) {
      if (error.message === 'User not found') {
        setSubmitError('No account found with this email. Please check your email or register.');
      } else if (error.message === 'Invalid email or password') {
        setSubmitError('Invalid email or password combination. Please try again.');
      } else {
        setSubmitError(error.message || 'An unexpected error occurred. Please try again later.');
      }
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="login-container">
      <div className="login-box">
        <div className="login-left">
          <div className="welcome-text">
            <h2>Welcome Back!</h2>
            <p>Please enter your details to sign in</p>
          </div>
          <form className="login-form" onSubmit={handleSubmit(onSubmit)} noValidate>
            {submitError && (
              <div className="submit-error">
                {submitError}
              </div>
            )}
            <div className="form-group">
              <label htmlFor="email">Email</label>
              <input
                type="email"
                id="email"
                {...register('email')}
                className={getInputStatus('email')}
                placeholder="Enter your email"
                autoComplete="email"
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
                  placeholder="Enter your password"
                  autoComplete="current-password"
                />
                <button
                  type="button"
                  className="show-password"
                  onClick={() => setShowPassword(!showPassword)}
                >
                  {showPassword ? <FaEyeSlash /> : <FaEye />}
                </button>
              </div>
              {errors.password && (
                <div className="error-message">{errors.password.message}</div>
              )}
            </div>
            <button
              type="submit"
              className="login-btn"
              disabled={isLoading || !isValid}
            >
              {isLoading ? 'Signing in...' : 'Sign in'}
            </button>
          </form>
        </div>
        <div className="login-right">
          <h3>New to CourseTech?</h3>
          <p>Join our community of learners and start your educational journey today!</p>
          <Link to="/register" className="register-btn">
            Create Account
          </Link>
        </div>
      </div>
    </div>
  );
};

export default Login;