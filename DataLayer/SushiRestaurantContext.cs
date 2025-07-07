using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer;

public partial class SushiRestaurantContext : DbContext
{
    public SushiRestaurantContext()
    {
    }

    public SushiRestaurantContext(DbContextOptions<SushiRestaurantContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<MenuItem> MenuItems { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Table> Tables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server =MINHCHAU\\DBI202; database = Sushi_Restaurant;uid=sa;pwd=12345;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.BillId).HasName("PK__Bill__11F2FC4A9CCDEB85");

            entity.ToTable("Bill");

            entity.Property(e => e.BillId).HasColumnName("BillID");
            entity.Property(e => e.BillDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.TableId).HasColumnName("TableID");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Bills)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bill__CustomerID__52593CB8");

            entity.HasOne(d => d.Table).WithMany(p => p.Bills)
                .HasForeignKey(d => d.TableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bill__TableID__5165187F");

            entity.HasMany(d => d.OrderDetails).WithMany(p => p.Bills)
                .UsingEntity<Dictionary<string, object>>(
                    "BillOrderDetail",
                    r => r.HasOne<OrderDetail>().WithMany()
                        .HasForeignKey("OrderDetailId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__BillOrder__Order__5629CD9C"),
                    l => l.HasOne<Bill>().WithMany()
                        .HasForeignKey("BillId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__BillOrder__BillI__5535A963"),
                    j =>
                    {
                        j.HasKey("BillId", "OrderDetailId").HasName("PK__BillOrde__CCC9617A68907DB8");
                        j.ToTable("BillOrderDetail");
                        j.IndexerProperty<int>("BillId").HasColumnName("BillID");
                        j.IndexerProperty<int>("OrderDetailId").HasColumnName("OrderDetailID");
                    });
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A2B14B1D2C5");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(255);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B8B7835F00");

            entity.ToTable("Customer");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Note).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF1DD3FC76F");

            entity.ToTable("Employee");

            entity.HasIndex(e => e.UserName, "UQ__Employee__C9F28456493E740B").IsUnique();

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.JobTitle).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(e => e.MenuItemId).HasName("PK__MenuItem__8943F702F1E97EF3");

            entity.ToTable("MenuItem");

            entity.Property(e => e.MenuItemId).HasColumnName("MenuItemID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.ImagePath).HasMaxLength(255);
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.MenuItems)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MenuItem__Catego__4222D4EF");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BAFFB84B9DE");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__CustomerI__45F365D3");

            entity.HasOne(d => d.Employee).WithMany(p => p.Orders)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__EmployeeI__46E78A0C");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D30CA62422D4");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.MenuItemId).HasColumnName("MenuItemID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.MenuItem).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.MenuItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__MenuI__4BAC3F29");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__4AB81AF0");
        });

        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.TableId).HasName("PK__Table__7D5F018E629F5E5E");

            entity.ToTable("Table");

            entity.HasIndex(e => e.TableNumber, "UQ__Table__E8E0DB52B0F9D477").IsUnique();

            entity.Property(e => e.TableId).HasColumnName("TableID");
            entity.Property(e => e.TableNumber).HasMaxLength(10);
            entity.Property(e => e.TableStatus).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
