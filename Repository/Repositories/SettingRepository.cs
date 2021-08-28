using Common.Enums;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public interface ISettingRepository
    {
        Task<IEnumerable<Setting>> GetAll();
        Task<Setting> GetById(string settingName);
        Task<bool> Update(Setting setting);
        Task<bool> IsEnabled(SettingName setting);
    }

    public class SettingRepository : ISettingRepository
    {
        private readonly AppDbContext appDbContext;

        public SettingRepository(AppDbContext _appDbContext)
        {
            appDbContext = _appDbContext;
        }

        public async Task<IEnumerable<Setting>> GetAll()
        {
            return await appDbContext.Settings.ToListAsync();
        }

        public async Task<Setting> GetById(string name)
        {
            return await appDbContext.Settings.FindAsync(name);
        }

        public async Task<bool> IsEnabled(SettingName setting)
        {
            return (await GetById(setting.ToString()))?.Status == 1;
        }

        public async Task<bool> Update(Setting setting)
        {
            appDbContext.Update(setting);
            await appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
