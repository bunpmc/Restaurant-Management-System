using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace Repositories
{
    public class TableRepository : ITableRepository
    {
        TableDAO _tableDAO = new TableDAO();
        public bool AddTable(Table table)
        {
            return _tableDAO.AddTable(table);
        }

        public bool DeleteTable(int id)
        {
            return _tableDAO.DeleteTable(id);
        }

        public List<Table> GetAllTables()
        {
            return _tableDAO.GetAllTables();
        }

        public Table? GetTableById(int id)
        {
            return _tableDAO.GetTableById(id);
        }

        public List<Table> GetTablesByNumber(string tableNumber)
        {
            return _tableDAO.GetTablesByNumber(tableNumber);
        }

        public List<Table> GetTablesByStatus(string status)
        {
            return _tableDAO.GetTablesByStatus(status);
        }

        public bool UpdateTable(Table table)
        {
            return _tableDAO.UpdateTable(table);
        }
    }
}
