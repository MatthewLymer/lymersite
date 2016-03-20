using System.Web.Optimization;

namespace Lymer.Web.Optimization
{
    public class LessBundle : Bundle
    {
        private static readonly IBundleTransform[] BundleTransforms = {
            new LessBundleTransform(), 
            new CssMinify()
        };

        public LessBundle(string virtualPath)
            : base(virtualPath, BundleTransforms)
        {

        }
    }
}
