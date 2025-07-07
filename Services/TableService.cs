using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using Repositories;

namespace Services
{
    public class TableService : ITableService
    {
        private readonly TableRepository _tableRepository;

        public TableService()
        {
            _tableRepository = new TableRepository();
        }
        public bool AddTable(Table table)
        {
            return _tableRepository.AddTable(table);
        }

        public bool DeleteTable(int id)
        {
            return _tableRepository.DeleteTable(id);
        }

        public List<Table> GetAllTables()
        {
            return _tableRepository.GetAllTables();
        }

        public Table? GetTableById(int id)
        {
            return _tableRepository.GetTableById(id);
        }

        public List<Table> GetTablesByNumber(string tableNumber)
        {
            return _tableRepository.GetTablesByNumber(tableNumber);
        }

        public List<Table> GetTablesByStatus(string status)
        {
            return _tableRepository.GetTablesByStatus(status);
        }

        public bool UpdateTable(Table table)
        {
            return _tableRepository.UpdateTable(table);
        }
    }
}
