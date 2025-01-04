import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import { courseService } from '../services/courseService';
import { categoryService } from '../services/categoryService';
import { useAuth } from '../contexts/AuthContext';
import { FaImage, FaVideo, FaBook, FaGlobe, FaDollarSign, FaClock, FaChalkboardTeacher, FaExclamationCircle } from 'react-icons/fa';
import './CreateCourse.css';

const schema = yup.object().shape({
  title: yup
    .string()
    .required('Course title is required')
    .min(5, 'Course title must be at least 5 characters')
    .max(100, 'Course title must not exceed 100 characters'),
  description: yup
    .string()
    .required('Course description is required')
    .min(20, 'Description must be at least 20 characters')
    .max(2000, 'Description must not exceed 2000 characters'),
  categoryId: yup
    .string()
    .required('Please select a category'),
  imageUrl: yup
    .string()
    .url('Please enter a valid image URL')
    .nullable(),
  videoUrl: yup
    .string()
    .url('Please enter a valid video URL')
    .nullable(),
  level: yup
    .string()
    .required('Please select a level')
    .oneOf(['Beginner', 'Intermediate', 'Advanced'], 'Invalid level selected'),
  language: yup
    .string()
    .required('Please select a language')
    .oneOf(['English', 'Turkish'], 'Invalid language selected'),
  price: yup
    .number()
    .required('Price is required')
    .min(0, 'Price cannot be negative')
    .max(999999, 'Price is too high'),
  hours: yup
    .number()
    .required('Hours is required')
    .min(0, 'Hours cannot be negative')
    .max(99, 'Hours cannot exceed 99'),
  minutes: yup
    .number()
    .required('Minutes is required')
    .min(0, 'Minutes cannot be negative')
    .max(59, 'Minutes cannot exceed 59')
});

