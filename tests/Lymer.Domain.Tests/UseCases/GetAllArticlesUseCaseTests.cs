using System;
using System.Collections.Generic;
using Xunit;

namespace Lymer.Domain.Tests.UseCases
{
    public sealed class GetAllArticlesUseCaseTests
    {
        [Fact]
        public void ShouldReturnZeroArticlesIfNoArticlesExist()
        {
            var useCase = new GetAllArticlesUseCase();

            var articles = useCase.Execute();
            
            Assert.Empty(articles);
        }

        [Fact]
        public void ShouldReturnArticlesMetadata()
        {
            var useCase = new GetAllArticlesUseCase();

            var articles = useCase.Execute();

            var article = Assert.Single(articles);
            
            
        }
    }

    public class GetAllArticlesUseCase
    {
        public IEnumerable<object> Execute()
        {
            return Array.Empty<object>();
        }
    }
}