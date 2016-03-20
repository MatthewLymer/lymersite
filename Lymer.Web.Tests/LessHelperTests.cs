using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lymer.Web.Tests
{
    [TestClass]
    [DeploymentItem(LessHelperTestResourcesPath, LessHelperTestResourcesPath)]
    public class WhenUsingLessHelperMethodParseLessFiles
    {
        private const string LessHelperTestResourcesPath = "LessHelperTestResources";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_exception_when_argument_is_null()
        {
            LessHelper.ParseLessFiles(null);
        }

        [TestMethod]
        public void should_return_empty_content_when_no_files_are_parsed()
        {
            FileInfo[] files = new FileInfo[0];

            string content = LessHelper.ParseLessFiles(files);

            Assert.AreEqual("", content);
        }

        [TestMethod]
        public void should_ignore_missing_files()
        {
            FileInfo[] files = {
                new FileInfo(Path.Combine(LessHelperTestResourcesPath, "MissingFileName.css")) 
            };

            string content = LessHelper.ParseLessFiles(files);

            Assert.AreEqual("", content);
        }

        [TestMethod]
        public void should_pass_through_css_files_unmodified()
        {
            string filePath = Path.Combine(LessHelperTestResourcesPath, "CssFile.css");
            FileInfo[] files = {
                new FileInfo(filePath) 
            };

            string content = LessHelper.ParseLessFiles(files);

            Assert.AreEqual(File.ReadAllText(filePath), content);
        }

        [TestMethod]
        public void should_parse_less_file_with_imports()
        {
            FileInfo[] files = {
                new FileInfo(Path.Combine(LessHelperTestResourcesPath, "ValidLessFile.less")) 
            };

            string content = LessHelper.ParseLessFiles(files);

            Assert.IsTrue(content.IndexOf("color: lime;", StringComparison.Ordinal) != -1);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void should_throw_exception_with_bad_import()
        {
            FileInfo[] files = {
                new FileInfo(Path.Combine(LessHelperTestResourcesPath, "BadImportLessFile.less")) 
            };

            LessHelper.ParseLessFiles(files);
        }
    }
}
