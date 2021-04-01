using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using WorkAppReactAPI.Controllers;


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
                            result.dataTable = dataTable;
                             
                        }

                    }
                    return result;
                }
                catch (Exception Ex)
                {
                    result.dataTable = dataTable; 
                    result.Message = Ex.Message;
                    transaction.Rollback();
                    return result;
                }
                finally { if (connection.State == ConnectionState.Open) connection.Close(); };
            }

        }
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
            result.data = listobj;
            result.totalrow = total;
            result.message = rs.Message; 
            return result;
        }
    }
}