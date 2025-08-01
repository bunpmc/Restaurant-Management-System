using System;
using System.Collections.Generic;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration.Json;


namespace DataAccessLayer;

public partial class SakanaHouseContext : DbContext
{
    public SakanaHouseContext()
    {
    }

    public SakanaHouseContext(DbContextOptions<SakanaHouseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<ExportLog> ExportLogs { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<MenuCategory> MenuCategories { get; set; }

    public virtual DbSet<MenuItem> MenuItems { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

    public virtual DbSet<Table> Tables { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    private string GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
        return configuration["ConnectionStrings:MyDbConnection"];
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(GetConnectionString());
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bills__3214EC07E2EDA121");

            entity.HasIndex(e => e.OrderId, "UQ__Bills__C3905BCEF362B5D4").IsUnique();

            entity.Property(e => e.BillTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DiscountAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Note).HasMaxLength(255);
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.Subtotal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TaxAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Order).WithOne(p => p.Bill)
                .HasForeignKey<Bill>(d => d.OrderId)
                .HasConstraintName("FK__Bills__OrderId__5812160E");

            entity.HasOne(d => d.PaidByEmployee).WithMany(p => p.Bills)
                .HasForeignKey(d => d.PaidByEmployeeId)
                .HasConstraintName("FK__Bills__PaidByEmp__59063A47");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC079203936D");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Note).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC07CB03EFBA");

            entity.HasIndex(e => e.Username, "UQ__Employee__536C85E49DC14F14").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.FullName).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PasswordHash).HasMaxLength(256);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        modelBuilder.Entity<ExportLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ExportLo__3214EC079C71B1BF");

            entity.Property(e => e.ExportType).HasMaxLength(100);
            entity.Property(e => e.ExportedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.ExportedByEmployee).WithMany(p => p.ExportLogs)
                .HasForeignKey(d => d.ExportedByEmployeeId)
                .HasConstraintName("FK__ExportLog__Expor__6477ECF3");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Invoices__3214EC07B774EEF8");

            entity.HasIndex(e => e.BillId, "UQ__Invoices__11F2FC6B136B0B2D").IsUnique();

            entity.HasIndex(e => e.InvoiceNumber, "UQ__Invoices__D776E981631F2234").IsUnique();

            entity.Property(e => e.Exported).HasDefaultValue(false);
            entity.Property(e => e.InvoiceNumber).HasMaxLength(100);
            entity.Property(e => e.IssuedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Printed).HasDefaultValue(false);

            entity.HasOne(d => d.Bill).WithOne(p => p.Invoice)
                .HasForeignKey<Invoice>(d => d.BillId)
                .HasConstraintName("FK__Invoices__BillId__60A75C0F");
        });

        modelBuilder.Entity<MenuCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MenuCate__3214EC07D481FD03");

            entity.HasIndex(e => e.Name, "UQ__MenuCate__737584F6AA8F29CF").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MenuItem__3214EC07771A2400");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.MenuItems)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__MenuItems__Categ__412EB0B6");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC07E38DE76F");

            entity.Property(e => e.OrderTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Orders__Customer__4D94879B");

            entity.HasOne(d => d.Employee).WithMany(p => p.Orders)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Orders__Employee__4CA06362");

            entity.HasOne(d => d.Table).WithMany(p => p.Orders)
                .HasForeignKey(d => d.TableId)
                .HasConstraintName("FK__Orders__TableId__4BAC3F29");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderIte__3214EC07A73524F7");

            entity.Property(e => e.TotalPrice)
                .HasComputedColumnSql("([Quantity]*[UnitPrice])", true)
                .HasColumnType("decimal(21, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.MenuItem).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.MenuItemId)
                .HasConstraintName("FK__OrderItem__MenuI__5165187F");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderItem__Order__5070F446");
        });

        modelBuilder.Entity<PasswordResetToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Password__3214EC0742569294");

            entity.Property(e => e.Expiration).HasColumnType("datetime");
            entity.Property(e => e.IsUsed).HasDefaultValue(false);
            entity.Property(e => e.Token).HasMaxLength(128);

            entity.HasOne(d => d.Employee).WithMany(p => p.PasswordResetTokens)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__PasswordR__Emplo__68487DD7");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Stock__3214EC07");

            entity.Property(e => e.ItemName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.Unit).HasMaxLength(20);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10,2)");
            entity.Property(e => e.TotalValue).HasColumnType("decimal(10,2)");
            entity.Property(e => e.Supplier).HasMaxLength(50);
            entity.Property(e => e.Notes).HasMaxLength(200);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Active");
            entity.Property(e => e.LastUpdated).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tables__3214EC074AC041F2");

            entity.HasIndex(e => e.TableNumber, "UQ__Tables__E8E0DB52A1DEA97E").IsUnique();

            entity.Property(e => e.Note).HasMaxLength(200);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Available");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
