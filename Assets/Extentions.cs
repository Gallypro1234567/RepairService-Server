using System;
using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace WorkAppReactAPI.Assets
{
    public static class Extentions
    {
        public static DataTable ExecuteDataTable(this DbContext context, string commandText, DbParameter[] parameters)
        {
            var dataTable = new DataTable();
            DbConnection connection = context.Database.GetDbConnection();
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                DbProviderFactory dbFactory = DbProviderFactories.GetFactory(transaction.Connection);
                try
                {
                    using (var cmd = dbFactory.CreateCommand())
                    {
                        cmd.Connection = transaction.Connection;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = commandText;
                        if (parameters != null)
                        {
                            foreach (var item in parameters)
                            {
                                cmd.Parameters.Add(item);
                            }
                        }
                        using (DbDataAdapter adapter = dbFactory.CreateDataAdapter())
                        {
                            adapter.SelectCommand = cmd;
                            transaction.Commit();
                            adapter.Fill(dataTable);
                        }

                    }
                    return dataTable;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
                finally { if (connection.State == ConnectionState.Open) connection.Close(); };
            }

        }
    }
}