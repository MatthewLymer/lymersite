using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lymer.ContentManagement.Markdown.Tests
{
    internal sealed class MemoryArticleRepository : IArticleRepository
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
}