using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoStore
{
    public class Customer
    {
        private readonly List<Rental> _rentals = new List<Rental>();
        public string Name { get; }

        public Customer(string name)
        {
            Name = name;
        }

        public void AddRental(Rental rental)
        {
            _rentals.Add(rental);
        }

        public string Statement()
        {           
            var result = "Rental Record for " + Name + "\n";  //change to string builder

            result += GenerateRentalStatement();

            // add footer lines
            result += $"Amount owed is " + $"{TotalCharge():F1}\n";
            result += $"You earned {this.FrequentRenterPoints()} frequent renter points";
            return result;
        }

        private string GenerateRentalStatement()
        {
            StringBuilder stringBuilder = new StringBuilder();

            _rentals.ForEach(r => stringBuilder.Append($"\t{r.Movie.Title}\t" + $"{AmountFor(r):F1}\n"));

            return stringBuilder.ToString();
        }

        private int FrequentRenterPoints()
        {            
            return _rentals.AsParallel().Sum(r => CalculateRenterPoints(r));            
        }

        private decimal TotalCharge()
        {
            return _rentals.AsParallel().Sum(rental => AmountFor(rental));
        }

        private static int CalculateRenterPoints(Rental rental)
        {
            int frequentRenterPoints = 1;
            // add frequent renter points
            // add bonus for a two day new release rental
            if ((rental.Movie.PriceCode == Movie.NewRelease) &&
                rental.DaysRented > 1)
                frequentRenterPoints++;
            return frequentRenterPoints;
        }

        private decimal AmountFor(Rental rental)
        {
            decimal thisAmount = 0;
            switch (rental.Movie.PriceCode)
            {
                case Movie.Regular:
                    thisAmount += 2;
                    if (rental.DaysRented > 2)
                        thisAmount += (rental.DaysRented - 2) * 1.5m;
                    break;
                case Movie.NewRelease:
                    thisAmount += rental.DaysRented * 3;
                    break;
                case Movie.Childrens:
                    thisAmount += 1.5m;
                    if (rental.DaysRented > 3)
                        thisAmount += (rental.DaysRented - 3) * 1.5m;
                    break;
                default:
                    break;
            }
            return thisAmount;
        }
    }
}