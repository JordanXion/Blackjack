using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    internal class Deck
    {
        Random _random;
        List<Card> _cards;

        public Deck(Random random)
        {
            _random = random;
            _cards = new List<Card>();
        }

        public void AddCard(Card card)
        {
            _cards.Add(card);
        }

        public Card GetCard(Int32 index)
        {
            return _cards[index];
        }

        public int CalculateScore()
        {
            int score = 0;
            int aceCount = 0;

            foreach (var card in _cards)
            {
                Rank rank = card.GetRank();

                if (rank >= Rank.Two && rank <= Rank.Ten)
                {
                    score += (int)rank;
                }
                else if (rank == Rank.Jack || rank == Rank.Queen || rank == Rank.King)
                {
                    score += 10;
                }
                else if (rank == Rank.Ace)
                {
                    aceCount++;
                    score += 11;
                }
            }

            // If the total score greater than 21, treat aces as 1s
            while (score > 21 && aceCount > 0)
            {
                score -= 10;
                aceCount--;
            }

            return score;
        }

        public int Count()
        {
            return _cards.Count;
        }

        public void Reset()
        {
            _cards.Clear();
        }

        public Card DrawCard()
        {
            Int32 index = _random.Next(_cards.Count);
            Card selected = _cards[index];
            _cards.RemoveAt(index);
            return selected;
        }

        public void Shuffle()
        {
            for (int i = _cards.Count - 1; i > 0; i--)
            {
                int index = _random.Next(i + 1);
                (_cards[i], _cards[index]) = (_cards[index], _cards[i]); 
            }
        }

        public void FillStandardDeck()
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    _cards.Add(new Card(rank, suit));
                }
            }
        }
    }
}
