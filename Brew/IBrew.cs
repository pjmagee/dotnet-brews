using System.Threading.Tasks;

namespace Brew;

public interface IBrew
{
    async Task Execute()
    {
        await Before();
        await After();
    }

    Task Before()
    {
        return Task.CompletedTask;
    }

    Task After()
    {
        return Task.CompletedTask;
    }
}