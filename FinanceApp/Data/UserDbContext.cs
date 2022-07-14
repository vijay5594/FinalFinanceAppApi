using FinanceApp.Model;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> Options) : base(Options)
        {
        }
        public DbSet<LoginModel> LoginModels { get; set; }
        public DbSet<ProductModel> ProductModels { get; set; }
        public DbSet<CustomerModel> CustomerModels { get; set; }
        public DbSet<PaymentModel> PaymentModels { get; set; }
        public DbSet<FileUploadModel> FileAttachment { get; set; }
        public DbSet<ProductCustomerModel> ProductCustomerModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBulider)
        {
            modelBulider.Entity<LoginModel>().ToTable("logintable");
            modelBulider.Entity<ProductModel>().ToTable("productdetails");
            modelBulider.Entity<CustomerModel>().ToTable("customerdetails");
            modelBulider.Entity<PaymentModel>().ToTable("paymentdetails");
            modelBulider.Entity<FileUploadModel>().ToTable("attachmentdetails");
            modelBulider.Entity<ProductCustomerModel>().ToTable("productcustomer");
        }
    }
}
