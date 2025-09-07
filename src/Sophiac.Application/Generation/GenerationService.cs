using Microsoft.Extensions.AI;
using Sophiac.Application.Questions;
using Sophiac.Domain.Chat;
using Sophiac.Domain.Generation;

namespace Sophiac.Application.Generation;

public class GenerationService : IGenerationService
{
    private readonly IChatProvider _chatProvider;
    private readonly QuestionsTools _tools;
    public GenerationService(IChatProvider chatProvider, QuestionsTools tools)
    {
        _chatProvider = chatProvider ?? throw new ArgumentNullException(nameof(chatProvider));
        _tools = tools ?? throw new ArgumentNullException(nameof(tools));
    }

    public async Task GenerateQuestionAsync(string testSetTitle, string textInput, CancellationToken token)
    {
        var client = await _chatProvider.ProvideAsync();
        client =
            new ChatClientBuilder(client)
                .UseFunctionInvocation()
                .Build();
        // Split the input into windows of 50 sentences each
        var sentences = textInput.Split(new[] { '.', '!', '?', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        var windowSize = 50;
        var windows = sentences
            .Select((sentence, index) => new { Sentence = sentence, Index = index })
            .GroupBy(x => x.Index / windowSize)
            .Select(g => string.Join(". ", g.Select(x => x.Sentence).ToArray()) + ".")
            .ToList();

        List<ChatMessage> conversation = new();

        var systemPrompt = "You are an AI agent generating questions from a given text using MCP tools available to you. The questions should be of three types: Single Choice, Multiple Choice, and Mapping Question. Please list existing questions for a given test set and introduce questions of each type by calling corresponding MCP tool, but MAKE SURE TO NOT DO ANYTHING IF TEXT IS NOT UNDERSTANDABLE.";
        ChatMessage message = new(ChatRole.System, systemPrompt);
        conversation.Add(message);
        // Iterate through windows and ask LLM to generate questions
        foreach (var window in windows)
        {
            var userPrompt = $"Please read existing questions from test set `{testSetTitle}` and introduce questions based on following chunk of text: ```text\n{window}\n```";
            message = new(ChatRole.User, userPrompt);
            conversation.Add(message);
            ChatOptions options = new()
            {
                Tools =
                [
                    AIFunctionFactory.Create(_tools.ListQuestionsInTestSet),
                    AIFunctionFactory.Create(_tools.IntroduceSingleChoiceQuestion),
                    AIFunctionFactory.Create(_tools.IntroduceMultipleChoiceQuestion),
                    AIFunctionFactory.Create(_tools.IntroduceMappingQuestion),
                ]
            };
            var response = await client.GetResponseAsync(conversation, options);
            message = new ChatMessage(ChatRole.Assistant, response.Text);
            conversation.Add(message);

            if (token.IsCancellationRequested)
                break;
        }
    }
}