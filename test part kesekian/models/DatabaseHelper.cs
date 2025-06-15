using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace test_part_kesekian.models
{
    // Interface for database operations (Abstraction)
    public interface IDatabaseOperations
    {
        DataTable GetData(string query, NpgsqlParameter[] parameters = null);
        int ExecuteNonQuery(string query, NpgsqlParameter[] parameters = null);
        object ExecuteScalar(string query, NpgsqlParameter[] parameters = null);
    }

    // Abstract base class for database operations
    public abstract class BaseDatabaseHelper : IDatabaseOperations
    {
        protected readonly string _connectionString;

        protected BaseDatabaseHelper(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        // Abstract method for creating connection
        public abstract NpgsqlConnection GetConnection();

        // Virtual methods that can be overridden (Polymorphism)
        public virtual DataTable GetData(string query, NpgsqlParameter[] parameters = null)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Query cannot be null or empty", nameof(query));

            DataTable dt = new DataTable();
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var adapter = new NpgsqlDataAdapter(query, conn))
                    {
                        if (parameters != null)
                            adapter.SelectCommand.Parameters.AddRange(parameters);
                        
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error executing query: {ex.Message}", ex);
            }
            return dt;
        }

        public virtual int ExecuteNonQuery(string query, NpgsqlParameter[] parameters = null)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Query cannot be null or empty", nameof(query));

            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);
                        
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error executing non-query: {ex.Message}", ex);
            }
        }

        public virtual object ExecuteScalar(string query, NpgsqlParameter[] parameters = null)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Query cannot be null or empty", nameof(query));

            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);
                        
                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error executing scalar query: {ex.Message}", ex);
            }
        }
    }

    public static class DatabaseHelper
    {
        private static readonly string _connectionString = "Host=localhost;Username=postgres;Password=Delion21.;Database=Mbok Wo Reserved";
        
        private static PostgreSQLHelper _instance;
        private static readonly object _lock = new object();

        public static PostgreSQLHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new PostgreSQLHelper(_connectionString);
                    }
                }
                return _instance;
            }
        }

        public static NpgsqlConnection GetConnection()
        {
            return Instance.GetConnection();
        }

        public static DataTable GetData(string query, NpgsqlParameter[] parameters = null)
        {
            return Instance.GetData(query, parameters);
        }

        public static int ExecuteNonQuery(string query, NpgsqlParameter[] parameters = null)
        {
            return Instance.ExecuteNonQuery(query, parameters);
        }

        public static object ExecuteScalar(string query, NpgsqlParameter[] parameters = null)
        {
            return Instance.ExecuteScalar(query, parameters);
        }

        public static void ExecuteTransaction(Action<NpgsqlTransaction> transactionAction)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        transactionAction(transaction);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        // Method to test database connection
        public static bool TestConnection()
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();
                
                // Test basic query
                using var command = new NpgsqlCommand("SELECT 1", connection);
                var result = command.ExecuteScalar();
                
                return result != null && result.ToString() == "1";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database connection test failed: {ex.Message}");
                return false;
            }
        }

        public static void FixReservationSequence()
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();
                
                string maxIdQuery = "SELECT COALESCE(MAX(id), 0) FROM reservations";
                using var maxIdCmd = new NpgsqlCommand(maxIdQuery, connection);
                var maxId = Convert.ToInt32(maxIdCmd.ExecuteScalar());
                
                string fixSequenceQuery = $"SELECT setval('reservations_id_seq', {maxId + 1})";
                using var fixCmd = new NpgsqlCommand(fixSequenceQuery, connection);
                fixCmd.ExecuteNonQuery();
                
                Console.WriteLine($"✅ Fixed reservation sequence: set to {maxId + 1}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to fix sequence: {ex.Message}");
                throw;
            }
        }
    }

    public class PostgreSQLHelper : BaseDatabaseHelper
    {
        public PostgreSQLHelper(string connectionString) : base(connectionString) { }

        public override NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        public override DataTable GetData(string query, NpgsqlParameter[] parameters = null)
        {
            return base.GetData(query, parameters);
        }

        public List<T> GetList<T>(string query, Func<NpgsqlDataReader, T> mapper, NpgsqlParameter[] parameters = null)
        {
            var list = new List<T>();
            
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(mapper(reader));
                        }
                    }
                }
            }
            
            return list;
        }

        public T GetSingle<T>(string query, Func<NpgsqlDataReader, T> mapper, NpgsqlParameter[] parameters = null) where T : class
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return mapper(reader);
                    }
                }
            }
            
            return null;
        }
    }

    public class DatabaseException : Exception
    {
        public DatabaseException(string message) : base(message) { }
        public DatabaseException(string message, Exception innerException) : base(message, innerException) { }
    }
}