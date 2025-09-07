namespace Sophiac.Domain.Generation;

public interface IGenerationService
{
    Task GenerateQuestionAsync(string testSetTitle, string textInput, CancellationToken cancellationToken);
}