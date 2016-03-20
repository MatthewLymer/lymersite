using System;
using System.IO;
using System.Web.Optimization;
using Lymer.Web.Optimization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lymer.Web.Tests.Optimization
{
    [TestClass]
    [DeploymentItem(LessHelperTestResourcesPath, LessHelperTestResourcesPath)]
    public class WhenProcessingALessBundleTransform
    {
        private const string LessHelperTestResourcesPath = "LessHelperTestResources";

        [TestMethod]
        public void should_parse_less_file_with_imports()
        {
            FileInfo[] files = {
                new FileInfo(Path.Combine(LessHelperTestResourcesPath, "ValidLessFile.less")) 
            };

            BundleResponse response = new BundleResponse("", files);

            LessBundleTransform lessBundleTransform = new LessBundleTransform();

            lessBundleTransform.Process(null, response);

            Assert.IsTrue(response.Content.IndexOf("color: lime;", StringComparison.Ordinal) != -1);

            Assert.AreEqual("text/css", response.ContentType);
        }
    }
}
