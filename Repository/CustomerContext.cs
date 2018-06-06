using MySql.Data.MySqlClient;
using SimpleWebService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebService.Repository
{
    public class CustomerContext
    {
        public string ConnectionString { get; set; }

        public CustomerContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<Customer> GetAllCustomers()
        {
            List<Customer> list = new List<Customer>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM customers", conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Customer()
                        {
                            id = reader.GetInt32("id"),
                            name = reader.GetString("name"),
                            surname = reader.GetString("surname")
                        });
                    }
                }
            }
            return list;
        }
        public Customer GetCustomer(int id)
        {
            Customer customer = null;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM customers WHERE id = ?id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add("?id", MySqlDbType.Int32).Value = id;
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        customer = new Customer()
                        {
                            id = reader.GetInt32("id"),
                            name = reader.GetString("name"),
                            surname = reader.GetString("surname")
                        };
                    }
                    else
                        return null;
                }
                conn.Close();
            }
            return customer;
        }
        public int CreateCustomer(Customer customer)
        {
            int result;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                try
                {
                    string query = "INSERT INTO customers(name,surname) VALUES( ?name, ?surname)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("?name", MySqlDbType.VarChar).Value = customer.name ;
                        cmd.Parameters.Add("?surname", MySqlDbType.VarChar).Value = customer.surname;
                        cmd.ExecuteNonQuery();
                        result = (int)cmd.LastInsertedId; 
                    }
                }
                catch (MySqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                conn.Close();
            }
            return result;
        }
        public bool DeleteCustomer(int id)
        {
            int result;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM customers WHERE id = ?id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add("?id", MySqlDbType.Int32).Value = id;
                result = cmd.ExecuteNonQuery();
                conn.Close();
            }
            return Convert.ToBoolean(result);
        }
        public bool UpdateCustomer(int id, Customer customer)
        {
            int result;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string query = "UPDATE customers SET name = ?name, surname = ?surname WHERE id = ?id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add("?name", MySqlDbType.VarChar).Value = customer.name;
                cmd.Parameters.Add("?surname", MySqlDbType.VarChar).Value = customer.surname;
                cmd.Parameters.Add("?id", MySqlDbType.Int32).Value = id;
                result = cmd.ExecuteNonQuery();
                conn.Close();

            }
            return Convert.ToBoolean(result);
        }
    }
}
