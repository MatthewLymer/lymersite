using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Lymer.ContentManagement.Markdown
{
    public interface IArticleRepository
    {
        Task<IReadOnlyCollection<string>> GetHandlesAsync(CancellationToken cancellationToken);
        Task<Stream> OpenByHandleAsync(string handle, CancellationToken cancellationToken);
    }
}