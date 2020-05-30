using ICafe.Application.Interfaces;
using ICafe.Domain.Entities;
using ICafe.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICafe.Application
{
    public class PhotoRepository: IRepository<Photo>
    {
        private readonly ICafeContext _context;

        public PhotoRepository(ICafeContext context)
        {
            _context = context;
        }

        public async Task<Photo> Add(Photo item)
        {
            await _context.Photos.AddAsync(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task<Photo> Delete(Photo item)
        {
            var photo = await this.Get(item);
            var result = _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Photo> Get(Photo item)
        {
            var order = await _context.Photos
                .FirstOrDefaultAsync(p => p.Id == item.Id);

            return order;
        }

        public async Task<ICollection<Photo>> Get(IFilter filter)
        {
            var orders = await _context.Photos
                .Skip(filter.Offset)
                .Take(filter.Limit).ToListAsync();

            return orders;
        }

        public async Task<Photo> Update(Photo item)
        {
            var result = _context.Photos.Update(item);
            await _context.SaveChangesAsync();

            return result.Entity;
        }
    }
}
