using nwBlog.BusinessLayer.Abstract;
using nwBlog.BusinessLayer.Result;
using nwBlog.Common.Helpers;
using nwBlog.Entities;
using nwBlog.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nwBlog.BusinessLayer
{
    public class SentMailManager : ManagerBase<SentMail>
    {
        public new BusinessLayerResult<SentMail> Insert(SentMail data)
        {
            BusinessLayerResult<SentMail> res = new BusinessLayerResult<SentMail>();
            res.Result = data;

            int dbResult = base.Insert(res.Result);
            if (dbResult > 0)
            {
                // kayıt başarılı
                string body = data.Text;
                string toEmail = data.ToEmail;
                string subject = data.Subject;

                MailHelper.SendMail(body, toEmail, subject);
            }
            else
            {
                // kayıt başarısız
                res.AddError(ErrorMessageCode.MailNotSend, "Başarısız! Mail gönderilmedi.");
            }

            return res;
        }


    }
}
