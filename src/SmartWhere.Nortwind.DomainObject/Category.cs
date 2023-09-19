using System.ComponentModel.DataAnnotations;

namespace SmartWhere.Northwind.DomainObject
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public byte[] Picture { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}
