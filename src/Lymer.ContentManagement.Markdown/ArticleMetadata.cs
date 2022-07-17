using System;
using System.Collections.Generic;

namespace Lymer.ContentManagement.Markdown
{
    [Serializable]
    public sealed class ArticleMetadata
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; }
    }
}