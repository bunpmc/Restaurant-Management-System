using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class TableDAO
    {
        SushiRestaurantContext _context = new SushiRestaurantContext();
        public List<Table> GetAllTables()
        {
            return _context.Tables.ToList();
        }
        public Table? GetTableById(int id)
        {
            return _context.Tables.FirstOrDefault(t => t.TableId == id);
        }
        public bool AddTable(Table table)
        {
            if (table == null) return false;
            _context.Tables.Add(table);
            return _context.SaveChanges() > 0;
        }
        public bool UpdateTable(Table table)
        {
            if (table == null) return false;
            var existingTable = _context.Tables.FirstOrDefault(t => t.TableId == table.TableId);
            if (existingTable == null) return false;
            existingTable.TableNumber = table.TableNumber;
            existingTable.TableStatus = table.TableStatus;
            existingTable.TableNumber = table.TableNumber;
            return _context.SaveChanges() > 0;
        }
        public bool DeleteTable(int id)
        {
            var table = _context.Tables.FirstOrDefault(t => t.TableId == id);
            if (table == null) return false;
            _context.Tables.Remove(table);
            return _context.SaveChanges() > 0;
        }
        public List<Table> GetTablesByStatus(string status)
        {
            return _context.Tables.Where(t => t.TableStatus == status).ToList();
        }
        public List<Table> GetTablesByNumber(string tableNumber)
        {
            return _context.Tables.Where(t => t.TableNumber == tableNumber).ToList();
        }
    }
}
