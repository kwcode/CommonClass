using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL
{
    public class UserDAL
    {
        public static Model.UserEntity Get_99()
        {
            return new Model.UserEntity() { Name = "开文" };
        }
    }
}