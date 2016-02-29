using System.Web;
using System.Web.Mvc;

namespace ERTO_of_Gujarat
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