const UpdateCourse = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const { user } = useAuth();
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(false);
  const [step, setStep] = useState(1);
  const [submitError, setSubmitError] = useState('');
  const [initialLoading, setInitialLoading] = useState(true);

  const {
    register,
    handleSubmit,
    formState: { errors, dirtyFields, isValid },
    setValue,
    trigger,
    watch,
    reset
  } = useForm({
    resolver: yupResolver(schema),
    mode: 'onChange'
  });

  const watchedFields = watch();

  useEffect(() => {
    const fetchCourseAndCategories = async () => {
      try {
        setInitialLoading(true);
        const [courseResponse, categoriesResponse] = await Promise.all([
          courseService.getCourseWithDetails(id),
          categoryService.getAll()
        ]);

        const courseData = courseResponse.data;
        setCategories(categoriesResponse.data);

        // Parse duration into hours and minutes
        const durationParts = courseData.duration.split(':');
        const hours = parseInt(durationParts[0]);
        const minutes = parseInt(durationParts[1]);

        // Set form values
        reset({
          title: courseData.title,
          description: courseData.description,
          categoryId: courseData.categoryId,
          imageUrl: courseData.imageUrl,
          videoUrl: courseData.videoUrl,
          level: courseData.level,
          language: courseData.language,
          price: courseData.price,
          hours: hours,
          minutes: minutes,
          instructorId: courseData.instructorId
        });
      } catch (error) {
        console.error('Error fetching course data:', error);
        setSubmitError('Failed to load course data');
      } finally {
        setInitialLoading(false);
      }
    };

    if (id && user?.sub) {
      fetchCourseAndCategories();
    }
  }, [id, user, reset]);

  const getInputStatus = (fieldName) => {
    if (!dirtyFields[fieldName]) return '';
    return errors[fieldName] ? 'error' : 'success';
  };

  const handleNextStep = async () => {
    let fieldsToValidate = [];
    switch (step) {
      case 1:
        fieldsToValidate = ['title', 'description', 'categoryId'];
        break;
      case 2:
        fieldsToValidate = ['imageUrl', 'videoUrl'];
        break;
      case 3:
        fieldsToValidate = ['level', 'language', 'price', 'hours', 'minutes'];
        break;
      default:
        break;
    }

    const isStepValid = await trigger(fieldsToValidate);
    if (isStepValid) {
      setStep(step + 1);
    }
  };

  const onSubmit = async (data) => {
    try {
      setLoading(true);
      setSubmitError('');

      // Convert hours and minutes to TimeSpan format
      const hours = parseInt(data.hours) || 0;
      const minutes = parseInt(data.minutes) || 0;
      const duration = `${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}:00`;

      const courseData = {
        ...data,
        id: id, // Include the course ID for update
        duration,
        instructorId: user.sub
      };

      delete courseData.hours;
      delete courseData.minutes;

      await courseService.updateCourse(courseData);
      navigate('/courses/my-courses');
    } catch (error) {
      console.error('Error updating course:', error);
      setSubmitError('Failed to update course. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const renderStepIndicator = () => (
    <div className="step-indicator">
      <div className={`step ${step >= 1 ? 'active' : ''}`}>
        <div className="step-number">1</div>
        <span>Basic Info</span>
      </div>
      <div className={`step ${step >= 2 ? 'active' : ''}`}>
        <div className="step-number">2</div>
        <span>Media</span>
      </div>
      <div className={`step ${step >= 3 ? 'active' : ''}`}>
        <div className="step-number">3</div>
        <span>Details</span>
      </div>
    </div>
  );

  const renderStep1 = () => (
    <div className="form-step">
      <div className="form-group">
        <label htmlFor="title">
          <FaBook className="input-icon" />
          Course Title*
        </label>
        <input
          type="text"
          id="title"
          {...register('title')}
          className={getInputStatus('title')}
          placeholder="Enter course title"
        />
        {errors.title && (
          <div className="field-error">
            <FaExclamationCircle /> {errors.title.message}
          </div>
        )}
      </div>

      <div className="form-group">
        <label htmlFor="description">
          <FaChalkboardTeacher className="input-icon" />
          Description*
        </label>
        <textarea
          id="description"
          {...register('description')}
          className={getInputStatus('description')}
          rows="4"
          placeholder="Describe your course"
        />
        {errors.description && (
          <div className="field-error">
            <FaExclamationCircle /> {errors.description.message}
          </div>
        )}
      </div>

      <div className="form-group">
        <label htmlFor="categoryId">
          <FaBook className="input-icon" />
          Category*
        </label>
        <select
          id="categoryId"
          {...register('categoryId')}
          className={getInputStatus('categoryId')}
        >
          <option value="">Select a category</option>
          {categories.map(category => (
            <option key={category.id} value={category.id}>
              {category.name}
            </option>
          ))}
        </select>
        {errors.categoryId && (
          <div className="field-error">
            <FaExclamationCircle /> {errors.categoryId.message}
          </div>
        )}
      </div>
    </div>
  );

  const renderStep2 = () => (
    <div className="form-step">
      <div className="form-group">
        <label htmlFor="imageUrl">
          <FaImage className="input-icon" />
          Course Image URL
        </label>
        <input
          type="url"
          id="imageUrl"
          {...register('imageUrl')}
          className={getInputStatus('imageUrl')}
          placeholder="Enter course image URL"
        />
        {errors.imageUrl && (
          <div className="field-error">
            <FaExclamationCircle /> {errors.imageUrl.message}
          </div>
        )}
        {watchedFields.imageUrl && !errors.imageUrl && (
          <div className="image-preview">
            <img src={watchedFields.imageUrl} alt="Course preview" />
          </div>
        )}
      </div>

      <div className="form-group">
        <label htmlFor="videoUrl">
          <FaVideo className="input-icon" />
          Course Video URL
        </label>
        <input
          type="url"
          id="videoUrl"
          {...register('videoUrl')}
          className={getInputStatus('videoUrl')}
          placeholder="Enter course video URL"
        />
        {errors.videoUrl && (
          <div className="field-error">
            <FaExclamationCircle /> {errors.videoUrl.message}
          </div>
        )}
      </div>
    </div>
  );

  const renderStep3 = () => (
    <div className="form-step">
      <div className="form-row">
        <div className="form-group">
          <label htmlFor="level">
            <FaChalkboardTeacher className="input-icon" />
            Level*
          </label>
          <select
            id="level"
            {...register('level')}
            className={getInputStatus('level')}
          >
            <option value="Beginner">Beginner</option>
            <option value="Intermediate">Intermediate</option>
            <option value="Advanced">Advanced</option>
          </select>
          {errors.level && (
            <div className="field-error">
              <FaExclamationCircle /> {errors.level.message}
            </div>
          )}
        </div>

        <div className="form-group">
          <label htmlFor="language">
            <FaGlobe className="input-icon" />
            Language*
          </label>
          <select
            id="language"
            {...register('language')}
            className={getInputStatus('language')}
          >
            <option value="English">English</option>
            <option value="Turkish">Turkish</option>
          </select>
          {errors.language && (
            <div className="field-error">
              <FaExclamationCircle /> {errors.language.message}
            </div>
          )}
        </div>
      </div>

      <div className="form-row">
        <div className="form-group">
          <label htmlFor="price">
            <FaDollarSign className="input-icon" />
            Price ($)*
          </label>
          <input
            type="number"
            id="price"
            {...register('price')}
            className={getInputStatus('price')}
            min="0"
            step="0.01"
            placeholder="Enter course price"
          />
          {errors.price && (
            <div className="field-error">
              <FaExclamationCircle /> {errors.price.message}
            </div>
          )}
        </div>

        <div className="form-group">
          <label htmlFor="duration">
            <FaClock className="input-icon" />
            Duration*
          </label>
          <div className="duration-inputs">
            <div className="time-input">
              <input
                type="number"
                id="hours"
                {...register('hours')}
                className={getInputStatus('hours')}
                min="0"
                max="99"
                placeholder="0"
              />
              <span>hours</span>
            </div>
            <div className="time-input">
              <input
                type="number"
                id="minutes"
                {...register('minutes')}
                className={getInputStatus('minutes')}
                min="0"
                max="59"
                placeholder="0"
              />
              <span>minutes</span>
            </div>
          </div>
          {(errors.hours || errors.minutes) && (
            <div className="field-error">
              <FaExclamationCircle /> {errors.hours?.message || errors.minutes?.message}
            </div>
          )}
        </div>
      </div>
    </div>
  );

  if (initialLoading) {
    return (
      <div className="create-course-container">
        <div className="loading-container">
          <div className="loading-spinner" />
          <p>Loading course data...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="create-course-container">
      <div className="create-course-card">
        <h1>Update Course</h1>
        {submitError && <div className="error-message">{submitError}</div>}
        
        {renderStepIndicator()}

        <form onSubmit={handleSubmit(onSubmit)} className="create-course-form" noValidate>
          {step === 1 && renderStep1()}
          {step === 2 && renderStep2()}
          {step === 3 && renderStep3()}

          <div className="form-actions">
            {step > 1 && (
              <button 
                type="button" 
                onClick={() => setStep(step - 1)}
                className="back-btn"
              >
                Back
              </button>
            )}
            
            {step < 3 ? (
              <button 
                type="button" 
                onClick={handleNextStep}
                className="next-btn"
              >
                Next
              </button>
            ) : (
              <>
                <button 
                  type="button" 
                  onClick={() => navigate('/courses/my-courses')}
                  className="cancel-btn"
                >
                  Cancel
                </button>
                <button 
                  type="submit" 
                  disabled={loading || !isValid}
                  className="submit-btn"
                >
                  {loading ? 'Updating...' : 'Update Course'}
                </button>
              </>
            )}
          </div>
        </form>
      </div>
    </div>
  );
};

export default UpdateCourse; 