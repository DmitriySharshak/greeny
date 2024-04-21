using Greeny.Common.Models;
using Greeny.Dal.Models;
using LinqToDB;

namespace Greeny.Dal
{
    public  class DictionaryCategoryMigration
    {
        private readonly GreenySchema _db;
        public DictionaryCategoryMigration(GreenySchema db)
        {
            _db = db;
        }
        public void Migrate()
        {
            // Получить список всех категорий   
            var сategories = _db.Category.ToArray();

            foreach (var root in Roots)
            {
                var rootModel = сategories.FirstOrDefault(q => q.Id == root.Id);

                if (rootModel == null)
                {
                    _db.Category.InsertWithIdentity(() => new CategoryDataModel()
                    {
                        Id = root.Id,
                        Name = root.Name,
                        ImagePath = root.ImagePath,
                        Show = true,
                    });
                }
                else
                {
                    _db.Category
                        .Where(q => q.Id == root.Id)
                        .Set(q => q.Name, root.Name)
                        .Set(q => q.ImagePath, root.ImagePath)
                        .Update();
                }

                if (root.Descendants == null)
                {
                    continue;
                }

                foreach (var descendant in root.Descendants)
                {
                    var descendantModel = сategories.FirstOrDefault(q => q.Id == descendant.Id && q.ParentId == root.Id);
                    if (descendantModel == null)
                    {
                        _db.Category.InsertWithIdentity(() => new CategoryDataModel()
                        {
                            Id = descendant.Id,
                            ParentId = root.Id,
                            Name = descendant.Name,
                            ImagePath = descendant.ImagePath,
                            Show = true,
                        });
                    }
                    else
                    {
                        _db.Category
                            .Where(q => q.Id == descendantModel.Id)
                            .Set(q => q.Name, descendant.Name)
                            .Set(q => q.ImagePath, descendant.ImagePath)
                            .Update();
                    }
                }
            }
        }


