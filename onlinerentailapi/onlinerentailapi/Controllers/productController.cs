using Google.Api.Ads.AdWords.v201809;
using Microsoft.AspNetCore.Mvc;
using onlinerentailapi.Models;
using System.Data.SqlClient;
using System.Drawing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace onlinerentailapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class productController : Controller
    {

        SqlConnection con = new SqlConnection("Data Source=DESKTOP-IR1C47V\\SQLEXPRESS;Initial Catalog=Online_retail;Integrated Security=True");


        public static IWebHostEnvironment _webHostEnvironment;

        public productController(IWebHostEnvironment webHostEnvironment )
        {
            _webHostEnvironment = webHostEnvironment;   
        }

        [HttpPost]
        //[Route("putUser")]
        public string postProduct([FromForm] product prod)
        {
            try
            {
                if (prod.product_image.Length>0)
                {
                    string path = _webHostEnvironment.WebRootPath + "\\uploads\\";
                    if(!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (FileStream fileStream = System.IO.File.Create(path + prod.product_image.FileName))
                    {
                        string image = path + prod.product_image.FileName;
                        prod.product_image.CopyTo(fileStream);
                        fileStream.Flush();
                        ProductResponse pr = new ProductResponse();
                        string msg = pr.insertProduct(prod,image);
                        return msg +image+ " and file uploaded";
                    }

                }
                else
                {
                    return "file not uploaded";
                }

            }
            catch(Exception ex)
            {
                return ex.Message;
            }
           // ProductResponse pr = new ProductResponse();
            //string message = pr.insertProduct(prod);
           // return message;
        }
        [HttpPost]
        [Route("addtocart")]
        public string addtocart([FromForm] addToCart at)
        {
            DateTime date  = DateTime.Now;
            int productId  = at.productId;
            int userId = at.userId;
            string sql = "SELECT name, selling_price, product_image FROM [product] WHERE id =@ProductId";
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@ProductId", productId);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        //Extract the product information from the data reader
                        string productName = reader.GetValue(0).ToString();
                        float productPrice = Convert.ToInt32(reader.GetValue(1).ToString());
                        string productImage = reader.GetValue(2).ToString();
                        reader.Close();
                        //insert the product into the cart table 
                        string s = "INSERT INTO [cart] (user_id, product_id, quantity, price,created_at,updated_at, productImage,productName )  VALUES (@userId, @productId, @quantity, @price,@created_at,@updated_at, @productImage,@productName)";
                        SqlCommand insertCommand = new SqlCommand(s, con);   
                            insertCommand.Parameters.AddWithValue("@userId", userId);
                            insertCommand.Parameters.AddWithValue("@productId", productId.ToString());
                            insertCommand.Parameters.AddWithValue("@quantity", 1);
                            insertCommand.Parameters.AddWithValue("@price", productPrice);
                            insertCommand.Parameters.AddWithValue("@created_at", date.ToString());
                            insertCommand.Parameters.AddWithValue("@updated_at", date.ToString());
                            insertCommand.Parameters.AddWithValue("@productImage", productImage);
                          insertCommand.Parameters.AddWithValue("@productName", productName);
                        int i = insertCommand.ExecuteNonQuery();
                        con.Close();
                        if (i > 0)
                        {
                            return "Product added to the cart!!";
                        }
                        else
                        {
                            return "Unable to add product into the cart!! something has gone WRONG!!";
                        }
                    }
                    else
                    {
                        return "data not found";
                    }
                }
            }

        }



      /*  [HttpPost]
        [Route("addtocart")]
public string addToOrder(Order order)
        {
                // Open the connection
                con.Open();

                // Start a new transaction
                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                    string sql = "SELECT * FROM [cart] WHERE user_id =@userid";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@userid", order.UserId);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
*//*                                string orderid = reader.GetValue(0).ToString();
*//*                                int productid = Convert.ToInt32(reader.GetValue(3).ToString());
                                string productImage = reader.GetValue(7).ToString();
                                reader.Close();
                                using (SqlCommand command = new SqlCommand())
                                {
                                    command.Connection = con;
                                    command.Transaction = transaction;

                                    command.CommandText = "INSERT INTO orders (userid, address, subtotal, grand_total, created_at, updated_at) " +
                                                          "VALUES (@UserId, @Address, @Subtotal, @GrandTotal, @CreatedAt, @UpdatedAt); " +
                                                          "SELECT SCOPE_IDENTITY()";
                                    command.Parameters.AddWithValue("@UserId", order.UserId);
                                    command.Parameters.AddWithValue("@Address", order.Address);
                                    command.Parameters.AddWithValue("@Subtotal", order.Subtotal);
                                    command.Parameters.AddWithValue("@GrandTotal", order.GrandTotal);
                                    command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                                    command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

                                    // Execute the command and retrieve the newly inserted order ID
                                    int orderId = Convert.ToInt32(command.ExecuteScalar());

                                    // Loop through the cart items and insert the order product information into the order_products table
                                    foreach (var item in cartItems)
                                    {
                                        using (SqlCommand cmd = new SqlCommand())
                                        {
                                            cmd.Connection = connection;
                                            cmd.Transaction = transaction;

                                            cmd.CommandText = "INSERT INTO order_products (order_id, product_id, quantity, price) " +
                                                              "VALUES (@OrderId, @ProductId, @Quantity, @Price)";
                                            cmd.Parameters.AddWithValue("@OrderId", orderId);
                                            cmd.Parameters.AddWithValue("@ProductId", item.ProductId);
                                            cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                                            cmd.Parameters.AddWithValue("@Price", item.Price);

                                            cmd.ExecuteNonQuery();
                                        }
                                    }

                                    // Commit the transaction
                                    transaction.Commit();
                                }

                            }

                            }

                        }
                        // Insert the order information into the orders table
                       
                    }
                    catch (Exception ex)
                    {
                        // If an error occurs, rollback the transaction
                        transaction.Rollback();
                        throw ex;
                    }
                }
            


            return "";
        }
*/
        [HttpGet]
        [Route("getcartProduct")]
        // [Route("Getusers")]
        public List<viewcart> getCartProducts(int userId)
        {
            string path = _webHostEnvironment.WebRootPath + "\\uploads\\";
            ProductResponse pr = new ProductResponse();

            List<viewcart> obj = pr.getcartProducts(userId);

            return obj;
        }



        [HttpGet]
        // [Route("Getusers")]
        public List<viewProduct> GetProducts()
        {
            string path = _webHostEnvironment.WebRootPath + "\\uploads\\";
            ProductResponse pr = new ProductResponse();

            List<viewProduct> obj = pr.getProducts();

            return obj;
        }

        [HttpGet("{id}")]
        // [Route("Getusers")]
        public List<viewProduct> GetProducts(int id)
        {
            string path = _webHostEnvironment.WebRootPath + "\\uploads\\";
            ProductResponse pr = new ProductResponse();

            List<viewProduct> obj = pr.getProduct(id);

            return obj;
        }

      /*  [HttpGet("{id}")]*/
        // [Route("Getusers")]
       /* public List<viewProduct> getCartProducts(int id)
        {
            string path = _webHostEnvironment.WebRootPath + "\\uploads\\";
            ProductResponse pr = new ProductResponse();

            List<viewProduct> obj = pr.getCartProduct(id);

            return obj;
        }*/


        [HttpDelete("{id}")]
        // [Route("delete/{id}")]

        public string deleteProduct(int id)
        {

            ProductResponse pr = new ProductResponse();
            //ur.getUser(id);
            string message = pr.deleteProduct(id);

            return message;

        }

        [HttpDelete]
         [Route("deleteCartProduct")]

        public string deleteCartProduct(int userId)
        {

            ProductResponse pr = new ProductResponse();
            //ur.getUser(id);
            string message = pr.deletecart(userId);

            return message;

        }


    }

}
