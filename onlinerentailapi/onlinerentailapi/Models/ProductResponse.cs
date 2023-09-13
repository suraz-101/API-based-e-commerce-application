using System.Drawing;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Data.Common;
using Microsoft.AspNetCore.Hosting.Server;
using System.IO;
using System.Linq;
using Google.Api.Ads.AdWords.v201809;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;

namespace onlinerentailapi.Models
{
    public class ProductResponse
    {

        SqlConnection con = new SqlConnection("Data Source=DESKTOP-IR1C47V\\SQLEXPRESS;Initial Catalog=Online_retail;Integrated Security=True");

        public string insertProduct(product prod,string image)
        {
            //string image = prod.product_image;
            //string fileName = Path.GetFileName(image);
            // string imagePath = Path.Combine(Server.MapPath("~/wwwroot/uploads/", image));  

            //Set the Image File Path.
           // string filePath = "~/wwwroot/uploads/" + fileName;
            // fileName.CopyTo("~/wwwroot/uploads/");

            // string path = Path.Combine(@"D:\BScIT\5th Sem\wma\asp.net\onlinerentailapi\onlinerentailapi\uploads\", fileName);


            SqlCommand cmd = new SqlCommand("insert into [product] (user_id,category_id,name,product_image,description,summary,marked_price,selling_price,quantity,created_at,updated_at) values(@userId, @categoryId, @productName, @productImage, @description, @summary,@markedPrice, @sellingPrice,@quantity,@created_at, @updated_at)", con);


            //cmd.Parameters.AddWithValue("@id", usr.id);

            cmd.Parameters.AddWithValue("@userId", prod.userId);
            cmd.Parameters.AddWithValue("@categoryId", prod.categoryId);
            cmd.Parameters.AddWithValue("@productName", prod.name);
            cmd.Parameters.AddWithValue("@productImage", image);
            cmd.Parameters.AddWithValue("@description", prod.description);
            cmd.Parameters.AddWithValue("@summary", prod.summary);
            cmd.Parameters.AddWithValue("@markedPrice", prod.marked_price);
            cmd.Parameters.AddWithValue("@sellingPrice", prod.selling_price);
            cmd.Parameters.AddWithValue("@quantity", prod.quantity);
            cmd.Parameters.AddWithValue("@created_at ", prod.created_at);
            cmd.Parameters.AddWithValue("@updated_at", prod.updated_at);
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

        public List<viewcart> getcartProducts(int userId)
        {

            string imagePath;
            List<viewcart> ListCart = new List<viewcart>();
            SqlCommand cmd = new SqlCommand("SELECT * FROM [cart] WHERE user_Id ='" + userId + "'", con);
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                viewcart vc = new viewcart();

                //product pr = new product();

                vc.id = Convert.ToInt32(dr.GetValue(0).ToString());
                vc.userId = Convert.ToInt32(dr.GetValue(1).ToString());
                vc.productId = Convert.ToInt32(dr.GetValue(2).ToString());
                vc.productName = dr.GetValue(8).ToString();
                imagePath = dr.GetValue(7).ToString();
                vc.product_image = imagePath;
                vc.quantity = Convert.ToInt32(dr.GetValue(3).ToString());
                /*vc.price = Convert.ToInt32(dr.GetValue(4).ToString());*/


                ListCart.Add(vc);

            }
            con.Close();
            return ListCart;
        }


        public List<viewProduct> getProduct(int id)
        {
            string imagePath;
            List<viewProduct> ListProduct = new List<viewProduct>();
            SqlCommand cmd = new SqlCommand("select * from [product] where id='" + id + "'", con);
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                viewProduct vp = new viewProduct();

                //product pr = new product();

               // pr.id = Convert.ToInt32(dr.GetValue(0).ToString());
                vp.userId = Convert.ToInt32(dr.GetValue(1).ToString());
                vp.categoryId = Convert.ToInt32(dr.GetValue(2).ToString());
                vp.name = dr.GetValue(3).ToString();
                imagePath = dr.GetValue(4).ToString();
                vp.product_image = imagePath;
                vp.description = dr.GetValue(5).ToString();
                vp.summary = dr.GetValue(6).ToString();
                vp.marked_price = Convert.ToInt32(dr.GetValue(7).ToString());
                vp.selling_price = Convert.ToInt32(dr.GetValue(8).ToString()); ;

                // vp.selling_price = dr.GetFloat(8);

                vp.quantity = Convert.ToInt32(dr.GetValue(9).ToString());
                
                ListProduct.Add(vp);
                
            }
            con.Close();
            return ListProduct;


        }
        public List<viewProduct> getProducts()
        {

            string imagePath;
            List<viewProduct> ListProduct = new List<viewProduct>();
            SqlCommand cmd = new SqlCommand("select * from [product] ", con);
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                viewProduct vp = new viewProduct();

                //product pr = new product();

                vp.id = Convert.ToInt32(dr.GetValue(0).ToString());
                vp.userId = Convert.ToInt32(dr.GetValue(1).ToString());
                vp.categoryId = Convert.ToInt32(dr.GetValue(2).ToString());
                vp.name = dr.GetValue(3).ToString();
                imagePath = dr.GetValue(4).ToString();
                vp.product_image = imagePath;
                vp.description = dr.GetValue(5).ToString();
                vp.summary = dr.GetValue(6).ToString();
                vp.marked_price = Convert.ToInt32(dr.GetValue(7).ToString());
                vp.selling_price = Convert.ToInt32(dr.GetValue(8).ToString()); ;
                vp.quantity = Convert.ToInt32(dr.GetValue(9).ToString());

                ListProduct.Add(vp);

            }
            con.Close();
            return ListProduct;
        }

        public string deleteProduct(int id)
        {
            SqlCommand cmd = new SqlCommand("delete from [product] where id='" + id + "'", con);
            cmd.Parameters.AddWithValue("@id", id);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i > 0)
            {
                return "Data deleted succesfully from the database!!";
            }

            else
            {
                return "Unable to delete data into database!! something has gone WRONG!!";
            }
        }

        public string deletecart(int userid)
        {
            SqlCommand cmd = new SqlCommand("delete from [cart] where user_id=@id", con);
            cmd.Parameters.AddWithValue("@id", userid);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i > 0)
            {
                return "order successfully placed";
            }

            else
            {
                return " something has gone WRONG!!";
            }
        }


    }
}
