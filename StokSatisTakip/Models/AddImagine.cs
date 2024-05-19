using System.ComponentModel.DataAnnotations;

namespace StokSatisTakip.Models
{
    public class AddImagine
    {
        public int UrunId { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public decimal Price { get; set; }
        
        public int Stock { get; set; }
        
        public bool Popular { get; set; }
        
        public bool IsApproved { get; set; }
        
        public IFormFile Image { get; set; }
        
        public int CategoryId { get; set; }
    }
}
