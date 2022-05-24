using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Persistence.Repositories
{
    public class ImageRepositoryAsync : GenericRepositoryAsync<Image>, IImageRepositoryAsync
    {
        private readonly DbSet<Image> _images;

        public ImageRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
          _images = dbContext.Set<Image>();
        }

    //    public async Task<IReadOnlyList<Image>> GetImagesByCommunityIdAsync(int communityId, int pageNumber, int pageSize)
    //    {
    //        return await _notifications
    //            .Where(n => n.CommunityId == communityId)
    //            .Skip((pageNumber - 1) * pageSize)
    //            .Take(pageSize)
    //            .AsNoTracking()
    //            .ToListAsync();
    //     }

    //    public async Task<IReadOnlyList<Image>> GetImagesWithRelationsAsync(int pageNumber, int pageSize)
    //    {
    //        return await _notifications
    //            .Include(e => e.Community)
    //            .Skip((pageNumber - 1) * pageSize)
    //            .Take(pageSize)
    //            .AsNoTracking()
    //            .ToListAsync();
    //    }
    }
}
