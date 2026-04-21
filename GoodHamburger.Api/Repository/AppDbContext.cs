using Microsoft.EntityFrameworkCore;
using Good_Hamburger.Models;
using Good_Hamburger.Enums;

namespace Good_Hamburger.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Food> Foods { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração das entidades
            modelBuilder.Entity<Food>().HasKey(f => f.Id);
            modelBuilder.Entity<Order>().HasKey(o => o.Id);
            modelBuilder.Entity<OrderItem>().HasKey(oi => oi.Id);

            // Relacionamentos
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne()
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Food)
                .WithMany()
                .HasForeignKey(oi => oi.FoodId);

            // Semear dados iniciais (Cardápio do Desafio)
            modelBuilder.Entity<Food>().HasData(
                new Food { Id = 1, Name = "X Burger", Price = 5.00m, TypeFood = FoodType.SANDUICHE },
                new Food { Id = 2, Name = "X Egg", Price = 4.50m, TypeFood = FoodType.SANDUICHE },
                new Food { Id = 3, Name = "X Bacon", Price = 7.00m, TypeFood = FoodType.SANDUICHE },
                new Food { Id = 4, Name = "Batata frita", Price = 2.00m, TypeFood = FoodType.ACOMPANHAMENTO },
                new Food { Id = 5, Name = "Refrigerante", Price = 2.50m, TypeFood = FoodType.BEBIDA }
            );
        }
    }
}
