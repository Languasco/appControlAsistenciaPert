using System.Web;
using System.Web.Mvc;

namespace WebApi_Asistencias_Pert
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
