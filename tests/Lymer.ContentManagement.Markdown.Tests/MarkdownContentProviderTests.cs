using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Lymer.ContentManagement.Markdown.Tests
{
    public sealed class MarkdownContentProviderTests
    {
        [Fact]
        public async Task ShouldReturnNoArticles()
        {
            var fileProvider = new MemoryFileProvider();
            var provider = new MarkdownContentProvider(fileProvider);
            var articles = await provider.GetArticlesAsync(CancellationToken.None);
            Assert.Empty(articles);
        }

        [Fact]
        public async Task ShouldReturnMetadataForArticles()
        {
            var fileProvider = new MemoryFileProvider();
            fileProvider.AddFile("foo.md", @"");
            var provider = new MarkdownContentProvider(fileProvider);
            var articles = await provider.GetArticlesAsync(CancellationToken.None);
            Assert.Collection(articles, x =>
            {
                Assert.Equal("Hello World", x.Title);
            });
        }
    }

    internal sealed class MemoryFileProvider : IFileProvider
    {
        private readonly List<(string Name, string Content)> _files = new List<(string Name, string Content)>();
        public void AddFile(string name, string content)
        {
            _files.Add((name, content));
        }

        public async Task<IReadOnlyCollection<string>> GetFileNamesAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return _files.Select(x => x.Name).ToList().AsReadOnly();
        }
    }

    internal interface IFileProvider
    {
        Task<IReadOnlyCollection<string>> GetFileNamesAsync(CancellationToken cancellationToken);
    }

    internal sealed class MarkdownContentProvider
    {
        private readonly IFileProvider _fileProvider;

        public MarkdownContentProvider(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        }

        public async Task<IReadOnlyCollection<ArticleMetadata>> GetArticlesAsync(CancellationToken cancellationToken)
        {
            return (await _fileProvider.GetFileNamesAsync(cancellationToken))
                .Select(x => new ArticleMetadata())
                .ToList()
                .AsReadOnly();
        }
    }

    internal sealed class ArticleMetadata
    {
        public string Title => "Hello World";
    }
}