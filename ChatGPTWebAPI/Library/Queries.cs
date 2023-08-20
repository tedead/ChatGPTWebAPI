using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ChatGPTWebAPI.Library.Structures;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using System.Configuration;
using ChatGPTWebAPI.SharedObjects;

namespace ChatGPTWebAPI.Library
{
    public class Queries
    {
        static string sConnectionString = ConfigurationManager.AppSettings.Get("Main.ConnectionString");
        public Queries() { }

        public static List<SharedObjects.Queries> ExportUserQueries(DateTime beginDate, DateTime endDate, int userID)
        {
            
            List<SharedObjects.Queries> list = new List<SharedObjects.Queries>();
            RefCountingDataReader reader = null;

            var db = new SqlDatabase(sConnectionString);
            DbCommand dbCmd = db.GetStoredProcCommand("ExportUserQueries");
            db.AddInParameter(dbCmd, "@BeginDate", DbType.DateTime, beginDate);
            db.AddInParameter(dbCmd, "@EndDate", DbType.DateTime, endDate);
            db.AddInParameter(dbCmd, "@UserID", DbType.Int32, userID);

            reader = ((RefCountingDataReader)db.ExecuteReader(dbCmd)) as RefCountingDataReader;

            while (reader.Read())
            {
                SharedObjects.Queries Query = new SharedObjects.Queries();
                Query.QueryID = Convert.ToInt32(reader.InnerReader["QueryID"]);
                Query.Input = reader.InnerReader["Input"].ToString();
                Query.Output = reader.InnerReader["Output"].ToString();
                Query.UserID = Convert.ToInt32(reader.InnerReader["UserID"]);
                Query.dt_Created = Convert.ToDateTime(reader.InnerReader["dt_Created"].ToString());

                list.Add(Query);
            }

            return list;
        }

        public static ImportResponse<SharedObjects.Queries> ImportUserQueries(List<SharedObjects.Queries> queries)
        {
            int totalCount = queries.Count;
            int updatedCount = 0, failedCount = 0;
            var invalidObjects = new List<SharedObjects.Queries>();
            try
            {
                var db = new SqlDatabase(sConnectionString);

                using (IDbConnection connection = db.CreateConnection())
                {
                    connection.Open();

                    foreach (var query in queries)
                    {
                        SqlTransaction sqlTransaction = (SqlTransaction)connection.BeginTransaction();

                        try
                        {
                            var dbCmd = db.GetStoredProcCommand("ImportUserQueries");
                            db.AddInParameter(dbCmd, "@Input", DbType.String, query.Input);
                            db.AddInParameter(dbCmd, "@Output", DbType.String, query.Output);
                            db.AddInParameter(dbCmd, "@UserID", DbType.Int32, query.UserID);
                            db.AddInParameter(dbCmd, "@dt_Created", DbType.DateTime, query.dt_Created);
                            Base.RetryChild<int>(db, dbCmd, sqlTransaction);

                            updatedCount++;
                            sqlTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            sqlTransaction.Rollback();
                            failedCount++;
                            invalidObjects.Add(query);

                        }
                    }
                }

            }
            catch(Exception ex)
            {

            }

            return new ImportResponse<SharedObjects.Queries>(totalCount, 0, updatedCount, failedCount, invalidObjects);
        }
    }
}