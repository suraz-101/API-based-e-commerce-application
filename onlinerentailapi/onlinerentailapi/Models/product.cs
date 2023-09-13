using System.ComponentModel.DataAnnotations;

namespace onlinerentailapi.Models
{
    public class product
    {

       // public int id { get; set; }
        public int userId { get; set; }
        public int categoryId { get; set; }
        public string name { get; set; }
        
        public IFormFile product_image { get; set; }
        public string description { get; set; }
        public string summary { get; set; }
        public float marked_price { get; set; }
        public float selling_price { get; set; }
        public int quantity { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

    }

    public class viewProduct
    {
        public int id { get; set; }
        public int userId { get; set; }
        public int categoryId { get; set; }
        public string name { get; set; }

        public string product_image { get; set; }
        public string description { get; set; }
        public string summary { get; set; }
        public float marked_price { get; set; }
        public float selling_price { get; set; }
        public int quantity { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class addToCart
    {
     
        public int userId { get; set; }
         public int productId { get; set; } 
      
    }

    public class viewcart
    {
        public int id { get; set; }
        public int userId { get; set; }
        public int productId { get; set; }
         public int quantity { get; set; }
        public float price { get; set; }
       
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string product_image { get; set; }
        public string productName { get; set; }


    }

    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Address { get; set; }
        public decimal Subtotal { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }
    }

    public class OrderProduct
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }


}
