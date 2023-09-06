using Greeny.Common.Models;
using Greeny.Core.Contracts;
using Greeny.Dal;
using LinqToDB;

namespace Greeny.Core.Services
{
    public sealed class CategoryDataService : ICategoryDataService
    {
        private readonly IDataService _dataService;
        public CategoryDataService(IDataService dataService)
        {
            if (dataService == null) throw new ArgumentNullException(nameof(dataService));

            _dataService = dataService;
        }

        public async Task<IEnumerable<CategoryModel>> GetList()
        {
            using (var db = new GreenySchema(_dataService))
            {
                var result = db.Category
                    .Select(q => new CategoryModel()
                    {
                        Id = q.Id,
                        Name = q.Name,
                    }).ToListAsync();

                return await result;
            }
        }
    }
}
