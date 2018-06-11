using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TherapyBoxDemo.Services
{
    class LoginTable
    {
        [PrimaryKey, AutoIncrement, Column("_Id")]

        public int id { get; set; }  

        [MaxLength(25)]

        public string username { get; set; }

        [MaxLength(15)]

        public string password { get; set; }

        [MaxLength(25)]

        public string email { get; set; }
    }
}
