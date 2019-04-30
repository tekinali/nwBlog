using nwBlog.DataAccessLayer.EntityFramework;
using nwBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nwBlog.BusinessLayer
{
    public class Test1
    {
        private Repository<Blog> repo_blog = new Repository<Blog>();

        public Test1()
        {
            var list = repo_blog.List();
        }
    }
}
