using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;

[assembly: AssemblyTitle("ProjectPlanning.Web")]
[assembly: AssemblyProduct("ProjectPlanning")]
[assembly: ComVisible(false)]
[assembly: Guid("55555555-5555-5555-5555-5555555555aa")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: PreApplicationStartMethod(typeof(ProjectPlanning.Web.PreStart), "Start")]

namespace ProjectPlanning.Web
{
    public static class PreStart
    {
        public static void Start() { }
    }
}
