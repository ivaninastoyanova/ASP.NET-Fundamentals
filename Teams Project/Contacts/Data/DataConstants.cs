namespace Contacts.Data
{
    public class DataConstants
    {
        public const int ContactFirstNameMaxLength = 50;
        public const int ContactFirstNameMinLength = 2;

        public const int ContactLastNameMaxLength = 50;
        public const int ContactLasttNameMinLength = 5;

        public const int ContactEmailMaxLength = 60;
        public const int ContactEmailMinLength = 10;

        public const int ContactPhoneMaxLength = 13;
        public const int ContactPhoneMinLength = 10;
        public const string ContactPhoneRegex = @"^(((\+359|0)[-| ]?))(8[789]\d([- ]?\d{2}){3})$";

        public const string ContactWebsiteRegex = @"^www.[a-zA-Z0-9\-]+.bg$";
    }
}
