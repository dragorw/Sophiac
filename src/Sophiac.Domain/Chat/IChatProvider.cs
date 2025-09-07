using Microsoft.Extensions.AI;

namespace Sophiac.Domain.Chat;

public interface IChatProvider
{
    Task<IChatClient> ProvideAsync();
}