using System.Web.Optimization;
using Lymer.Web.Optimization;

namespace Lymer.UserUI.App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            LessBundle masterStyle = new LessBundle("~/Content/Styles/Master");
            
            masterStyle.Include(
                "~/Content/Styles/normalize.css",
                "~/Content/Styles/font-economica.css",
                "~/Content/Styles/master.less"
            );

            bundles.Add(masterStyle);
        }
    }
}