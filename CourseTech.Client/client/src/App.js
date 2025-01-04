import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { AuthProvider } from './contexts/AuthContext';
import Navbar from './components/Navbar';
import Footer from './components/Footer';
import Home from './pages/Home';
import CourseDetail from './pages/CourseDetail';
import Basket from './pages/Basket';
import Login from './pages/Login';
import Register from './pages/Register';
import CategoryDetail from './pages/CategoryDetail';
import InstructorDetail from './pages/InstructorDetail';
import Categories from './pages/Categories';
import Courses from './pages/Courses';
import CreateCourse from './pages/CreateCourse';
import UpdateCourse from './pages/UpdateCourse';
import MyCourses from './pages/MyCourses';
import './App.css';

function App() {
  return (
    <AuthProvider>
      <Router>
        <div className="app">
          <Navbar />
          <div className="main-content">
            <Routes>
              <Route path="/" element={<Home />} />
              <Route path="/courses" element={<Courses />} />
              <Route path="/categories" element={<Categories />} />
              <Route path="/course/:slug" element={<CourseDetail />} />
              <Route path="/basket" element={<Basket />} />
              <Route path="/login" element={<Login />} />
              <Route path="/register" element={<Register />} />
              <Route path="/category/:slug" element={<CategoryDetail />} />
              <Route path="/instructor/:slug" element={<InstructorDetail />} />
              <Route path="/courses/create" element={<CreateCourse />} />
              <Route path="/courses/update/:id" element={<UpdateCourse />} />
              <Route path="/courses/edit/:id" element={<UpdateCourse />} />
              <Route path="/courses/my-courses" element={<MyCourses />} />
            </Routes>
          </div>
          <Footer />
        </div>
      </Router>
    </AuthProvider>
  );
}

export default App;