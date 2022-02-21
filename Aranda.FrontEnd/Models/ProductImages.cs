using System;
using System.Collections.Generic;

namespace Aranda.FrontEnd.Models
{
    public class ProductImages
    {
        public Guid ProductImagesId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid ProductId { get; set; }
    }
    public class ProductImagesCommand
    {
        public List<string> ImagesUrl { get; set; }
        public string ProductId { get; set; }
    }
}