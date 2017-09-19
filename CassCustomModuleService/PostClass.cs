using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CassCustomModuleService
{
    class PostClass
    {

        public PostClass(int userid, int idValue, string titleValue, string bodyValue) 
        {
            userID = userid;
            id = idValue;
            title = titleValue;
            body = bodyValue;
        }

        public int userID { get; set; }
        public int id{ get; set; }
        public string title { get; set; }
        public string body { get; set; }
        
    }
}
