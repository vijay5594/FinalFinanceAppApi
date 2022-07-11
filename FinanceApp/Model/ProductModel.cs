using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Model
{
    public class ProductModel
    {

        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public int ProductTenure { get; set; }
        public int NoOfCustomers { get; set; }
        public string ProductDescription { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateOfCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateOfModified { get; set; }
        public bool IsActive { get; set; } = true;
        public int Price { get; set; }
        public DateTime StartDate { get; set; }
        public string IsStatus { get; set; }




    }
}
