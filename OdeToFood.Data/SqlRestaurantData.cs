using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OdeToFood.Core;

namespace OdeToFood.Data
{
    public class SqlRestaurantData : IRestaurantData
    {
        private readonly OdeToFoodDbContext _dbContext;

        public SqlRestaurantData(OdeToFoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<Restaurant> GetRestaurantsByName(string name)
        {
            var query = _dbContext.Restaurants.Where(
                x => x.Name.StartsWith(name) || string.IsNullOrEmpty(name));
            return query.OrderBy(x => x.Name);

        }

        public Restaurant GetById(int id)
        {
            return _dbContext.Restaurants.Find(id);
        }

        public Restaurant Update(Restaurant updatedRestaurant)
        {
            var entity = _dbContext.Restaurants.Attach(updatedRestaurant);
            entity.State = EntityState.Modified;
            return updatedRestaurant;
        }

        public Restaurant Add(Restaurant newRestaurant)
        {
            _dbContext.Add(newRestaurant);
            Commit();
            return newRestaurant;
        }

        public Restaurant Delete(int id)
        {
            var restaurant = GetById(id);
            if (restaurant != null)
            {
                _dbContext.Restaurants.Remove(restaurant);
            }

            return restaurant;
        }

        public int Commit()
        {
            return _dbContext.SaveChanges();
        }
    }
}