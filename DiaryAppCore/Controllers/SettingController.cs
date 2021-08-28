using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/settings")]
    [ApiController]
    [Authorize]
    public class SettingController : ControllerBase
    {
        private readonly ISettingRepository _settingRepository;
        public SettingController(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        [HttpGet("")]
        public async Task<IEnumerable<Setting>> Get()
        {
            return await _settingRepository.GetAll();
        }

        [HttpGet("{settingName}")]
        public async Task<ActionResult<Setting>> GetById([FromRoute] string settingName)
        {
            var note = await _settingRepository.GetById(settingName);
            return note != null ? Ok(note) : NotFound();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update([FromRoute] string id, [FromBody] Setting setting)
        {
            var found = await _settingRepository.GetById(id);

            if (found == null)
                return NotFound();

            found.Status = setting.Status;

            await _settingRepository.Update(found);
            return NoContent();
        }
    }
}
