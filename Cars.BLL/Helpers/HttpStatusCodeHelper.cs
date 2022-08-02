using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cars.BLL.Helpers
{
    public static class HttpStatusCodeHelper
    {
        public static StatusCodeResult Error500()
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}