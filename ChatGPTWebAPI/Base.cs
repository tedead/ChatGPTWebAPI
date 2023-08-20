using System;
using System.Data;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using ChatGPTWebAPI.Library.Structures;

namespace ChatGPTWebAPI
{
    public class Base
    {
        //Sample call to retry method for dataset
        //return RetryMaster<DataSet>(db, dbCommand, null);
        //this would replace for call with DatabaseFactory.CreateDatabase();
        //return db.ExecuteDataSet(dbCommand);

        //Sample call to retry method for dataset
        //return RetryChild<DataSet>(db, dbCommand, null);
        //this would replace for call with new SqlDatabase((HttpContext.Current.User as Principal).ConnectionString);
        //return db.ExecuteDataSet(dbCommand);

        public static void RetryChild(SqlBulkCopy bc, DataTable dt)
        {
            // Define your retry strategy: retry 3 times, 1 second apart.
            //var retryStrategy = new FixedInterval(3, TimeSpan.FromSeconds(3));
            //var retryStrategy = new ExponentialBackoff(3, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(120), TimeSpan.FromSeconds(4));
            var retryStrategy = new Incremental(3, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(9));
            retryStrategy.FastFirstRetry = true;

            // Define your retry policy using the retry strategy and the Azure storage
            // transient fault detection strategy.
            var retryPolicy =
              new RetryPolicy<StorageTransientErrorDetectionStrategy>(retryStrategy);

            try
            {
                retryPolicy.ExecuteAction(
                  () =>
                  {
                      bc.WriteToServer(dt);
                  });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static T RetryChild<T>(DbCommand cw)
        {
            var returnValue = Default<T>();

            // Define your retry strategy: retry 3 times, 1 second apart.
            //var retryStrategy = new FixedInterval(3, TimeSpan.FromSeconds(3));
            //var retryStrategy = new ExponentialBackoff(3, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(120), TimeSpan.FromSeconds(4));
            var retryStrategy = new Incremental(3, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(9));
            retryStrategy.FastFirstRetry = true;

            // Define your retry policy using the retry strategy and the Azure storage
            // transient fault detection strategy.
            var retryPolicy =
              new RetryPolicy<StorageTransientErrorDetectionStrategy>(retryStrategy);

            try
            {
                retryPolicy.ExecuteAction(
                  () =>
                  {
                      if (typeof(T) == typeof(int))
                      {
                          returnValue = Cast<T>(cw.ExecuteNonQuery());
                      }
                      else if (typeof(T) == typeof(object))
                      {
                          returnValue = Cast<T>(cw.ExecuteScalar());
                      }
                      else if (typeof(T) == typeof(RefCountingDataReader) ||
                          typeof(T) == typeof(SqlDataReader))
                      {
                          returnValue = Cast<T>(cw.ExecuteReader());
                      }
                      else if (typeof(T) == typeof(SafeDataReader))
                      {
                          returnValue = Cast<T>(new SafeDataReader(cw.ExecuteReader()));
                      }
                  });

                return returnValue;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static T RetryChild<T>(SqlDatabase db, DbCommand cw)
        {
            return RetryChild<T>(db, cw, null);
        }

        public static T RetryChild<T>(SqlDatabase db, DbCommand cw, SqlTransaction tx)
        {
            var returnValue = Default<T>();

            // Define your retry strategy: retry 3 times, 1 second apart.
            //var retryStrategy = new FixedInterval(3, TimeSpan.FromSeconds(3));
            //var retryStrategy = new ExponentialBackoff(3, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(120), TimeSpan.FromSeconds(4));
            var retryStrategy = new Incremental(3, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(9));
            retryStrategy.FastFirstRetry = true;

            // Define your retry policy using the retry strategy and the Azure storage
            // transient fault detection strategy.
            var retryPolicy =
              new RetryPolicy<StorageTransientErrorDetectionStrategy>(retryStrategy);

            try
            {
                retryPolicy.ExecuteAction(
                  () =>
                  {
                      if (typeof(T) == typeof(DataSet))
                      {
                          if (tx == null)
                          {
                              returnValue = Cast<T>(db.ExecuteDataSet(cw));
                          }
                          else
                          {
                              returnValue = Cast<T>(db.ExecuteDataSet(cw, tx));
                          }
                      }
                      else if (typeof(T) == typeof(RefCountingDataReader) ||
                               typeof(T) == typeof(SqlDataReader))
                      {
                          if (tx == null)
                          {
                              returnValue = Cast<T>(db.ExecuteReader(cw));
                          }
                          else
                          {
                              returnValue = Cast<T>(db.ExecuteReader(cw, tx));
                          }
                      }
                      else if (typeof(T) == typeof(SafeDataReader))
                      {
                          if (tx == null)
                          {
                              returnValue = Cast<T>(new SafeDataReader(db.ExecuteReader(cw)));
                          }
                          else
                          {
                              returnValue = Cast<T>(new SafeDataReader(db.ExecuteReader(cw, tx)));
                          }
                      }
                      else if (typeof(T) == typeof(int))
                      {
                          if (tx == null)
                          {
                              returnValue = Cast<T>(db.ExecuteNonQuery(cw));
                          }
                          else
                          {
                              returnValue = Cast<T>(db.ExecuteNonQuery(cw, tx));
                          }
                      }
                      else if ((typeof(T) == typeof(object)) || (typeof(T) == typeof(Guid)) || (typeof(T) == typeof(string)))
                      {
                          if (tx == null)
                          {
                              returnValue = Cast<T>(db.ExecuteScalar(cw));
                          }
                          else
                          {
                              returnValue = Cast<T>(db.ExecuteScalar(cw, tx));
                          }
                      }
                      else if (typeof(T) == typeof(void))
                      {
                          if (tx == null)
                          {
                              returnValue = Cast<T>(db.ExecuteNonQuery(cw));
                          }
                          else
                          {
                              returnValue = Cast<T>(db.ExecuteNonQuery(cw, tx));
                          }
                      }
                      else if (typeof(T) == typeof(XmlDocument))
                      {
                          if (tx == null)
                          {
                              XmlReader xmlReader = db.ExecuteXmlReader(cw);

                              if (xmlReader.Read())
                              {
                                  XmlDocument xmlDocument = new XmlDocument();
                                  xmlDocument.Load(xmlReader);
                                  returnValue = Cast<T>(xmlDocument);
                              }
                          }
                          else
                          {
                              XmlReader xmlReader = db.ExecuteXmlReader(cw, tx);

                              if (xmlReader.Read())
                              {
                                  XmlDocument xmlDocument = new XmlDocument();
                                  xmlDocument.Load(xmlReader);
                                  returnValue = Cast<T>(xmlDocument);
                              }
                          }
                      }
                  });

                return returnValue;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static T Default<T>()
        {
            Type t = typeof(T);

            return default(T);
        }

        public static T Cast<T>(object o)
        {
            if (o == null)
            {
                return Default<T>();
            }

            Type t = typeof(T);

            return (T)Convert.ChangeType(o, t, CultureInfo.InvariantCulture);
        }

        private static string _redisConnectionString;

        public static string RedisConnectionString
        {
            get
            {

#if !RuntimeVersion_2
                if (string.IsNullOrWhiteSpace(_redisConnectionString))
                    return ConfigurationManager.AppSettings["Redis.ConnectionString"];
                else
#endif
                    return _redisConnectionString;

            }
            set { _redisConnectionString = value; }
        }

        public static string DatabaseName
        {
            get
            {
                string databaseName = (HttpContext.Current.User as Principal).ConnectionString;
                int startIdx;
                if (databaseName.ToUpper().Contains("INITIAL CATALOG"))
                {
                    startIdx = databaseName.ToUpper().IndexOf("INITIAL CATALOG=", StringComparison.InvariantCulture);
                    databaseName = databaseName.Substring(startIdx + 16);
                }
                else
                {
                    startIdx = databaseName.ToUpper().IndexOf("DATABASE=", StringComparison.InvariantCulture);
                    databaseName = databaseName.Substring(startIdx + 9);
                }

                int endIdx = databaseName.IndexOf(";", StringComparison.InvariantCulture);
                databaseName = databaseName.Substring(0, endIdx);

                return databaseName;
            }
        }

        public static string DatabaseServer
        {
            get
            {
                var connectionString = (HttpContext.Current.User as Principal).ConnectionString;
                string[] tokens = connectionString.Split(";".ToCharArray());

                List<KeyValuePair<string, string>> kv = new List<KeyValuePair<string, string>>();

                foreach (var token in tokens.Where(t => t.Contains("=")))
                {
                    var item = new KeyValuePair<string, string>(
                        token.Split("=".ToCharArray())[0].ToUpper(), token.Split("=".ToCharArray())[1]);
                    kv.Add(item);
                }

                return kv.First(x =>
                    x.Key.Contains("DATA SOURCE") ||
                    x.Key.Contains("SERVER") ||
                    x.Key.Contains("ADDRESS") ||
                    x.Key.Contains("ADDR") ||
                    x.Key.Contains("NETWORK ADDRESS")).Value;
            }
        }
    }
}