using Greeny.Dal.Models;
using LinqToDB;

namespace Greeny.Dal
{
    public  class DictionaryCategoryMigration
    {
        private readonly GreenySchema _db;
        private readonly bool _show;
        public DictionaryCategoryMigration(GreenySchema db)
        {
            _db = db;
            _show = true;
        }
        public void Migrate()
        {
            // Получить список всех категорий   
            var сategories = _db.Category.ToArray();

            foreach (var root in Roots)
            {
                root.Show = _show;

                var rootId = 0L;
                var rootModel = сategories.FirstOrDefault(q => q.Name == root.Name);

                if (rootModel == null)
                {
                    rootId = (long)_db.Category.InsertWithIdentity(() => new CategoryDataModel()
                    {
                        Name = root.Name,
                        ImagePath = root.ImagePath,
                        Show = root.Show,
                    });
                }
                else
                {
                    rootId = rootModel.Id;

                    _db.Category
                        .Where(q => q.Id == rootId)
                        .Set(q => q.Name, root.Name)
                        .Set(q => q.ImagePath, root.ImagePath)
                        .Set(q => q.Show, root.Show)
                        .Update();
                }

                if (Descendants.TryGetValue(root.Name, out var descendants))
                {
                    foreach (var descendant in descendants)
                    {
                        descendant.Show = _show;

                        var descendantModel = сategories.FirstOrDefault(q => q.Name == descendant.Name && q.ParentId == rootId);
                        if (descendantModel == null)
                        {
                            _db.Category.InsertWithIdentity(() => new CategoryDataModel()
                            {
                                ParentId = rootId,
                                Name = descendant.Name,
                                ImagePath = descendant.ImagePath,
                                Show = descendant.Show,
                            });
                        }
                        else
                        {
                            _db.Category
                                .Where(q => q.Id == descendantModel.Id)
                                .Set(q => q.Name, descendant.Name)
                                .Set(q => q.ImagePath, descendant.ImagePath)
                                .Set(q => q.Show, descendant.Show)
                                .Update();
                        }
                    }
                }
            }
        }

        // Категории 1-го уровня
        private static List<CategoryDataModel>  Roots = new List<CategoryDataModel>()
        {
            new CategoryDataModel() { Name = "Мясо",    ImagePath = "categories/meat.png"},
            new CategoryDataModel() { Name = "Молоко+", ImagePath = "categories/cheese.png" },
            new CategoryDataModel() { Name = "Птица+",  ImagePath = "categories/chicken.png" },
            new CategoryDataModel() { Name = "Овощи",   ImagePath = "categories/vegetables.png" },
            new CategoryDataModel() { Name = "Фрукты",  ImagePath = "categories/fruits.png" },
            new CategoryDataModel() { Name = "Ягоды",   ImagePath = "categories/berries.png" },
            new CategoryDataModel() { Name = "Грибы",   ImagePath = "categories/mushroom.png" },
            new CategoryDataModel() { Name = "Мед+",    ImagePath = "categories/honey.png" },
            new CategoryDataModel() { Name = "Масла",   ImagePath = "categories/oil.png" },
        };

