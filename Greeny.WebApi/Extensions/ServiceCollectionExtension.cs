using Greeny.Common.Models;
using Greeny.Core.Contract;
using Greeny.Core.Contracts;
using Greeny.Core.Services;
using Greeny.Dal;
using Greeny.Dal.Models;
using Greeny.Dal.Provider;
using LinqToDB;

namespace Greeny.WebApi.Extensions
{
    public static class ServiceCollectionExtension
    {

        public static IServiceCollection RegisterServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();

            var storePath = builder.Configuration.GetValue<string>("Greeny:StorePath");

            var fileManagerService = new FileManagerService(storePath);
            services.AddSingleton<IFileManagerService>(fileManagerService);

            return services;
        }

        public static IServiceCollection AddCache(this IServiceCollection services, WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                services.AddDistributedMemoryCache();
                Console.WriteLine("add DistributedMemoryCache...");
            }
            else if (builder.Environment.IsProduction())
            {
                var host = builder.Configuration.GetValue<string>("Redis:Host");
                var port = builder.Configuration.GetValue<string>("Redis:Port");
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = $"{host}:{port}";
                });

                Console.WriteLine($"add StackExchangeRedisCache[{host}:{port}]...");
            }

            return services;
        }

        public static IServiceCollection RegisterDbConnections(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var dataService = new NpqsqlDataService(connectionString);
            
            services.AddSingleton<IDataService>(dataService);
            SchemaMigration.Migrate<GreenySchema>(dataService);

            // Категории 1-го уровня
            var categories = new List<CategoryDataModel>()
                {
                    new CategoryDataModel() { Name = "Мясо",    Image = "categories/meat.png", Show = true },
                    //new CategoryDataModel() { Name = "Молоко+", Image = "categories/cheese.png" },
                    //new CategoryDataModel() { Name = "Птица+",  Image = "categories/chicken.png" },
                    //new CategoryDataModel() { Name = "Овощи",   Image = "categories/vegetables.png" },
                    //new CategoryDataModel() { Name = "Фрукты",  Image = "categories/fruits.png" },
                    //new CategoryDataModel() { Name = "Ягоды",   Image = "categories/berries.png" },
                    //new CategoryDataModel() { Name = "Грибы",   Image = "categories/mushroom.png" },
                    //new CategoryDataModel() { Name = "Мед+",    Image = "categories/honey.png" },
                    //new CategoryDataModel() { Name = "Масла",   Image = "categories/oil.png" },
                };

            // Категории 2-ого уровня вложенности
            var subCategories = new Dictionary<string, CategoryDataModel[]>();
            subCategories.Add("Мясо", new CategoryDataModel[]
            {
                    new CategoryDataModel() { Name = "Говядина", Image = "categories/meat/cow.png", Show = true},
                    new CategoryDataModel() { Name = "Свинина",  Image = "categories/meat/pig.png", Show = true },
                    new CategoryDataModel() { Name = "Баранина", Image = "categories/meat/sheep.png", Show = true },
                    new CategoryDataModel() { Name = "Телятина", Image = "categories/meat/youngCow.png", Show = true },
            });

            //subCategories.Add("Молоко+", new CategoryDataModel[]
            //{
            //        new CategoryDataModel() { Name = "Молоко", Image = "categories/milk/milk.png" },
            //        new CategoryDataModel() { Name = "Cыр",  Image = "categories/milk/cheese.png" },
            //        new CategoryDataModel() { Name = "Творог", Image = "categories/milk/cottageCheese.png" },
            //        new CategoryDataModel() { Name = "Масло", Image = "categories/milk/spread.png" },
            //        new CategoryDataModel() { Name = "Сметана", Image = "categories/milk/soupCream.png" },
            //});

            //subCategories.Add("Птица+", new CategoryDataModel[]
            //{
            //        new CategoryDataModel() { Name = "Курица", Image = "categories/bird/chicken.png" },
            //        new CategoryDataModel() { Name = "Цыпленок",  Image = "categories/bird/chick.png" },
            //        new CategoryDataModel() { Name = "Гусь", Image = "categories/bird/goose.png" },
            //        new CategoryDataModel() { Name = "Утка", Image = "categories/bird/duck.png" },
            //        new CategoryDataModel() { Name = "Индейка", Image = "categories/bird/turkey.png" },
            //        new CategoryDataModel() { Name = "Яйца", Image = "categories/bird/eggs.png" },
            //});

            //subCategories.Add("Овощи", new CategoryDataModel[]
            //{
            //    new CategoryDataModel() { Name = "Картошка", Image = "categories/vegetables/potato.png" },
            //    new CategoryDataModel() { Name = "Помидоры",  Image = "categories/vegetables/tomato.png" },
            //    new CategoryDataModel() { Name = "Огурцы", Image = "categories/vegetables/cucumber.png" },
            //    new CategoryDataModel() { Name = "Капуста", Image = "categories/vegetables/cabbage.png" },
            //    new CategoryDataModel() { Name = "Морковь", Image = "categories/vegetables/carrot.png" },
            //    new CategoryDataModel() { Name = "Лук", Image = "categories/vegetables/onion.png" },
            //    new CategoryDataModel() { Name = "Чеснок", Image = "categories/vegetables/garlic.png" },
            //    new CategoryDataModel() { Name = "Свекла", Image = "categories/vegetables/beetroot.png" },
            //    new CategoryDataModel() { Name = "Зелень", Image = "categories/vegetables/greens.png" },
            //});

            //subCategories.Add("Фрукты", new CategoryDataModel[]
            //{
            //    new CategoryDataModel() { Name = "Яблоки", Image = "categories/fruits/apple.png" },
            //    new CategoryDataModel() { Name = "Слива",  Image = "categories/fruits/plum.png" },
            //    new CategoryDataModel() { Name = "Вишня", Image = "categories/fruits/cherries.png" },
            //    new CategoryDataModel() { Name = "Груша", Image = "categories/fruits/pear.png" },
            //});

            //subCategories.Add("Ягоды", new CategoryDataModel[]
            //{
            //    new CategoryDataModel() { Name = "Земляника", Image = "categories/berries/strawberry_2.png" },
            //    new CategoryDataModel() { Name = "Черника",  Image = "categories/berries/blueberry.png" },
            //    new CategoryDataModel() { Name = "Брусника", Image = "categories/berries/cowberry.png" },
            //    new CategoryDataModel() { Name = "Клюква", Image = "categories/berries/cranberry.png" },
            //    new CategoryDataModel() { Name = "Клубника", Image = "categories/berries/strawberry_1.png" },
            //    new CategoryDataModel() { Name = "Смородина", Image = "categories/berries/currant.png" },
            //    new CategoryDataModel() { Name = "Малина", Image = "categories/berries/raspberries.png" },
            //    new CategoryDataModel() { Name = "Крыжовник", Image = "categories/berries/gooseberry.png" },
            //});

            //subCategories.Add("Грибы", new CategoryDataModel[]
            //{
            //    new CategoryDataModel() { Name = "Лисички", Image = "categories/mashroom/chanterelle.png" },
            //    new CategoryDataModel() { Name = "Белые",  Image = "categories/mashroom/mashroom.png" },
            //});

            //subCategories.Add("Мед+", new CategoryDataModel[]
            //{
            //    new CategoryDataModel() { Name = "Мед", Image = "categories/honey/honey.png" },
            //    new CategoryDataModel() { Name = "Соты",  Image = "categories/honey/apitherapy.png" },
            //    new CategoryDataModel() { Name = "Перга",  Image = "categories/honey/honeycomb.png" },
            //    new CategoryDataModel() { Name = "Воск",  Image = "categories/honey/wax.png" },
            //});

            //subCategories.Add("Масла", new CategoryDataModel[]
            //{
                
            //});

            using (var db = new GreenySchema(dataService))
            {
                var allCategories = db.Category.ToArray();

                foreach (var category in categories)
                {
                    var categoryId = 0L;
                    var categoryModel = allCategories.FirstOrDefault(q => q.Name == category.Name);

                    if (categoryModel == null)
                    {
                        categoryId = (long)db.Category.InsertWithIdentity(() => new CategoryDataModel()
                        {
                            Name = category.Name,
                            Image = category.Image,
                            Show = category.Show,
                        });
                    }
                    else
                    {
                        categoryId = categoryModel.Id;

                        db.Category
                            .Where(q => q.Id == categoryId)
                            .Set(q => q.Name, category.Name)
                            .Set(q => q.Image, category.Image)
                            .Set(q => q.Show, category.Show)
                            .Update();
                    }

                   
                    if (subCategories.TryGetValue(category.Name, out var childrens))
                    {
                        foreach (var children in childrens)
                        {
                            var childrenModel = allCategories.FirstOrDefault(q =>  q.Name == children.Name && q.ParentId == categoryId);
                            if (childrenModel == null)
                            {
                                db.Category.Insert(() => new CategoryDataModel()
                                {
                                    ParentId = categoryId,
                                    Name = children.Name,
                                    Image = children.Image,
                                    Show = children.Show,
                                });
                            }
                            else
                            {
                                db.Category
                                    .Where(q => q.Id == childrenModel.Id)
                                    .Set(q => q.Name, children.Name)
                                    .Set(q => q.Image, children.Image)
                                    .Set(q => q.Show, children.Show)
                                    .Update();
                            }
                        }
                    }
                }
            }

            return services;
        }
    }
}
