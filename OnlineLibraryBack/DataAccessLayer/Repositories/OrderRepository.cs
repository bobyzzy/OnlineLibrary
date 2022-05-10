﻿using AutoMapper;
using OnlineLibrary.DataAccessLayer.Data;
using OnlineLibrary.DataAccessLayer.Entities;
using OnlineLibrary.DataAccessLayer.Interfaces.Repositories;
using OnlineLibrary.DataAccessLayer.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineLibrary.DataAccessLayer.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApiDbContext _dbContext;
        private readonly IMapper _mapper;

        public OrderRepository(ApiDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public async Task<IReadOnlyCollection<OrderEntityModel>> GetAllAsync()
        {
            var orders = await _dbContext.Orders.Include(x => x.Book).Include(u => u.User)
                .AsNoTracking()
                    .ToListAsync();

            return _mapper.Map<IReadOnlyCollection<OrderEntityModel>>(orders);
        }

        public async Task<OrderEntityModel> GetByIdAsync(int orderId)
        {
            var entity = await _dbContext.Orders.Include(x => x.User).ThenInclude(x => x.Books).Include(x => x.Book)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            return _mapper.Map<OrderEntityModel>(entity);
        }

        public async Task<OrderEntityModel> GetByUserIdBookIdAsync(string userId, int bookId)
        {
            var entity = await _dbContext.Orders.Include(x => x.User).Include(x => x.Book)
                .FirstOrDefaultAsync(o => o.User.Id == userId && o.Book.Id == bookId);

            return _mapper.Map<OrderEntityModel>(entity);
        }

        public async Task<bool> CreateAsync(string userId, int bookId)
        {
            var order = new Order { BookId = bookId, UserId = userId };
            var entityEntry = await _dbContext.Orders.AddAsync(order);

            return entityEntry != null;
        }

        public async Task<bool> UpdateAsync(OrderEntityModel model)
        {
            var entity = await _dbContext.Orders.Include(x => x.User).ThenInclude(x => x.Books).Include(x => x.Book).FirstOrDefaultAsync(o => o.Id == model.Id);

            _mapper.Map<OrderEntityModel, Order>(model, entity);

            return entity != null;
        }

        public async Task<bool> DeleteAsync(int orderId)
        {
            var entity = await _dbContext.Orders.Include(x => x.User).ThenInclude(x => x.Books).Include(x => x.Book).FirstOrDefaultAsync(b => b.Id == orderId);

            if (entity != null)
            {
                var entityEntry = _dbContext.Orders.Remove(entity);
                return entityEntry.Entity != null;
            }

            return false;
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
