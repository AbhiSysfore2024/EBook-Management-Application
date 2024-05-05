using AppSettings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Models;
using Services.Interface;
using System.Data;

namespace Services
{
    public class AuthorStoredProcedure : IAuthorDatabaseManager
    {
        private readonly ConnectionStrings _connection;

        public AuthorStoredProcedure(IOptions<ConnectionStrings> connection)
        {
            _connection = connection.Value;
        }

        public List<AuthorModel> GetAllAuthors()
        {
            List<AuthorModel> _authorDatabase = new List<AuthorModel>();

            using SqlConnection connection = new SqlConnection(_connection.SQLServerManagementStudio);
            try
            {
                connection.Open();
                string selectQuery = "GetAuthorDetails";
                SqlCommand command = new SqlCommand(selectQuery, connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    AuthorModel auth = MapReaderAuthor(reader);
                    _authorDatabase.Add(auth);
                }
                return _authorDatabase;
            }
            catch (Exception e)
            {
                return [];
            }
        }

        public AuthorModel MapReaderAuthor(SqlDataReader reader)
        {
            var author = new AuthorModel();
            author.AuthorID = reader.GetGuid(0);

            string firstName = reader.GetString(1);
            string lastName = reader.GetString(2);

            Name authorName = new Name();
            authorName.FirstName = firstName;
            authorName.LastName = lastName;
            author.Name = authorName;

            author.Biography = reader.GetString(3);
            author.BirthDate = reader.GetDateTime(4);
            author.Country = reader.GetString(5);
            return author;
        }

        public string AddAuthor(DTOAuthor authors)
        {
            using SqlConnection connection = new SqlConnection(_connection.SQLServerManagementStudio);

            try
            {
                connection.Open();
                AuthorModel author = new AuthorModel(authors);

                string insertQuery = "InsertAuthor";
                SqlCommand command = new SqlCommand(insertQuery, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@AuthorID", SqlDbType.UniqueIdentifier).Value = author.AuthorID;
                command.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = author.Name.FirstName;
                command.Parameters.Add("@LastName", SqlDbType.VarChar).Value = author.Name.LastName;
                command.Parameters.Add("@Biography", SqlDbType.VarChar).Value = author.Biography;
                command.Parameters.Add("@BirthDate", SqlDbType.DateTime).Value = author.BirthDate;
                command.Parameters.Add("@Country", SqlDbType.VarChar).Value = author.Country;
                command.Parameters.Add("@CreatedAt", SqlDbType.DateTime).Value = author.getCreatedon();
                command.Parameters.Add("@UpdatedAt", SqlDbType.DateTime).Value = author.getUpdatedon();


                command.ExecuteNonQuery();
                return "Author added successfully";
            }
            catch (Exception e)
            {
                return "Cannot add author";
            }
        }

        public string UpdateAuthor(UpdateAuthorModel author)
        {
            using SqlConnection connection = new SqlConnection(_connection.SQLServerManagementStudio);
            try
            {
                connection.Open();

                string updateQuery = "UpdateAuthor";
                SqlCommand command = new SqlCommand(updateQuery, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@AuthorID", SqlDbType.UniqueIdentifier).Value = author.AuthorID;
                command.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = author.Name.FirstName;
                command.Parameters.Add("@LastName", SqlDbType.VarChar).Value = author.Name.LastName;
                command.Parameters.Add("@Biography", SqlDbType.VarChar).Value = author.Biography;
                command.Parameters.Add("@BirthDate", SqlDbType.DateTime).Value = author.BirthDate;
                command.Parameters.Add("@Country", SqlDbType.VarChar).Value = author.Country;
                command.Parameters.Add("@UpdatedAt", SqlDbType.DateTime).Value = DateTime.Now;

                command.ExecuteNonQuery();
                return "Updated successfully";
            }
            catch (Exception e)
            {
                return "Author not found";
            }
        }

        public bool DeleteAuthor(Guid id)
        {
            using SqlConnection connection = new SqlConnection(_connection.SQLServerManagementStudio);
            try
            {
                connection.Open();
                AuthorModel book = new AuthorModel();
                string deleteQuery = "DeleteAuthor";
                SqlCommand command = new SqlCommand(deleteQuery, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AuthorID", id);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}