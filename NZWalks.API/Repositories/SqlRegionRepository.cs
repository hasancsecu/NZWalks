using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class SqlRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext _dbContext;

        public SqlRegionRepository(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Region> CreateAsync(Region region)
        {
            await _dbContext.Regions.AddAsync(region);
            await _dbContext.SaveChangesAsync();

            return region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var regionModel = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (regionModel == null)
            {
                return null;
            }
            _dbContext.Regions.Remove(regionModel);
            await _dbContext.SaveChangesAsync();

            return regionModel;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await _dbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetByIdAsync(Guid id)
        {
            return await _dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
            var regionModel = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (regionModel == null)
            {
                return null;
            }
            regionModel.Name = region.Name;
            regionModel.RegionImageUrl = region.RegionImageUrl;

            await _dbContext.SaveChangesAsync();

            return regionModel;
        }
    }
}
