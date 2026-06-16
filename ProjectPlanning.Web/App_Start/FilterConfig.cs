using System.Web.Mvc;
using ProjectPlanning.Web.Filters;

namespace ProjectPlanning.Web.App_Start
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthenticatedUserFilter());
        }
    }
}
