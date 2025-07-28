using BusinessObjects;
using DataAccessLayer;
using System.Collections.Generic;

namespace Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly IStockDAO _stockDAO;

        public StockRepository()
        {
            _stockDAO = new StockDAO();
        }

        public List<Stock> GetAllStocks()
        {
            return _stockDAO.GetAllStocks();
        }

        public Stock GetStockById(int id)
        {
            return _stockDAO.GetStockById(id);
        }

        public bool SaveStock(Stock stock)
        {
            return _stockDAO.SaveStock(stock);
        }

        public bool UpdateStock(Stock stock)
        {
            return _stockDAO.UpdateStock(stock);
        }

        public bool DeleteStock(int id)
        {
            return _stockDAO.DeleteStock(id);
        }

        public List<Stock> SearchStocks(string searchTerm)
        {
            return _stockDAO.SearchStocks(searchTerm);
        }

        public List<Stock> GetLowStockItems()
        {
            return _stockDAO.GetLowStockItems();
        }

        public List<Stock> GetExpiringItems(int days = 7)
        {
            return _stockDAO.GetExpiringItems(days);
        }

        public List<Stock> GetStocksByCategory(string category)
        {
            return _stockDAO.GetStocksByCategory(category);
        }

        public List<string> GetAllCategories()
        {
            return _stockDAO.GetAllCategories();
        }

        public List<string> GetAllSuppliers()
        {
            return _stockDAO.GetAllSuppliers();
        }
    }
}
