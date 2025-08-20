<template>
  <div class="flex h-screen items-center justify-center bg-gray-100">
    <div class="w-full max-w-md bg-white p-6 rounded-2xl shadow">
      <h2 class="text-2xl font-bold mb-6 text-center">Register</h2>
      <form @submit.prevent="register">
        <input
          v-model="username"
          type="text"
          placeholder="Username"
          class="w-full mb-4 px-4 py-2 border rounded-lg"
        />
        <input
          v-model="password"
          type="password"
          placeholder="Password"
          class="w-full mb-4 px-4 py-2 border rounded-lg"
        />
        <button
          type="submit"
          class="w-full bg-green-600 hover:bg-green-700 text-white py-2 rounded-lg"
        >
          Register
        </button>
      </form>
      <p class="text-sm text-center mt-4">
        Already have an account?
        <router-link to="/login" class="text-blue-600">Login</router-link>
      </p>
    </div>
  </div>
</template>

<script setup>
import { ref } from "vue";
import api from "../services/api";

const username = ref("");
const password = ref("");

async function register() {
  try {
    await api.post("/auth/register", {
      username: username.value,
      password: password.value,
    });
    alert("Registration successful! Please login.");
    window.location.href = "/login";
  } catch (error) {
    alert("Registration failed");
  }
}
</script>