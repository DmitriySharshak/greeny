using Greeny.Common.Models;
using Greeny.Core.Contract;
using Greeny.Dal;
using Greeny.Dal.Provider;
using LinqToDB;

namespace Greeny.Core.Services
{
    public sealed class UserService: IUserService
    {
        private readonly IDataService _dataService;
        public UserService(IDataService dataService)
        {
            if(dataService == null) throw  new ArgumentNullException(nameof(dataService));

            _dataService = dataService;
        }

        public Task<UserModel?> GetAsync(long id)
        {
            using (var db = new GreenySchema(_dataService))
            {
                return db.Users
                    .Where(q=>q.Id == id)
                    .Select(q=> new UserModel()
                    {
                        Id = q.Id,
                        FirstName = q.FirstName,
                        LastName = q.LastName,
                        MiddleName = q.MiddleName,
                        PhoneNumber = q.PhoneNumber,
                        Email = q.Email,
                        RegisterTime = q.RegisterTime,   
                    }).FirstOrDefaultAsync();
            }
        }

        public Task<IEnumerable<UserModel>> ListAsync()
        {
            return null;
            //return Task.Run(() =>
            //{
            //    return new List<UserModel>()
            //    {
            //        new UserModel() { FirstName = "Фамилия 1" },
            //        new UserModel() { FirstName = "Фамилия 2" }
            //    };
            //}).Result;
        }
    }
}
