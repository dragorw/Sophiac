using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using Microsoft.Maui.Storage;
using OllamaSharp;
using OpenAI;
using Sophiac.Domain.Chat;
using Sophiac.Domain.Settings;

namespace Sophiac.Infrastructure.Chat;

public class ChatProvider : IChatProvider
{
    private readonly ISettingsRepository _repository;

    public ChatProvider(ISettingsRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IChatClient> ProvideAsync()
    {
        var key = await _repository.GetByKeyAsync("llm_api_key");
        var model = await _repository.GetByKeyAsync("llm_model");
        var selectedProvider = await _repository.GetByKeyAsync("llm_api_provider");
        var url = await _repository.GetByKeyAsync("llm_api_url");

        return selectedProvider.Value switch
        {
            "Ollama" => new OllamaApiClient(new Uri(url.Value), model.Value),
            "OpenAI" => new OpenAIClient(key.Value).GetChatClient(model.Value).AsIChatClient(),
            _ => throw new InvalidOperationException("Unknown provider type!")
        };
    }
}