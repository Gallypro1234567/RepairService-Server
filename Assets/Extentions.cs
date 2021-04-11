using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkAppReactAPI.Configuration;


namespace WorkAppReactAPI.Assets
{
    public static class Extentions
    {

        public static SqlResult ExecuteDataTable(this DbContext context, string commandText, DbParameter[] parameters)
        {
            var result = new SqlResult();
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
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = commandText;
                        cmd.Transaction = transaction;
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
                            adapter.Fill(dataTable);
                            transaction.Commit();
                            result.Status = true;
                            result.Type = "Success";
                            result.dataTable = dataTable;

                        }

                    }
                    return result;
                }
                catch (Exception Ex)
                {
                    result.Status = false;
                    result.Type = "Error";
                    result.dataTable = dataTable;
                    result.Message = Ex.Message;
                    transaction.Rollback();
                    return result;
                }
                finally { if (connection.State == ConnectionState.Open) connection.Close(); };
            }

        }
        // public static SqlResult ExecuteStoreUpdateMutilple(this DbContext context, string[] listcommandText, List<DbParameter[]> parameters)
        // {
        //     var result = new SqlResult();
        //     var dataTable = new DataTable();
        //     DbConnection connection = context.Database.GetDbConnection();
        //     connection.Open();
        //     using (var transaction = connection.BeginTransaction())
        //     {
        //         DbProviderFactory dbFactory = DbProviderFactories.GetFactory(transaction.Connection);
        //         try
        //         {
        //             using (var cmd = dbFactory.CreateCommand())
        //             {
        //                 foreach (var commandText in listcommandText)
        //                 {
        //                     cmd.Connection = transaction.Connection;
        //                     cmd.CommandType = CommandType.StoredProcedure;
        //                     cmd.CommandText = commandText;
        //                     cmd.Transaction = transaction;
        //                     if (parameters != null)
        //                     {
        //                         foreach (var item in parameters)
        //                         {
        //                             cmd.Parameters.Add(item);
        //                         }
        //                     }
        //                     using (DbDataAdapter adapter = dbFactory.CreateDataAdapter())
        //                     {
        //                         adapter.SelectCommand = cmd;
        //                         adapter.Fill(dataTable);
                                
        //                         result.Status = true;
        //                         result.Type = "Success";
        //                         result.dataTable = dataTable;

        //                     }
        //                 }
        //                 transaction.Commit();  
        //             }
        //             return result;
        //         }
        //         catch (Exception Ex)
        //         {
        //             result.Status = false;
        //             result.Type = "Error";
        //             result.dataTable = dataTable;
        //             result.Message = Ex.Message;
        //             transaction.Rollback();
        //             return result;
        //         }
        //         finally { if (connection.State == ConnectionState.Open) connection.Close(); };
        //     }

        // }

        public static DynamicResult JsonData(this SqlResult rs)
        {

            var result = new DynamicResult();
            List<Dictionary<string, object>> listobj = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            var total = 0;

            foreach (DataRow dr in rs.dataTable.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in rs.dataTable.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                listobj.Add(row);
            }

            result.Data = listobj;
            result.Totalrow = total;
            result.Message = rs.Message;
            result.Status = rs.Status;
            result.Type = rs.Type;

            return result;
        }
        public static Task<DynamicResult> JsonDataAsync(this SqlResult rs)
        {

            var result = new DynamicResult();
            List<Dictionary<string, object>> listobj = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            var total = 0;

            foreach (DataRow dr in rs.dataTable.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in rs.dataTable.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                listobj.Add(row);
            }

            result.Data = listobj;
            result.Totalrow = total;
            result.Message = rs.Message;
            result.Status = rs.Status;
            result.Type = rs.Type;

            return Task.Run(() => { return result; });
        }

    }
}