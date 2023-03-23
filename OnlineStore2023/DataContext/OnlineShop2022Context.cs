using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OnlineStore2023.DataContext.Models;

namespace OnlineStore2023.DataContext;

public partial class OnlineShop2022Context : DbContext
{
    public OnlineShop2022Context()
    {
    }

    public OnlineShop2022Context(DbContextOptions<OnlineShop2022Context> options)
        : base(options)
    {
    }


    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductOrder> ProductOrders { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersDatum> UsersData { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=.;Database=OnlineShop2022;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
            entity.Property(e => e.CustomerAdressLine)
                .HasMaxLength(300);
            entity.Property(e => e.CustomerEmailAdress).HasMaxLength(50);

            entity.HasOne(d => d.User).WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Customers__UserI__656C112C");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
            entity.Property(e => e.JobTitle)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.User).WithOne(p => p.Employee)
                .HasForeignKey<Employee>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employees__UserI__6754599E");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAF73D0D8EC");

            entity.Property(e => e.OrderId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("OrderID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DeliveryAddress)
                .IsRequired()
                .HasMaxLength(300);
            entity.Property(e => e.OrderPaid);
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(50);
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.TotalPrice).HasColumnType("money");

            entity.HasOne(d => d.Customer).WithMany(p => p.OrderCustomers)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Orders__Customer__6EF57B66");

            entity.HasOne(d => d.Employee).WithMany(p => p.OrderEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Orders__Employee__6E01572D");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6ED8C87D26A");

            entity.Property(e => e.ProductId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ProductID");
            entity.Property(e => e.IsArchive).HasDefaultValueSql("((1))");
            entity.Property(e => e.ProductColor).HasMaxLength(100);
            entity.Property(e => e.ProductName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.ProductPhoto).HasColumnType("image");
            entity.Property(e => e.ProductPrice).HasColumnType("money");
            entity.Property(e => e.ProductQuantity).HasDefaultValueSql("((0))");
            entity.Property(e => e.ProductVolume).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ProductWeight).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<ProductOrder>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("PK_OrderID_ProductID");

            entity.ToTable("ProductOrder");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.ProductsInOrderTotalPrice).HasColumnType("money");

            entity.HasOne(d => d.Order).WithMany(p => p.ProductOrders)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ProductOr__Order__73BA3083");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductOrders)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductOr__Produ__74AE54BC");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC292FA3AF");

            entity.HasIndex(e => e.UserLogin, "UQ__Users__7F8E8D5E4A59B250").IsUnique();

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("UserID");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.UserLogin)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.UserPasswordHash).IsRequired();
            entity.Property(e => e.UserPasswordSalt)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.UserType)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<UsersDatum>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
            entity.Property(e => e.UserFirstName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.UserLastName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.UserMiddleName).HasMaxLength(50);
            entity.Property(e => e.UserPhoneNumber)
                .HasMaxLength(25);

            entity.HasOne(d => d.User).WithOne(p => p.UsersDatum)
                .HasForeignKey<UsersDatum>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsersData__UserI__60A75C0F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
