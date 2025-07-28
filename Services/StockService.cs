using BusinessObjects;
using Repositories;
using System;
using System.Collections.Generic;

namespace Services
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;

        public StockService()
        {
            _stockRepository = new StockRepository();
        }

        public List<Stock> GetAllStocks()
        {
            return _stockRepository.GetAllStocks();
        }

        public Stock GetStockById(int id)
        {
            return _stockRepository.GetStockById(id);
        }

        public bool SaveStock(Stock stock)
        {
            if (ValidateStock(stock, out string errorMessage))
            {
                return _stockRepository.SaveStock(stock);
            }
            throw new ArgumentException(errorMessage);
        }

        public bool UpdateStock(Stock stock)
        {
            if (ValidateStock(stock, out string errorMessage))
            {
                return _stockRepository.UpdateStock(stock);
            }
            throw new ArgumentException(errorMessage);
        }

        public bool DeleteStock(int id)
        {
            return _stockRepository.DeleteStock(id);
        }

        public List<Stock> SearchStocks(string searchTerm)
        {
            return _stockRepository.SearchStocks(searchTerm);
        }

        public List<Stock> GetLowStockItems()
        {
            return _stockRepository.GetLowStockItems();
        }

        public List<Stock> GetExpiringItems(int days = 7)
        {
            return _stockRepository.GetExpiringItems(days);
        }

        public List<Stock> GetStocksByCategory(string category)
        {
            return _stockRepository.GetStocksByCategory(category);
        }

        public List<string> GetAllCategories()
        {
            return _stockRepository.GetAllCategories();
        }

        public List<string> GetAllSuppliers()
        {
            return _stockRepository.GetAllSuppliers();
        }

        public bool ValidateStock(Stock stock, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(stock.ItemName))
            {
                errorMessage = "Item name is required.";
                return false;
            }

            if (stock.Quantity < 0)
            {
                errorMessage = "Quantity cannot be negative.";
                return false;
            }

            if (stock.UnitPrice.HasValue && stock.UnitPrice < 0)
            {
                errorMessage = "Unit price cannot be negative.";
                return false;
            }

            if (stock.MinimumStock.HasValue && stock.MinimumStock < 0)
            {
                errorMessage = "Minimum stock cannot be negative.";
                return false;
            }

            if (stock.ExpiryDate.HasValue && stock.ExpiryDate < DateTime.Now.Date)
            {
                errorMessage = "Expiry date cannot be in the past.";
                return false;
            }

            return true;
        }
    }
}
