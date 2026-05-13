using Unity;
using System.Windows;

namespace WorkProject.UI.Composition
{
    public class ViewModelLocator
    {
        public static IUnityContainer Container { get; set; }

        public static T Resolve<T>() => Container.Resolve<T>();
    }
}
