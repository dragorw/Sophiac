using System;
using Sophiac.Core.Answers;
using Sophiac.Core.Questions;
using Sophiac.Core.TestSets;

namespace Sophiac.Core.IntegrationTests.TestSets
{
	public class TestSetRepositoryTests
	{
		[Fact]
		public void CreateTestSet_ComplexPayload_Success()
		{
			// Arrange
			var path = Path.GetTempPath();
			path = Path.Combine(path, "Sophiac", "IntegrationTests");
			Directory.CreateDirectory(path);
			path = Path.Combine(path, nameof(TestSetRepositoryTests), ".json");
			var repository = new TestSetsRepository(path);

			var expected =
				new TestSet
				{
					Title = nameof(CreateTestSet_ComplexPayload_Success),
					Strategy = QuestionDeliveryStrategies.SequentialDelivery,
					Questions =
						new List<QuestionBase>()
						{
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
                            }
						}
				};

			// Act
			repository.CreateTestSet(expected);
			var actual = repository.ReadTestSet(expected.FileName);

			// Assert
			Assert.Equivalent(expected, actual);
		}
	}
}

