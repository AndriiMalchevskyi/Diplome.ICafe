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
    public class OrderRepository : IRepository<Order>
    {
        private readonly ICafeContext _context;

        public OrderRepository(ICafeContext context)
        {
            _context = context;
        }

        public async Task<Order> Add(Order item)
        {
            await _context.Orders.AddAsync(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task<Order> Delete(Order item)
        {
            var order = await this.Get(item);
            var result = _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Order> Get(Order item)
        {
            var order = await _context.Orders
                .Include(u => u.Owner)
                .Include(u => u.ProductOrders)
                .ThenInclude(o => o.Product)
                .FirstOrDefaultAsync(p => p.Id == item.Id);

            return order;
        }

        public async Task<ICollection<Order>> Get(IFilter filter)
        {
            var orders = await _context.Orders
                .Include(u => u.Owner)
                .Include(u => u.ProductOrders)
                .ThenInclude(o => o.Product)
                .Where(o => o.Status == filter.Status)
                .Skip(filter.Offset)
                .Take(filter.Limit).ToListAsync();

            return orders;
        }

        public async Task<Order> Update(Order item)
        {
            var result = _context.Orders.Update(item);
            await _context.SaveChangesAsync();

            return result.Entity;
        }
    }
}
