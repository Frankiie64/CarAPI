using CarsAPI.Data;
using CarsAPI.Models;
using CarsAPI.Repository.Irepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsAPI.Repository
{
    public class CarsRepository : ICarsRepository
    {
        private readonly ApplicationDbContext _db;

        public CarsRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CreateCars(Cars cars)
        {
            await _db.cars.AddAsync(cars);
            return await Save();
        }
        public async Task<bool> UpdateCars(Cars cars)
        {
            _db.cars.Update(cars);
            return await Save();
        }

        public async Task<bool> DeleteCars(Cars cars)
        {
            _db.cars.Remove(cars);
            return await Save();
        }

        public async Task<bool> ExistCars(int id)
        {
            return await _db.cars.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> ExistCars(string name)
        {
            return await _db.cars.AnyAsync(c => c.Name.Trim().ToLower() == name.Trim().ToLower());
        }

        public async Task<Cars> GetCarById(int id)
        {
            return await _db.cars.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Cars>> GetCars()
        {
            return await _db.cars.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync() >= 0 ? true : false;
        }


    }
}
