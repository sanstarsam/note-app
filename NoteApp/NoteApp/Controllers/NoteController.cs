using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoteApp.Models;
using NoteModelApp.Repositories;
using System.Security.Claims;

namespace NoteApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NoteController : ControllerBase
    {
        private readonly INoteRepository _notes;

        public NoteController(INoteRepository notes)
        {
            _notes = notes;
        }

        // Helper method to extract UserId from JWT
        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        }

        // GET: api/notes
        [HttpGet]
        public async Task<IActionResult> GetNotes()
        {
            var notes = await _notes.GetNotesByUserAsync(GetUserId());
            return Ok(notes);
        }

        // GET: api/notes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNote(int id)
        {
            var note = await _notes.GetNoteByIdAsync(id, GetUserId());
            if (note == null)
                return NotFound("Note not found");

            return Ok(note);
        }

        // POST: api/notes
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Note note)
        {
            note.user_id = GetUserId();
            var newId = await _notes.CreateNoteAsync(note);
            return Ok(new { Id = newId });
        }

        // PUT: api/notes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Note note)
        {
            note.id = id;
            note.user_id = GetUserId();
            var success = await _notes.UpdateNoteAsync(note);

            return success ? Ok("Note updated") : NotFound("Note not found or not yours");
        }

        // DELETE: api/notes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _notes.DeleteNoteAsync(id, GetUserId());
            return success ? Ok("Note deleted") : NotFound("Note not found or not yours");
        }
    }
}
