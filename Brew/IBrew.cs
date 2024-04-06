using System.Threading;
using System.Threading.Tasks;

namespace Brew;

public interface IBrew
{
    string? Description { get; }

    Task RunAsync(CancellationToken token);
}