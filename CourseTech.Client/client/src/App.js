import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { AuthProvider } from './contexts/AuthContext';
import { BasketProvider } from './contexts/BasketContext';
import { CategoryProvider } from './contexts/CategoryContext';
import { CourseProvider } from './contexts/CourseContext';
import { UserProvider } from './contexts/UserContext';
import { PaymentProvider } from './contexts/PaymentContext';
import { OrderProvider } from './contexts/OrderContext';
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

const App = () => {
  return (
    <AuthProvider>
      <UserProvider>
        <CategoryProvider>
          <CourseProvider>
            <PaymentProvider>
              <OrderProvider>
                <BasketProvider>
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
                        </Routes>
                      </main>
                      <Footer />
                    </div>
                  </Router>
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