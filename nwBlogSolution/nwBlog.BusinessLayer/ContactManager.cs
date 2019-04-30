using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nwBlog.Entities;
using nwBlog.BusinessLayer.Abstract;

namespace nwBlog.BusinessLayer
{
    public class ContactManager : ManagerBase<ContactMessage>
    {
        public void Read(int MessageId)
        {
            var message = Find(x => x.Id == MessageId);

            message.IsRead = true;

            int db = base.Update(message);


        }


    }
}
