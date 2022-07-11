using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceApp.Model
{
    public class CustomerModel
    {
        [Key]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string GuarantorName{ get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public string AdditionalMobileNumber { get; set; }
        public string AadharNumber { get; set; }      
        public string ReferredBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateOfCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateOfModified { get; set; }
        public bool IsActive { get; set; } = true;
        public int AttachmentId { get; set; }
        public string Status { get; set; }


    }
}
