using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Net.Http;
using System.Net;
using System.Web.Http;

namespace ChatGPTWebAPI.Library
{
    public class Utility
    {
        public static void ThrowException(Exception ex, HttpRequestMessage request)
        {
            string message;
            if (ex.InnerException == null)
            {
                message = ex.Message;
            }
            else
            {
                message = ex.InnerException.InnerException == null ? ex.InnerException.Message : ex.InnerException.InnerException.Message;
            }

            var resp = new HttpResponseMessage(HttpStatusCode.Conflict)
            {
                Content = new StringContent(message),
                ReasonPhrase =
                                   message.Replace(Environment.NewLine, " - ").Substring(0,
                                                                                         Math.Min(512, message.Length)),
                RequestMessage = request
            };

            //  Don't log SQL business rules.
            SqlException sqlEx = ex as SqlException;
            if (sqlEx == null || sqlEx.Number < 5000)
            {
                //LogErrors.LogWebErrors(ex);
            }

            //There is a lenght limit on the reasonPhrase
            throw new HttpResponseException(resp);

        }

        public static void ThrowException(string ex, HttpRequestMessage request)
        {
            string reasonPhrase = ex.Replace(Environment.NewLine, " - ").Replace("\n", " - ").Substring(0, Math.Min(512, ex.Length));

            var resp = new HttpResponseMessage(HttpStatusCode.Conflict)
            {
                Content = new StringContent(ex),
                ReasonPhrase = reasonPhrase,
                RequestMessage = request
            };

            //There is a lenght limit on the reasonPhrase
            throw new HttpResponseException(resp);

        }

        public static void ThrowSystemException(Exception ex, HttpRequestMessage request)
        {
            string message;
            if (ex.InnerException == null)
            {
                message = ex.Message;
            }
            else
            {
                message = ex.InnerException.InnerException == null ? ex.InnerException.Message : ex.InnerException.InnerException.Message;
            }

            var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(message),
                ReasonPhrase =
                                   message.Replace(Environment.NewLine, " - ").Substring(0,
                                                                                         Math.Min(512, message.Length)),
                RequestMessage = request
            };

            //LogErrors.LogWebErrors(ex);

            //There is a length limit on the reasonPhrase
            throw new HttpResponseException(resp);

        }

        public static void NotImplemented()
        {
            var resp = new HttpResponseMessage(HttpStatusCode.NotImplemented);
            throw new HttpResponseException(resp);
        }

        public static bool IsLength(string s, string objType)
        {
            bool hasLength = false;

            switch (objType.ToLower())
            {
                case "item":
                    if (s.Length <= 150)
                        hasLength = true;
                    break;
                case "lp":
                    if (s.Length <= 50)
                        hasLength = true;
                    break;
                case "bin":
                    if (s.Length <= 50)
                        hasLength = true;
                    break;
                case "lot":
                    if (s.Length <= 50)
                        hasLength = true;
                    break;
                case "uom":
                    if (s.Length <= 50)
                        hasLength = true;
                    break;
                case "qty":
                    if (s.Length > 0)
                        hasLength = true;
                    break;
                case "username":
                    if (s.Length <= 100)
                        hasLength = true;
                    break;
                case "warehousename":
                    if (s.Length <= 100)
                        hasLength = true;
                    break;
                case "ordernumber":
                    if (s.Length <= 50)
                        hasLength = true;
                    break;
                case "ponumber":
                    if (s.Length <= 50)
                        hasLength = true;
                    break;
                case "movementType":
                    if (s.Length <= 50)
                        hasLength = true;
                    break;
                case "erpShortCode":
                    if (s.Length <= 400)
                        hasLength = true;
                    break;
                case "returnNumber":
                    if (s.Length <= 50)
                        hasLength = true;
                    break;
                case "note":
                    if (s.Length <= 500)
                        hasLength = true;
                    break;
                default:
                    hasLength = true;
                    break;
            }

            return hasLength;
        }

        public static string NullToEmptyString(string value)
        {
            return value == "(null)" ? String.Empty : (value ?? String.Empty);
        }

        public static decimal DecimalCheck(decimal s)
        {
            return SqlDecimal.ConvertToPrecScale(new SqlDecimal(s), 19, 5).Value;
        }

        public static string ReplaceHtmlEnCode(string value)
        {
            return value.Replace("%20", " ");
        }


    }
}