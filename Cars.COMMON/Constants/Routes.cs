namespace Cars.COMMON.Constants
{
    public static class Routes
    {
        public static class Controllers
        {
            public const string CARS = "api/v{version:apiVersion}/Cars";
            public const string AUTH = "api/v{version:apiVersion}/Auth";
            public const string USERS = "api/v{version:apiVersion}/Users";
        }
        public static class Methods
        {
            public const string GETALLCARS = "GetAllCars";
            public const string ID = "{id}";
            public const string UPDATE = "Update";
            public const string CREATE = "Create";
            public const string EDIT = "Edit";
            public const string BAN = "Ban";
            public const string UNBAN = "Unban";
            public const string DELETE = "Delete";
            public const string RESTORE = "Restore";
            public const string LOGIN = "Login";
            public const string CHANGEPASSWORD = "ChangePassword";
            public const string REGISTER = "Register";
            public const string IMPORT = "Import";
            public const string GETNEWTOKEN = "GetNewToken";
            public const string RESETPASSWORD = "ResetPassword";
            public const string FORGOTPASSWORD = "ForgotPassword";
        }

        public static class Paths
        {
            public const string RESETPASSWORD = "/resetpassword/";
        }
    }
}
