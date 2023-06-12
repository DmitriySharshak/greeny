using Greeny.Common.Models;
using Greeny.Core.Contract;
using Greeny.Dal;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Greeny.Core.Services
{
    public sealed class UserDataService: IUserDataService
    {
        private readonly IDataService _dataService;
        public UserDataService(IDataService dataService)
        {
            if(dataService == null) throw  new ArgumentNullException(nameof(dataService));

            _dataService = dataService;
        }

        public UserModel? Get(long id)
        {
            using (var db = new GreenySchema(_dataService))
            {
                return db.Users
                    .Where(q=>q.Id == id)
                    .Select(q=> new UserModel()
                    {
                        Id = q.Id,
                        FullName = q.FullName,
                        PhoneNumber = q.PhoneNumber,
                        Email = q.Email,
                        RegisterTime = q.RegisterTime,   
                    }).FirstOrDefault();
            }
        }

        public IEnumerable<UserModel> GetAll()
        {
            return new List<UserModel>()
            {

                new UserModel() { Id = 1, FullName = "Вася", PhoneNumber = "7926..." },
                new UserModel() { Id = 2, FullName = "Петя", PhoneNumber = "7926..." }
            };
            //using (var db = new GreenySchema(_dataService))
            //{
            //    return db.Users
            //        .Select(q => new UserModel()
            //        {
            //            Id = q.Id,
            //            FullName = q.FullName,
            //            PhoneNumber = q.PhoneNumber,
            //            Email = q.Email,
            //            RegisterTime = q.RegisterTime,
            //        }).ToArray();
            //}
        }

    }
}
