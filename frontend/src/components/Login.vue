<template>
  <div class="login">
    <div class="close-button">
      ×
    </div>
    <h2>Login</h2>
    <form @submit.prevent="login">
      <div class="input-wrapper">
        <label v-if="errors.email" class="error">{{ errors.email }}</label>
        <input v-model="email" type="email" placeholder="Email address" @input="validateField('email')"
          :class="{ 'is-invalid': errors.email }">
      </div>
      <div class="input-wrapper">
        <label v-if="errors.password" class="error">{{ errors.password }}</label>
        <input v-model="password" type="password" placeholder="Password" @input="validateField('password')"
          :class="{ 'is-invalid': errors.password }">
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
        localStorage.setItem('userName', response.data.data.name); 

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

<style src="@/assets/style.css"></style>
