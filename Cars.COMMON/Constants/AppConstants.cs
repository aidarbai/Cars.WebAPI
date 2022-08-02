namespace Cars.COMMON.Constants
{
    public static class AppConstants
    {
        public static class Roles
        {
            public const string SUPER_ADMIN = "SuperAdmin";
            public const string ADMIN = "Admin";
            public const string USER = "User";
            public static class Groups
            {
                public static readonly string[] ROLES = { SUPER_ADMIN, ADMIN, USER };
                public const string ADMINSGROUP = SUPER_ADMIN + "," + ADMIN;
            }
        }
        public static class ApiAttributes
        {
            public const string NAME = "Cars.API";
            public const string VERSION = "1.0";
            public const string VERSION_NUMBER = "V1";
            public const string CONTENTTYPE = "application/json";
        }

        public enum Order
        {
            ASC,
            DESC
        }

        public static class Attributes
        {
            public const string PRICE = "Price";
            public const string LASTNAME = "LastName";
        }
    }
}