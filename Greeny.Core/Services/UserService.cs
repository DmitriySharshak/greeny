using Greeny.Common.Enums;
using Greeny.Common.Models;
using Greeny.Core.Contract;
using Greeny.Core.Security;
using Greeny.Core.Validation;
using Greeny.Dal;
using Greeny.Dal.Models;
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
                return db.User
                    .Where(q=>q.Id == id)
                    .Select(q=>Convert(q)).FirstOrDefaultAsync();
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

        public async Task<UserModel?> GetAsync(string phone, string password)
        {
            using (var db = new GreenySchema(_dataService))
            {
                var passHash = CryptoHelper.GetBase64Hash(password);

                var user = await db.User
                    .Where(q =>
                        q.PhoneNumber.ToUpper().Equals(phone.ToUpper()) &&
                        q.PasswordHash.Equals(passHash))
                    .Select(q => Convert(q)).FirstOrDefaultAsync();

                return user;
            }
        }

        public async Task<bool> AddAsync(UserRegisterModel user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var validateResult = Validate(user);

            if (!validateResult.IsSucceed)
            {
                throw new ValidatorException(validateResult);
            }

            string hash = CryptoHelper.GetBase64Hash(user.Password);

            using (var db = new GreenySchema(_dataService))
            {
                var id = await db.User.InsertWithIdentityAsync(() => new UserDataModel
                {
                    Address = user.Address,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    MiddleName = user.MiddleName,
                    PasswordHash = hash,
                    IsActive = true,
                    PhoneNumber = user.PhoneNumber,
                    RegisterTime = DateTime.UtcNow,
                    Type = (int)UserType.Сustomer
                });

                return true;
            }
        }

        private ValidationResult Validate(UserRegisterModel model)
        {
            CompositeValidator validator = new CompositeValidator();
            validator
                .AddRequired(model.FirstName, nameof(model.FirstName))
                .AddRequired(model.LastName, nameof(model.LastName))
                .AddRequired(model.Address, nameof(model.Address))
                .AddRequired(model.Email, nameof(model.Email))
                .AddRequired(model.PhoneNumber, nameof(model.PhoneNumber))
                .AddRequired(model.Password, nameof(model.Password));

            return validator.Validate();
        }

        internal UserModel Convert(UserDataModel dataModel)
        {
            return new UserModel
            {
                Id = dataModel.Id,
                Type = (UserType)dataModel.Type,
                FirstName = dataModel.FirstName,
                LastName = dataModel.LastName,
                MiddleName = dataModel.MiddleName,
                PhoneNumber = dataModel.PhoneNumber,
                Address = dataModel.Address,
                Email = dataModel.Email,
                RegisterTime = dataModel.RegisterTime,
                IsActive = dataModel.IsActive,
            };
        }
    }
}
