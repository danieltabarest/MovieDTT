using SQLite.Net;
using SQLite.Net.Async;

namespace MovieDTT.Interfaces
{
    public interface ISQLite
    {
        void CloseConnection();
        SQLiteConnection GetConnection();
        SQLiteAsyncConnection GetAsyncConnection();
        void DeleteDatabase();
    }
}