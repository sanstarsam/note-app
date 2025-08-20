<template>
  <div class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
    <div class="bg-white w-full max-w-md p-6 rounded-xl shadow">
      <h2 class="text-xl font-bold mb-4">{{ note ? "Edit Note" : "New Note" }}</h2>
      <form @submit.prevent="save">
        <input
          v-model="title"
          type="text"
          placeholder="Title"
          class="w-full mb-4 px-4 py-2 border rounded-lg"
        />
        <textarea
          v-model="content"
          placeholder="Content"
          class="w-full mb-4 px-4 py-2 border rounded-lg"
        ></textarea>
        <div class="flex justify-end gap-2">
          <button
            @click="$emit('close')"
            type="button"
            class="px-4 py-2 rounded-lg bg-gray-300"
          >
            Cancel
          </button>
          <button
            type="submit"
            class="px-4 py-2 rounded-lg bg-green-600 text-white flex items-center gap-2"
            :disabled="loading"
          >
            <span v-if="loading" class="loader-border animate-spin rounded-full border-2 w-4 h-4"></span>
            Save
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, watch } from "vue";
import { useNotesStore } from "../store/useNotesStore";

const props = defineProps({
  note: { type: Object, default: null }
});

const emit = defineEmits(["close", "saved"]);

const store = useNotesStore();

const title = ref(props.note?.title || "");
const content = ref(props.note?.content || "");

watch(() => props.note, (newNote) => {
  title.value = newNote?.title || "";
  content.value = newNote?.content || "";
});

const loading = ref(false);

async function save() {
  if (!title.value.trim()) return;

  loading.value = true;
  try {
    if (props.note) {
      await store.updateNote(props.note.id, { title: title.value, content: content.value });
    } else {
      await store.addNote({ title: title.value, content: content.value });
    }

    emit("saved");  // notify parent
    emit("close");   // close modal
  } finally {
    loading.value = false;
  }
}
</script>

<style>
/* Simple spinner inside button */
.loader-border {
  border-top-color: transparent;
  border-right-color: transparent;
  border-bottom-color: white;
  border-left-color: white;
  width: 16px;
  height: 16px;
  display: inline-block;
}
</style>