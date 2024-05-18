<template>
  <div class="login">
    <h2>Login</h2>
    <form @submit.prevent="login">
      <input v-model="email" type="email" placeholder="Email address" required>
      <input v-model="password" type="password" placeholder="Password" required>
      <button type="submit">Login</button>
    </form>
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
      password: ''
    }
  },
  methods: {
    async login() {
      const toast = useToast()

      try {
        const response = await axios.post('/account/login', {
          email: this.email,
          password: this.password
        })
        toast.success("Login successful")
      } catch (error) {
        console.error('Login error:', error.response)
        toast.error(error.response?.data?.message || "Login failed")
      }
    }
  }
}
</script>

<style>
body {
  background: linear-gradient(to bottom right, #0081A7, #00AFB9);
  font-family: 'Arial', sans-serif;
  color: #333;
}

.login {
  width: 100%;
  max-width: 340px;
  margin: 122px auto;
  padding: 2em;
  background: rgba(255, 255, 255, 0.9); /* Nền bán trong suốt */
  border-radius: 8px;
  box-shadow: 0 0 15px rgba(0, 0, 0, 0.1);
  text-align: center;
}

.login h2 {
  margin-bottom: 1.5em;
  color: #333;
  font-size: 24px;
}

.login input {
  width: calc(100% - 20px);
  padding: 10px;
  margin-bottom: 10px;
  border: 1px solid #ddd;
  border-radius: 5px;
  font-size: 14px;
}

.login input:focus {
  outline: none;
  border-color: #029ba3;
}

.login button {
  width: 100%;
  padding: 10px;
  margin-top: 1em;
  border: none;
  border-radius: 2px;
  background: linear-gradient(to bottom right, #0081A7, #00AFB9);
  color: white;
  font-size: 14px;
  font-weight: 600;
  cursor: pointer;
  transition: background 0.3s ease-in-out;
}

.login button:hover {
  background: linear-gradient(to bottom right, #007099, #0098A6);
}

.register-prompt {
  margin-top: 1em;
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
}
</style>
