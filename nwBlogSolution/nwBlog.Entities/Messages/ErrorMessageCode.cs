using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nwBlog.Entities.Messages
{
    public enum ErrorMessageCode
    {
        UsernameAlreadyExists = 101,
        EmailAlreadyExists = 102,
        UserIsNotActive = 103,
        UsernameOrPassWrong = 104,
        CheckYourEmail = 105,
        UserAlreadyActive = 106,
        ActivateIdDoesNotExists = 107,
        UserNotFound = 108,
        ProfileCouldNotUpdated = 109,
        UserCouldNotRemove = 110,
        UserCouldNotFind = 111,
        UserCouldNotInserted = 112,
        UserCouldNotUpdated = 113,
        UserIsDelete = 114,

        BlogAlreadyExists = 101,
        BlogNotFound = 202,
        BlogCouldNotUpdated = 203,
        BlogCouldNotInserted = 204,
        BlogCouldNotRemove = 205,
        BlogIsDelete = 206,


        CategoryAlreadyExists = 401,
        CategoryNotFound = 402,
        CategoryCouldNotUpdated = 403,
        CategoryCouldNotInserted = 404,
        CategoryCouldNotRemove = 405,
        CategoryIsDelete = 406,

        MailNotSend=501

    }
}
