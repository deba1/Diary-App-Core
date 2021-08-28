using Application.Exceptions;
using Application.Managers;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Extensions;

namespace WebAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly INoteManager _noteManager;

        public UserController(UserManager<IdentityUser> userManager, INoteManager noteManager)
        {
            _userManager = userManager;
            _noteManager = noteManager;
        }

        [HttpGet("")]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<UserViewModel>> GetAll()
        {
            var users = await _userManager.Users.ToListAsync();
            List<UserViewModel> allUsers = new();

            foreach (var user in users)
            {
                allUsers.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Username = user.UserName,
                    Roles = await _userManager.GetRolesAsync(user)
                });
            }

            return allUsers;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<UserViewModel> GetById([FromRoute] string id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            return user != null ? new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                Roles = await _userManager.GetRolesAsync(user)
            } : null;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete([FromRoute] string id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return NotFound();
            await _userManager.DeleteAsync(user);
            return NoContent();
        }


        [HttpGet("me/notes/{id}")]
        public async Task<ActionResult<Note>> GetNoteById(int id)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                List<Note> notes = ((List<Note>)await _noteManager.GetOwnExceptDeleted(userId));
                var note = notes.Find(n => n.Id.Equals(id));
                return note != null ? Ok(note) : NotFound();
            }
            catch (FeatureDisabledException)
            {
                return Forbid();
            }
        }

        [HttpGet("me/notes")]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var result = await _noteManager.GetOwnExceptDeleted(userId);
                return Ok(result);
            }
            catch (FeatureDisabledException)
            {
                return Forbid();
            }
        }
    }
}
