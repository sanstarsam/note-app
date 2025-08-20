<template>
  <div class="flex h-screen items-center justify-center bg-gray-100">
    <div class="w-full max-w-md bg-white p-6 rounded-2xl shadow">
      <h2 class="text-2xl font-bold mb-6 text-center">Login</h2>
      <form @submit.prevent="login">
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
          class="w-full bg-blue-600 hover:bg-blue-700 text-white py-2 rounded-lg"
        >
          Login
        </button>
      </form>
      <p class="text-sm text-center mt-4">
        Don’t have an account?
        <router-link to="/register" class="text-blue-600">Register</router-link>
      </p>
    </div>
  </div>
</template>

<script setup>
import { ref } from "vue";
import api from "../services/api";

const username = ref("");
const password = ref("");

async function login() {
  try {
    const { data } = await api.post("/auth/login", {
      username: username.value,
      password: password.value,
    });
    localStorage.setItem("token", data.token);
    window.location.href = "/notes";
  } catch (error) {
    alert("Login failed");
  }
}
</script>