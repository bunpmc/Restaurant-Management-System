
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer;
using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Tests
{
    public class TestOrderDAO : OrderDAO
    {
        private readonly SakanaHouseContext _testContext;

        public TestOrderDAO(SakanaHouseContext context)
        {
            _testContext = context;
        }

        public bool SaveOrder(Order order)
        {
            try
            {
                _testContext.Orders.Add(order);
                _testContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public List<Order> GetAllOrders()
        {
            try
            {
                return _testContext.Orders
                    .Include(o => o.Employee)
                    .Include(o => o.Table)
                    .Include(o => o.Customer)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.MenuItem)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Order>();
            }
        }

        public List<Order> GetOrdersByDateRange(DateTime fromDate, DateTime toDate)
        {
            try
            {
                return _testContext.Orders
                    .Include(o => o.Employee)
                    .Include(o => o.Table)
                    .Include(o => o.Customer)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.MenuItem)
                    .Where(o => o.OrderTime >= fromDate && o.OrderTime <= toDate.AddDays(1))
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Order>();
            }
        }

        public List<Order> GetOrdersByStatus(string status)
        {
            try
            {
                return _testContext.Orders
                    .Include(o => o.Employee)
                    .Include(o => o.Table)
                    .Include(o => o.Customer)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.MenuItem)
                    .Where(o => o.Status == status)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Order>();
            }
        }

        public List<Order> GetOrdersByEmployee(int employeeId)
        {
            try
            {
                return _testContext.Orders
                    .Include(o => o.Employee)
                    .Include(o => o.Table)
                    .Include(o => o.Customer)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.MenuItem)
                    .Where(o => o.EmployeeId == employeeId)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Order>();
            }
        }

        public List<Order> GetOrdersByTable(int tableId)
        {
            try
            {
                return _testContext.Orders
                    .Include(o => o.Employee)
                    .Include(o => o.Table)
                    .Include(o => o.Customer)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.MenuItem)
                    .Where(o => o.TableId == tableId)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Order>();
            }
        }

        public Order GetOrderById(int id)
        {
            try
            {
                return _testContext.Orders
                    .Include(o => o.Employee)
                    .Include(o => o.Table)
                    .Include(o => o.Customer)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.MenuItem)
                    .FirstOrDefault(o => o.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public bool UpdateOrder(Order order)
        {
            try
            {
                _testContext.Orders.Update(order);
                _testContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool DeleteOrder(int id)
        {
            try
            {
                var order = _testContext.Orders.Find(id);
                if (order != null)
                {
                    _testContext.Orders.Remove(order);
                    _testContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }

    // Test-specific OrderItemDAO
    public class TestOrderItemDAO : OrderItemDAO
    {
        private readonly SakanaHouseContext _testContext;

        public TestOrderItemDAO(SakanaHouseContext context)
        {
            _testContext = context;
        }

        public List<OrderItem> GetAllOrderItems()
        {
            try
            {
                return _testContext.OrderItems
                    .Include(oi => oi.MenuItem)
                        .ThenInclude(mi => mi.Category)
                    .Include(oi => oi.Order)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<OrderItem>();
            }
        }

        public List<OrderItem> GetOrderItemsByDateRange(DateTime fromDate, DateTime toDate)
        {
            try
            {
                return _testContext.OrderItems
                    .Include(oi => oi.MenuItem)
                        .ThenInclude(mi => mi.Category)
                    .Include(oi => oi.Order)
                    .Where(oi => oi.Order.OrderTime >= fromDate && oi.Order.OrderTime <= toDate.AddDays(1))
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<OrderItem>();
            }
        }

        public List<OrderItem> GetOrderItemsByMenuItem(int menuItemId)
        {
            try
            {
                return _testContext.OrderItems
                    .Include(oi => oi.MenuItem)
                        .ThenInclude(mi => mi.Category)
                    .Include(oi => oi.Order)
                    .Where(oi => oi.MenuItemId == menuItemId)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<OrderItem>();
            }
        }

        public List<OrderItem> GetOrderItemsByOrder(int orderId)
        {
            try
            {
                return _testContext.OrderItems
                    .Include(oi => oi.MenuItem)
                        .ThenInclude(mi => mi.Category)
                    .Include(oi => oi.Order)
                    .Where(oi => oi.OrderId == orderId)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<OrderItem>();
            }
        }

        public List<MenuPerformanceData> GetMenuPerformanceByDateRange(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var result = _testContext.OrderItems
                    .Include(oi => oi.MenuItem)
                        .ThenInclude(mi => mi.Category)
                    .Include(oi => oi.Order)
                    .Where(oi => oi.Order.OrderTime >= fromDate &&
                                oi.Order.OrderTime <= toDate.AddDays(1) &&
                                oi.Order.Status == "Completed")
                    .GroupBy(oi => new { oi.MenuItemId, MenuItemName = oi.MenuItem.Name, CategoryName = oi.MenuItem.Category.Name })
                    .Select(g => new MenuPerformanceData
                    {
                        MenuItemId = g.Key.MenuItemId ?? 0,
                        MenuItemName = g.Key.MenuItemName,
                        CategoryName = g.Key.CategoryName,
                        QuantitySold = g.Sum(oi => oi.Quantity),
                        Revenue = g.Sum(oi => oi.Quantity * oi.UnitPrice),
                        AveragePrice = g.Average(oi => oi.UnitPrice)
                    })
                    .OrderByDescending(mp => mp.QuantitySold)
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<MenuPerformanceData>();
            }
        }
    }

    [TestClass]
    public class OrderDAOTests
    {
        private SakanaHouseContext _context;
        private TestOrderDAO _orderDAO;

        [TestInitialize]
        public void Setup()
        {
            // Use In-Memory database for testing
            var options = new DbContextOptionsBuilder<SakanaHouseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SakanaHouseContext(options);

            // Create a custom OrderDAO that uses our test context
            _orderDAO = new TestOrderDAO(_context);

            // Seed test data
            SeedTestData();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context?.Dispose();
        }

        private void SeedTestData()
        {
            // Seed MenuCategories
            var category1 = new MenuCategory { Id = 1, Name = "Sushi", Description = "Fresh sushi items" };
            var category2 = new MenuCategory { Id = 2, Name = "Drinks", Description = "Beverages" };
            _context.MenuCategories.AddRange(category1, category2);

            // Seed MenuItems
            var menuItem1 = new MenuItem
            {
                Id = 1,
                Name = "Salmon Roll",
                Description = "Fresh salmon roll",
                Price = 12.99m,
                CategoryId = 1,
                IsAvailable = true,
                CreatedAt = DateTime.Now,
                Category = category1
            };
            var menuItem2 = new MenuItem
            {
                Id = 2,
                Name = "Green Tea",
                Description = "Traditional green tea",
                Price = 3.50m,
                CategoryId = 2,
                IsAvailable = true,
                CreatedAt = DateTime.Now,
                Category = category2
            };
            _context.MenuItems.AddRange(menuItem1, menuItem2);

            // Seed Employee
            var employee = new Employee
            {
                Id = 1,
                Username = "johndoe",
                PasswordHash = "hashedpassword123",
                FullName = "John Doe",
                Email = "john.doe@sakanahouse.com",
                Role = "Server",
                IsActive = true,
                CreatedAt = DateTime.Now
            };
            _context.Employees.Add(employee);

            // Seed Table
            var table = new Table
            {
                Id = 1,
                TableNumber = 5,
                Capacity = 4,
                Status = "Available",
                Note = "Window table"
            };
            _context.Tables.Add(table);

            // Seed Customer
            var customer = new Customer
            {
                Id = 1,
                FullName = "Jane Smith",
                Phone = "123-456-7890",
                Email = "jane.smith@email.com",
                Note = "Regular customer"
            };
            _context.Customers.Add(customer);

            _context.SaveChanges();

            // Seed Orders
            var order1 = new Order
            {
                Id = 1,
                TableId = 1,
                EmployeeId = 1,
                CustomerId = 1,
                OrderTime = DateTime.Now.AddDays(-2),
                TotalAmount = 29.48m,
                Status = "Completed"
            };

            var order2 = new Order
            {
                Id = 2,
                TableId = 1,
                EmployeeId = 1,
                CustomerId = 1,
                OrderTime = DateTime.Now.AddDays(-1),
                TotalAmount = 16.49m,
                Status = "Pending"
            };

            _context.Orders.AddRange(order1, order2);
            _context.SaveChanges();

            // Seed OrderItems
            var orderItem1 = new OrderItem
            {
                Id = 1,
                OrderId = 1,
                MenuItemId = 1,
                Quantity = 2,
                UnitPrice = 12.99m
                // TotalPrice is computed, so don't set it manually
            };

            var orderItem2 = new OrderItem
            {
                Id = 2,
                OrderId = 1,
                MenuItemId = 2,
                Quantity = 1,
                UnitPrice = 3.50m
            };

            var orderItem3 = new OrderItem
            {
                Id = 3,
                OrderId = 2,
                MenuItemId = 1,
                Quantity = 1,
                UnitPrice = 12.99m
            };

            var orderItem4 = new OrderItem
            {
                Id = 4,
                OrderId = 2,
                MenuItemId = 2,
                Quantity = 1,
                UnitPrice = 3.50m
            };

            _context.OrderItems.AddRange(orderItem1, orderItem2, orderItem3, orderItem4);
            _context.SaveChanges();
        }

        [TestMethod]
        public void SaveOrder_ValidOrder_ReturnsTrue()
        {
            // Arrange
            var newOrder = new Order
            {
                TableId = 1,
                EmployeeId = 1,
                CustomerId = 1,
                OrderTime = DateTime.Now,
                TotalAmount = 25.99m,
                Status = "Pending"
            };

            // Act
            bool result = _orderDAO.SaveOrder(newOrder);

            // Assert
            Assert.IsTrue(result, "SaveOrder should return true for valid order");
            Assert.IsTrue(newOrder.Id > 0, "Order should have an assigned ID after saving");
        }

        [TestMethod]
        public void SaveOrder_NullOrder_ReturnsFalse()
        {
            // Act
            bool result = _orderDAO.SaveOrder(null);

            // Assert
            Assert.IsFalse(result, "SaveOrder should return false for null order");
        }

        [TestMethod]
        public void GetAllOrders_ReturnsAllOrdersWithIncludes()
        {
            // Act
            var orders = _orderDAO.GetAllOrders();

            // Assert
            Assert.IsNotNull(orders, "Orders list should not be null");
            Assert.AreEqual(2, orders.Count, "Should return exactly 2 orders");

            // Verify includes are working
            foreach (var order in orders)
            {
                Assert.IsNotNull(order.Employee, $"Order {order.Id} should have Employee included");
                Assert.IsNotNull(order.Table, $"Order {order.Id} should have Table included");
                Assert.IsNotNull(order.Customer, $"Order {order.Id} should have Customer included");
                Assert.IsNotNull(order.OrderItems, $"Order {order.Id} should have OrderItems included");
                Assert.IsTrue(order.OrderItems.Count > 0, $"Order {order.Id} should have at least one OrderItem");

                // Verify nested includes
                foreach (var orderItem in order.OrderItems)
                {
                    Assert.IsNotNull(orderItem.MenuItem, "OrderItem should have MenuItem included");
                }
            }
        }

        [TestMethod]
        public void GetOrdersByDateRange_ValidDateRange_ReturnsFilteredOrders()
        {
            // Arrange
            DateTime fromDate = DateTime.Now.AddDays(-3);
            DateTime toDate = DateTime.Now;

            // Act
            var orders = _orderDAO.GetOrdersByDateRange(fromDate, toDate);

            // Assert
            Assert.IsNotNull(orders, "Orders list should not be null");
            Assert.AreEqual(2, orders.Count, "Should return 2 orders within date range");

            foreach (var order in orders)
            {
                Assert.IsTrue(order.OrderTime >= fromDate,
                    $"Order {order.Id} time should be >= fromDate");
                Assert.IsTrue(order.OrderTime <= toDate.AddDays(1),
                    $"Order {order.Id} time should be <= toDate + 1 day");
            }
        }

        [TestMethod]
        public void GetOrdersByDateRange_NoOrdersInRange_ReturnsEmptyList()
        {
            // Arrange - date range with no orders
            DateTime fromDate = DateTime.Now.AddDays(-10);
            DateTime toDate = DateTime.Now.AddDays(-5);

            // Act
            var orders = _orderDAO.GetOrdersByDateRange(fromDate, toDate);

            // Assert
            Assert.IsNotNull(orders, "Orders list should not be null");
            Assert.AreEqual(0, orders.Count, "Should return empty list when no orders in range");
        }

        [TestMethod]
        public void GetOrdersByStatus_CompletedStatus_ReturnsFilteredOrders()
        {
            // Act
            var completedOrders = _orderDAO.GetOrdersByStatus("Completed");

            // Assert
            Assert.IsNotNull(completedOrders, "Completed orders list should not be null");
            Assert.AreEqual(1, completedOrders.Count, "Should return 1 completed order");
            Assert.AreEqual("Completed", completedOrders.First().Status, "Order status should be Completed");
        }

        [TestMethod]
        public void GetOrdersByStatus_PendingStatus_ReturnsFilteredOrders()
        {
            // Act
            var pendingOrders = _orderDAO.GetOrdersByStatus("Pending");

            // Assert
            Assert.IsNotNull(pendingOrders, "Pending orders list should not be null");
            Assert.AreEqual(1, pendingOrders.Count, "Should return 1 pending order");
            Assert.AreEqual("Pending", pendingOrders.First().Status, "Order status should be Pending");
        }

        [TestMethod]
        public void GetOrdersByStatus_NonExistentStatus_ReturnsEmptyList()
        {
            // Act
            var orders = _orderDAO.GetOrdersByStatus("NonExistent");

            // Assert
            Assert.IsNotNull(orders, "Orders list should not be null");
            Assert.AreEqual(0, orders.Count, "Should return empty list for non-existent status");
        }

        [TestMethod]
        public void GetOrdersByEmployee_ValidEmployeeId_ReturnsFilteredOrders()
        {
            // Act
            var orders = _orderDAO.GetOrdersByEmployee(1);

            // Assert
            Assert.IsNotNull(orders, "Orders list should not be null");
            Assert.AreEqual(2, orders.Count, "Should return 2 orders for employee 1");

            foreach (var order in orders)
            {
                Assert.AreEqual(1, order.EmployeeId, "All orders should belong to employee 1");
            }
        }

        [TestMethod]
        public void GetOrdersByEmployee_NonExistentEmployeeId_ReturnsEmptyList()
        {
            // Act
            var orders = _orderDAO.GetOrdersByEmployee(999);

            // Assert
            Assert.IsNotNull(orders, "Orders list should not be null");
            Assert.AreEqual(0, orders.Count, "Should return empty list for non-existent employee");
        }

        [TestMethod]
        public void GetOrdersByTable_ValidTableId_ReturnsFilteredOrders()
        {
            // Act
            var orders = _orderDAO.GetOrdersByTable(1);

            // Assert
            Assert.IsNotNull(orders, "Orders list should not be null");
            Assert.AreEqual(2, orders.Count, "Should return 2 orders for table 1");

            foreach (var order in orders)
            {
                Assert.AreEqual(1, order.TableId, "All orders should belong to table 1");
            }
        }

        [TestMethod]
        public void GetOrdersByTable_NonExistentTableId_ReturnsEmptyList()
        {
            // Act
            var orders = _orderDAO.GetOrdersByTable(999);

            // Assert
            Assert.IsNotNull(orders, "Orders list should not be null");
            Assert.AreEqual(0, orders.Count, "Should return empty list for non-existent table");
        }

        [TestMethod]
        public void GetOrderById_ValidId_ReturnsOrderWithIncludes()
        {
            // Act
            var order = _orderDAO.GetOrderById(1);

            // Assert
            Assert.IsNotNull(order, "Order should not be null");
            Assert.AreEqual(1, order.Id, "Order ID should be 1");
            Assert.IsNotNull(order.Employee, "Order should have Employee included");
            Assert.IsNotNull(order.Table, "Order should have Table included");
            Assert.IsNotNull(order.Customer, "Order should have Customer included");
            Assert.IsNotNull(order.OrderItems, "Order should have OrderItems included");
            Assert.IsTrue(order.OrderItems.Count > 0, "Order should have at least one OrderItem");

            // Verify nested includes
            foreach (var orderItem in order.OrderItems)
            {
                Assert.IsNotNull(orderItem.MenuItem, "OrderItem should have MenuItem included");
            }
        }

        [TestMethod]
        public void GetOrderById_InvalidId_ReturnsNull()
        {
            // Act
            var order = _orderDAO.GetOrderById(999);

            // Assert
            Assert.IsNull(order, "Order should be null for invalid ID");
        }

        [TestMethod]
        public void UpdateOrder_ValidOrder_ReturnsTrue()
        {
            // Arrange
            var order = _orderDAO.GetOrderById(1);
            Assert.IsNotNull(order, "Test setup failed - could not get order");

            var originalStatus = order.Status;
            order.Status = "In Progress";
            order.TotalAmount = 35.99m;

            // Act
            bool result = _orderDAO.UpdateOrder(order);

            // Assert
            Assert.IsTrue(result, "UpdateOrder should return true for valid order");

            var updatedOrder = _orderDAO.GetOrderById(1);
            Assert.IsNotNull(updatedOrder, "Updated order should not be null");
            Assert.AreEqual("In Progress", updatedOrder.Status, "Order status should be updated");
            Assert.AreEqual(35.99m, updatedOrder.TotalAmount, "Order total amount should be updated");
            Assert.AreNotEqual(originalStatus, updatedOrder.Status, "Status should have changed");
        }

        [TestMethod]
        public void UpdateOrder_NullOrder_ReturnsFalse()
        {
            // Act
            bool result = _orderDAO.UpdateOrder(null);

            // Assert
            Assert.IsFalse(result, "UpdateOrder should return false for null order");
        }

        [TestMethod]
        public void DeleteOrder_ValidId_ReturnsTrue()
        {
            // Arrange
            var orderToDelete = _orderDAO.GetOrderById(2);
            Assert.IsNotNull(orderToDelete, "Test setup failed - order 2 should exist");

            // Act
            bool result = _orderDAO.DeleteOrder(2);

            // Assert
            Assert.IsTrue(result, "DeleteOrder should return true for valid ID");

            var deletedOrder = _orderDAO.GetOrderById(2);
            Assert.IsNull(deletedOrder, "Order should be null after deletion");

            // Verify other orders still exist
            var remainingOrders = _orderDAO.GetAllOrders();
            Assert.AreEqual(1, remainingOrders.Count, "Should have 1 order remaining after deletion");
        }

        [TestMethod]
        public void DeleteOrder_InvalidId_ReturnsFalse()
        {
            // Act
            bool result = _orderDAO.DeleteOrder(999);

            // Assert
            Assert.IsFalse(result, "DeleteOrder should return false for invalid ID");

            // Verify no orders were deleted
            var orders = _orderDAO.GetAllOrders();
            Assert.AreEqual(2, orders.Count, "All orders should still exist");
        }
    }

    [TestClass]
    public class OrderItemDAOTests
    {
        private SakanaHouseContext _context;
        private TestOrderItemDAO _orderItemDAO;

        [TestInitialize]
        public void Setup()
        {
            // Use In-Memory database for testing
            var options = new DbContextOptionsBuilder<SakanaHouseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SakanaHouseContext(options);

            // Create a custom OrderItemDAO that uses our test context
            _orderItemDAO = new TestOrderItemDAO(_context);

            // Seed test data
            SeedTestData();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context?.Dispose();
        }

        private void SeedTestData()
        {
            // Same seeding logic as OrderDAOTests
            // Seed MenuCategories
            var category1 = new MenuCategory { Id = 1, Name = "Sushi", Description = "Fresh sushi items" };
            var category2 = new MenuCategory { Id = 2, Name = "Drinks", Description = "Beverages" };
            _context.MenuCategories.AddRange(category1, category2);

            // Seed MenuItems
            var menuItem1 = new MenuItem
            {
                Id = 1,
                Name = "Salmon Roll",
                Description = "Fresh salmon roll",
                Price = 12.99m,
                CategoryId = 1,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            };
            var menuItem2 = new MenuItem
            {
                Id = 2,
                Name = "Green Tea",
                Description = "Traditional green tea",
                Price = 3.50m,
                CategoryId = 2,
                IsAvailable = true,
                CreatedAt = DateTime.Now
            };
            _context.MenuItems.AddRange(menuItem1, menuItem2);

            // Seed Employee
            var employee = new Employee
            {
                Id = 1,
                Username = "johndoe",
                PasswordHash = "hashedpassword123",
                FullName = "John Doe",
                Email = "john.doe@sakanahouse.com",
                Role = "Server",
                IsActive = true,
                CreatedAt = DateTime.Now
            };
            _context.Employees.Add(employee);

            // Seed Table
            var table = new Table { Id = 1, TableNumber = 5, Capacity = 4, Status = "Available" };
            _context.Tables.Add(table);

            // Seed Customer
            var customer = new Customer { Id = 1, FullName = "Jane Smith", Phone = "123-456-7890" };
            _context.Customers.Add(customer);

            _context.SaveChanges();

            // Seed Orders
            var order1 = new Order
            {
                Id = 1,
                TableId = 1,
                EmployeeId = 1,
                CustomerId = 1,
                OrderTime = DateTime.Now.AddDays(-2),
                TotalAmount = 29.48m,
                Status = "Completed"
            };

            var order2 = new Order
            {
                Id = 2,
                TableId = 1,
                EmployeeId = 1,
                CustomerId = 1,
                OrderTime = DateTime.Now.AddDays(-1),
                TotalAmount = 16.49m,
                Status = "Pending"
            };

            _context.Orders.AddRange(order1, order2);
            _context.SaveChanges();

            // Seed OrderItems
            var orderItems = new List<OrderItem>
            {
                new OrderItem { Id = 1, OrderId = 1, MenuItemId = 1, Quantity = 2, UnitPrice = 12.99m },
                new OrderItem { Id = 2, OrderId = 1, MenuItemId = 2, Quantity = 1, UnitPrice = 3.50m },
                new OrderItem { Id = 3, OrderId = 2, MenuItemId = 1, Quantity = 1, UnitPrice = 12.99m },
                new OrderItem { Id = 4, OrderId = 2, MenuItemId = 2, Quantity = 1, UnitPrice = 3.50m }
            };

            _context.OrderItems.AddRange(orderItems);
            _context.SaveChanges();
        }

        [TestMethod]
        public void GetAllOrderItems_ReturnsAllOrderItemsWithIncludes()
        {
            // Act
            var orderItems = _orderItemDAO.GetAllOrderItems();

            // Assert
            Assert.IsNotNull(orderItems, "OrderItems list should not be null");
            Assert.AreEqual(4, orderItems.Count, "Should return exactly 4 order items");

            foreach (var item in orderItems)
            {
                Assert.IsNotNull(item.MenuItem, $"OrderItem {item.Id} should have MenuItem included");
                Assert.IsNotNull(item.MenuItem.Category, $"OrderItem {item.Id} MenuItem should have Category included");
                Assert.IsNotNull(item.Order, $"OrderItem {item.Id} should have Order included");
            }
        }

        [TestMethod]
        public void GetOrderItemsByDateRange_ValidDateRange_ReturnsFilteredItems()
        {
            // Arrange
            DateTime fromDate = DateTime.Now.AddDays(-3);
            DateTime toDate = DateTime.Now;

            // Act
            var orderItems = _orderItemDAO.GetOrderItemsByDateRange(fromDate, toDate);

            // Assert
            Assert.IsNotNull(orderItems, "OrderItems list should not be null");
            Assert.AreEqual(4, orderItems.Count, "Should return 4 order items within date range");

            foreach (var item in orderItems)
            {
                Assert.IsNotNull(item.Order, "OrderItem should have Order included");
                Assert.IsTrue(item.Order.OrderTime >= fromDate,
                    $"OrderItem {item.Id} order time should be >= fromDate");
                Assert.IsTrue(item.Order.OrderTime <= toDate.AddDays(1),
                    $"OrderItem {item.Id} order time should be <= toDate + 1 day");
            }
        }

        [TestMethod]
        public void GetOrderItemsByDateRange_NoItemsInRange_ReturnsEmptyList()
        {
            // Arrange - date range with no order items
            DateTime fromDate = DateTime.Now.AddDays(-10);
            DateTime toDate = DateTime.Now.AddDays(-5);

            // Act
            var orderItems = _orderItemDAO.GetOrderItemsByDateRange(fromDate, toDate);

            // Assert
            Assert.IsNotNull(orderItems, "OrderItems list should not be null");
            Assert.AreEqual(0, orderItems.Count, "Should return empty list when no items in range");
        }

        [TestMethod]
        public void GetOrderItemsByMenuItem_ValidMenuItemId_ReturnsFilteredItems()
        {
            // Act
            var orderItems = _orderItemDAO.GetOrderItemsByMenuItem(1);

            // Assert
            Assert.IsNotNull(orderItems, "OrderItems list should not be null");
            Assert.AreEqual(2, orderItems.Count, "Should return 2 order items for menu item 1");

            foreach (var item in orderItems)
            {
                Assert.AreEqual(1, item.MenuItemId, "All order items should belong to menu item 1");
            }
        }

        [TestMethod]
        public void GetOrderItemsByMenuItem_NonExistentMenuItemId_ReturnsEmptyList()
        {
            // Act
            var orderItems = _orderItemDAO.GetOrderItemsByMenuItem(999);

            // Assert
            Assert.IsNotNull(orderItems, "OrderItems list should not be null");
            Assert.AreEqual(0, orderItems.Count, "Should return empty list for non-existent menu item");
        }

        [TestMethod]
        public void GetOrderItemsByOrder_ValidOrderId_ReturnsFilteredItems()
        {
            // Act
            var orderItems = _orderItemDAO.GetOrderItemsByOrder(1);

            // Assert
            Assert.IsNotNull(orderItems, "OrderItems list should not be null");
            Assert.AreEqual(2, orderItems.Count, "Should return 2 order items for order 1");

            foreach (var item in orderItems)
            {
                Assert.AreEqual(1, item.OrderId, "All order items should belong to order 1");
            }
        }

        [TestMethod]
        public void GetOrderItemsByOrder_NonExistentOrderId_ReturnsEmptyList()
        {
            // Act
            var orderItems = _orderItemDAO.GetOrderItemsByOrder(999);

            // Assert
            Assert.IsNotNull(orderItems, "OrderItems list should not be null");
            Assert.AreEqual(0, orderItems.Count, "Should return empty list for non-existent order");
        }

        [TestMethod]
        public void GetMenuPerformanceByDateRange_ValidDateRange_ReturnsPerformanceData()
        {
            // Arrange
            DateTime fromDate = DateTime.Now.AddDays(-3);
            DateTime toDate = DateTime.Now;

            // Act
            var performanceData = _orderItemDAO.GetMenuPerformanceByDateRange(fromDate, toDate);

            // Assert
            Assert.IsNotNull(performanceData, "Performance data list should not be null");
            Assert.AreEqual(2, performanceData.Count, "Should return performance data for 2 menu items");

            foreach (var data in performanceData)
            {
                Assert.IsTrue(data.MenuItemId > 0, "MenuItemId should be greater than 0");
                Assert.IsFalse(string.IsNullOrEmpty(data.MenuItemName), "MenuItemName should not be null or empty");
                Assert.IsFalse(string.IsNullOrEmpty(data.CategoryName), "CategoryName should not be null or empty");
                Assert.IsTrue(data.QuantitySold > 0, "QuantitySold should be greater than 0");
                Assert.IsTrue(data.Revenue > 0, "Revenue should be greater than 0");
                Assert.IsTrue(data.AveragePrice > 0, "AveragePrice should be greater than 0");
            }

            // Verify the data is ordered by quantity sold descending
            for (int i = 0; i < performanceData.Count - 1; i++)
            {
                Assert.IsTrue(performanceData[i].QuantitySold >= performanceData[i + 1].QuantitySold,
                    "Performance data should be ordered by QuantitySold descending");
            }
        }

        [TestMethod]
        public void GetMenuPerformanceByDateRange_OnlyCompletedOrders_ReturnsCorrectData()
        {
            // Arrange - this test verifies only completed orders are included
            DateTime fromDate = DateTime.Now.AddDays(-3);
            DateTime toDate = DateTime.Now;

            // Act
            var performanceData = _orderItemDAO.GetMenuPerformanceByDateRange(fromDate, toDate);

            // Assert
            Assert.IsNotNull(performanceData, "Performance data should not be null");

            // Since only order 1 is "Completed", we should only see data from that order
            var salmonRollData = performanceData.FirstOrDefault(p => p.MenuItemName == "Salmon Roll");
            Assert.IsNotNull(salmonRollData, "Should have performance data for Salmon Roll");
            Assert.AreEqual(2, salmonRollData.QuantitySold, "Should show quantity from completed order only");

            var greenTeaData = performanceData.FirstOrDefault(p => p.MenuItemName == "Green Tea");
            Assert.IsNotNull(greenTeaData, "Should have performance data for Green Tea");
            Assert.AreEqual(1, greenTeaData.QuantitySold, "Should show quantity from completed order only");
        }

        [TestMethod]
        public void GetMenuPerformanceByDateRange_NoCompletedOrdersInRange_ReturnsEmptyList()
        {
            // Arrange - date range with no completed orders
            DateTime fromDate = DateTime.Now.AddDays(-10);
            DateTime toDate = DateTime.Now.AddDays(-5);

            // Act
            var performanceData = _orderItemDAO.GetMenuPerformanceByDateRange(fromDate, toDate);

            // Assert
            Assert.IsNotNull(performanceData, "Performance data should not be null");
            Assert.AreEqual(0, performanceData.Count, "Should return empty list when no completed orders in range");
        }
    }
}
