using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Lymer.ContentManagement.Markdown
{
    internal sealed class MarkdownContentProvider
    {
        private const string MetadataDelimiter = "---";
        
        private readonly IArticleRepository _articleRepository;

        public MarkdownContentProvider(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository ?? throw new ArgumentNullException(nameof(articleRepository));
        }

        public async Task<IReadOnlyCollection<Article>> GetArticlesAsync(CancellationToken cancellationToken)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            
            var result = new List<Article>();
            
            foreach (var handle in await _articleRepository.GetHandlesAsync(cancellationToken))
            {
                await using var stream = await _articleRepository.OpenByHandleAsync(handle, cancellationToken);
                
                using var streamReader = new StreamReader(stream);

                var metadataBuilder = new StringBuilder();

                var line = await streamReader.ReadLineAsync();
                
                if (line == MetadataDelimiter)
                {
                    while (true)
                    {
                        line = await streamReader.ReadLineAsync();

                        if (line == null)
                        {
                            throw new IOException("Got EOF, expected '---'.");
                        }

                        if (line == MetadataDelimiter)
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
}