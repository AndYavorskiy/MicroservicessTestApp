﻿using MongoDB.Bson;
using MongoDB.Driver;
using FoodService.DBContext;
using FoodService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodService.Entities;

namespace FoodService.Repositories
{
    public class FoodRepository : IFoodRepository
    {
        private readonly IHomeHelperDbContext _context;

        public FoodRepository(IHomeHelperDbContext context)
        {
            _context = context;
        }

        public Task<List<Food>> GetAll()
        {
            return _context
                .Food
                .Find(x => true)
                .SortBy(x => x.Name)
                .ToListAsync();
        }


        public Task<Food> Get(string id)
        {
            return _context
                    .Food
                    .Find(s => s.Id == id)
                    .FirstOrDefaultAsync();
        }

        public async Task<Food> Create(Food food)
        {
            await _context.Food.InsertOneAsync(food);

            return food;
        }

        public async Task<bool> Update(Food food)
        {
            var updateResult =
                await _context
                    .Food
                    .ReplaceOneAsync(
                        filter: g => g.Id == food.Id,
                        replacement: food);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            var deleteResult = await _context
                .Food
                .DeleteOneAsync(x=>x.Id == id);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }
    }
}