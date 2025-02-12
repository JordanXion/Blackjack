using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public enum Suit : byte { Hearts, Diamonds, Clubs, Spades }
    public enum Rank : byte { Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace }

    internal class Card
    {
        Rank _rank;
        Suit _suit;

        public Card(Rank rank, Suit suit)
        {
            _rank = rank;
            _suit = suit;
        }

        public Rank GetRank()
        {
            return _rank;
        }

        public string GetDisplayRank()
        {
            return _rank.ToString();
        }
        public Suit GetSuit()
        {
            return _suit;
        }
        public string GetDisplaySuit()
        {
            return _suit.ToString();
        }

        public string GetDisplayName()
        {
            return _rank.ToString() + " of " + _suit.ToString();
        }
    }
}
