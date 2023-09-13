using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace onlinerentailapi.Models
{
    public class User
    {
        
        public int id { get; set; }
        public string name { get; set; }
        [Required]
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public long contact_no { get; set; }
        public string type { get; set; }
        public string profile_img { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

       
       // public IFormFile files { get; set; }    
    }
}
