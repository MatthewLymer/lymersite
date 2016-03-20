using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using dotless.Core;

namespace Lymer.Web
{
    /// <summary>
    /// Helpers for less.css
    /// </summary>
    public static class LessHelper
    {
        private const string LessFileExtension = ".less";

        private static readonly Regex LessImportRegex = new Regex(
            "@import\\s+(?<quote>[\"|'])(.+\\.less)\\k<quote>;",
            RegexOptions.Compiled | RegexOptions.IgnoreCase
        );

        /// <summary>
        /// Parse a series of less/css files, combine them into one result string
        /// </summary>
        /// <param name="files">An enumeration of files</param>
        /// <returns>All files combined, parsed by the less compiler</returns>
        public static string ParseLessFiles(IEnumerable<FileInfo> files)
        {
            if (files == null)
            {
                throw new ArgumentNullException("files");
            }

            StringBuilder buffer = new StringBuilder();

            foreach (FileInfo file in files)
            {
                if (!file.Exists)
                {
                    continue;
                }

                string contents = File.ReadAllText(file.FullName);

                if (file.Extension.Equals(LessFileExtension, StringComparison.OrdinalIgnoreCase))
                {
                    Debug.Assert(file.Directory != null);

                    contents = ResolveImportUrls(contents, file.Directory.FullName);
                    buffer.Append(Less.Parse(contents));
                }
                else
                {
                    buffer.Append(contents);
                }
            }

            return buffer.ToString();
        }

        /// <summary>
        /// Resolve @import tags, converts relative paths to absolute ones
        /// </summary>
        /// <param name="fileContents">Contents of the .less file</param>
        /// <param name="absoluteFileDirectory">Absolute directory of where the file resides</param>
        /// <returns>The .less file contents with its @import directives converted to absolute paths</returns>
        public static string ResolveImportUrls(string fileContents, string absoluteFileDirectory)
        {
            return LessImportRegex.Replace(
                fileContents,
                match =>
                {
                    string import = match.Groups[1].Value;

                    if (import.Contains("://"))
                    {
                        return match.Value;
                    }

                    return match.Value.Replace(
                        import,
                        Path.Combine(absoluteFileDirectory, import)
                    );
                }
            );
        }
    }
}
