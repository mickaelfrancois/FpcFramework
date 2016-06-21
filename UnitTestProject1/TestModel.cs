using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FpcFramework.DataAccess;

namespace UnitTestProject1
{
    [FpcFramework.DataAccess.Table("testModel")]
    public class TestModel:FpcFramework.DataAccess.IDBModel
    {
        [FpcFramework.DataAccess.PrimaryKey]
        public int ID { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }

        [ColumnAttribute("CreatDate")]
        public DateTime CreationDate { get; set; }

        [FpcFramework.DataAccess.NotMapped]
        public string NotMapped { get; set; }
    }
}
