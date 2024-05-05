
using Services;
using Services.Interface;

namespace EBook_Management_Application.Extensions
{
    public static class DIExtensions
    {
        public static void SingletonService(this IServiceCollection services)
        {
            services.AddSingleton<IDatabaseManager, BooksStoredProcedure>();
            services.AddSingleton<IAuthorDatabaseManager, AuthorStoredProcedure>();
        }
    }
}
