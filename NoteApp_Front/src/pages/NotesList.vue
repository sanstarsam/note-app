<template>
  <div class="min-h-screen bg-gray-100 p-6">
    <div class="flex justify-between items-center mb-4">
      <h1 class="text-2xl font-bold">My Notes</h1>
      <button
        @click="addNote"
        class="bg-blue-600 text-white px-4 py-2 rounded-lg"
      >
        + Add Note
      </button>
    </div>

    <!-- Search -->
    <input
      v-model="search"
      type="text"
      placeholder="Search notes..."
      class="w-full mb-4 px-4 py-2 border rounded-lg"
    />

    <!-- Table -->
    <table class="min-w-full border border-gray-200 rounded-lg">
      <thead class="bg-gray-100">
        <tr>
          <th @click="sort('id')" class="cursor-pointer">
            #
            <span v-if="sortKey === 'id'">{{ sortAsc ? "▲" : "▼" }}</span>
          </th>
          <th @click="sort('title')" class="cursor-pointer">
            Title
            <span v-if="sortKey === 'title'">{{ sortAsc ? "▲" : "▼" }}</span>
          </th>
          <th @click="sort('content')" class="cursor-pointer">
            Content
            <span v-if="sortKey === 'content'">{{ sortAsc ? "▲" : "▼" }}</span>
          </th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody class="text-center">
        <tr
          v-for="(note, index) in paginatedNotes"
          :key="note.id"
          class="border-t hover:bg-gray-50"
        >
          <td>{{ index + 1 + (currentPage - 1) * perPage }}</td>
          <td>{{ note.title }}</td>
          <td>{{ note.content }}</td>
          <td>
            <button
              @click="edit(note)"
              class="text-blue-600 hover:underline mr-2"
            >
              Edit
            </button>
            <button
              @click="remove(note.id)"
              class="text-red-600 hover:underline"
            >
              Delete
            </button>
          </td>
        </tr>
      </tbody>
    </table>

    <!-- Pagination -->
    <div class="flex justify-between items-center mt-4">
      <button
        :disabled="currentPage === 1"
        @click="currentPage--"
        class="px-3 py-1 border rounded disabled:opacity-50"
      >
        Prev
      </button>
      <span>Page {{ currentPage }} / {{ totalPages }}</span>
      <button
        :disabled="currentPage === totalPages"
        @click="currentPage++"
        class="px-3 py-1 border rounded disabled:opacity-50"
      >
        Next
      </button>
    </div>

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

// DataTable state
const sortKey = ref("");
const sortAsc = ref(true);
const currentPage = ref(1);
const perPage = 5;

onMounted(() => store.fetchNotes());

/* Filtering */
const filteredNotes = computed(() =>
  store.notes.filter((n) =>
    n.title.toLowerCase().includes(search.value.toLowerCase())
  )
);

/* Sorting */
const sortedNotes = computed(() => {
  if (!sortKey.value) return filteredNotes.value;
  return [...filteredNotes.value].sort((a, b) => {
    const valA = a[sortKey.value]?.toString().toLowerCase() || "";
    const valB = b[sortKey.value]?.toString().toLowerCase() || "";
    return sortAsc.value ? valA.localeCompare(valB) : valB.localeCompare(valA);
  });
});

/* Pagination */
const totalPages = computed(() =>
  Math.ceil(sortedNotes.value.length / perPage)
);
const paginatedNotes = computed(() => {
  const start = (currentPage.value - 1) * perPage;
  return sortedNotes.value.slice(start, start + perPage);
});

/* Actions */
function sort(key) {
  if (sortKey.value === key) {
    sortAsc.value = !sortAsc.value;
  } else {
    sortKey.value = key;
    sortAsc.value = true;
  }
}

function edit(note) {
  currentNote.value = note;
  openForm.value = true;
}

function addNote() {
  currentNote.value = null;
  openForm.value = true;
}

async function remove(id) {
  await store.deleteNote(id);
}

function handleSaved() {
  openForm.value = false;
  currentNote.value = null;
}
</script>