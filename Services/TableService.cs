using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.EntityFrameworkCore.Metadata;
using Repositories;

namespace Services
{
    public class TableService : ITableService
    {
        ITableRepository repository = new TableRepository();

        public List<Table> GetTables()
        {
            return repository.GetTables();
        }
    }
}
