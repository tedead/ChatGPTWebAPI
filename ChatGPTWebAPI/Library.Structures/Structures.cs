using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace ChatGPTWebAPI.Library.Structures
{
    #region Principal
    public class Principal : IPrincipal
    {
        private string _connectionString;

        public int WarehouseID { get; set; }
        public int UserID { get; set; }
        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    return ConfigurationManager.AppSettings["Main.ConnectionString"];
                }
                else
                {
                    return _connectionString;
                }
            }
            set
            {
                _connectionString = value;
            }
        }
        public int ReceivingBinID { get; set; }
        public int DistributorID { get; set; }
        public int Pin { get; set; }
        public int UserBinId { get; set; }
        public string UserBinNumber { get; set; }
        public string WarehouseName { get; set; }
        public string UserName { get; set; }
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public Guid ClientKey { get; set; }
        public string MobileVersion { get; set; }
        public string Platform { get; set; }
        public string Context { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string AllocationSetName { get; set; }

        public Principal()
        {
        }

        public Principal(IIdentity identity, Guid clientKey, string connectionString)
        {
            Identity = identity;
            ClientKey = clientKey;
            ConnectionString = connectionString;
            Context = String.Empty;
        }

        public Principal(IIdentity identity, int warehouseID, int userID, string connectionString, int receivingBinID, int distributorID,
            int pin, int userBinId, string userBinNumber, string warehouseName)
        {
            Identity = identity;
            WarehouseID = warehouseID;
            UserID = userID;
            ConnectionString = connectionString;
            ReceivingBinID = receivingBinID;
            DistributorID = distributorID;
            Pin = pin;
            UserBinId = userBinId;
            UserBinNumber = userBinNumber;
            WarehouseName = warehouseName;
            Context = String.Empty;
        }

        public Principal(IIdentity identity, int warehouseID, int userID, string connectionString, int receivingBinID, int distributorID,
            int pin, int userBinId, string userBinNumber, string warehouseName, int clientID)
        {
            Identity = identity;
            WarehouseID = warehouseID;
            UserID = userID;
            ConnectionString = connectionString;
            ReceivingBinID = receivingBinID;
            DistributorID = distributorID;
            Pin = pin;
            UserBinId = userBinId;
            UserBinNumber = userBinNumber;
            WarehouseName = warehouseName;
            ClientID = clientID;
            Context = String.Empty;
            AllocationSetName = String.Empty;
        }

        public Principal(IIdentity identity, string userName, string connectionString)
        {
            Identity = identity;
            UserName = userName;
            ConnectionString = connectionString;
            Context = String.Empty;
        }

        public void SetPropertiesFromHeaderFields(System.Collections.Specialized.NameValueCollection headers)
        {
            //allows us to set the principal properties directly from headers in the request
            foreach (string key in headers.Keys)
            {
                var property = GetType().GetProperty(key);

                if (property != null)
                    property.SetValue(this, headers[key]);
            }
        }
        #region Implementation of IPrincipal

        /// <summary>
        /// Determines whether the current principal belongs to the specified role.
        /// </summary>
        /// <returns>
        /// true if the current principal is a member of the specified role; otherwise, false.
        /// </returns>
        /// <param name="role">The name of the role for which to check membership. 
        ///                 </param>
        public bool IsInRole(string role)
        {
            return true;
        }

        /// <summary>
        /// Gets the identity of the current principal.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Security.Principal.IIdentity"/> object associated with the current principal.
        /// </returns>
        public IIdentity Identity { get; set; }

        #endregion
    }
    #endregion
    public class Structures
    {
    }
}