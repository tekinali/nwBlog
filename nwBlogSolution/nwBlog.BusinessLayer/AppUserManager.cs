using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nwBlog.Entities;
using nwBlog.BusinessLayer.Abstract;
using nwBlog.BusinessLayer.Result;
using nwBlog.Entities.Messages;
using System.Globalization;
using nwBlog.Entities.ValueObjects;
using System.Web.Helpers;
using nwBlog.Common.Helpers;

namespace nwBlog.BusinessLayer
{
    public class AppUserManager : ManagerBase<AppUser>
    {
        UserRoleManager _userRoleManager;
        AppRoleManager _roleManager;
        BlogManager _blogManager;
        CommentManager _commentManager;
        LikeManager _likeManager;
        LastVisitManager _lastVisitManager;

        public AppUserManager()
        {
            _userRoleManager = new UserRoleManager();
            _roleManager = new AppRoleManager();
            _blogManager = new BlogManager();
            _commentManager = new CommentManager();
            _lastVisitManager = new LastVisitManager();
            _likeManager = new LikeManager();
        }

        // Admin
        public List<AppUser> GetAllRoleUsers()
        {
            var roleId = _roleManager.GetUserRoleId();

            var usersList = _userRoleManager.ListQueryable().Where(x => x.AppRoleId == roleId).Select(u => u.AppUser).ToList();
            return usersList;
        }

        public List<AppUser> GetAllRoleAuthorUsers()
        {
            var roleId = _roleManager.GetAuthorUserRoleId();

            var usersList = _userRoleManager.ListQueryable().Where(x => x.AppRoleId == roleId).Select(u => u.AppUser).ToList();
            return usersList;
        }

        public List<AppUser> GetAllRoleAdmins()
        {
            var roleId = _roleManager.GetAdminRoleId();

            var usersList = _userRoleManager.ListQueryable().Where(x => x.AppRoleId == roleId).Select(u => u.AppUser).ToList();
            return usersList;
        }

        public BusinessLayerResult<AppUser> UpdateFromAdmin(AppUser data)
        {

            data.Username = CultureInfo.CurrentCulture.TextInfo.ToLower(data.Username).Trim().Replace(" ", string.Empty);
            data.Email = CultureInfo.CurrentCulture.TextInfo.ToLower(data.Email).Trim().Replace(" ", string.Empty);

            if (data.IsDeleted == true)
            {
                data.IsActive = false;
            }


            var db_userList = ListQueryable().Where(x => x.Username == data.Username || x.Email == data.Email).ToList();
            BusinessLayerResult<AppUser> res = new BusinessLayerResult<AppUser>();

            if (db_userList.Count() > 0)
            {
                foreach (var db_user in db_userList)
                {
                    if (db_user.Id != data.Id)
                    {
                        if (db_user.Username == data.Username)
                        {
                            res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                        }

                        if (db_user.Email == data.Email)
                        {
                            res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                        }

                        return res;
                    }

                }
            }


            res.Result = Find(x => x.Id == data.Id);

            res.Result.Username = CultureInfo.CurrentCulture.TextInfo.ToLower(data.Username).Trim();
            res.Result.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(data.Name).TrimStart().TrimEnd();
            res.Result.Surname = CultureInfo.CurrentCulture.TextInfo.ToUpper(data.Surname).TrimStart().TrimEnd();
            res.Result.Email = CultureInfo.CurrentCulture.TextInfo.ToLower(data.Email).Trim();
            res.Result.About = data.About.Trim();
            res.Result.CityId = data.CityId;
            res.Result.IsActive = data.IsActive;
            res.Result.IsDeleted = data.IsDeleted;

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı güncellenemedi.");
            }

            return res;
        }

        public new BusinessLayerResult<AppUser> Delete(AppUser data)
        {
            BusinessLayerResult<AppUser> res = new BusinessLayerResult<AppUser>();

            res.Result = data;

            res.Result.IsDeleted = true;
            res.Result.IsActive = false;

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı silinemedi");
            }


            if (res.Result.Blogs.Count > 0)
            {
                BusinessLayerResult<Blog> res2 = new BusinessLayerResult<Blog>();

                foreach (var item in res.Result.Blogs)
                {
                    res2 = _blogManager.Delete(item);
                    if (res2.Errors.Count > 0)
                    {
                        res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı silindi. Blog hata !");
                    }
                }
            }

