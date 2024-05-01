using Microsoft.Data.SqlClient;
using Models;
using Services.Interface;
using System.Data;
using AppSettings;
using Microsoft.Extensions.Options;

namespace Services
{
    public class BooksStoredProcedure : IDatabaseManager
    {
        private readonly ConnectionStrings _connection;

        public BooksStoredProcedure(IOptions<ConnectionStrings> connection)
        {
            _connection = connection.Value;
        }

        public List<BooksModel> GetAllBooks()
        {
            List<BooksModel> _bookDatabase = new List<BooksModel>();

            using SqlConnection connection = new SqlConnection(_connection.SQLServerManagementStudio);
            try
            {
                connection.Open();
                string selectQuery = "GetBookDetails";
                SqlCommand command = new SqlCommand(selectQuery, connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    BooksModel emp = MapReaderBook(reader);
                    _bookDatabase.Add(emp);
                }
                return _bookDatabase;
            }
            catch (Exception e)
            {
                return [];
            }
        }

        public BooksModel MapReaderBook(SqlDataReader reader)
        {
            var book = new BooksModel();
            book.BookID = reader.GetGuid(0);
            book.Title = reader.GetString(1);
            book.Description = reader.GetString(2);
            book.ISBN = reader.GetGuid(3);
            book.Publication_Date = reader.GetDateTime(4);
            book.Price = (float)reader.GetDouble(5);
            book.Language = reader.GetString(6);
            book.Publisher = reader.GetString(7);
            book.PageCount = reader.GetInt32(8);
            book.AvgRating = (float)reader.GetDouble(9);
            book.BookGenre = reader.GetInt32(10);
            book.IsAvailable = reader.GetBoolean(11);

            return book;
        }

        public string AddBook (DTOBooks books)
        {
            using SqlConnection connection = new SqlConnection(_connection.SQLServerManagementStudio);

            try
            {
                connection.Open();
                BooksModel book = new BooksModel(books);

                string insertQuery = "InsertBook";
                SqlCommand command = new SqlCommand(insertQuery, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@BookID", SqlDbType.UniqueIdentifier).Value = book.BookID;
                command.Parameters.Add("@Title", SqlDbType.VarChar).Value = book.Title;
                command.Parameters.Add("@Descriptions", SqlDbType.VarChar).Value = book.Description;
                command.Parameters.Add("@ISBN", SqlDbType.UniqueIdentifier).Value = book.ISBN;
                command.Parameters.Add("@PublicationDate", SqlDbType.DateTime).Value = book.Publication_Date;
                command.Parameters.Add("@Price", SqlDbType.Float).Value = book.Price;
                command.Parameters.Add("@BookLanguage", SqlDbType.VarChar).Value = book.Language;
                command.Parameters.Add("@Publisher", SqlDbType.VarChar).Value = book.Publisher;
                command.Parameters.Add("@PagesCount", SqlDbType.Int).Value = book.PageCount;
                command.Parameters.Add("@AvgRating", SqlDbType.Float).Value = book.AvgRating;
                command.Parameters.Add("@BookGenre", SqlDbType.Int).Value = book.BookGenre;
                command.Parameters.Add("@IsAvailable", SqlDbType.Bit).Value = book.IsAvailable;
                command.Parameters.Add("@CreatedAt", SqlDbType.DateTime).Value = book.getCreatedon();
                command.Parameters.Add("@UpdatedAt", SqlDbType.DateTime).Value = book.UpdatedAt;

                int rowsAffected = command.ExecuteNonQuery();
                return $"Book added successfully, number of rowsaffected is {rowsAffected}";
            }
            catch (Exception e)
            {
                return "Cannot add book";
            }
        }

        public bool UpdateBook(BooksModel book)
        {
            using SqlConnection connection = new SqlConnection(_connection.SQLServerManagementStudio);
            try
            {
                connection.Open();

                string updateQuery = "UpdateBook";
                SqlCommand command = new SqlCommand(updateQuery, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@BookID", SqlDbType.UniqueIdentifier).Value = book.BookID;
                command.Parameters.Add("@Title", SqlDbType.VarChar).Value = book.Title;
                command.Parameters.Add("@Descriptions", SqlDbType.VarChar).Value = book.Description;
                command.Parameters.Add("@ISBN", SqlDbType.UniqueIdentifier).Value = book.ISBN;
                command.Parameters.Add("@PublicationDate", SqlDbType.DateTime).Value = book.Publication_Date;
                command.Parameters.Add("@Price", SqlDbType.Float).Value = book.Price;
                command.Parameters.Add("@BookLanguage", SqlDbType.VarChar).Value = book.Language;
                command.Parameters.Add("@Publisher", SqlDbType.VarChar).Value = book.Publisher;
                command.Parameters.Add("@PagesCount", SqlDbType.Int).Value = book.PageCount;
                command.Parameters.Add("@AvgRating", SqlDbType.Float).Value = book.AvgRating;
                command.Parameters.Add("@BookGenre", SqlDbType.Int).Value = book.BookGenre;
                command.Parameters.Add("@IsAvailable", SqlDbType.Bit).Value = book.IsAvailable;
                command.Parameters.Add("@UpdatedAt", SqlDbType.DateTime).Value = DateTime.Now;

                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public bool DeleteBook(Guid id)
        {
            using SqlConnection connection = new SqlConnection(_connection.SQLServerManagementStudio);
            try
            {
                connection.Open();
                BooksModel book = new BooksModel();
                string deleteQuery = "DeleteBook";
                SqlCommand command = new SqlCommand(deleteQuery, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BookID", id);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                return false;
            } 
        }

        public List<BooksModel> GetBooksByTitle(string title)
        {
            List<BooksModel> _booksByTitle = new List<BooksModel>();

            using (SqlConnection connection = new SqlConnection(_connection.SQLServerManagementStudio))
            {
                try
                {
                    connection.Open();
                    string getBookByTitleQuery = "GetBooksByTitle";
                    SqlCommand command = new SqlCommand(getBookByTitleQuery, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Title", title);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        BooksModel book = MapReaderBook(reader);
                        _booksByTitle.Add(book);
                    }
                }
                catch (Exception e)
                {

                }
            }

            return _booksByTitle;
        }

        public List<BooksModel> GetBooksByGenre(int genre_id)
        {
            List<BooksModel> _booksByGenre = new List<BooksModel>();

            using SqlConnection connection = new SqlConnection(_connection.SQLServerManagementStudio);
            {
                try
                {
                    connection.Open();
                    string getBookByGenreQuery = "GetBooksByGenre";
                    SqlCommand command = new SqlCommand(getBookByGenreQuery, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BookGenre", genre_id);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        BooksModel book = MapReaderBook(reader);
                        _booksByGenre.Add(book);
                    }
                }
                catch (Exception e)
                {

                }
            }
            return _booksByGenre;
        }
    }
}
