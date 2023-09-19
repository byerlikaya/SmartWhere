using System.ComponentModel.DataAnnotations;

namespace SmartWhere.Northwind.DomainObject
{
    public class OrderDetail
    {
        [Key]
        public int OrderID { get; set; }

        public int ProductID { get; set; }

        public decimal UnitPrice { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }
    }
}