            return res;
        }

        public BusinessLayerResult<AppUser> RemoveSystem(AppUser data)
        {
            // delete userrole, comments, blogs, likes, lastvisit
            BusinessLayerResult<AppUser> res = new BusinessLayerResult<AppUser>();
            res.Result = Find(x => x.Id == data.Id);

            var userrole = _userRoleManager.ListQueryable().Where(x => x.AppUserId == data.Id).ToList();
            var comments = _commentManager.ListQueryable().Where(x => x.AppUserId == data.Id).ToList();
            var blogs = _blogManager.ListQueryable().Where(x => x.AppUserId == data.Id).ToList();
            var likes = _likeManager.ListQueryable().Where(x => x.AppUserId == data.Id).ToList();
            var visits = _lastVisitManager.ListQueryable().Where(x => x.AppUserId == data.Id).ToList();

            // delete likes
            if (likes.Count() > 0)
            {
                foreach (var item in likes)
                {
                    if (_likeManager.Delete(item) == 0)
                    {
                        res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcıya ait beğeni silinirken hata oluştu. İşlem tamamlanamadı.");
                        return res;
                    }
                }
            }

            // delete comments
            if (comments.Count() > 0)
            {
                foreach (var item in comments)
                {
                    if (_commentManager.Delete(item) == 0)
                    {
                        res.AddError(ErrorMessageCode.BlogCouldNotRemove, "Kullanıcıya ait yorum silinirken hata oluştu. İşlem tamamlanamadı.");
                        return res;
                    }
                }
            }

            // delete visits
            if (visits.Count() > 0)
            {
                foreach (var item in visits)
                {
                    if (_lastVisitManager.Delete(item) == 0)
                    {
                        res.AddError(ErrorMessageCode.BlogCouldNotRemove, "Kullanıcıya ait ziyaret silinirken hata oluştu. İşlem tamamlanamadı.");
                        return res;
                    }
                }
            }

            // delete role
            if (userrole.Count() > 0)
            {
                foreach (var item in userrole)
                {
                    if (_userRoleManager.Delete(item) == 0)
                    {
                        res.AddError(ErrorMessageCode.BlogCouldNotRemove, "Kullanıcıya ait rol silinirken hata oluştu. İşlem tamamlanamadı.");
                        return res;
                    }
                }
            }

            // delete blogs
            if (blogs.Count() > 0)
            {
                foreach (var item in blogs)
                {
                    BusinessLayerResult<Blog> res2 = _blogManager.RemoveSystem(item);
                    if (res2.Errors.Count > 0)
                    {
                        res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullnacıya ait blog silinirken hata oluştu. İşlem tamamlanamadı.");
                        return res;
                    }

                }


            }

            if (base.Delete(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı silinemedi.");
            }

            return res;
        }

        public BusinessLayerResult<AppUser> InsertFromAdmin(AppUser data, UserType typeUser)
        {
            data.Username = CultureInfo.CurrentCulture.TextInfo.ToLower(data.Username).Trim().Replace(" ", string.Empty);
            data.Email = CultureInfo.CurrentCulture.TextInfo.ToLower(data.Email).Trim().Replace(" ", string.Empty);



            var db_userList = ListQueryable().Where(x => x.Username == data.Username || x.Email == data.Email).ToList();
            BusinessLayerResult<AppUser> res = new BusinessLayerResult<AppUser>();

            if (db_userList.Count() > 0)
            {
                foreach (var db_user in db_userList)
                {
                    if (db_user.Id != data.Id)
                    {
                        if (db_user.Username == data.Username)
                        {
                            res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                        }

                        if (db_user.Email == data.Email)
                        {
                            res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                        }

                        return res;
                    }

                }
            }
            else
            {
                // insert           
                int dbResult = Insert(new AppUser()
                {
                    Username = data.Username,
                    Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(data.Name).TrimStart().TrimEnd(),
                    Surname = CultureInfo.CurrentCulture.TextInfo.ToUpper(data.Surname).TrimStart().TrimEnd(),
                    CityId = data.CityId,
                    Email = data.Email,
                    ActivateGuid = Guid.NewGuid(),
                    IsDeleted = false,
                    About = data.About.Trim(),
                    IsActive = data.IsActive,
                    Password = Crypto.HashPassword(data.Password.Replace(" ", string.Empty)),
                    ProfileImageFilename = "default.png"

                });

                // insert başarılı
                if (dbResult > 0)
                {
                    res.Result = Find(x => x.Email == data.Email && x.Username == data.Username);


                    Guid roleId;

                    if (typeUser == UserType.User)
                    {
                        roleId = _roleManager.GetUserRoleId();
                    }
                    else if (typeUser == UserType.AurhorUser)
                    {
                        roleId = _roleManager.GetAuthorUserRoleId();
                    }
                    else if (typeUser == UserType.Admin)
                    {
                        roleId = _roleManager.GetAdminRoleId();
                    }
                    else
                    {
                        res.AddError(ErrorMessageCode.UserCouldNotInserted, "Hata oluştu. Rol Eklenemedi.");
                        return res;
                    }


                    UserRole aur = new UserRole()
                    {
                        AppUserId = res.Result.Id,
                        AppRoleId = roleId
                    };


                    BusinessLayerResult<UserRole> res2 = _userRoleManager.Insert(aur);


                    if (res2.Errors.Count() > 0)
                    {
                        // işlem başarısız
                        res.AddError(ErrorMessageCode.UserCouldNotInserted, "Hata oluştu. Rol Eklenemedi.");
                    }
                    else
                    {
                        // kayıt başarılı mail at
                    }


                }
                else
                {
                    // işlem başarısız
                    res.AddError(ErrorMessageCode.UserCouldNotInserted, "Kullanıcı kayıt edilemedi.");
                }
            }

            return res;

        }

        public enum UserType
        {
            User,
            AurhorUser,
            Admin
        }


        // Home

        public BusinessLayerResult<AppUser> RegisterUser(RegisterViewModel data)
        {
            // kullanıcı username kontrolü
            // kullanıcı email kontrolü
            // kayıt işlemi
            // aktivasyon epostası gönderimi

            data.Username = CultureInfo.CurrentCulture.TextInfo.ToLower(data.Username).Trim().Replace(" ", string.Empty);
            data.Email = CultureInfo.CurrentCulture.TextInfo.ToLower(data.Email).Trim().Replace(" ", string.Empty);

            var db_userList = ListQueryable().Where(x => x.Username == data.Username || x.Email == data.Email).ToList();
            BusinessLayerResult<AppUser> res = new BusinessLayerResult<AppUser>();

            if (db_userList.Count() > 0)
            {
                foreach (var db_user in db_userList)
                {
                    if (db_user.Username == data.Username)
                    {
                        res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                    }
                    if (db_user.Email == data.Email)
                    {
                        res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                    }

                    return res;

                }
            }

            // insert           
            int dbResult = Insert(new AppUser()
            {
                Username = data.Username,
                Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(data.Name).TrimStart().TrimEnd(),
                Surname = CultureInfo.CurrentCulture.TextInfo.ToUpper(data.Surname).TrimStart().TrimEnd(),
                CityId = data.CityId,
                Email = data.Email,
                ActivateGuid = Guid.NewGuid(),
                IsDeleted = false,
                IsActive = false,
                Password = Crypto.HashPassword(data.Password.Replace(" ", string.Empty)),
                ProfileImageFilename = "default.png"

            });

            // insert başarılı
            if (dbResult > 0)
            {
                res.Result = Find(x => x.Email == data.Email && x.Username == data.Username);

                Guid roleId = _roleManager.GetUserRoleId();

                UserRole aur = new UserRole()
                {
                    AppUserId = res.Result.Id,
                    AppRoleId = roleId
                };


                BusinessLayerResult<UserRole> res2 = _userRoleManager.Insert(aur);


                if (res2.Errors.Count() > 0)
                {
                    // işlem başarısız
                    res.AddError(ErrorMessageCode.UserCouldNotInserted, "Hata oluştu. Rol Eklenemedi.");
                }
                else
                {
                    // kayıt başarılı mail at
                    res.Result = Find(x => x.Email == data.Email && x.Username == data.Username);

                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Account/UserActivate/{res.Result.ActivateGuid}";
                    string body = $"Merhaba {res.Result.Username}; <br>Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız.</a>";

                    MailHelper.SendMail(body, res.Result.Email, "Hesap aktifleştirme");
                }


            }
            else
            {
                // işlem başarısız
                res.AddError(ErrorMessageCode.UserCouldNotInserted, "Kullanıcı kayıt edilemedi.");
            }

            return res;
        }

        public List<AppUser> ListUsersActiveIsOk()
        {
            var usersList = ListQueryable().Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
            return usersList;
        }

        public BusinessLayerResult<AppUser> ActivateUser(Guid activateId)
        {
            BusinessLayerResult<AppUser> res = new BusinessLayerResult<AppUser>();
            res.Result = Find(x => x.ActivateGuid == activateId);
            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir.");
                    return res;
                }
                if (res.Result.IsDeleted)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı yakın zamanda silinmiştir. Lütfen yöneticilerle iletişime geçin.");
                    return res;
                }

                res.Result.IsActive = true;
                Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExists, "Aktifleştirilecek kullanıcı bulunamadı.");
            }

            return res;            
        }

        public BusinessLayerResult<AppUser> LoginUser(LoginViewModel data)
        {
            BusinessLayerResult<AppUser> res = new BusinessLayerResult<AppUser>();
            res.Result = Find(x => x.Username == data.Username);

            if (res.Result != null)
            {
                if (!res.Result.IsDeleted)
                {
                    if (!res.Result.IsActive)
                    {
                        res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı aktifleştirilmemiştir.");
                        res.AddError(ErrorMessageCode.CheckYourEmail, "Lütfen e-posta adresinizi kontrol ediniz.");
                    }
                }
                else
                {
                    res.AddError(ErrorMessageCode.UserIsDelete, "Kullanıcı yakın zamanda silinmiştir.");
                }

                if (!Crypto.VerifyHashedPassword(res.Result.Password, data.Password))
                {
                    res.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı adı ya da şifre uyuşmuyor.");
                }

            }
            else
            {
                res.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı Adı ve/veya Parola hatalı.");
            }
            return res;
        }

        // User
        public BusinessLayerResult<AppUser> UpdateFromUser(AppUser data)
        {
            data.Email = CultureInfo.CurrentCulture.TextInfo.ToLower(data.Email).Trim().Replace(" ", string.Empty);


            var db_user = Find(x => x.Email == data.Email);
            BusinessLayerResult<AppUser> res = new BusinessLayerResult<AppUser>();

            if (db_user != null)
            {
                if (db_user.Id != data.Id)
                {
                    if (db_user.Email == data.Email)
                    {
                        res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                    }
                    return res;
                }
            }


            res.Result = Find(x => x.Id == data.Id);

            res.Result.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(data.Name).TrimStart().TrimEnd();
            res.Result.About = data.About.Trim();
            res.Result.Surname = CultureInfo.CurrentCulture.TextInfo.ToUpper(data.Surname).TrimStart().TrimEnd();
            res.Result.Email = CultureInfo.CurrentCulture.TextInfo.ToLower(data.Email).Trim();
            res.Result.CityId = data.CityId;

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Bilgiler güncellenemedi.");
            }

            return res;
        }

        public BusinessLayerResult<AppUser> DeleteFromUser(AppUser data)
        {
            var db_user = Find(x => x.Id == data.Id);
            BusinessLayerResult<AppUser> res = new BusinessLayerResult<AppUser>();

            res.Result = db_user;

            res.Result.IsActive = false;
            res.Result.IsDeleted = true;
          
            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Bilgiler güncellenemedi.");
            }

            return res;
        }

        public BusinessLayerResult<AppUser> ChangePasswordFromUser(AppUser data)
        {
            var db_user = Find(x => x.Id == data.Id);          

            BusinessLayerResult<AppUser> res = new BusinessLayerResult<AppUser>();

            res.Result = db_user;

            res.Result.Password = Crypto.HashPassword(data.Password.Replace(" ", string.Empty));

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "İşlem gerçekleşemedi.");
            }

            return res;
        }

    }
}
