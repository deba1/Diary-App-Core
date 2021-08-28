using Application.Exceptions;
using Application.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Extensions;

namespace WebAPI.Controllers
{
    [Route("api/trashes")]
    [ApiController]
    [Authorize]
    public class TrashController : ControllerBase
    {
        private readonly INoteManager _noteManager;

        public TrashController(INoteManager noteManager)
        {
            _noteManager = noteManager;
        }

        [HttpGet("notes")]
        public async Task<ActionResult> GetTrashedNotes()
        {
            try
            {
                string userId = HttpContext.GetUserId();
                return Ok(await _noteManager.GetOwnTrashed(userId));
            }
            catch (FeatureDisabledException)
            {
                return Forbid();
            }
        }

        [HttpPatch("notes/{noteId}")]
        public async Task<ActionResult> RecoverTrashedNotes([FromRoute] int noteId)
        {
            try
            {
                var note = await _noteManager.GetById(noteId);
                if (note == null)
                    return NotFound();

                if (note.UserId != HttpContext.GetUserId())
                    return Forbid();

                note.Deleted = false;

                return Ok(await _noteManager.Update(note));
            }
            catch (FeatureDisabledException)
            {
                return Forbid();
            }
        }
    }
}
