using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;

using System.Data;
using System.Configuration;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Data.Common;
using Microsoft.AspNetCore.Hosting.Server;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.Abstractions;
using Microsoft.AspNetCore.Session;
using Google.Api.Ads.AdWords.v201809;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace onlinerentailapi.Models
{

    public class UserRespose
    {

        SqlConnection con = new SqlConnection("Data Source=DESKTOP-IR1C47V\\SQLEXPRESS;Initial Catalog=Online_retail;Integrated Security=True");

        //methods to get all user details from database
        public List<User> getUsers()
        {
            List<User> listuser = new List<User>();
            SqlCommand cmd = new SqlCommand("select * from [user]", con);
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                User user = new User();

                user.id = Convert.ToInt32(dr.GetValue(0).ToString());
                user.name = dr.GetValue(1).ToString();
                user.username = dr.GetValue(2).ToString();
                user.password = dr.GetValue(3).ToString();
                user.email = dr.GetValue(4).ToString();
                //user.contact_no = Convert.ToInt32(dr.GetValue(5).ToString());
                user.type = dr.GetValue(6).ToString();
                user.profile_img = dr.GetValue(7).ToString();
                //user.created_at = dr.GetDateTime(8);
                // user.updated_at = dr.GetDateTime(9);
                listuser.Add(user);
            }
            con.Close();
            return listuser;
        }


        //method to get the detail information of an individual user from database
        public List<User> getUser(int id )
        {

            List<User> listuser = new List<User>();
            SqlCommand cmd = new SqlCommand("select * from [user] where id='" + id + "'", con);
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                User user = new User();

                user.id = Convert.ToInt32(dr.GetValue(0).ToString());
                user.name = dr.GetValue(1).ToString();
                user.username = dr.GetValue(2).ToString();
                user.password = dr.GetValue(3).ToString();
                user.email = dr.GetValue(4).ToString();
                //user.contact_no = Convert.ToInt32(dr.GetValue(5).ToString());
                user.type = dr.GetValue(6).ToString();
                user.profile_img = dr.GetValue(7).ToString();
                //user.created_at = dr.GetDateTime(8);
                // user.updated_at = dr.GetDateTime(9);
                listuser.Add(user);
            }
            con.Close();
            return listuser;


        }


        //method to insert
        public string registerUser(User usr)
        {
            string image = usr.profile_img;
            string fileName = Path.GetFileName(image);
            string filePath = "~/wwwroot/uploads/" + fileName;
            SqlCommand cmd = new SqlCommand("insert into [user] (name,username,password,email,contact_no,type,profile_img,created_at,updated_at) values(@name, @username, @password, @email, @contact_no, @type,@profile_img, @created_at, @updated_at)", con);
 
            cmd.Parameters.AddWithValue("@name", usr.name);
            cmd.Parameters.AddWithValue("@username", usr.username);
            cmd.Parameters.AddWithValue("@password", usr.password);
            cmd.Parameters.AddWithValue("@email", usr.email);
            cmd.Parameters.AddWithValue("@contact_no", usr.contact_no);
            cmd.Parameters.AddWithValue("@type", usr.type);
            cmd.Parameters.AddWithValue("@profile_img",filePath); 
            cmd.Parameters.AddWithValue("@created_at ", usr.created_at);
            cmd.Parameters.AddWithValue("@updated_at", usr.updated_at);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i > 0)
            {
                return "Data inserted succesfully into the database!!";
            }

            else
            {
                return "Unable to insert data into database!! something has gone WRONG!!";
            }
        }

       



        //code for authentication check username and password
        public string Login(string username, string password)
        {
            
            SqlCommand cmd = new SqlCommand("select * from [user] where username='" + username + "' and password = '" + password + "'", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if(dr.HasRows)
            {
                while(dr.Read())
                {
                    
                    string  customerid = dr.GetValue(0).ToString();
                    return "customer" ;
                }

            }
            else
            {
                if (username == "admin" && password == "admin")
                {
                    return "admin";
                }
                else
                {
                    return "invalid";

                }

            }
            return "invalid";
            
           

          //  return "invalid";

        }


        [HttpPost]
        public IActionResult login(string username, string password)
        {
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
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes("JWT:Secret");
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                            new Claim(ClaimTypes.Name,user.username)
                        }),
                        Expires = DateTime.UtcNow.AddDays(7),
                        Issuer = "JWT:ValidIssuer",
                        Audience = "JWT:ValidAudience",
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);

                    //Store the user's Id and role in local storage for later use
                    // localStorage.setItem("UserId", user.id.ToString());

                    //Return the JWT token, userId, and user role to the client
                    return Ok(new
                    {
                        token = tokenString,
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
                    return null;

                }

            }
            return null ;


        }

        private IActionResult Ok(object value)
        {
            throw new NotImplementedException();
        }

        public string updateusers(User usr, int id)
        {
            SqlCommand cmd = new SqlCommand("update [user] set name=@name,username=@username,password=@password, email=@email, contact_no=@contact_no, type=@type, profile_img=@profile_img, created_at=@created_at, updated_at=@updated_at where id='"+id+"'", con);
            cmd.Parameters.AddWithValue("@id",id);
            cmd.Parameters.AddWithValue("@name", usr.name);
            cmd.Parameters.AddWithValue("@username", usr.username);
            cmd.Parameters.AddWithValue("@password", usr.password);
            cmd.Parameters.AddWithValue("@email", usr.email);
            cmd.Parameters.AddWithValue("@contact_no", usr.contact_no);
            cmd.Parameters.AddWithValue("@type", usr.type);
            cmd.Parameters.AddWithValue("@profile_img", usr.profile_img); cmd.Parameters.AddWithValue("@created_at ", usr.created_at);
            cmd.Parameters.AddWithValue("@updated_at", usr.updated_at);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i > 0)
            {
                return "Data updated succesfully into the database!!";
            }

            else
            {
                return "Unable to update data into database!! something has gone WRONG!!";
            }
            return "";
        }


        public string deleteUser( int id)
        {
            SqlCommand cmd = new SqlCommand("delete from [user] where id='" + id + "'", con);
            cmd.Parameters.AddWithValue("@id", id);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i > 0)
            {
                return "Data deleted succesfully into the database!!";
            }

            else
            {
                return "Unable to delete data into database!! something has gone WRONG!!";
            }
            
        }
    }
}
