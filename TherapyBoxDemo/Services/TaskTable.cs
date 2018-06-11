using System;
using SQLite;
using Xamarin.Forms;

namespace TherapyBoxDemo.Services
{
    public class TaskTable 
    {
		[PrimaryKey, AutoIncrement, Column("_Id")]
		public int id { get; set; } 

		[MaxLength(25)]

        public string TaskName { get; set; }

		[MaxLength(4)]

		public int TaskToggle { get; set; }
       
    }
}

