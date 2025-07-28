# Sakanahouse POS Project

## Overview
Sakanahouse POS is a desktop point-of-sale system tailored for small to medium-sized retail and hospitality businesses. Built with .NET WPF, it offers a modern, responsive interface for managing sales, inventory, and customer data. Using C# and XAML, it delivers a robust, Windows-native experience with high performance and reliability.

## Features
- **Sales Management**: Process transactions, issue refunds, and apply discounts seamlessly.
- **Inventory Tracking**: Monitor stock levels, receive low-stock alerts, and manage product variants.
- **Customer Management**: Track purchase history and manage loyalty programs.
- **Reporting**: Generate sales and inventory reports for business insights.
- **User-Friendly Interface**: Customizable WPF UI with modern, Windows-native design.
- **Offline Support**: Operate offline with data synchronization upon reconnection.

## Tech Stack
- **Framework**: .NET (WPF, C#)
- **UI**: XAML for responsive, Windows-native interfaces
- **Database**: SQL Server (default), with SQLite support
- **Tools**: Visual Studio 2022 or later
- **Dependencies**: Entity Framework Core, MahApps.Metro (optional for enhanced UI styling)

## Prerequisites
- Visual Studio 2022 or later with .NET Desktop Development workload
- .NET 8.0 SDK
- SQL Server or SQLite (based on configuration)

## Installation
1. Clone the repository:
   ```
   git clone https://github.com/username/sakanahouse-pos.git
   ```
2. Open the solution in Visual Studio:
   - Open `SakanahousePOS.sln`.
3. Restore NuGet packages:
   - Right-click the solution in Solution Explorer and select "Restore NuGet Packages."
4. Configure the database:
   - Update the connection string in `appsettings.json` for SQL Server.
5. Build and run:
   - Press `F5` or select "Start" in Visual Studio.

## Usage
- Launch the application from Visual Studio or the compiled executable.
- Log in with default credentials: `admin` / `password`.
- Configure products, categories, and users via the Settings menu.
- Use the main dashboard for sales, inventory management, and reports.
- Customize UI themes (if using MahApps.Metro) in the application settings.

## Project Structure
- **SakanahousePOS/**: Main WPF project with XAML views and C# code-behind.
- **Models/**: Data models for products, customers, and transactions.
- **ViewModels/**: MVVM pattern for UI logic.
- **Data/**: Entity Framework Core context and database migrations.
- **Resources/**: XAML styles and assets.

## Contributing
Contributions are welcome! To contribute:
1. Fork the repository.
2. Create a feature branch: `git checkout -b feature-name`
3. Commit changes: `git commit -m "Add feature"`
4. Push to the branch: `git push origin feature-name`
5. Open a pull request with a detailed description.
