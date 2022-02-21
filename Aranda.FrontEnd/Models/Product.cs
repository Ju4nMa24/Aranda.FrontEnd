using System;
using System.Collections.Generic;

namespace Aranda.FrontEnd.Models
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string BriefDescription { get; set; }
        public string CategoryName { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime CreationDate { get; set; }
    }
    public class TransactionalProductCommand
    {
        public Product ProductRequest { get; set; }
        public Guid CategoryId { get; set; }
        public Proccesors ProccessType { get; set; }
    }
    public class ProductDetail
    {
        public Product ProductEdit { get; set; }
        public List<Category> Categories { get; set; }
    }
}