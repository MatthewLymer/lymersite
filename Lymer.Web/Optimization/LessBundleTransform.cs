using System.Web.Optimization;

namespace Lymer.Web.Optimization
{
    public class LessBundleTransform : IBundleTransform
    {
        public void Process(BundleContext context, BundleResponse response)
        {
            response.ContentType = "text/css";

            response.Content = LessHelper.ParseLessFiles(response.Files);
        }
    }
}
