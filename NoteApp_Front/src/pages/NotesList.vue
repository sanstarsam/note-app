<template>
  <div class="min-h-screen bg-gray-100 p-6">
    <div class="flex justify-between items-center mb-4">
      <h1 class="text-2xl font-bold">My Notes</h1>
      <button @click="addNote" class="bg-blue-600 text-white px-4 py-2 rounded-lg">
        + Add Note
      </button>
    </div>

    <input
      v-model="search"
      type="text"
      placeholder="Search notes..."
      class="w-full mb-4 px-4 py-2 border rounded-lg"
    />

    <table class="min-w-full border border-gray-200 rounded-lg">
      <thead class="bg-gray-100">
        <tr>
          <th>#</th>
          <th>Title</th>
          <th>Content</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody class="text-center">
        <tr v-for="(note, index) in filteredNotes" :key="note.id" class="border-t hover:bg-gray-50">
          <td>{{ index + 1 }}</td>
          <td>{{ note.title }}</td>
          <td>{{ note.content }}</td>
          <td>
            <button @click="edit(note)" class="text-blue-600 hover:underline mr-2">Edit</button>
            <button @click="remove(note.id)" class="text-red-600 hover:underline">Delete</button>
          </td>
        </tr>
      </tbody>
    </table>

    <!-- Reusable NoteForm for Add/Edit -->
    <NoteForm
      v-if="openForm"
      :note="currentNote"
      @close="openForm = false"
      @saved="handleSaved" 
    />
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from "vue";
import { useNotesStore } from "../store/useNotesStore";
import NoteForm from "../pages/NoteForm.vue";

const store = useNotesStore();
const search = ref("");
const openForm = ref(false);
const currentNote = ref(null);

onMounted(() => store.fetchNotes());

const filteredNotes = computed(() =>
  store.notes.filter(n =>
    n.title.toLowerCase().includes(search.value.toLowerCase())
  )
);

function edit(note) {
  currentNote.value = note; // set note for editing
  openForm.value = true;    // open modal
}

function addNote() {
  currentNote.value = null; // new note
  openForm.value = true;    // open modal
}

async function remove(id) {
  await store.deleteNote(id);
}

function handleSaved() {
  openForm.value = false;
  currentNote.value = null;
}
</script>