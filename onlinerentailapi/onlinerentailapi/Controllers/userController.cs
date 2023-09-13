using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using onlinerentailapi.Models;
using System.Data.SqlClient;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;

namespace onlinerentailapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class userController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public userController(IConfiguration _configurationnfig)
        {
            this._configuration = _configuration;
        }




        [HttpGet]
        // [Route("Getusers")]
        public List<User> Getusers()
        {
            UserRespose ur = new UserRespose();

            List<User> obj = ur.getUsers();


            return obj;
        }

        [HttpGet("{id}")]
        
        // [Route("Getuser/{id}")]
        public List<User> Getuser(int id)
        {
           
            UserRespose ur = new UserRespose();
            List<User> obj = ur.getUser(id);
            return obj;
        }

        [HttpPost]
        //[Route("putUser")]

        public string putUser(User usr)
        {
            
            UserRespose ur = new UserRespose();
            string message = ur.registerUser(usr);
            return message;
        }

        //update user api

        [HttpPut("{id}")]
       // [Route("update/{id}")]
        public string update( User usr,int id)
        {
           

            UserRespose ur = new UserRespose();
            
            string message = ur.updateusers(usr, id);   

            return message;

        }


        //Delete user api
        [HttpDelete("{id}")]
       // [Route("delete/{id}")]

        public string delete(int id)
        {

            UserRespose ur = new UserRespose();
            //ur.getUser(id);
            string message = ur.deleteUser( id);

            return message;

        }

        //login through api
      /// // [HttpPost]
       // [Route("Login")]
        //public String Login(login login)
       // {
            //string username = login.username;
           // string password = login.password;
          //  UserRespose userRespose = new UserRespose();    
           //  string obj =  userRespose.Login(username,password);
           // return obj;
            
     //   }

       /* private JwtSecurityToken getToken(List<Claim> authClaim)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration//["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(24),
                claims: authClaim,
                signingCredentials: new SigningCredentials(authSigningKey,SecurityAlgorithms.HmacSha256)
                );
            return token;
        }*/

        [HttpPost]
        [Route("Login")]
        public IActionResult login(login login)
        {
            SqlConnection con = new SqlConnection("Data Source=DESKTOP-IR1C47V\\SQLEXPRESS;Initial Catalog=Online_retail;Integrated Security=True");
            string username = login.username;
            string password = login.password;

            con.Open();
            SqlCommand cmd = new SqlCommand("select * from [user] where username='" + username + "' and password = '" + password + "'", con);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    var user = new User
                    {
                        id = Convert.ToInt32(dr.GetValue(0).ToString()),
                        username = dr.GetValue(2).ToString()
                    };
                    return Ok(new
                    {
                        UserId = user.id,
                        role = "customer"
                    });
                }
            }

            else
            {
                if (username == "admin" && password == "admin")
                {
                    return Ok(new
                    {
                        role = "admin"
                    });
                }
                else
                {
                    return Ok(new
                    {
                        role = "invalid"
                    });
                }
            }
            return Ok(new
            {  role = "invalid"
            });
        }
    }
}
