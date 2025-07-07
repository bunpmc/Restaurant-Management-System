using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace Repositories
{
    public interface ITableRepository
    {
        public List<Table> GetAllTables();
        public Table? GetTableById(int id);
        public bool AddTable(Table table);
        public bool UpdateTable(Table table);
        public bool DeleteTable(int id);
        public List<Table> GetTablesByStatus(string status);
        public List<Table> GetTablesByNumber(string tableNumber);
    }
}
