namespace Apps.APIRest.Configuration.JWT
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int HoursToExpire { get; set; }
        public string Issuer { get; set; }
        public string ValidIn { get; set; }
    }
}
