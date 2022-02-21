using System;

namespace Aranda.FrontEnd.Models
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreationDate { get; set; }
    }
    public class CategoryRequest
    {
        public string CategoryName { get; set; }
    }
}