using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using nwBlog.Entities;

namespace nwBlog.DataAccessLayer.EntityFramework
{
    public class MyIntializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            string[] ctList = {"Bilimsel","Bilişim","Çevre","Çocuk Gelişimi","Diğer","Eğitim",
                "İletişim","Kamu","Sağlık","Sanat","Tarih","Tasarım","Teknoloji","Turizm","Yazılım"
            };

            string[] tgList = {"scelerisque","euismod","iaculis","eu","lacus","nunc","mi","elit","vehicula","ut","laoreet","ac","aliquam","sit","justo","nunc","tempor","metus","vel","placerat","suscipit","orci",
            "nisl","iaculis","eros","tincidunt","nisi","odio","eget","lorem","nulla","ondimentum","tempor","mattis","ut","vitae","feugiat","augue"
            };

            string[] turkeyCity ={"Adana","Adıyaman","Afyonkarahisar","Ağrı","Aksaray","Amasya","Ankara","Antalya","Ardahan","Artvin","Aydın","Balıkesir","Bartın","Batman","Bayburt","Bilecik","Bingöl","Bitlis",
            "Bolu","Burdur","Bursa","Çanakkale","Çankırı","Çorum","Denizli","Diyarbakır","Düzce","Edirne",
            "Elazığ","Erzincan","Erzurum","Eskişehir","Gaziantep","Giresun","Gümüşhane","Hakkâri","Hatay","Iğdır","Isparta","İçel","İstanbul","İzmir","Kahramanmaraş","Karabük",
            "Karaman","Kars","Kastamonu","Kayseri","Kırıkkale","Kırklareli","Kırşehir","Kilis","Kocaeli","Konya","Kütahya",
            "Malatya","Manisa","Mardin","Muğla","Muş","Nevşehir","Niğde","Ordu","Osmaniye","Rize","Sakarya","Samsun","Siirt","Sinop","Sivas","Şanlıurfa","Şırnak","Tekirdağ","Tokat","Trabzon","Tunceli","Uşak","Van","Yalova","Yozgat","Zonguldak","Diğer"
            };

            // create AppRole
            AppRole adminRole = new AppRole()
            {
                Name = "Admin",
                Description = "Tüm yetkilere sahiptir.",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUsername = "system"
            };
            context.AppRoles.Add(adminRole);

            AppRole userRole = new AppRole()
            {
                Name = "User",
                Description = "Blog yazamaz. Yorum ve beğeni yapabilir.",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUsername = "system"
            };
            context.AppRoles.Add(userRole);

            AppRole userAuthorRole = new AppRole()
            {
                Name = "AuthorUser",
                Description = "Blog yazabilir. Yorum ve beğeni yapabilir.",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUsername = "system"
            };
            context.AppRoles.Add(userAuthorRole);

            context.SaveChanges();

            // [0]:Admin, [1]:User, [2]:UserAuthor
            List<AppRole> db_roleList = context.AppRoles.ToList();

