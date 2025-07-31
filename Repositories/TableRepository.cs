using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessLayer;

namespace Repositories
{
    public class TableRepository : ITableRepository
    {
        TableDAO tableDAO = new TableDAO();
        public List<Table> GetTables()
        {
            return tableDAO.GetTables();
        }
    }
}
