using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICafe.Persistence.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application error:", message);
            response.Headers.Add("Access-Control-Expose-Headers", "ApplicationError");
            response.Headers.Add("Access-Control-Allow_Origin", "*");
        }

        //public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
        //{
        //    var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
        //    var camalCaseFormatter = new JsonSerializerSettings();
        //    camalCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
        //    response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader, camalCaseFormatter));
        //    response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        //}

        //public static int CalculateAge(this DateTime theDateTime)
        //{
        //    var age = DateTime.Today.Year - theDateTime.Year;
        //    if (theDateTime.AddYears(age) > DateTime.Today)
        //    {
        //        age--;
        //    }
        //    return age;
        //}
    }
}