        private static List<Category> Roots = new List<Category>()
        {
            new Category() { Id = 100, Name = "Мясо", ImagePath = "categories/meat.png", Descendants = new List<Category>() 
            {
                new Category() { Id = 101, Name = "Говядина", ImagePath = "categories/meat/cow.png"},
                new Category() { Id = 102, Name = "Свинина", ImagePath = "categories/meat/pig.png"},
                new Category() { Id = 103, Name = "Баранина", ImagePath = "categories/meat/sheep.png"},
                new Category() { Id = 104, Name = "Телятина", ImagePath = "categories/meat/youngCow.png"},
            }},
            new Category() { Id = 200, Name = "Молоко+", ImagePath = "categories/cheese.png", Descendants = new List<Category>()
            {
                    new Category() { Id = 201, Name = "Молоко", ImagePath = "categories/milk/milk.png"},
                    new Category() { Id = 202, Name = "Cыр", ImagePath = "categories/milk/cheese.png"},
                    new Category() { Id = 203 , Name = "Творог", ImagePath = "categories/milk/cottageCheese.png"},
                    new Category() { Id = 204, Name = "Масло", ImagePath = "categories/milk/spread.png"},
                    new Category() { Id = 205, Name = "Сметана", ImagePath = "Сметана"},
            }},
            new Category() { Id = 300, Name = "Птица+", ImagePath = "categories/chicken.png", Descendants = new List<Category>()
            {
                    new Category() { Id = 301, Name = "Курица", ImagePath = "categories/bird/chicken.png"},
                    new Category() { Id = 302, Name = "Цыпленок", ImagePath = "categories/bird/chick.png"},
                    new Category() { Id = 303, Name = "Гусь", ImagePath = "categories/bird/goose.png"},
                    new Category() { Id = 304, Name = "Утка", ImagePath = "categories/bird/duck.png"},
                    new Category() { Id = 305, Name = "Индейка", ImagePath = "categories/bird/turkey.png"},
                    new Category() { Id = 306, Name = "Яйца", ImagePath = "categories/bird/eggs.png"},
            }},
            new Category() { Id = 400, Name = "Овощи", ImagePath = "categories/vegetables.png", Descendants = new List<Category>()
            {
                new Category() { Id = 401, Name = "Картошка",  ImagePath = "categories/vegetables/potato.png" },
                new Category() { Id = 402, Name = "Помидоры",  ImagePath = "categories/vegetables/tomato.png" },
                new Category() { Id = 403, Name = "Огурцы",    ImagePath = "categories/vegetables/cucumber.png" },
                new Category() { Id = 404, Name = "Капуста",   ImagePath = "categories/vegetables/cabbage.png" },
                new Category() { Id = 405, Name = "Морковь",   ImagePath = "categories/vegetables/carrot.png" },
                new Category() { Id = 406, Name = "Лук",       ImagePath = "categories/vegetables/onion.png" },
                new Category() { Id = 407, Name = "Чеснок",    ImagePath = "categories/vegetables/garlic.png" },
                new Category() { Id = 408, Name = "Свекла",    ImagePath = "categories/vegetables/beetroot.png" },
                new Category() { Id = 409, Name = "Зелень",    ImagePath = "categories/vegetables/greens.png" },
            }},
            new Category() { Id = 500, Name = "Фрукты",  ImagePath = "categories/fruits.png", Descendants = new List<Category>()
            {
                new Category() { Id = 501, Name = "Яблоки", ImagePath = "categories/fruits/apple.png" },
                new Category() { Id = 502, Name = "Слива",  ImagePath = "categories/fruits/plum.png" },
                new Category() { Id = 503, Name = "Вишня",  ImagePath = "categories/fruits/cherries.png" },
                new Category() { Id = 504, Name = "Груша",  ImagePath = "categories/fruits/pear.png" },
            }},
            new Category() { Id = 600, Name = "Ягоды",  ImagePath = "categories/berries.png", Descendants = new List<Category>()
            {
                new Category() { Id = 601, Name = "Земляника", ImagePath = "categories/berries/strawberry_2.png" },
                new Category() { Id = 602, Name = "Черника",   ImagePath = "categories/berries/blueberry.png" },
                new Category() { Id = 603, Name = "Брусника",  ImagePath = "categories/berries/cowberry.png" },
                new Category() { Id = 604, Name = "Клюква",    ImagePath = "categories/berries/cranberry.png" },
                new Category() { Id = 605, Name = "Клубника",  ImagePath = "categories/berries/strawberry_1.png" },
                new Category() { Id = 606, Name = "Смородина", ImagePath = "categories/berries/currant.png" },
                new Category() { Id = 607, Name = "Малина",    ImagePath = "categories/berries/raspberries.png" },
                new Category() { Id = 608, Name = "Крыжовник", ImagePath = "categories/berries/gooseberry.png" },
            }},
            new Category() { Id = 700, Name = "Грибы",  ImagePath = "categories/mushroom.png", Descendants = new List<Category>()
            {
                new Category() { Id = 701, Name = "Лисички", ImagePath = "categories/mushroom/chanterelle.png" },
                new Category() { Id = 702, Name = "Белые",   ImagePath = "categories/mushroom/mushroom.png" },
            }},
            new Category() { Id = 800, Name = "Мед+",  ImagePath = "categories/honey.png", Descendants = new List<Category>()
            {
                new Category() { Id = 801, Name = "Мед",   ImagePath = "categories/honey/honey.png" },
                new Category() { Id = 802, Name = "Соты",  ImagePath = "categories/honey/apitherapy.png" },
                new Category() { Id = 803, Name = "Перга", ImagePath = "categories/honey/honeycomb.png" },
                new Category() { Id = 804, Name = "Воск",  ImagePath = "categories/honey/wax.png" },
            }},
            new Category() { Id = 900, Name = "Масла",  ImagePath = "categories/oil.png" },
        };
    }
}
