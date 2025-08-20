<template>
  <nav class="bg-white shadow-md">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div class="flex justify-between h-16">
        <div class="flex-shrink-0 flex items-center">
          <router-link to="/" class="text-2xl font-bold text-gray-800">Note Application</router-link>
        </div>
        <div class="hidden md:flex space-x-8 items-center">
          <router-link to="/notes" class="text-gray-700 hover:text-blue-500">Notes</router-link>
          <router-link to="/login" class="text-gray-700 hover:text-blue-500">Login</router-link>
        </div>
        <div class="flex items-center md:hidden">
          <button @click="isOpen = !isOpen" class="text-gray-700 hover:text-blue-500 focus:outline-none">
            <svg v-if="!isOpen" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
            </svg>
            <svg v-else class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"/>
            </svg>
          </button>
        </div>
      </div>
    </div>

    <div v-show="isOpen" class="md:hidden px-2 pt-2 pb-3 space-y-1">
      <router-link to="/notes" class="block text-gray-700 hover:text-blue-500">Notes</router-link>
      <router-link to="/login" class="block text-gray-700 hover:text-blue-500">Login</router-link>
    </div>
  </nav>
</template>

<script>
export default {
  name: "Navbar",
  data() {
    return { isOpen: false };
  }
};
</script>