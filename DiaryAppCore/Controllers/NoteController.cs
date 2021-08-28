using Application.Exceptions;
using Application.Managers;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Extensions;

namespace WebAPI.Controllers
{
    [Route("api/notes")]
    [ApiController]
    [Authorize]
    public class NoteController : ControllerBase
    {
        private readonly INoteManager _noteManager;
        public NoteController(INoteManager noteManager)
        {
            _noteManager = noteManager;
        }

        [HttpGet("")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Note>>> Get()
        {
            try
            {
                var result = await _noteManager.GetAllExceptDeleted();
                return Ok(result);
            }
            catch (FeatureDisabledException)
            {
                return Forbid();
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Note>> GetById(int id)
        {
            try
            {
                var note = await _noteManager.GetById(id);
                return note != null ? Ok(note) : NotFound();
            }
            catch (FeatureDisabledException)
            {
                return Forbid();
            }
        }

        [HttpPost("")]
        public async Task<ActionResult> Create(Note note)
        {
            try
            {
                note.UserId = HttpContext.GetUserId();
                var newNote = await _noteManager.Create(note);
                return CreatedAtAction(nameof(GetById), new { id = newNote.Id }, newNote);
            }
            catch (FeatureDisabledException)
            {
                return Forbid();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] Note note)
        {
            try
            {
                var noteFound = await _noteManager.GetById(id);

                if (noteFound == null)
                    return NotFound();

                if (noteFound.UserId != HttpContext.GetUserId())
                    return Forbid();

                noteFound.Title = note.Title;
                noteFound.Body = note.Body;
                noteFound.Date = note.Date;

                await _noteManager.Update(note);
                return NoContent();
            }
            catch (FeatureDisabledException)
            {
                return Forbid();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            try
            {
                if (!HttpContext.HasUserRoles("Admin"))
                {
                    var note = await _noteManager.GetById(id);

                    if (note == null)
                        return NotFound();

                    if (!note.UserId.Equals(HttpContext.GetUserId()))
                        return Forbid();
                }
                await _noteManager.SoftDelete(id);
                return NoContent();
            }
            catch (FeatureDisabledException)
            {
                return Forbid();
            }
        }
    }
}
