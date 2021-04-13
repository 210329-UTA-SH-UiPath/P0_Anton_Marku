using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace PbDb.Storing.Entities
{
    public partial class myDBContext : DbContext
    {
        public myDBContext()
        {
        }

        public myDBContext(DbContextOptions<myDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Crust> Crusts { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderPizza> OrderPizzas { get; set; }
        public virtual DbSet<Pizza> Pizzas { get; set; }
        public virtual DbSet<PizzaTopping> PizzaToppings { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<Topping> Toppings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:p1.database.windows.net,1433;Initial Catalog=myDB;User ID=p1;Password=Pword000");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Crust>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Price).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LastStoreVisitTime).HasColumnType("datetime");

                entity.Property(e => e.LastTimeOrdered).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.LastStoreVisitedNavigation)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.LastStoreVisited)
                    .HasConstraintName("FK_Customers_Stores");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.DateAndTime).HasColumnType("datetime");

                entity.Property(e => e.Price).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Orders_Customers");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Stores");
            });

            modelBuilder.Entity<OrderPizza>(entity =>
            {
                entity.ToTable("Order-Pizzas");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.PizzaId).HasColumnName("PizzaID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderPizzas)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_Order-Pizzas_Orders");

                entity.HasOne(d => d.Pizza)
                    .WithMany(p => p.OrderPizzas)
                    .HasForeignKey(d => d.PizzaId)
                    .HasConstraintName("FK_Order-Pizzas_Pizzas");
            });

            modelBuilder.Entity<Pizza>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CrustId).HasColumnName("CrustID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.SizeId).HasColumnName("SizeID");

                entity.HasOne(d => d.Crust)
                    .WithMany(p => p.Pizzas)
                    .HasForeignKey(d => d.CrustId)
                    .HasConstraintName("FK_Pizzas_Crusts");

                entity.HasOne(d => d.Size)
                    .WithMany(p => p.Pizzas)
                    .HasForeignKey(d => d.SizeId)
                    .HasConstraintName("FK_Pizzas_Sizes");
            });

            modelBuilder.Entity<PizzaTopping>(entity =>
            {
                entity.HasKey(e => new { e.PizzaId, e.ToppingId });

                entity.ToTable("Pizza-Toppings");

                entity.Property(e => e.PizzaId).HasColumnName("PizzaID");

                entity.Property(e => e.ToppingId).HasColumnName("ToppingID");

                entity.HasOne(d => d.Pizza)
                    .WithMany(p => p.PizzaToppings)
                    .HasForeignKey(d => d.PizzaId)
                    .HasConstraintName("FK_PizzaToppings_Pizzas");

                entity.HasOne(d => d.Topping)
                    .WithMany(p => p.PizzaToppings)
                    .HasForeignKey(d => d.ToppingId)
                    .HasConstraintName("FK_PizzaToppings_Toppings");
            });

            modelBuilder.Entity<Size>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Price).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Topping>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Price).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
