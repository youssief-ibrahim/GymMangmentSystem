using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using GymMangmentDAL.Data.Contexts;
using GymMangmentDAL.Entities;

namespace GymMangmentDAL.Data.DataSeeding
{
    public static class GymDbContextSeeding
    {
        public static bool SeedData(GymDbContext dbContext)
        {
            try
            {
                bool HasCategories = dbContext.Categories.Any();
                bool HasPlans = dbContext.Plans.Any();
                bool HasTranair = dbContext.Trainers.Any();
                if (HasCategories && HasPlans && HasTranair) return false;

                if (!HasCategories)
                {
                    var Categories = LoadDataFromJsonFile<Category>("categories.json");
                    if(Categories.Any()) 
                    dbContext.Categories.AddRange(Categories);
                }

                if (!HasPlans)
                {
                    var Planss = LoadDataFromJsonFile<Plan>("plans.json");
                    if (Planss.Any())
                        dbContext.Plans.AddRange(Planss);
                }
                if (!HasTranair)
                {
                    var Tranairs = LoadDataFromJsonFile<Trainer>("trainers.json");
                    if (Tranairs.Any())
                        dbContext.Trainers.AddRange(Tranairs);
                }
                
                int RowsAffected = dbContext.SaveChanges();
                return RowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding Failed : {ex}");
               return false;
            }
        }

        private static List<T> LoadDataFromJsonFile<T>(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","Files", fileName);

            if (!File.Exists(filePath)) throw new FileNotFoundException();

            string Data = File.ReadAllText(filePath);
            var Options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            Options.Converters.Add(new JsonStringEnumConverter());
            return JsonSerializer.Deserialize<List<T>>(Data, Options) ?? new List<T>();
        }
    }
}