        // Категории 2-ого уровня вложенности
        private static Dictionary<string, CategoryDataModel[]> Descendants = new Dictionary<string, CategoryDataModel[]>()
        {
            { "Мясо", new CategoryDataModel[]
                    {
                        new CategoryDataModel() { Name = "Говядина", ImagePath = "categories/meat/cow.png", },
                        new CategoryDataModel() { Name = "Свинина",  ImagePath = "categories/meat/pig.png", },
                        new CategoryDataModel() { Name = "Баранина", ImagePath = "categories/meat/sheep.png", },
                        new CategoryDataModel() { Name = "Телятина", ImagePath = "categories/meat/youngCow.png", },
                    }},
            {"Молоко+", new CategoryDataModel[]
                    {
                        new CategoryDataModel() { Name = "Молоко",  ImagePath = "categories/milk/milk.png" },
                        new CategoryDataModel() { Name = "Cыр",     ImagePath = "categories/milk/cheese.png" },
                        new CategoryDataModel() { Name = "Творог",  ImagePath = "categories/milk/cottageCheese.png" },
                        new CategoryDataModel() { Name = "Масло",   ImagePath = "categories/milk/spread.png" },
                        new CategoryDataModel() { Name = "Сметана", ImagePath = "categories/milk/soupCream.png" },
                    }},
            {"Птица+", new CategoryDataModel[]
                    {
                            new CategoryDataModel() { Name = "Курица",    ImagePath = "categories/bird/chicken.png" },
                            new CategoryDataModel() { Name = "Цыпленок",  ImagePath = "categories/bird/chick.png" },
                            new CategoryDataModel() { Name = "Гусь",      ImagePath = "categories/bird/goose.png" },
                            new CategoryDataModel() { Name = "Утка",      ImagePath = "categories/bird/duck.png" },
                            new CategoryDataModel() { Name = "Индейка",   ImagePath = "categories/bird/turkey.png" },
                            new CategoryDataModel() { Name = "Яйца",      ImagePath = "categories/bird/eggs.png" },
                    }},
            {"Овощи", new CategoryDataModel[]
                    {
                        new CategoryDataModel() { Name = "Картошка",  ImagePath = "categories/vegetables/potato.png" },
                        new CategoryDataModel() { Name = "Помидоры",  ImagePath = "categories/vegetables/tomato.png" },
                        new CategoryDataModel() { Name = "Огурцы",    ImagePath = "categories/vegetables/cucumber.png" },
                        new CategoryDataModel() { Name = "Капуста",   ImagePath = "categories/vegetables/cabbage.png" },
                        new CategoryDataModel() { Name = "Морковь",   ImagePath = "categories/vegetables/carrot.png" },
                        new CategoryDataModel() { Name = "Лук",       ImagePath = "categories/vegetables/onion.png" },
                        new CategoryDataModel() { Name = "Чеснок",    ImagePath = "categories/vegetables/garlic.png" },
                        new CategoryDataModel() { Name = "Свекла",    ImagePath = "categories/vegetables/beetroot.png" },
                        new CategoryDataModel() { Name = "Зелень",    ImagePath = "categories/vegetables/greens.png" },
                    }},
            {"Фрукты", new CategoryDataModel[]
                    {
                        new CategoryDataModel() { Name = "Яблоки", ImagePath = "categories/fruits/apple.png" },
                        new CategoryDataModel() { Name = "Слива",  ImagePath = "categories/fruits/plum.png" },
                        new CategoryDataModel() { Name = "Вишня",  ImagePath = "categories/fruits/cherries.png" },
                        new CategoryDataModel() { Name = "Груша",  ImagePath = "categories/fruits/pear.png" },
                    }},
            {"Ягоды", new CategoryDataModel[]
                    {
                        new CategoryDataModel() { Name = "Земляника", ImagePath = "categories/berries/strawberry_2.png" },
                        new CategoryDataModel() { Name = "Черника",   ImagePath = "categories/berries/blueberry.png" },
                        new CategoryDataModel() { Name = "Брусника",  ImagePath = "categories/berries/cowberry.png" },
                        new CategoryDataModel() { Name = "Клюква",    ImagePath = "categories/berries/cranberry.png" },
                        new CategoryDataModel() { Name = "Клубника",  ImagePath = "categories/berries/strawberry_1.png" },
                        new CategoryDataModel() { Name = "Смородина", ImagePath = "categories/berries/currant.png" },
                        new CategoryDataModel() { Name = "Малина",    ImagePath = "categories/berries/raspberries.png" },
                        new CategoryDataModel() { Name = "Крыжовник", ImagePath = "categories/berries/gooseberry.png" },
                    }},
            {"Грибы", new CategoryDataModel[]
                    {
                        new CategoryDataModel() { Name = "Лисички", ImagePath = "categories/mushroom/chanterelle.png" },
                        new CategoryDataModel() { Name = "Белые",   ImagePath = "categories/mushroom/mushroom.png" },
                    }},
            {"Мед+", new CategoryDataModel[]
                {
                    new CategoryDataModel() { Name = "Мед",   ImagePath = "categories/honey/honey.png" },
                    new CategoryDataModel() { Name = "Соты",  ImagePath = "categories/honey/apitherapy.png" },
                    new CategoryDataModel() { Name = "Перга", ImagePath = "categories/honey/honeycomb.png" },
                    new CategoryDataModel() { Name = "Воск",  ImagePath = "categories/honey/wax.png" },
                }},
            {"Масла", new CategoryDataModel[]
                {

                }}

        };
    }
}
