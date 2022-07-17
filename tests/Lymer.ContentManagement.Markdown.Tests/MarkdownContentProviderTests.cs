using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Lymer.ContentManagement.Markdown.Tests
{
    public sealed class MarkdownContentProviderTests
    {
        [Fact]
        public async Task ShouldReturnNoArticles()
        {
            var fileProvider = new MemoryArticleProvider();
            var provider = new MarkdownContentProvider(fileProvider);
            var articles = await provider.GetArticlesAsync(CancellationToken.None);
            Assert.Empty(articles);
        }

        [Fact]
        public async Task ShouldGetExceptionWhenMetadataNotDelimited()
        {
            const string fileContent = "---";
            
            var fileProvider = new MemoryArticleProvider();
            
            fileProvider.AddFile("foo.md", fileContent);
            
            var provider = new MarkdownContentProvider(fileProvider);
            
            await Assert.ThrowsAsync<IOException>(() => provider.GetArticlesAsync(CancellationToken.None));
        }

        [Theory]
        [InlineData("")]
        [InlineData("\n---\ntitle: foo\n---")]
        [InlineData("hello world\n---\ntitle: foo\n---")]
        [InlineData(" ---\ntitle: foo\n---")]
        public async Task ShouldReturnArticleWithNoMetadata(string fileContent)
        {   
            var fileProvider = new MemoryArticleProvider();
            
            fileProvider.AddFile("foo.md", fileContent);
            
            var provider = new MarkdownContentProvider(fileProvider);
            
            var articles = await provider.GetArticlesAsync(CancellationToken.None);
            
            Assert.Collection(articles, x => Assert.Null(x.Metadata));
        }

        [Fact]
        public async Task ShouldReturnMetadataForArticles()
        {
            const string fileContent = @"---
title: This is my title
description: |
  This is my description,
  isn't it great?
tags:
  - tag1
  - tag2
  - tag3
---";
            
            var fileProvider = new MemoryArticleProvider();
            fileProvider.AddFile("foo.md", fileContent);
            
            var provider = new MarkdownContentProvider(fileProvider);
            
            var articles = await provider.GetArticlesAsync(CancellationToken.None);
            
            Assert.Collection(
                articles, 
                x =>
                {
                    var metadata = x.Metadata;
                    Assert.Equal("This is my title", metadata.Title);
                    Assert.Equal("This is my description,\nisn't it great?\n", metadata.Description);
                    Assert.True(metadata.Tags.SequenceEqual(new[] {"tag1", "tag2", "tag3"}));
                });
        }
    }

    internal sealed class MemoryArticleProvider : IArticleProvider
    {
        private readonly List<(string Handle, string Content)> _files = new List<(string Handle, string Content)>();
        
        public void AddFile(string handle, string content)
        {
            _files.Add((handle, content));
        }

        public async Task<IReadOnlyCollection<string>> GetHandlesAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return _files.Select(x => x.Handle).ToList().AsReadOnly();
        }

        public async Task<Stream> OpenByHandleAsync(string handle, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            var file = _files.Single(x => x.Handle == handle);

            return new MemoryStream(Encoding.UTF8.GetBytes(file.Content));
        }
    }

    internal interface IArticleProvider
    {
        Task<IReadOnlyCollection<string>> GetHandlesAsync(CancellationToken cancellationToken);
        Task<Stream> OpenByHandleAsync(string handle, CancellationToken cancellationToken);
    }

    internal sealed class MarkdownContentProvider
    {
        private readonly IArticleProvider _articleProvider;

        public MarkdownContentProvider(IArticleProvider articleProvider)
        {
            _articleProvider = articleProvider ?? throw new ArgumentNullException(nameof(articleProvider));
        }

        public async Task<IReadOnlyCollection<Article>> GetArticlesAsync(CancellationToken cancellationToken)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            
            var result = new List<Article>();
            
            foreach (var handle in await _articleProvider.GetHandlesAsync(cancellationToken))
            {
                await using var stream = await _articleProvider.OpenByHandleAsync(handle, cancellationToken);
                
                using var streamReader = new StreamReader(stream);

                var metadataBuilder = new StringBuilder();

                var line = await streamReader.ReadLineAsync();
                
                if (line == "---")
                {
                    while (true)
                    {
                        line = await streamReader.ReadLineAsync();

                        if (line == null)
                        {
                            throw new IOException("Got EOF, expected '---'.");
                        }

                        if (line == "---")
                        {
                            break;
                        }

                        metadataBuilder.AppendLine(line);
                    }
                }

                var metadata = deserializer.Deserialize<ArticleMetadata>(metadataBuilder.ToString());

                var article = new Article
                {
                    Metadata = metadata
                };
                
                result.Add(article);
            }

            return result.AsReadOnly();
        }
    }

    public sealed class Article
    {
        public ArticleMetadata Metadata { get; set; }
    }

    public sealed class ArticleMetadata
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; }
    }
}