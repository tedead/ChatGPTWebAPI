using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;

namespace ChatGPTWebAPI.Library
{
    public class Endpoint
    {
        static string sConnectionString = ConfigurationManager.AppSettings.Get("Main.ConnectionString");

        public static List<SharedObjects.Endpoint> ExportUserEndpoint(int userID)
        {

            List<SharedObjects.Endpoint> list = new List<SharedObjects.Endpoint>();
            RefCountingDataReader reader = null;

            var db = new SqlDatabase(sConnectionString);
            DbCommand dbCmd = db.GetStoredProcCommand("ExportUserEndpoint");
            db.AddInParameter(dbCmd, "@UserID", DbType.Int32, userID);

            reader = ((RefCountingDataReader)db.ExecuteReader(dbCmd)) as RefCountingDataReader;

            while (reader.Read())
            {
                SharedObjects.Endpoint endpoint = new SharedObjects.Endpoint();
                endpoint.EndPointID = Convert.ToInt32(reader.InnerReader["EndpointID"]);
                endpoint.UserEndpoint = reader.InnerReader["UserEndpoint"].ToString();
                endpoint.ConnectionString = reader.InnerReader["ConnectionString"].ToString();
                endpoint.UserID = Convert.ToInt32(reader.InnerReader["UserID"]);

                list.Add(endpoint);
            }

            return list;
        }
    }
}