using Common.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public interface INoteRepository
    {
        Task<Note> Create(Note note);
        Task<bool> Delete(int id);
        Task<IEnumerable<Note>> GetAll();
        Task<IEnumerable<Note>> GetAllByUserId(string userId);
        Task<IEnumerable<Note>> GetDeleted();
        Task<IEnumerable<Note>> GetDeletedByUserId(string userId);
        Task<Note> GetById(int id);
        Task<bool> Update(Note note);
    }

    public class NoteRepository : INoteRepository
    {
        private readonly AppDbContext appDbContext;

        public NoteRepository(AppDbContext _appDbContext)
        {
            appDbContext = _appDbContext;
        }

        public async Task<IEnumerable<Note>> GetAll()
        {
            return await appDbContext.Notes.Where(n => !n.Deleted).ToListAsync();
        }

        public async Task<IEnumerable<Note>> GetDeleted()
        {
            return await appDbContext.Notes.Where(n => n.Deleted).ToListAsync();
        }

        public async Task<Note> GetById(int id)
        {
            return await appDbContext.Notes.FindAsync(id);
        }

        public async Task<Note> Create(Note note)
        {
            note.CreatedAt = DateTime.Now;
            appDbContext.Add(note);
            await appDbContext.SaveChangesAsync();
            return note;
        }

        public async Task<bool> Update(Note note)
        {
            appDbContext.Update(note);

            await appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var noteFound = await GetById(id);

            if (noteFound == null)
                return false;

            noteFound.Deleted = true;
            await appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Note>> GetAllByUserId(string userId)
        {
            return await appDbContext.Notes.Where(n => !n.Deleted && n.UserId.Equals(userId)).ToListAsync();
        }

        public async Task<IEnumerable<Note>> GetDeletedByUserId(string userId)
        {
            return await appDbContext.Notes.Where(n => n.Deleted && n.UserId.Equals(userId)).ToListAsync();
        }
    }
}
