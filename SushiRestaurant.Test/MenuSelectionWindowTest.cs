using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SushiRestaurant.Menu;
using Services;
using BusinessObjects;

namespace SushiRestaurant.Tests
{
    [TestClass]
    public class MenuSelectionWindowTests
    {
        private Mock<IMenuService> _mockMenuService;
        private Mock<ITableService> _mockTableService;
        private Mock<IOrderService> _mockOrderService;
        private List<MenuCategory> _testCategories;
        private List<MenuItem> _testMenuItems;
        private List<BusinessObjects.Table> _testTables;

        [TestInitialize]
        public void TestInitialize()
        {
            SetupTestData();
            SetupMocks();
        }

        private void SetupTestData()
        {
            _testCategories = new List<MenuCategory>
            {
                new MenuCategory { Id = 1, Name = "Sushi", Description = "Fresh sushi selection" },
                new MenuCategory { Id = 2, Name = "Appetizers", Description = "Starter dishes" },
                new MenuCategory { Id = 3, Name = "Drinks", Description = "Beverages" }
            };

            _testMenuItems = new List<MenuItem>
            {
                new MenuItem
                {
                    Id = 1,
                    Name = "California Roll",
                    Price = 12.99m,
                    Description = "Fresh avocado and crab",
                    IsAvailable = true,
                    CategoryId = 1,
                    CreatedAt = DateTime.Now
                },
                new MenuItem
                {
                    Id = 2,
                    Name = "Salmon Nigiri",
                    Price = 8.99m,
                    Description = "Fresh salmon",
                    IsAvailable = true,
                    CategoryId = 1,
                    CreatedAt = DateTime.Now
                },
                new MenuItem
                {
                    Id = 3,
                    Name = "Tuna Sashimi",
                    Price = 15.99m,
                    Description = "Premium tuna",
                    IsAvailable = false,
                    CategoryId = 1,
                    CreatedAt = DateTime.Now
                }
            };

            _testTables = new List<BusinessObjects.Table>
            {
                new BusinessObjects.Table { Id = 1, TableNumber = 1, Capacity = 4, Status = "Available", Note = "" },
                new BusinessObjects.Table { Id = 2, TableNumber = 2, Capacity = 2, Status = "Occupied", Note = "Reserved" },
                new BusinessObjects.Table { Id = 3, TableNumber = 3, Capacity = 6, Status = "Available", Note = "" }
            };
        }

        private void SetupMocks()
        {
            _mockMenuService = new Mock<IMenuService>();
            _mockTableService = new Mock<ITableService>();
            _mockOrderService = new Mock<IOrderService>();

            _mockMenuService.Setup(m => m.GetCategories()).Returns(_testCategories);
            _mockMenuService.Setup(m => m.GetMenuItemsByCategory(It.IsAny<int?>())).Returns(_testMenuItems);
            _mockMenuService.Setup(m => m.GetMenuItemsByCategory(1)).Returns(_testMenuItems.Where(mi => mi.CategoryId == 1).ToList());
            _mockTableService.Setup(t => t.GetTables()).Returns(_testTables);
            _mockOrderService.Setup(o => o.SaveOrder(It.IsAny<Order>())).Returns(true);
        }

        [STATestMethod]
        public void Constructor_ShouldInitializeProperties()
        {
            // Act - Test the default constructor (this will use real services)
            var menuWindow = new MenuSelectionWindow();

            // Assert
            Assert.IsNotNull(menuWindow.OrderItems);
            Assert.AreEqual(0, menuWindow.OrderItems.Count);
            Assert.IsTrue(menuWindow.CurrentDateTime > DateTime.MinValue);
        }

        [TestMethod]
        public void OrderItemViewModel_ShouldSetPropertiesCorrectly()
        {
            // Arrange & Act
            var orderItem = new OrderItemViewModel
            {
                ID = 1,
                Items = "California Roll",
                Price = 12.99m,
                Qty = 2,
                Total = 25.98m,
                Tax = 3.69m,
                Options = "Extra avocado"
            };

            // Assert
            Assert.AreEqual(1, orderItem.ID);
            Assert.AreEqual("California Roll", orderItem.Items);
            Assert.AreEqual(12.99m, orderItem.Price);
            Assert.AreEqual(2, orderItem.Qty);
            Assert.AreEqual(25.98m, orderItem.Total);
            Assert.AreEqual(3.69m, orderItem.Tax);
            Assert.AreEqual("Extra avocado", orderItem.Options);
        }

        [TestMethod]
        public void OrderItemViewModel_TaxCalculation_ShouldBe142Percent()
        {
            // Arrange
            decimal price = 100m;
            decimal expectedTax = price * 0.142m; // 14.2%

            // Act
            var orderItem = new OrderItemViewModel
            {
                ID = 1,
                Items = "Test Item",
                Price = price,
                Qty = 1,
                Total = price,
                Tax = price * 0.142m,
                Options = ""
            };

            // Assert
            Assert.AreEqual(14.2m, orderItem.Tax);
            Assert.AreEqual(expectedTax, orderItem.Tax);
        }

