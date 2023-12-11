using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
/// <summary>
/// Summary description for Product
/// </summary>
public class Product
{
    //Private string _connStr = Properties.Settings.Default.DBConnStr;
    //System.Configuration.ConnectionStringSettings _connStr;
    string _connStr = ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString;
    private string _product_id = "";
    private string _product_image = string.Empty;
    private string _product_name = ""; // this is another way to specify empty string
    private string _product_brand = "";
    private string _product_type = "";
    private string _product_desc = "";
    private string _product_size = "";
    private string _product_status = "";
    private decimal _product_cost = 0;

    // Default constructor
    public Product()
    {
    }

    // Constructor that take in all data required to build a Product object
    public Product(string product_id, string product_image, string product_name, string product_brand, string product_type, string product_desc, string product_size, string product_status, decimal product_cost)
    {
        _product_id = product_id;
        _product_image = product_image;
        _product_name = product_name;
        _product_brand = product_brand;
        _product_type = product_type;
        _product_desc = product_desc;
        _product_size = product_size;
        _product_status = product_status;
        _product_cost = product_cost;
    }

    // Constructor that take in all except product ID
    public Product(string product_image, string product_name, string product_brand, string product_type, string product_desc, string product_size, string product_status, decimal product_cost)
        : this(null,product_image, product_name, product_brand, product_type, product_desc, product_size, product_status, product_cost)
    {
    }

    // Constructor that take in only Product ID. The other attributes will be set to 0 or empty.
    public Product(string product_id)
        : this(product_id, "", "", "", "", "", "", 0)
    {
    }

    // Get/Set the attributes of the Product object.
    // Note the attribute name (e.g. Product_ID) is same as the actual database field name.
    // This is for ease of referencing.

    public string Product_ID
    {
        get { return _product_id; }
        set { _product_id = value; }
    }
    public string Product_Image
    {
        get { return _product_image; }
        set { _product_image = value; }
    }
    public string Product_Name
    {
        get { return _product_name; }
        set { _product_name = value; }
    }
    public string Product_Brand
    {
        get { return _product_brand; }
        set { _product_brand = value; }
    }
    public string Product_Type
    {
        get { return _product_type; }
        set { _product_type = value; }
    }
    public string Product_Desc
    {
        get { return _product_desc; }
        set { _product_desc = value; }
    }
    public string Product_Size
    {
        get { return _product_size; }
        set { _product_size = value; }
    }
    public string Product_Status
    {
        get { return _product_status; }
        set { _product_status = value; }
    }
    public decimal Product_Cost
    {
        get { return _product_cost; }
        set { _product_cost = value; }
    }

    //Below as the Class methods for some DB operations.
    public Product getProduct(string product_id)
    {
        Product prodDetail = null;
        string product_image, product_name, product_brand, product_type, product_desc, product_size, product_status;
        decimal product_cost;
        string queryStr = "SELECT * FROM Products WHERE product_id = @product_id";
        SqlConnection conn = new SqlConnection(_connStr);
        SqlCommand cmd = new SqlCommand(queryStr, conn);
        cmd.Parameters.AddWithValue("@ProdID", product_id);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            product_image = dr["product_image"].ToString();
            product_name = dr["product_name"].ToString();
            product_brand = dr["product_brand"].ToString();
            product_type = dr["product_type"].ToString();
            product_desc = dr["product_desc"].ToString();
            product_size = dr["product_size"].ToString();
            product_status = dr["product_status"].ToString();
            product_cost = decimal.Parse(dr["product_cost"].ToString());
            prodDetail = new Product(product_id, product_image, product_name, product_brand, product_type, product_desc, product_size, product_status, product_cost);
        }
        else
        {
            prodDetail = null;
        }
        conn.Close();
        dr.Close();
        dr.Dispose();
        return prodDetail;
    }

    public List<Product> getProductAll()
    {
        List<Product> prodList = new List<Product>();
        string product_id, product_image, product_name, product_brand, product_type, product_desc, product_size, product_status;
        decimal product_cost;
        string queryStr = "SELECT * FROM Products ORDER BY product_name";
        SqlConnection conn = new SqlConnection(_connStr);
        SqlCommand cmd = new SqlCommand(queryStr, conn);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            product_id = dr["product_id"].ToString();
            product_image = dr["product_image"].ToString();
            product_name = dr["product_name"].ToString();
            product_brand = dr["product_brand"].ToString();
            product_type = dr["product_type"].ToString();
            product_desc = dr["product_desc"].ToString();
            product_size = dr["product_size"].ToString();
            product_status = dr["product_status"].ToString();
            product_cost = decimal.Parse(dr["product_cost"].ToString());
            Product a = new Product(product_id, product_image, product_name, product_brand, product_type, product_desc, product_size, product_status, product_cost);
            prodList.Add(a);
        }
        conn.Close();
        dr.Close();
        dr.Dispose();
        return prodList;
    }
}