using Greeny.Common.Models;
using Greeny.Core.Contracts;
using Greeny.Dal;
using LinqToDB;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Greeny.Core.Services
{
    public sealed class CategoryService : ICategoryService
    {
        private readonly IDataService _dataService;
        private readonly IDistributedCache _cache;
        public CategoryService(IDataService dataService, IDistributedCache cache)
        {
            if (dataService == null) throw new ArgumentNullException(nameof(dataService));
            if (cache == null) throw new ArgumentNullException(nameof(cache));

            _dataService = dataService;
            _cache = cache;
        }

        public async Task<IEnumerable<CategoryModel>?> GetListAsync()
        {
            //var value = await _cache.GetAsync("categoryList");
            
            //if (value != null)
            //{
            //    var stream = new MemoryStream(value);
            //    return await JsonSerializer.DeserializeAsync<IEnumerable<CategoryModel>>(stream);
            //}

            using (var db = new GreenySchema(_dataService))
            {
                var result = db.Category
                    .Select(q => new CategoryModel()
                    {
                        Id = q.Id,
                        Name = q.Name,
                    }).ToListAsync();

                var valueSerialize  = JsonSerializer.Serialize(result);

                //// сохраняем строковое представление объекта в формате json в кэш на 5 минут
                //await _cache.SetStringAsync("categoryList", valueSerialize, new DistributedCacheEntryOptions
                //{
                //    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                //});


                return await result;
            }
        }
    }
}
