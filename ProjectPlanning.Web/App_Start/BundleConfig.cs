using System.Web.Optimization;

namespace ProjectPlanning.Web.App_Start
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/gantt/css").Include("~/Content/gantt/gantt.css"));
            bundles.Add(new ScriptBundle("~/Scripts/gantt/js").Include("~/Scripts/gantt/gantt.js"));
        }
    }
}
