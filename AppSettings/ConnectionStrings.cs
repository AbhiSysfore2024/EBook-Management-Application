namespace AppSettings
{
    public class ConnectionStrings
    {
        public string AzureDataStudio {  get; set; }
        public string SQLServerManagementStudio { get; set; }
    }

    public class JWTClaimDetails
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
    }
}
