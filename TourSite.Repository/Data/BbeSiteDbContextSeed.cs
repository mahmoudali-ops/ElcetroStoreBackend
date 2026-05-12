using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TourSite.Core.Entities;
using TourSite.Repository.Data.Contexts;

namespace TourSite.Repository.Data
{
    public static class BbeSiteDbContextSeed
    {
        public static async Task SeedAsync(ElectroDbContext context)
        {

            //if (context.Abouts.Count() == 0)
            //{
            //    // 1- Read data from file 
            //    var strings = File.ReadAllText(@"..\TourSite.Repository\Data\DataSeed\about.json");

            //    if (string.IsNullOrEmpty(strings))
            //    {
            //        throw new ArgumentException("The Tours.json file is empty or not found.");
            //    }
            //    // 2- Deserialize the JSON data into a list of ProductBrand objects

            //    var tours = JsonSerializer.Deserialize<List<About>>(strings);

            //    if (tours is not null && tours.Count() > 0)
            //    {
            //        await context.Abouts.AddRangeAsync(tours);

            //        // 3- Save changes to the database

            //        await context.SaveChangesAsync();
            //    }

            //}



        }
    }
}