            // create City           
            for (int i = 0; i < turkeyCity.Length; i++)
            {
                City cty = new City()
                {

                    Name = turkeyCity[i],
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUsername = "system"
                };
                context.Cities.Add(cty);

            }
            context.SaveChanges();
            List<City> db_cityList = context.Cities.ToList();

            
            // create category
            for (int i = 0; i < ctList.Count(); i++)
            {
                Category category = new Category()
                {
                    Name = ctList[i],
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now, DateTime.Now.AddHours(5)),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddHours(6), DateTime.Now.AddHours(10)),
                    ModifiedUsername = "system",

                };
                context.Categories.Add(category);
            }
            context.SaveChanges();

            List<Category> db_allCategoryList = context.Categories.ToList();

            // create admin, user, authoruser
            AppUser userAdmin = new AppUser()
            {
                Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase("Ali"),
                Surname = CultureInfo.CurrentCulture.TextInfo.ToUpper("Tekin"),
                Username = CultureInfo.CurrentCulture.TextInfo.ToLower("alitekin"),
                Password = Crypto.HashPassword("123456"),
                Email = CultureInfo.CurrentUICulture.TextInfo.ToLower("alitekin@abcblog.com"),
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsDeleted = false,
                About = "hakkımda yazı.",
                CityId = db_cityList[35].Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUsername = "system",
                ProfileImageFilename = "default.png"
            };
            context.AppUsers.Add(userAdmin);

            AppUser userAdmin2 = new AppUser()
            {
                Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase("Admin"),
                Surname = CultureInfo.CurrentCulture.TextInfo.ToUpper("Admin"),
                Username = CultureInfo.CurrentCulture.TextInfo.ToLower("admin"),
                Password = Crypto.HashPassword("123456"),
                Email = CultureInfo.CurrentUICulture.TextInfo.ToLower("admin@abcblog.com"),
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsDeleted = false,
                About = "Hakkımda yazı. "+FakeData.TextData.GetSentence(),
                CityId = db_cityList[39].Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUsername = "system",
                ProfileImageFilename = "default.png"
            };
            context.AppUsers.Add(userAdmin2);

            // User
            for (int i = 0; i < 47; i++)
            {
                AppUser user = new AppUser()
                {
                    Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(FakeData.NameData.GetFirstName()),
                    Surname = CultureInfo.CurrentCulture.TextInfo.ToUpper(FakeData.NameData.GetSurname()),
                    Username = CultureInfo.CurrentCulture.TextInfo.ToLower($"user_{i}").Trim(),
                    Password = Crypto.HashPassword("123456"),
                    Email = CultureInfo.CurrentUICulture.TextInfo.ToLower($"user_{i}@abcblog.com"),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = i % 7 == 0 ? false : true,
                    IsDeleted = false,
                    About = $"user_{i} hakkında. " + FakeData.TextData.GetSentence(),
                    CityId = db_cityList[FakeData.NumberData.GetNumber(1, db_cityList.Count())].Id,
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddMinutes(1), DateTime.Now.AddHours(30)),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddMinutes(32), DateTime.Now.AddHours(50)),
                    ModifiedUsername = "system",
                    ProfileImageFilename = "default.png"
                };

                context.AppUsers.Add(user);
            }
            context.SaveChanges();

            // userAuthor
            for (int i = 0; i < 38; i++)
            {
                AppUser userAuthor = new AppUser()
                {
                    Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(FakeData.NameData.GetFirstName()),
                    Surname = CultureInfo.CurrentCulture.TextInfo.ToUpper(FakeData.NameData.GetSurname()),
                    Username = CultureInfo.CurrentCulture.TextInfo.ToLower($"userAuthor{i}"),
                    Password = Crypto.HashPassword("123456"),
                    Email = CultureInfo.CurrentUICulture.TextInfo.ToLower($"userAuthor{i}@abcblog.com"),
                    ActivateGuid = Guid.NewGuid(),
                    About= $"userauthor_{i} hakkında. " + FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(2,5)),
                    IsActive = true,
                    IsDeleted = false,
                    CityId = db_cityList[FakeData.NumberData.GetNumber(1, db_cityList.Count())].Id,
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddMinutes(1), DateTime.Now.AddMinutes(30)),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddMinutes(32), DateTime.Now.AddMinutes(50)),
                    ModifiedUsername = "system",
                    ProfileImageFilename = "default.png"
                };

                context.AppUsers.Add(userAuthor);
            }
            context.SaveChanges();

            List<AppUser> db_userAllList = context.AppUsers.ToList();

            // create AppUserRole
            UserRole admnRole = new UserRole()
            {
                AppUserId = db_userAllList[0].Id,
                AppRoleId = db_roleList[0].Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUsername = "system"

            };
            context.UserRoles.Add(admnRole);
            context.SaveChanges();

            UserRole admnRole2 = new UserRole()
            {
                AppUserId = db_userAllList[1].Id,
                AppRoleId = db_roleList[0].Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUsername = "system"

            };
            context.UserRoles.Add(admnRole2);
            context.SaveChanges();

            for (int i = 2; i < db_userAllList.Count(); i++)
            {
                UserRole ur = new UserRole()
                {
                    AppUserId = db_userAllList[i].Id,
                    AppRoleId = db_userAllList[i].Username.StartsWith("userauthor") ? db_roleList[2].Id : db_roleList[1].Id,
                    CreatedOn = db_userAllList[i].CreatedOn,
                    ModifiedOn = db_userAllList[i].ModifiedOn,
                    ModifiedUsername = "system"
                };
                context.UserRoles.Add(ur);
            }
            context.SaveChanges();
            context.SaveChanges();

            var userroles = context.UserRoles.ToList();
            var rrle = db_roleList[2].Id;
            var llst = userroles.Where(x => x.AppRoleId == rrle).ToList();

            List<Guid> db_authorUserList = llst.Select(x => x.AppUserId).ToList();

            // create blog
            for (int i = 0; i < 80; i++)
            {
                Blog blog = new Blog()
                {
                    Tittle = CultureInfo.CurrentCulture.TextInfo.ToTitleCase($"blog tittle {i} ." + FakeData.TextData.GetSentence()).TrimStart().TrimEnd(),
                    Text = HttpUtility.HtmlEncode(FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(15, 35))),
                    Summary = $"blog summary { i } ." + FakeData.TextData.GetSentences((FakeData.NumberData.GetNumber(3, 10))),               
                    IsDelete = false,
                    IsDraft = i % 5 == 0 ? true : false,
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddHours(1), DateTime.Now.AddHours(3)),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddHours(3), DateTime.Now.AddHours(5)),
                    AppUserId = db_authorUserList[FakeData.NumberData.GetNumber(1, (db_authorUserList.Count() - 1))],
                    ModifiedUsername = "system"
                };
                context.Blogs.Add(blog);
            }
            context.SaveChanges();


            List<Blog> db_alllBlogList = context.Blogs.ToList();

            // create blogcategory
            for (int i = 0; i < db_alllBlogList.Count; i++)
            {
                int k = FakeData.NumberData.GetNumber(1, db_allCategoryList.Count() - 6);

                for (int j = 0; j < FakeData.NumberData.GetNumber(1,5); j++)
                {
                    BlogCategory bc = new BlogCategory()
                    {
                        BlogId = db_alllBlogList[i].Id,
                        CategoryId = db_allCategoryList[k].Id,
                        CreatedOn = db_alllBlogList[i].CreatedOn.AddMinutes(1),
                        ModifiedOn = db_alllBlogList[i].ModifiedOn.AddMinutes(1),
                        ModifiedUsername = db_alllBlogList[i].AppUser.Username
                    };
                    context.BlogCategories.Add(bc);

                    k++;
                }
            }
            context.SaveChanges();

            // create blogtag
            for (int i = 0; i < db_alllBlogList.Count; i++)
            {
                for (int j = 0; j < FakeData.NumberData.GetNumber(5,10); j++)
                {
                    Tag tg = new Tag()
                    {
                        BlogId = db_alllBlogList[i].Id,
                        Name= CultureInfo.CurrentCulture.TextInfo.ToTitleCase("tag_" + FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 10))).TrimStart().TrimEnd(),
                        CreatedOn = db_alllBlogList[i].CreatedOn.AddMinutes(1),
                        ModifiedOn = db_alllBlogList[i].ModifiedOn.AddMinutes(1),
                        ModifiedUsername = db_alllBlogList[i].AppUser.Username

                    };

                    context.Tags.Add(tg);
                }
            }

            // create comment
            var okBlog = db_alllBlogList.Where(x => x.IsDraft == false).ToList();

            for (int i = 0; i < okBlog.Count(); i++)
            {
                int k = FakeData.NumberData.GetNumber(1, db_userAllList.Count() - 10);

                for (int j = 0; j < FakeData.NumberData.GetNumber(1, 9); j++)
                {
                    Comment comment = new Comment()
                    {
                        Text = FakeData.TextData.GetSentence(),
                        BlogId = okBlog[i].Id,
                        AppUserId = db_userAllList[k].IsActive == true ? db_userAllList[k].Id : db_userAllList[0].Id,
                        CreatedOn = okBlog[i].CreatedOn.AddMinutes(FakeData.NumberData.GetNumber(10, 40)),
                        ModifiedOn = okBlog[i].CreatedOn.AddMinutes(FakeData.NumberData.GetNumber(41, 55)),
                        ModifiedUsername = "system"
                    };
                    context.Comments.Add(comment);
                    k++;
                }
            }
            context.SaveChanges();

            // create like
            for (int i = 0; i < okBlog.Count(); i++)
            {
                int k = FakeData.NumberData.GetNumber(1, db_userAllList.Count() - 10);

                for (int j = 0; j < FakeData.NumberData.GetNumber(1, 9); j++)
                {
                    Like like = new Like()
                    {                       
                        BlogId = okBlog[i].Id,
                        AppUserId = db_userAllList[k].IsActive == true ? db_userAllList[k].Id : db_userAllList[0].Id,
                        CreatedOn = okBlog[i].CreatedOn.AddMinutes(FakeData.NumberData.GetNumber(10, 40)),
                        ModifiedOn = okBlog[i].CreatedOn.AddMinutes(FakeData.NumberData.GetNumber(41, 55)),
                        ModifiedUsername = "system"
                    };
                    context.Likes.Add(like);
                    k++;
                }
            }
            context.SaveChanges();


            // create contactmessage
            for (int i = 0; i < 40; i++)
            {
                ContactMessage cm = new ContactMessage()
                {
                    Name=FakeData.NameData.GetFirstName(),
                    Surname=FakeData.NameData.GetSurname(),
                    Email=FakeData.NetworkData.GetEmail(),
                    Subject=FakeData.TextData.GetSentence(),
                    Text=FakeData.TextData.GetSentences(2),
                    IsRead= i%4==0 ? true : false,
                    DateTime= FakeData.DateTimeData.GetDatetime(DateTime.Now.AddMinutes(32), DateTime.Now.AddHours(50))

                };
                context.ContactMessages.Add(cm);
            }
            context.SaveChanges();

            // create lastvisit
            for (int i = 0; i < db_userAllList.Count(); i++)
            {
                int k = FakeData.NumberData.GetNumber(5, 50);

                for (int j = 0; j < FakeData.NumberData.GetNumber(2,50); j++)
                {
                    LastVisit lv = new LastVisit()
                    {
                        AppUserId = db_userAllList[i].Id,
                        IpAddress = FakeData.NetworkData.GetIpAddress(),
                        DateTime = db_userAllList[i].CreatedOn.AddMinutes(k)
                    };
                    context.LastVisits.Add(lv);

                k++;
                }
                
            }
            context.SaveChanges();

            // create log
            for (int i = 0; i < db_userAllList.Count(); i++)
            {
                for (int j = 0; j < FakeData.NumberData.GetNumber(10,90); j++)
                {
                    Log log = new Log()
                    {
                        Username=db_userAllList[i].Username,
                        ActionName="action_"+FakeData.TextData.GetAlphabetical(10),
                        ControllerName="contoller_"+FakeData.TextData.GetAlphabetical(12),
                        Content=FakeData.TextData.GetSentence(),
                        DateTime= db_userAllList[i].CreatedOn.AddMinutes(j)

                    };
                    context.Logs.Add(log);
                }
            }
            context.SaveChanges();

        }
      
    }
}
