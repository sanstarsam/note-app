import { defineStore } from "pinia";
import api from "../services/api";

export const useNotesStore = defineStore("note", {
  state: () => ({
    notes: [],
    loading: false,
  }),
  actions: {
    async fetchNotes() {
      this.loading = true;
      try {
        const { data } = await api.get("/note");
        this.notes = data; // reactive array
      } finally {
        this.loading = false;
      }
    },

    async addNote(note) {
        const { data } = await api.post("/note", note);
        this.notes = [...this.notes, data]; // triggers reactivity
        return data;
    },

    async updateNote(id, updatedNote) {
        const { data } = await api.put(`/note/${id}`, updatedNote);
        this.notes = this.notes.map(n => (n.id === id ? data : n)); // triggers reactivity
    },

    async deleteNote(id) {
      await api.delete(`/note/${id}`);
      this.notes = this.notes.filter(n => n.id !== id);
    },
  },
});