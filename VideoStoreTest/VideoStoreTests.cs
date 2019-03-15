using NUnit.Framework;
using VideoStore;

namespace VideoStoreSpec
{
    [TestFixture]
    public class VideoStoreSpec
    {
        private Customer _customer;
        private readonly Movie _newRelease1 = new Movie("New Release 1", Movie.NewRelease);
        private readonly Movie _newRelease2 = new Movie("New Release 2", Movie.NewRelease);
        private readonly Movie _childrens = new Movie("Childrens", Movie.Childrens);
        private readonly Movie _regular1 = new Movie("Regular 1", Movie.Regular);
        private readonly Movie _regular2 = new Movie("Regular 2", Movie.Regular);
        private readonly Movie _regular3 = new Movie("Regular 3", Movie.Regular);

        [SetUp]
        public void Init()
        {
            _customer = new Customer("Customer Name");
        }

        private void AssertAmountAndPointsForStatement(double expectedAmount, int expectedPoints)
        {
            var statement = _customer.Statement();
            Assert.True(statement.Contains($"Amount owed is {expectedAmount:F1}"));
            Assert.True(statement.Contains($"You earned {expectedPoints} frequent renter points"));
        }

        [Test]
        public void TestSingleNewReleaseStatement()
        {
            _customer.AddRental(new Rental(_newRelease1, 3));
            AssertAmountAndPointsForStatement(9.0, 2);
        }

        [Test]
        public void TestDualNewReleaseStatement()
        {
            _customer.AddRental(new Rental(_newRelease1, 3));
            _customer.AddRental(new Rental(_newRelease2, 3));
            AssertAmountAndPointsForStatement(18.0, 4);
        }

        [Test]
        public void TestSingleChildrensStatement()
        {
            _customer.AddRental(new Rental(_childrens, 3));
            AssertAmountAndPointsForStatement(1.5, 1);
        }

        [Test]
        public void TestMultipleRegularStatement()
        {
            _customer.AddRental(new Rental(_regular1, 1));
            _customer.AddRental(new Rental(_regular2, 2));
            _customer.AddRental(new Rental(_regular3, 3));
            AssertAmountAndPointsForStatement(7.5, 3);
        }

        [Test]
        public void TestRentalStatementFormat()
        {
            _customer.AddRental(new Rental(_regular1, 1));
            _customer.AddRental(new Rental(_regular2, 2));
            _customer.AddRental(new Rental(_regular3, 3));

            const string expectedStatement = "Rental Record for Customer Name\n" +
                                             "\tRegular 1\t2.0\n" +
                                             "\tRegular 2\t2.0\n" +
                                             "\tRegular 3\t3.5\n" +
                                             "Amount owed is 7.5\n" +
                                             "You earned 3 frequent renter points";
            Assert.True(_customer.Statement().Equals(expectedStatement));
        }
    }
}
