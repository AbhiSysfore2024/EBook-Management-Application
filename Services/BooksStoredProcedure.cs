using Microsoft.Data.SqlClient;
using Models;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BooksStoredProcedure : IDatabaseManager
    {
        private readonly string _connectionString = "Data Source=192.168.10.28\\SQLEX2017;Initial Catalog=Abhilash;User Id=sysfore.ea;Password=Sys@2024#;Encrypt=false";

        public List<BooksModel> GetAllBooks()
        {
            List<BooksModel> _bookDatabase = new List<BooksModel>();

            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            string selectQuery = "GetBookDetails";
            SqlCommand command = new SqlCommand(selectQuery, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                BooksModel emp = MapReaderBook(reader);
                _bookDatabase.Add(emp);
            }
            return _bookDatabase;
        }

        public BooksModel MapReaderBook(SqlDataReader reader)
        {
            var book = new BooksModel();
            book.BookID = reader.GetGuid(0);
            book.Title = reader.GetString(1);
            book.Description = reader.GetString(2);
            book.ISBN = reader.GetGuid(3);
            book.Publication_Date = reader.GetDateTime(4);
            book.Price = reader.GetFloat(5);
            book.Language = reader.GetString(6);
            book.Publisher = reader.GetString(7);
            book.PageCount = reader.GetInt32(8);
            book.AvgRating  =reader.GetFloat(9);
            book.BookGenre = (Genre)reader.GetInt32(10);
            book.IsAvailable = reader.GetBoolean(11);

            return book;
        }

        public string AddBook (DTOBooks books)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                BooksModel book = new BooksModel(books);

                string insertQuery = "InsertBook";
                SqlCommand command = new SqlCommand(insertQuery, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@BookID", SqlDbType.UniqueIdentifier).Value = book.BookID;
                command.Parameters.Add("@Title", SqlDbType.VarChar).Value = book.Title;
                command.Parameters.Add("@Description", SqlDbType.VarChar).Value = book.Description;
                command.Parameters.Add("@ISBN", SqlDbType.UniqueIdentifier).Value = book.ISBN;
                command.Parameters.Add("@Publication_Date", SqlDbType.DateTime).Value = book.Publication_Date;
                command.Parameters.Add("@Price", SqlDbType.Float).Value = book.Price;
                command.Parameters.Add("@Language", SqlDbType.VarChar).Value = book.Language;
                command.Parameters.Add("@Publisher", SqlDbType.VarChar).Value = book.Publisher;
                command.Parameters.Add("@PageCount", SqlDbType.Int).Value = book.PageCount;
                command.Parameters.Add("@AvgRating", SqlDbType.Float).Value = book.AvgRating;
                command.Parameters.Add("@BookGenre", SqlDbType.Int).Value = (int)book.BookGenre;
                command.Parameters.Add("@IsAvailable", SqlDbType.Bit).Value = book.IsAvailable;
                command.Parameters.Add("@CreatedAt", SqlDbType.DateTime).Value = book.getCreatedon();
                command.Parameters.Add("@UpdatedAt", SqlDbType.DateTime).Value = book.UpdatedAt;

                int rowsAffected = command.ExecuteNonQuery();
                return $"Employee added successfully, number of rowsaffected is {rowsAffected}";
            }
            catch (Exception e)
            {
                return "Cannot add employee";
            }
        }
    }
}
