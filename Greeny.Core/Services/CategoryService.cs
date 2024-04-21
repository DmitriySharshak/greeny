using Greeny.Common.Models;
using Greeny.Core.Contracts;
using Greeny.Dal;
using Greeny.Dal.Models;
using Greeny.Dal.Provider;
using LinqToDB;
using System.Collections.Concurrent;

namespace Greeny.Core.Services
{
    public sealed class CategoryService : ICategoryService
    {
        private readonly IDataService _dataService;
        private readonly IFileManagerService _fileManagerService;

        private static readonly ConcurrentDictionary<int, CategoryHash> _cache = new ConcurrentDictionary<int, CategoryHash>();
        public CategoryService(IDataService dataService, IFileManagerService fileManagerService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            _fileManagerService = fileManagerService;
        }

        public async Task<CategoryHash> GetListAsync(int hash)
        {
            try
            {
                if (_cache.TryGetValue(hash, out var _))
                {
                    return null;
                }

                if (_cache.Values.Any())
                {
                    return _cache.Values.First();
                }

                int currentHash = 0;
                var categories = new List<Category>();

                using (var db = new GreenySchema(_dataService))
                {
                    var list = await db.Category.Where(q => q.Show).ToArrayAsync();

                    foreach (var item in list.Where(q => q.ParentId is null))
                    {
                        var categoryModel = CategoryDataModelTo(item);

                        currentHash += categoryModel.GetHashCode();
                        categories.Add(categoryModel);

                        var descendants = list.Where(q => q.ParentId == item.Id).ToArray();

                        if (descendants.Any())
                        {
                            var subCategories = new List<Category>(descendants.Count());

                            foreach (var descendant in descendants)
                            {
                                var desc = CategoryDataModelTo(descendant);

                                currentHash += categoryModel.GetHashCode();

                                subCategories.Add(desc);
                            }

                            categoryModel.Descendants = subCategories;
                        }
                    }
                }

                var categoryHash = new CategoryHash()
                {
                    Hash = currentHash,
                    List = categories
                };

                if (!_cache.TryGetValue(currentHash, out var _))
                {
                    _cache.TryAdd(currentHash, categoryHash);
                }

                return categoryHash;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Category CategoryDataModelTo(CategoryDataModel dataModel)
        {
            var imageBaser64 = _fileManagerService.GetFile(dataModel.ImagePath);

            var categoryModel = new Category()
            {
                Id = dataModel.Id,
                Name = dataModel.Name,
                ImagePath = dataModel.ImagePath,
                ImageBase64 = imageBaser64,
            };

            return categoryModel;
        }
    }
}
