using Greeny.Common.Models;
using Greeny.Core.Contracts;
using Greeny.Dal;
using Greeny.Dal.Provider;
using LinqToDB;

namespace Greeny.Core.Services
{
    public sealed class CategoryService : ICategoryService
    {
        private readonly IDataService _dataService;
        private readonly IFileManagerService _fileManagerService;
        //private readonly IDistributedCache _cache;
        public CategoryService(IDataService dataService, IFileManagerService fileManagerService)
        {
            if (dataService == null) throw new ArgumentNullException(nameof(dataService));

            _dataService = dataService;
            _fileManagerService = fileManagerService;
            //_cache = cache;
        }

        public async Task<IEnumerable<CategoryModel>> GetRootsAsync()
        {
            try
            {
                //var value = await _cache.GetAsync("categoryList");

                //if (value != null)
                //{
                //    var stream = new MemoryStream(value);
                //    return await JsonSerializer.DeserializeAsync<IEnumerable<CategoryModel>>(stream);
                //}

                using (var db = new GreenySchema(_dataService))
                {
                    var result = db.Category.Where(q => q.ParentId == null && q.Show)
                        .Select(q => new CategoryModel()
                        {
                            Id = q.Id,
                            Name = q.Name,
                            ParentId = q.ParentId,
                            Image = _fileManagerService.GetFile(q.ImagePath)
                        }).ToListAsync();

                    //var valueSerialize = JsonSerializer.Serialize(result);

                    //// сохраняем строковое представление объекта в формате json в кэш на 5 минут
                    //await _cache.SetStringAsync("categoryList", valueSerialize, new DistributedCacheEntryOptions
                    //{
                    //    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                    //});


                    return await result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<CategoryModel>> GetDescendantsAsync(long id)
        {
            try
            {
                using (var db = new GreenySchema(_dataService))
                {
                    var result = db.Category.Where(q => q.ParentId == id && q.Show)
                        .Select(q => new CategoryModel()
                        {
                            Id = q.Id,
                            Name = q.Name,
                            ParentId = q.ParentId,
                            Image = _fileManagerService.GetFile(q.ImagePath)
                        }).ToListAsync();

                    return await result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
