using Application.Exceptions;
using Common.Enums;
using Common.Models;
using Repository.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Managers
{
    public interface INoteManager
    {
        Task<Note> Create(Note note);
        Task<int> SoftDelete(int id);
        Task<IEnumerable<Note>> GetAllExceptDeleted();
        Task<Note> GetById(int id);
        Task<IEnumerable<Note>> GetOwnExceptDeleted(string userId);
        Task<IEnumerable<Note>> GetOwnTrashed(string userId);
        Task<IEnumerable<Note>> GetTrashed();
        Task<int> Update(Note note);
    }

    public class NoteManager : INoteManager
    {
        private readonly INoteRepository noteRepository;
        private readonly ISettingRepository settingRepository;

        public NoteManager(INoteRepository _noteRepository, ISettingRepository _settingRepository)
        {
            noteRepository = _noteRepository;
            settingRepository = _settingRepository;
        }

        public async Task<IEnumerable<Note>> GetAllExceptDeleted()
        {
            if (await settingRepository.IsEnabled(SettingName.ViewNote))
                return await noteRepository.GetAll();
            throw new FeatureDisabledException();
        }

        public async Task<IEnumerable<Note>> GetOwnExceptDeleted(string userId)
        {
            if (await settingRepository.IsEnabled(SettingName.ViewNote))
                return await noteRepository.GetAllByUserId(userId);
            throw new FeatureDisabledException();
        }

        public async Task<IEnumerable<Note>> GetTrashed()
        {
            if (await settingRepository.IsEnabled(SettingName.ViewTrashNote))
                return await noteRepository.GetDeleted();
            throw new FeatureDisabledException();
        }

        public async Task<IEnumerable<Note>> GetOwnTrashed(string userId)
        {
            if (await settingRepository.IsEnabled(SettingName.ViewTrashNote))
                return await noteRepository.GetDeletedByUserId(userId);
            throw new FeatureDisabledException();
        }

        public async Task<Note> GetById(int id)
        {
            if (await settingRepository.IsEnabled(SettingName.ViewNote))
                return await noteRepository.GetById(id);
            throw new FeatureDisabledException();
        }

        public async Task<Note> Create(Note note)
        {
            if (await settingRepository.IsEnabled(SettingName.CreateNote))
                return await noteRepository.Create(note);
            throw new FeatureDisabledException();
        }

        public async Task<int> Update(Note note)
        {
            if (!await settingRepository.IsEnabled(SettingName.UpdateNote))
                throw new FeatureDisabledException();

            await noteRepository.Update(note);
            return 1;
        }

        public async Task<int> SoftDelete(int id)
        {
            if (!await settingRepository.IsEnabled(SettingName.DeleteNote))
                throw new FeatureDisabledException();
            return await noteRepository.Delete(id) ? 1 : 0;
        }
    }
}