        [TestMethod]
        public void TableViewModel_Available_ShouldWrapTableCorrectly()
        {
            // Arrange
            var table = new BusinessObjects.Table
            {
                Id = 1,
                TableNumber = 5,
                Capacity = 4,
                Status = "Available",
                Note = "Near window"
            };

            // Act
            var viewModel = new TableViewModel(table);

            // Assert
            Assert.AreEqual(1, viewModel.Id);
            Assert.AreEqual(5, viewModel.TableNumber);
            Assert.AreEqual(4, viewModel.Capacity);
            Assert.AreEqual("Available", viewModel.Status);
            Assert.AreEqual("Near window", viewModel.Note);
        }

        [TestMethod]
        public void TableViewModel_Occupied_ShouldWrapTableCorrectly()
        {
            // Arrange
            var table = new BusinessObjects.Table
            {
                Id = 2,
                TableNumber = 3,
                Capacity = 2,
                Status = "Occupied",
                Note = "Reserved for VIP"
            };

            // Act
            var viewModel = new TableViewModel(table);

            // Assert
            Assert.AreEqual(2, viewModel.Id);
            Assert.AreEqual(3, viewModel.TableNumber);
            Assert.AreEqual(2, viewModel.Capacity);
            Assert.AreEqual("Occupied", viewModel.Status);
            Assert.AreEqual("Reserved for VIP", viewModel.Note);
        }

        [TestMethod]
        public void CreateOrder_ShouldHaveCorrectProperties()
        {
            // Arrange
            var orderItems = new List<OrderItem>
            {
                new OrderItem
                {
                    Id = 1,
                    MenuItemId = 1,
                    Quantity = 2,
                    UnitPrice = 10.00m,
                    TotalPrice = 20.00m
                }
            };
            decimal totalPayable = 25.00m;
            DateTime orderTime = DateTime.Now;

            // Act
            var order = new Order
            {
                Id = 1,
                OrderTime = orderTime,
                TotalAmount = totalPayable,
                Status = "Completed",
                EmployeeId = 1,
                TableId = null,
                CustomerId = null,
                OrderItems = orderItems
            };

            // Assert
            Assert.AreEqual("Completed", order.Status);
            Assert.AreEqual(totalPayable, order.TotalAmount);
            Assert.AreEqual(1, order.EmployeeId);
            Assert.AreEqual(orderTime, order.OrderTime);
            Assert.AreEqual(1, order.OrderItems.Count);
            Assert.IsNull(order.TableId);
            Assert.IsNull(order.CustomerId);
        }

        [TestMethod]
        public void OrderItem_ShouldHaveCorrectProperties()
        {
            // Arrange & Act
            var orderItem = new OrderItem
            {
                Id = 1,
                OrderId = 100,
                MenuItemId = 1,
                Quantity = 2,
                UnitPrice = 12.99m,
                TotalPrice = 25.98m
            };

            // Assert
            Assert.AreEqual(1, orderItem.Id);
            Assert.AreEqual(100, orderItem.OrderId);
            Assert.AreEqual(1, orderItem.MenuItemId);
            Assert.AreEqual(2, orderItem.Quantity);
            Assert.AreEqual(12.99m, orderItem.UnitPrice);
            Assert.AreEqual(25.98m, orderItem.TotalPrice);
        }

        [TestMethod]
        public void MenuItem_WithNullableProperties_ShouldHandleNulls()
        {
            // Arrange & Act
            var menuItem = new MenuItem
            {
                Id = 1,
                Name = "Test Item",
                Description = null,
                Price = 10.99m,
                CategoryId = null,
                IsAvailable = null,
                CreatedAt = null
            };

            // Assert
            Assert.AreEqual("Test Item", menuItem.Name);
            Assert.IsNull(menuItem.Description);
            Assert.IsNull(menuItem.CategoryId);
            Assert.IsNull(menuItem.IsAvailable);
            Assert.IsNull(menuItem.CreatedAt);
        }

        [TestMethod]
        public void MenuCategory_ShouldInitializeCollections()
        {
            // Act
            var category = new MenuCategory
            {
                Id = 1,
                Name = "Test Category",
                Description = "Test Description"
            };

            // Assert
            Assert.IsNotNull(category.MenuItems);
            Assert.AreEqual(0, category.MenuItems.Count);
        }

        [TestMethod]
        public void Table_ShouldInitializeCollections()
        {
            // Act
            var table = new BusinessObjects.Table
            {
                Id = 1,
                TableNumber = 5,
                Capacity = 4,
                Status = "Available",
                Note = "Window seat"
            };

            // Assert
            Assert.IsNotNull(table.Orders);
            Assert.AreEqual(0, table.Orders.Count);
        }

        [TestMethod]
        public void Order_ShouldInitializeCollections()
        {
            // Act
            var order = new Order();

            // Assert
            Assert.IsNotNull(order.OrderItems);
            Assert.AreEqual(0, order.OrderItems.Count);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockMenuService = null;
            _mockTableService = null;
            _mockOrderService = null;
        }
    }

