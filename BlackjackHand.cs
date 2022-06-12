using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    internal class BlackjackHand
    {
        private readonly List<Card> hand;
        private Boolean bust;

        /// <summary>
        /// Default blackjack hand constructor
        /// </summary>
        internal BlackjackHand()
        {
            this.hand = new List<Card>();
            this.bust = false;
        }

        /// <summary>
        /// Blackjack hand constructor
        /// </summary>
        /// <param name="card1">The first card in the hand</param>
        /// <param name="card2">The second card in the hand</param>
        internal BlackjackHand(Card card1, Card card2)
        {
            this.hand = new List<Card>();
            this.hand.Add(card1);
            if (card2 != null)
            {
                this.hand.Add(card2);
            }
            this.bust = false;
        }

        /// <summary>
        /// Add a card to the hand (they hit)
        /// </summary>
        /// <param name="card">The card to add</param>
        internal void AddCard(Card card)
        {
            this.hand.Add(card);
        }

        /// <summary>
        /// Removes the last card in the hand
        /// </summary>
        /// <returns>The removed card</returns>
        internal Card RemoveCard()
        {
            Card removedCard = this.hand.ElementAt(this.hand.Count - 1);
            this.hand.RemoveAt(this.hand.Count - 1);
            return removedCard;
        }

        /// <summary>
        /// Splits a hand
        /// </summary>
        /// <returns>The new (second) hand</returns>
        internal BlackjackHand Split()
        {
            return new BlackjackHand(RemoveCard(), null);
        }

        /// <summary>
        /// Check if we can split the hand (hand needs to be 2 cards of the same rank)
        /// </summary>
        /// <returns>True if we can split the hand</returns>
        internal Boolean CanSplit()
        {
            if (this.hand.Count != 2)
            {
                return false;
            }
            else if (this.hand.ElementAt(0).GetRank() != this.hand.ElementAt(1).GetRank())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the current size of the hand
        /// </summary>
        /// <returns>The size of the hand</returns>
        internal int HandSize()
        {
            return this.hand.Count;
        }

        /// <summary>
        /// Busts the hand
        /// </summary>
        internal void BustHand()
        {
            this.bust = true;
        }

        /// <summary>
        /// Gets if the hand is busted or not
        /// </summary>
        /// <returns>True if the hand is busted</returns>
        internal Boolean CheckBust()
        {
            return this.bust;
        }

        /// <summary>
        /// Gets the value of a hand
        /// </summary>
        /// <returns>The value of the hand</returns>
        internal int HandValue()
        {
            int value = 0;
            int aces = 0;

            for (int i = 0; i < this.hand.Count; i++)
            {
                if (this.hand.ElementAt(i).GetRank() != Rank.ACE)
                {
                    value += RankExtensions.Value(this.hand.ElementAt(i).GetRank());
                }
                else
                {
                    aces++;
                }
            }
            for (int i = 0; i < aces; i++)
            {
                if (value > 10)
                {
                    value += RankExtensions.Value(Rank.ACE) - 10;
                }
                else
                {
                    value += RankExtensions.Value(Rank.ACE);
                }
            }

            return value;
        }

        /// <summary>
        /// Displays the cards in a hand
        /// </summary>
        /// <param name="hideSecondCard">If true, the second card will be hidden</param>
        /// <returns>The cards in the hand</returns>
        internal String DisplayHand(Boolean hideSecondCard)
        {
            String hand = String.Empty;

            for (int i = 0; i < this.hand.Count; i++)
            {
                if (i != 1)
                {
                    hand += this.hand.ElementAt(i) + "\n";
                }
                if (i == 1 && !hideSecondCard)
                {
                    hand += this.hand.ElementAt(i) + "\n";
                }
            }
            return hand.Trim();
        }
    }
}
