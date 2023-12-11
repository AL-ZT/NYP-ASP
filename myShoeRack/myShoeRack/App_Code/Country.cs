using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace myShoeRack.App_Code
{
    public class Country
    {
        string _connStr = ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString;
        private string _countryName = string.Empty;
        private string _countryCode = string.Empty;

        public Country()
        {
        }

        public Country(string countryName, string countryCode)
        {
            _countryName = countryName;
            _countryCode = countryCode;
        }

        public string country
        {
            get { return _countryName; }
            set { _countryName = value; }
        }

        public string code
        {
            get { return _countryCode; }
            set { _countryCode = value; }
        }

        public List<Country> getCountryAll()
        {
            List<Country> countryList = new List<Country>();
            string country_name, country_code;
            string queryStr = "SELECT code, country FROM RegistrationForm_Country Order By country";

            SqlConnection conn = new SqlConnection(_connStr);
            SqlCommand cmd = new SqlCommand(queryStr, conn);

            conn.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                country_name = dr["country"].ToString();
                country_code = dr["code"].ToString();
                Country c = new Country(country_name, country_code);
                countryList.Add(c);
            }

            conn.Close();
            dr.Close();
            dr.Dispose();

            return countryList;
        }
    }
}