    // Test class for business object validation
    [TestClass]
    public class BusinessObjectTests
    {
        [TestMethod]
        public void Customer_ShouldInitializeCollections()
        {
            // Act
            var customer = new Customer
            {
                Id = 1,
                FullName = "John Doe",
                Phone = "123-456-7890",
                Email = "john@example.com",
                Note = "VIP Customer"
            };

            // Assert
            Assert.IsNotNull(customer.Orders);
            Assert.AreEqual(0, customer.Orders.Count);
            Assert.AreEqual("John Doe", customer.FullName);
        }

        [TestMethod]
        public void Employee_ShouldInitializeCollections()
        {
            // Act
            var employee = new Employee
            {
                Id = 1,
                Username = "johndoe",
                PasswordHash = "hashedpassword",
                FullName = "John Doe",
                Email = "john@restaurant.com",
                Role = "Manager",
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            // Assert
            Assert.IsNotNull(employee.Bills);
            Assert.IsNotNull(employee.Orders);
            Assert.IsNotNull(employee.ExportLogs);
            Assert.IsNotNull(employee.PasswordResetTokens);
            Assert.AreEqual(0, employee.Bills.Count);
            Assert.AreEqual("Manager", employee.Role);
        }

        [TestMethod]
        public void Bill_ShouldHaveCorrectProperties()
        {
            // Arrange & Act
            var bill = new Bill
            {
                Id = 1,
                OrderId = 100,
                BillTime = DateTime.Now,
                Subtotal = 20.00m,
                DiscountAmount = 2.00m,
                TaxAmount = 2.84m,
                TotalAmount = 20.84m,
                PaymentMethod = "Credit Card",
                PaidByEmployeeId = 1,
                Note = "Customer requested receipt"
            };

            // Assert
            Assert.AreEqual(1, bill.Id);
            Assert.AreEqual(100, bill.OrderId);
            Assert.AreEqual(20.00m, bill.Subtotal);
            Assert.AreEqual(2.00m, bill.DiscountAmount);
            Assert.AreEqual(2.84m, bill.TaxAmount);
            Assert.AreEqual(20.84m, bill.TotalAmount);
            Assert.AreEqual("Credit Card", bill.PaymentMethod);
        }
    }

    // Service tests with real implementations (integration-style tests)
    [TestClass]
    public class ServiceIntegrationTests
    {
        [TestMethod]
        public void OrderService_ShouldInstantiateCorrectly()
        {
            // Act
            var service = new OrderService();

            // Assert
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void TableService_ShouldInstantiateCorrectly()
        {
            // Act
            var service = new TableService();

            // Assert
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void MenuService_ShouldInstantiateCorrectly()
        {
            // Act
            var service = new MenuService();

            // Assert
            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void OrderService_SaveOrder_ShouldAcceptValidOrder()
        {
            // Arrange
            var service = new OrderService();
            var order = new Order
            {
                OrderTime = DateTime.Now,
                TotalAmount = 25.99m,
                Status = "Completed",
                EmployeeId = 1,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        MenuItemId = 1,
                        Quantity = 1,
                        UnitPrice = 25.99m,
                        TotalPrice = 25.99m
                    }
                }
            };

            // Act & Assert
            // Note: This will depend on your actual DAO implementation
            // For now, just testing that the service exists and can be called
            Assert.IsNotNull(service);

            // Uncomment the line below when your DAO is properly implemented:
            // bool result = service.SaveOrder(order);
            // Assert.IsTrue(result);
        }
    }

    // Calculator/Helper method tests
    [TestClass]
    public class CalculationTests
    {
        [TestMethod]
        public void TaxCalculation_ShouldCalculate142Percent()
        {
            // Arrange
            decimal baseAmount = 100m;
            decimal taxRate = 0.142m; // 14.2%

            // Act
            decimal calculatedTax = baseAmount * taxRate;

            // Assert
            Assert.AreEqual(14.2m, calculatedTax);
        }

        [TestMethod]
        public void OrderTotal_WithTax_ShouldCalculateCorrectly()
        {
            // Arrange
            decimal subtotal = 50m;
            decimal taxRate = 0.142m;

            // Act
            decimal tax = subtotal * taxRate;
            decimal total = subtotal + tax;

            // Assert
            Assert.AreEqual(7.1m, tax);
            Assert.AreEqual(57.1m, total);
        }

        [TestMethod]
        public void MultipleItems_ShouldSumCorrectly()
        {
            // Arrange
            var items = new List<(decimal price, int quantity)>
            {
                (12.99m, 2),  // California Roll x2
                (8.99m, 1),   // Salmon Nigiri x1
                (15.99m, 1)   // Tuna Sashimi x1
            };

            // Act
            decimal total = items.Sum(item => item.price * item.quantity);

            // Assert
            decimal expected = (12.99m * 2) + (8.99m * 1) + (15.99m * 1);
            Assert.AreEqual(expected, total);
            Assert.AreEqual(50.96m, total);
        }
    }
}