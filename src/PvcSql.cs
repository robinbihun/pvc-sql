using PvcCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;

namespace PvcPlugins
{
    public class PvcSql : PvcPlugin
    {
        private string _connectionString;
        private string _providerName;

        public PvcSql(string connectionString, string providerName = "System.Data.SqlClient")
        {
            _connectionString = connectionString;
            _providerName = providerName;
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
            var provider = DbProviderFactories.GetFactory(_providerName);

            using (var conn = provider.CreateConnection())
            {
                conn.ConnectionString = _connectionString;
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {

                    foreach (var inputStream in inputStreams)
                    {
                        Console.WriteLine("Executing Sql File: {0}", inputStream.StreamName);
                        string sql = new StreamReader(inputStream).ReadToEnd();

                        using (var cmd = conn.CreateCommand())
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
    }
}
