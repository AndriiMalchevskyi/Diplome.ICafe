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
    public class ProductRepository : IRepository<Product>
    {
        private readonly ICafeContext _context;

        public ProductRepository(ICafeContext context)
        {
            _context = context;
        }
        public async Task<Product> Add(Product item)
        {
            await _context.Products.AddAsync(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task<Product> Delete(Product item)
        {
            var product = await this.Get(item);
            var result = _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Product> Get(Product item)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.Id);

            return product;
        }

        public async Task<ICollection<Product>> Get(IFilter filter)
        {
            var products = await _context.Products.Skip(filter.Offset).Take(filter.Limit).ToListAsync();

            return products;
        }

        public async Task<Product> Update(Product item)
        {
            var result = _context.Products.Update(item);
            await _context.SaveChangesAsync();

            return result.Entity;
        }
    }
}
