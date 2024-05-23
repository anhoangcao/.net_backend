<template>
  <div class="login">
    <div class="close-button">
      × 
    </div>
    <h2>Login</h2>
    <form @submit.prevent="login">
      <div class="input-wrapper">
        <label v-if="errors.email" class="error">{{ errors.email }}</label>
        <input v-model="email" type="email" placeholder="Email address" @input="validateField('email')" :class="{ 'is-invalid': errors.email }">
      </div>
      <div class="input-wrapper">
        <label v-if="errors.password" class="error">{{ errors.password }}</label>
        <input v-model="password" type="password" placeholder="Password" @input="validateField('password')" :class="{ 'is-invalid': errors.password }">
      </div>
      <button type="submit">Login</button>
    </form>
    <div class="separator"></div>
    <p class="register-prompt">
      Don't have an account? <a href="/register">Register</a>
    </p>
  </div>
</template>

<script>
import { useToast } from 'vue-toastification'
import axios from 'axios'

export default {
  name: 'Login',
  data() {
    return {
      email: '',
      password: '',
      errors: {
        email: '',
        password: '',
      }
    }
  },
  methods: {
    async login() {
      const toast = useToast();
      this.clearErrors();

      this.validateAllFields();

      if (this.hasErrors()) {
        return;
      }

      try {
        const response = await axios.post('/account/login', {
          email: this.email,
          password: this.password
        });
        toast.success("Login successful");

        localStorage.setItem('userToken', response.data.token);
        localStorage.setItem('userName', response.data.data.name); // Lưu userName vào localStorage

        setTimeout(() => {
          this.$router.push('/users');
        }, 2000);
      } catch (error) {
        console.error('Login error:', error.response);
        toast.error(error.response?.data?.message || "Login failed");
      }
    },

    clearErrors() {
      this.errors = {
        email: '',
        password: ''
      };
    },
    hasErrors() {
      return Object.values(this.errors).some(error => error);
    },
    validateField(field) {
      if (!this[field]) {
        this.errors[field] = 'Please fill in this field';
      } else if (field === 'email' && !this.isValidEmail(this.email)) {
        this.errors[field] = 'Invalid email format';
      } else {
        this.errors[field] = '';
      }
    },
    validateAllFields() {
      this.validateField('email');
      this.validateField('password');
    },
    isValidEmail(email) {
      const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@(([^<>()[\]\.,;:\s@"]+\.)+[^<>()[\]\.,;:\s@"]{2,})$/i;
      return re.test(String(email).toLowerCase());
    }
  }
}
</script>

<style>

.login {
  width: 100%;
  max-width: 310px;
  margin: 70px auto;
  padding: 2em 2em 1em 2em;
  background: rgba(255, 255, 255, 0.9);
  border-radius: 5px;
  box-shadow: 0 0 15px rgba(0, 0, 0, 0.1);
  text-align: center;
  position: relative;
}

.login .close-button {
  position: absolute;
  top: 10px;
  right: 10px;
  background-color: rgba(255, 255, 255, 0.9);
  border: none;
  border-radius: 50%;
  width: 30px;
  height: 30px;
  font-size: 20px;
  font-weight: bold;
  color: #333;
  cursor: pointer;
  text-align: center;
  line-height: 30px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  transition: background-color 0.3s ease;
}

.login .close-button:hover {
  background-color: #e0e0e0;
}

.login h2 {
  margin-bottom: 1em;
  color: #333;
  font-size: 28px;
}

.input-wrapper {
  position: relative;
  text-align: left;
}

.error {
  position: absolute;
  color: red;
  font-size: 11px;
  display: block;
  margin-top: -14px;
}

.login input {
  width: calc(100% - 25px);
  padding: 12px;
  margin-bottom: 15px;
  border: 1px solid #ddd;
  border-radius: 5px;
  font-size: 15px;
}

.login input:focus {
  outline: none;
  border-color: #029ba3;
}

.login input.is-invalid {
  border-color: red;
}

.login button {
  width: 100%;
  padding: 12px;
  margin-top: 1em;
  margin-bottom: 7.25em;
  border: none;
  border-radius: 2px;
  background: linear-gradient(to bottom right, #0081A7, #00AFB9);
  color: white;
  font-size: 16px;
  font-weight: 600;
  cursor: pointer;
  transition: background 0.3s ease-in-out;
}

.login button:hover {
  background: linear-gradient(to bottom right, #007099, #0098A6);
}

.register-prompt {
  padding-top: 1.5em;
  font-size: 14px;
  color: #333;
}

.register-prompt a {
  color: #007BFF;
  text-decoration: none;
  font-weight: 600;
}

.register-prompt a:hover {
  text-decoration: underline;
}

.separator {
  margin-top: 2em;
  color: #333;
  position: relative;
}

.separator::before {
  content: '';
  position: absolute;
  top: 50%;
  left: -10%;
  right: -10%;
  border-top: 1px solid #ddd;
  z-index: 1;
  max-width: 372px;
}

@media (max-width: 600px) {
  .login {
    padding: 1.5em;
  }
  .login h2 {
    font-size: 20px;
  }
  .login input,
  .login button {
    font-size: 14px;
  }
  .separator::before {
    content: '';
    position: absolute;
    top: 50%;
    left: -24px;
    right: -10%;
    border-top: 1px solid #ddd;
    z-index: 1;
    max-width: 358px;
  }
}
</style>
