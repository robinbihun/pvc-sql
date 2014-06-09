using MySql.Data.MySqlClient;
using Npgsql;
using PvcCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace PvcPlugins
{
    public class PvcSql : PvcPlugin
    {
        private readonly string _connectionString;
        private readonly Provider _providerName;

        public PvcSql(string connectionString, string providerName = "MsSql")
        {
            _connectionString = connectionString;
            if (!Enum.TryParse(providerName, true, out _providerName))
            {
                throw new Exception(String.Format("Provider {0} not supported.", providerName));
            };
        }

        public override string[] SupportedTags
        {
            get
            {
                return new[] { ".sql" };
            }
        }

        public override IEnumerable<PvcStream> Execute(IEnumerable<PvcStream> inputStreams)
        {
            using (var connection = GetConnection(_providerName, _connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var inputStream in inputStreams)
                    {
                        Console.WriteLine("Executing Sql File: {0}", inputStream.StreamName);
                        string sql = new StreamReader(inputStream).ReadToEnd();

                        using (var cmd = connection.CreateCommand())
                        {
                            cmd.Transaction = transaction;
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                        }
                    }

                    try
                    {
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Transaction failed... Rolling back.");
                        transaction.Rollback();
                    }
                }
            }

            return inputStreams;
        }

        internal enum Provider
        {
            MsSql,
            MySql,
            PostgreSql,
        }

        private static IDbConnection GetConnection(Provider provider, string connectionString)
        {
            switch (provider)
            {
                case Provider.MsSql:
                    return new SqlConnection(connectionString);
                case Provider.MySql:
                    return new MySqlConnection(connectionString);
                case Provider.PostgreSql:
                    return new NpgsqlConnection(connectionString);
            }

            throw new ApplicationException("Provider not supported.");
        }
    }
}
