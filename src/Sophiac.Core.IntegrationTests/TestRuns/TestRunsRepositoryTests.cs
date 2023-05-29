using System;
using Sophiac.Core.Answers;
using Sophiac.Core.IntegrationTests.TestSets;
using Sophiac.Core.Questions;
using Sophiac.Core.TestRuns;
using Sophiac.Core.TestSets;

namespace Sophiac.Core.IntegrationTests.TestRuns
{
    public class TestRunsRepositoryTests
    {
        [Fact]
        public void CreateTestRun_ComplexPayload_Success()
        {
            // Arrange
            var path = Path.GetTempPath();
            path = Path.Combine(path, "Sophiac", "IntegrationTests");
            Directory.CreateDirectory(path);
            path = Path.Combine(path, nameof(TestSetRepositoryTests), ".json");
            var repository = new TestRunsRepository(path);
            var expected =
                new TestRun
                {
                    Title = "Test Run",
                    Time = DateTime.UtcNow,
                    Entries =
                        new List<TestRunEntry>
                        {
                            new TestRunEntry
                            {
                                Question =
                                    new SingleChoiceQuestion()
                                    {
                                        Title = "Single Choice Question",
                                        Description = "Single Choice Question Description",
                                        Answers =
                                            new List<AnswerBase>
                                            {
                                                new SingleChoiceAnswer
                                                {
                                                    Content = "Answer A",
                                                    Points = 10
                                                },
                                                new SingleChoiceAnswer
                                                {
                                                    Content = "Answer B",
                                                    Points = 0
                                                },
                                                new SingleChoiceAnswer
                                                {
                                                    Content = "Answer C",
                                                    Points = 0
                                                },
                                                new SingleChoiceAnswer
                                                {
                                                    Content = "Answer D",
                                                    Points = -10
                                                },
                                            }
                                    },
                                    AnswerSpan = TimeSpan.FromSeconds(1),
                                    Answer =
                                        new SingleChoiceAnswer
                                        {
                                            Content = "Answer A",
                                            Points = 10
                                        }
                            },
                            new TestRunEntry
                            {
                                Question =
                                    new MultipleChoicesQuestion()
                                    {
                                        Title = "Multiple Choices Question",
                                        Description = "Multiple Choices Question Description",
                                        Answers =
                                            new List<AnswerBase>
                                            {
                                                new MultipleChoicesAnswer
                                                {
                                                    Content = new List<AnswerOption>() { new AnswerOption { Content = "Answer A" }, new AnswerOption { Content = "Answer B" } },
                                                    Points = 10
                                                },
                                                new MultipleChoicesAnswer
                                                {
                                                    Content = new List<AnswerOption>() { new AnswerOption { Content = "Answer C" }, new AnswerOption { Content = "Answer D" } },
                                                    Points = 0
                                                }
                                            }
                                    },
                                    AnswerSpan = TimeSpan.FromSeconds(1),
                                    Answer =
                                        new MultipleChoicesAnswer
                                        {
                                            Content = new List<AnswerOption>() { new AnswerOption { Content = "Answer A" }, new AnswerOption { Content = "Answer B" } },
                                            Points = 10
                                        }
                            },
                            new TestRunEntry
                            {
                                Question =
                                    new MappingQuestion()
                                    {
                                        Title = "Mapping Question",
                                        Description = "Mapping Question Description",
                                        Answers =
                                            new List<AnswerBase>
                                            {
                                                new MappingAnswer
                                                {
                                                    Content =
                                                        new List<MappingAnswerOption>()
                                                        {
                                                            new MappingAnswerOption { Source = "Source A", Destination = "Destination A" },
                                                            new MappingAnswerOption { Source = "Source B", Destination = "Destination B" },
                                                            new MappingAnswerOption { Source = "Source C", Destination = "Destination C" },
                                                            new MappingAnswerOption { Source = "Source D", Destination = "Destination D" },
                                                        },
                                                    Points = 10
                                                }
                                            }
                                    },
                                AnswerSpan = TimeSpan.FromSeconds(1),
                                Answer =
                                    new MappingAnswer
                                    {
                                        Content =
                                               new List<MappingAnswerOption>()
                                                {
                                                    new MappingAnswerOption { Source = "Source A", Destination = "Destination A" },
                                                    new MappingAnswerOption { Source = "Source B", Destination = "Destination B" },
                                                    new MappingAnswerOption { Source = "Source C", Destination = "Destination C" },
                                                    new MappingAnswerOption { Source = "Source D", Destination = "Destination D" },
                                                },
                                        Points = 10
                                    }
                            }
                        }
                };

            // Act
            repository.CreateTestRun(expected);
            var actual = repository.ReadTestRun(expected.FileName);

            // Assert
            Assert.Equivalent(expected, actual);
        }
    }
}

