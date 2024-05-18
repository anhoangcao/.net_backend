<template>
  <div class="register">
    <h2>Register</h2>
    <form @submit.prevent="register">
      <input v-model="name" type="text" placeholder="Name" required>
      <input v-model="email" type="email" placeholder="Email address" required>
      <input v-model="password" type="password" placeholder="Password" required>
      <input v-model="confirmPassword" type="password" placeholder="Confirm password" required>
      <button type="submit">Register</button>
    </form>
    <p class="login-prompt">
      Already have an account? <a href="/login">Login</a>
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
      confirmPassword: ''
    }
  },
  methods: {
    async register() {
      const toast = useToast()

      if (this.password !== this.confirmPassword) {
        toast.error("Passwords do not match")
        return
      }

      try {
        const response = await axios.post('/account/register', {
          name: this.name,
          email: this.email,
          password: this.password
        })
        toast.success("Registration successful")
      } catch (error) {
        console.error('Registration error:', error.response)
        const errorMessage = error.response?.data?.message || "Registration failed"
        toast.error(errorMessage)
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

.register {
  width: 100%;
  max-width: 340px;
  margin: 74px auto;
  padding: 2em;
  background: rgba(255, 255, 255, 0.9);
  border-radius: 8px;
  box-shadow: 0 0 15px rgba(0, 0, 0, 0.1);
  text-align: center;
}

.register h2 {
  margin-bottom: 1.5em;
  color: #333;
  font-size: 24px;
}

.register input {
  width: calc(100% - 20px);
  padding: 10px;
  margin-bottom: 10px;
  border: 1px solid #ddd;
  border-radius: 5px;
  font-size: 14px;
}

.register input:focus {
  outline: none;
  border-color: #029ba3;
}

.register button {
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

.register button:hover {
  background: linear-gradient(to bottom right, #007099, #0098A6);
}

.login-prompt {
  margin-top: 1em;
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
