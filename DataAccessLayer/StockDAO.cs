using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    public class StockDAO : IStockDAO
    {
        public List<Stock> GetAllStocks()
        {
            try
            {
                using var context = new SakanaHouseContext();
                return context.Stocks.OrderBy(s => s.ItemName).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving stocks: {ex.Message}");
            }
        }

        public Stock GetStockById(int id)
        {
            try
            {
                using var context = new SakanaHouseContext();
                return context.Stocks.FirstOrDefault(s => s.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving stock: {ex.Message}");
            }
        }

        public bool SaveStock(Stock stock)
        {
            try
            {
                using var context = new SakanaHouseContext();
                stock.LastUpdated = DateTime.Now;
                stock.TotalValue = stock.Quantity * (stock.UnitPrice ?? 0);
                context.Stocks.Add(stock);
                return context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving stock: {ex.Message}");
            }
        }

        public bool UpdateStock(Stock stock)
        {
            try
            {
                using var context = new SakanaHouseContext();
                stock.LastUpdated = DateTime.Now;
                stock.TotalValue = stock.Quantity * (stock.UnitPrice ?? 0);
                context.Stocks.Update(stock);
                return context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating stock: {ex.Message}");
            }
        }

        public bool DeleteStock(int id)
        {
            try
            {
                using var context = new SakanaHouseContext();
                var stock = context.Stocks.FirstOrDefault(s => s.Id == id);
                if (stock != null)
                {
                    context.Stocks.Remove(stock);
                    return context.SaveChanges() > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting stock: {ex.Message}");
            }
        }

        public List<Stock> SearchStocks(string searchTerm)
        {
            try
            {
                using var context = new SakanaHouseContext();
                return context.Stocks
                    .Where(s => s.ItemName.Contains(searchTerm) || 
                               s.Category.Contains(searchTerm) || 
                               s.Supplier.Contains(searchTerm))
                    .OrderBy(s => s.ItemName)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching stocks: {ex.Message}");
            }
        }

        public List<Stock> GetLowStockItems()
        {
            try
            {
                using var context = new SakanaHouseContext();
                return context.Stocks
                    .Where(s => s.MinimumStock.HasValue && s.Quantity <= s.MinimumStock.Value)
                    .OrderBy(s => s.Quantity)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving low stock items: {ex.Message}");
            }
        }

        public List<Stock> GetExpiringItems(int days = 7)
        {
            try
            {
                using var context = new SakanaHouseContext();
                var cutoffDate = DateTime.Now.AddDays(days);
                return context.Stocks
                    .Where(s => s.ExpiryDate.HasValue && s.ExpiryDate.Value <= cutoffDate)
                    .OrderBy(s => s.ExpiryDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving expiring items: {ex.Message}");
            }
        }

        public List<Stock> GetStocksByCategory(string category)
        {
            try
            {
                using var context = new SakanaHouseContext();
                return context.Stocks
                    .Where(s => s.Category == category)
                    .OrderBy(s => s.ItemName)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving stocks by category: {ex.Message}");
            }
        }

        public List<string> GetAllCategories()
        {
            try
            {
                using var context = new SakanaHouseContext();
                return context.Stocks
                    .Where(s => !string.IsNullOrEmpty(s.Category))
                    .Select(s => s.Category)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving categories: {ex.Message}");
            }
        }

        public List<string> GetAllSuppliers()
        {
            try
            {
                using var context = new SakanaHouseContext();
                return context.Stocks
                    .Where(s => !string.IsNullOrEmpty(s.Supplier))
                    .Select(s => s.Supplier)
                    .Distinct()
                    .OrderBy(s => s)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving suppliers: {ex.Message}");
            }
        }
    }
}
