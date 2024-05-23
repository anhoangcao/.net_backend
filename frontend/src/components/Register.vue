<template>
  <div class="register">
    <div class="close-button">
      ×
    </div>
    <h2>Register</h2>
    <form @submit.prevent="register">
      <div class="input-wrapper">
        <label v-if="errors.name" class="error">{{ errors.name }}</label>
        <input v-model="name" type="text" placeholder="Name" @input="validateField('name')" :class="{ 'is-invalid': errors.name }">
      </div>
      <div class="input-wrapper">
        <label v-if="errors.email" class="error">{{ errors.email }}</label>
        <input v-model="email" type="email" placeholder="Email address" @input="validateField('email')" :class="{ 'is-invalid': errors.email }">
      </div>
      <div class="input-wrapper">
        <label v-if="errors.password" class="error">{{ errors.password }}</label>
        <input v-model="password" type="password" placeholder="Password" @input="validateField('password')" :class="{ 'is-invalid': errors.password }">
      </div>
      <div class="input-wrapper">
        <label v-if="errors.confirmPassword" class="error">{{ errors.confirmPassword }}</label>
        <input v-model="confirmPassword" type="password" placeholder="Confirm password" @input="validateField('confirmPassword')" :class="{ 'is-invalid': errors.confirmPassword }">
      </div>
      <button type="submit">Register</button>
    </form>
    <div class="separator"></div>
    <p class="login-prompt">
      Don't have an account? <a href="/login">Login</a>
    </p>
  </div>
</template>

<script>
import { useToast } from 'vue-toastification'
import axios from 'axios'

export default {
  name: 'Register',
  data() {
    return {
      name: '',
      email: '',
      password: '',
      confirmPassword: '',
      errors: {
        name: '',
        email: '',
        password: '',
        confirmPassword: ''
      }
    }
  },
  methods: {
    async register() {
      const toast = useToast();
      this.clearErrors();

      this.validateAllFields();

      if (this.password !== this.confirmPassword) {
        this.errors.confirmPassword = "Passwords do not match";
      }

      if (this.hasErrors()) {
        return;
      }

      try {
        const response = await axios.post('/account/register', {
          name: this.name,
          email: this.email,
          password: this.password
        });
        toast.success("Registration successful");

        localStorage.setItem('userToken', response.data.token);
        localStorage.setItem('userName', response.data.data.name); 
        
        setTimeout(() => {
          this.$router.push('/login');
        }, 2000);
      } catch (error) {
        console.error('Registration error:', error.response);
        const errorMessage = error.response?.data?.message || "Registration failed";
        toast.error(errorMessage);
      }
    },

    clearErrors() {
      this.errors = {
        name: '',
        email: '',
        password: '',
        confirmPassword: ''
      }
    },
    hasErrors() {
      return Object.values(this.errors).some(error => error)
    },
    async validateField(field) {
      if (!this[field]) {
        this.errors[field] = 'Please fill in this field'
      } else if (field === 'email' && !this.isValidEmail(this.email)) {
        this.errors[field] = 'Invalid email format'
      } else if (field === 'email') {
        try {
          const response =  await axios.post('/account/check-email', {
            email: this.email
          })
          if (response.data.exists) {
            this.errors[field] = 'Email already exists'
          } else {
            this.errors[field] = ''
          }
        } catch (error) {
          console.error('Email validation error:', error)
          this.errors[field] = 'Error validating email'
        }
      } else {
        this.errors[field] = ''
      }
    },
    validateAllFields() {
      this.validateField('name')
      this.validateField('email')
      this.validateField('password')
      this.validateField('confirmPassword')
    },
    isValidEmail(email) {
      const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@(([^<>()[\]\.,;:\s@"]+\.)+[^<>()[\]\.,;:\s@"]{2,})$/i
      return re.test(String(email).toLowerCase())
    }
  }
}
</script>

<style>
.register {
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

.register .close-button {
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

.register .close-button:hover {
  background-color: #e0e0e0;
}

.register h2 {
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

.register input {
  width: calc(100% - 25px);
  padding: 12px;
  margin-bottom: 15px;
  border: 1px solid #ddd;
  border-radius: 5px;
  font-size: 15px;
}

.register input:focus {
  outline: none;
  border-color: #029ba3;
}

.register input.is-invalid {
  border-color: red;
}

.register button {
  width: 100%;
  padding: 12px;
  margin-top: 1em;
  border: none;
  border-radius: 2px;
  background: linear-gradient(to bottom right, #0081A7, #00AFB9);
  color: white;
  font-size: 16px;
  font-weight: 600;
  cursor: pointer;
  transition: background 0.3s ease-in-out;
}

.register button:hover {
  background: linear-gradient(to bottom right, #007099, #0098A6);
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

.login-prompt {
  padding-top: 1.5em;
  font-size: 14px;
  color: #333;
}

.login-prompt a {
  color: #007BFF;
  text-decoration: none;
  font-weight: 600;
}

.login-prompt a:hover {
  text-decoration: underline;
}

@media (max-width: 600px) {
  .register {
    padding: 1.5em;
  }
  .register h2 {
    font-size: 20px;
  }
  .register input,
  .register button {
    font-size: 14px;
  }
}
</style>
