using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace DataAccessLayer
{
    public class TableDAO
    {
        SakanaHouseContext context = new SakanaHouseContext();

        public List<Table> GetTables()
        {
            return context.Tables.ToList();
        }
    }
}
