using Sophiac.Domain.Answers;
using Sophiac.Domain.Questions;

namespace Sophiac.Domain.UnitTests.Models
{
    public class SingleChoiceQuestionTests
    {
        [Fact]
        public void GetMaximumPoints_Returns_Maximum_From_Answers()
        {
            var question = new SingleChoiceQuestion
            {
                Answers = new List<SingleChoiceAnswer>
                {
                    new SingleChoiceAnswer { Points = 1 },
                    new SingleChoiceAnswer { Points = 6 },
                    new SingleChoiceAnswer { Points = 4 }
                }
            };

            int max = question.GetMaximumPoints();

            Assert.Equal(6, max);
        }

        [Fact]
        public void GetMaximumPoints_With_Single_Answer_Returns_Its_Points()
        {
            var question = new SingleChoiceQuestion
            {
                Answers = new List<SingleChoiceAnswer>
                {
                    new SingleChoiceAnswer { Points = 9 }
                }
            };

            int max = question.GetMaximumPoints();

            Assert.Equal(9, max);
        }

        [Fact]
        public void GetMaximumPoints_With_No_Answers_Throws()
        {
            var question = new SingleChoiceQuestion
            {
                Answers = new List<SingleChoiceAnswer>()
            };

            Assert.Throws<InvalidOperationException>(() => question.GetMaximumPoints());
        }
    }
}

