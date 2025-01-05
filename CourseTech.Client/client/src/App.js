import React, { useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route, useNavigate } from 'react-router-dom';
import { AuthProvider, useAuth } from './contexts/AuthContext';
import { BasketProvider } from './contexts/BasketContext';
import { CategoryProvider } from './contexts/CategoryContext';
import { CourseProvider } from './contexts/CourseContext';
import { UserProvider } from './contexts/UserContext';
import { PaymentProvider } from './contexts/PaymentContext';
import { OrderProvider } from './contexts/OrderContext';
import { EnrollmentProvider } from './contexts/EnrollmentContext';
import Navbar from './components/Navbar';
import Footer from './components/Footer';
import Home from './pages/Home';
import Login from './pages/Login';
import Settings from './pages/Settings';
import Register from './pages/Register';
import Courses from './pages/Courses';
import PrivateRoute from './components/PrivateRoute';
import './styles/App.css';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import CourseDetail from './pages/CourseDetail';
import Basket from './pages/Basket';
import Order from './pages/Order';
import Payment from './pages/Payment';
import EnrolledCourses from './pages/EnrolledCourses';
import CategoryDetails from './pages/CategoryDetails';
import InstructorCourses from './pages/instructor/InstructorCourses';
import CreateCourse from './pages/instructor/CreateCourse';
import EditCourse from './pages/instructor/EditCourse';

const InstructorRoute = ({ children }) => {
  const { user } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    if (!user?.roles?.includes('Instructor')) {
      navigate('/');
    }
  }, [user, navigate]);

  return user?.roles?.includes('Instructor') ? children : null;
};

const App = () => {
  return (
    <AuthProvider>
      <UserProvider>
        <CategoryProvider>
          <CourseProvider>
            <PaymentProvider>
              <OrderProvider>
                <BasketProvider>
                  <EnrollmentProvider>
                    <Router>
                      <div className="app">
                        <ToastContainer
                          position="bottom-right"
                          autoClose={3000}
                          hideProgressBar={false}
                          newestOnTop
                          closeOnClick
                          rtl={false}
                          pauseOnFocusLoss
                          draggable
                          pauseOnHover
                          theme="light"
                        />
                        <Navbar />
                        <main className="main">
                          <Routes>
                            <Route path="/" element={<Home />} />
                            <Route path="/login" element={<Login />} />
                            <Route path="/register" element={<Register />} />
                            <Route path="/courses" element={<Courses />} />
                            <Route
                              path="/settings/*"
                              element={
                                <PrivateRoute>
                                  <Settings />
                                </PrivateRoute>
                              }
                            />
                            <Route path="/courses/:id" element={<CourseDetail />} />
                            <Route path="/basket" element={<Basket />} />
                            <Route path="/order" element={<Order />} />
                            <Route path="/payment" element={<Payment />} />
                            <Route
                              path="/my-learning"
                              element={
                                <PrivateRoute>
                                  <EnrolledCourses />
                                </PrivateRoute>
                              }
                            />
                            <Route path="/category/:categoryId" element={<CategoryDetails />} />
                            <Route
                              path="/instructor/courses"
                              element={
                                <InstructorRoute>
                                  <InstructorCourses />
                                </InstructorRoute>
                              }
                            />
                            <Route
                              path="/instructor/courses/create"
                              element={
                                <InstructorRoute>
                                  <CreateCourse />
                                </InstructorRoute>
                              }
                            />
                            <Route
                              path="/instructor/courses/edit/:id"
                              element={
                                <InstructorRoute>
                                  <EditCourse />
                                </InstructorRoute>
                              }
                            />
                          </Routes>
                        </main>
                        <Footer />
                      </div>
                    </Router>
                  </EnrollmentProvider>
                </BasketProvider>
              </OrderProvider>
            </PaymentProvider>
          </CourseProvider>
        </CategoryProvider>
      </UserProvider>
    </AuthProvider>
  );
};

export default App;