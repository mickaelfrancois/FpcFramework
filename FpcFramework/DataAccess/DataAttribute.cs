using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FpcFramework.DataAccess
{
    public class NotMappedAttribute : Attribute
    {
        
    }

    public class PrimaryKeyAttribute : Attribute
    {
        public bool AutoIncrement { get; private set; }

        public PrimaryKeyAttribute()
        {
            AutoIncrement = true;
        }
            
        public PrimaryKeyAttribute(bool autoIncrement)
        {
            this.AutoIncrement = autoIncrement;

        }
    }


    public class TableAttribute : Attribute
    {
        public string Name { get; private set; }

        public TableAttribute(string name)
        {
            this.Name = name;

        }

    }

    public class ColumnAttribute : Attribute
    {
        public string Name { get; private set; }

        public ColumnAttribute(string name)
        {
            this.Name = name;
        }

    }
}
