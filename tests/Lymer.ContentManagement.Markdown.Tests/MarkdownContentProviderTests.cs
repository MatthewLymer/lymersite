using System.IO;
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
            var fileProvider = new MemoryArticleRepository();
            var provider = new MarkdownContentProvider(fileProvider);
            var articles = await provider.GetArticlesAsync(CancellationToken.None);
            Assert.Empty(articles);
        }

        [Fact]
        public async Task ShouldGetExceptionWhenMetadataNotDelimited()
        {
            const string fileContent = "---";
            
            var fileProvider = new MemoryArticleRepository();
            
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
            var fileProvider = new MemoryArticleRepository();
            
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
            
            var fileProvider = new MemoryArticleRepository();
            fileProvider.AddFile("foo.md", fileContent);
            
            var provider = new MarkdownContentProvider(fileProvider);
            
            var articles = await provider.GetArticlesAsync(CancellationToken.None);
            
            Assert.Collection(
                articles, 
                x =>
                {
                    var metadata = x.Metadata;
                    Assert.NotNull(metadata);
                    Assert.Equal("This is my title", metadata.Title);
                    Assert.Equal("This is my description,\nisn't it great?\n", metadata.Description);
                    Assert.True(metadata.Tags.SequenceEqual(new[] {"tag1", "tag2", "tag3"}));
                });
        }
    }
}