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
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
        }

        public async Task<UserModel?> GetAsync(string login, string password)
        {
            using (var db = new GreenySchema(_dataService))
            {
                var passHash = CryptoHelper.GetBase64Hash(password);

                var user = await db.User
                    .Where(q =>
                        q.PhoneNumber.ToUpper().Equals(login.ToUpper()) &&
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
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    MiddleName = user.MiddleName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    PasswordHash = hash,
                    RegisterTime = DateTime.UtcNow,
                    Address = user.Address,
                    IsActive = true,
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
                FirstName = dataModel.FirstName,
                LastName = dataModel.LastName,
                MiddleName = dataModel.MiddleName,
            };
        }
    }
}
