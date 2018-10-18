using System;
using System.Collections;
using DatabaseLayer;

namespace DatabaseLayer.Entity
{
    internal class AuthorizationObject
    {
        public String SystemID { get; set; }
        public String SystemName { get; set; }
        public String UserCompany { get; set; }
        public String DefaultDataSource { get; set; }
        public String DevelopeCompany { get; set; }
        public String ReleaseDate { get; set; }
        public String ExpiryDate { get; set; }
        public String Version { get; set; }
        public String DataSource { get; set; }
        public String InitialCatalog { get; set; }
    }
}
