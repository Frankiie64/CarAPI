using CarsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsAPI.Repository.Irepository
{
    public interface ICarsRepository
    {
        public Task<List<Cars>> GetCars();
        public Task<Cars> GetCarById(int id);
        public Task<bool> CreateCars(Cars cars);
        public Task<bool> UpdateCars(Cars cars);
        public Task<bool> DeleteCars(Cars cars);
        public Task<bool> ExistCars(int id);
        public Task<bool> ExistCars(string name);
        public Task<bool> Save();
    }
}
