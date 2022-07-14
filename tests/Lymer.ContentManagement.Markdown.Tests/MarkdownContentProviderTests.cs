using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            var fileProvider = new MemoryFileProvider();
            var provider = new MarkdownContentProvider(fileProvider);
            var articles = await provider.GetArticlesAsync(CancellationToken.None);
            Assert.Empty(articles);
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
---
Hello World";
            
            var fileProvider = new MemoryFileProvider();
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

        public async Task<Stream> OpenFileAsync(string name, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            var file = _files.Single(x => x.Name == name);

            return new MemoryStream(Encoding.UTF8.GetBytes(file.Content));
        }
    }

    internal interface IFileProvider
    {
        Task<IReadOnlyCollection<string>> GetFileNamesAsync(CancellationToken cancellationToken);
        Task<Stream> OpenFileAsync(string name, CancellationToken cancellationToken);
    }

    internal sealed class MarkdownContentProvider
    {
        private readonly IFileProvider _fileProvider;

        public MarkdownContentProvider(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        }

        public async Task<IReadOnlyCollection<Article>> GetArticlesAsync(CancellationToken cancellationToken)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            
            var result = new List<Article>();
            
            foreach (var fileName in await _fileProvider.GetFileNamesAsync(cancellationToken))
            {
                await using var stream = await _fileProvider.OpenFileAsync(fileName, cancellationToken);
                using var reader = new StreamReader(stream);

                var content = await reader.ReadToEndAsync();

                var matches = Regex.Matches(content, "---(.*)---", RegexOptions.Singleline);

                var metadata = deserializer.Deserialize<ArticleMetadata>(matches[0].Groups[1].Value);

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