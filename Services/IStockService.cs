using BusinessObjects;
using System.Collections.Generic;

namespace Services
{
    public interface IStockService
    {
        List<Stock> GetAllStocks();
        Stock GetStockById(int id);
        bool SaveStock(Stock stock);
        bool UpdateStock(Stock stock);
        bool DeleteStock(int id);
        List<Stock> SearchStocks(string searchTerm);
        List<Stock> GetLowStockItems();
        List<Stock> GetExpiringItems(int days = 7);
        List<Stock> GetStocksByCategory(string category);
        List<string> GetAllCategories();
        List<string> GetAllSuppliers();
        bool ValidateStock(Stock stock, out string errorMessage);
    }
}
