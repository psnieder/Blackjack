using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    /// <summary>
    /// Defines a card
    /// </summary>
    internal class Card
    {
        private Rank rank;
        private Suit suit;

        /// <summary>
        /// Card constructor
        /// </summary>
        /// <param name="rank">The rank of the card</param>
        /// <param name="suit">The suir of the card</param>
        internal Card(Rank rank, Suit suit)
        {
            this.rank = rank;
            this.suit = suit;
        }

        /// <summary>
        /// Get the rank of a card
        /// </summary>
        /// <returns>The rank of the card</returns>
        internal Rank GetRank()
        {
            return this.rank;
        }

        /// <summary>
        /// Get the suit of a card
        /// </summary>
        /// <returns>The suit of a card</returns>
        internal Suit GetSuit()
        {
            return this.suit;
        }

        /// <summary>
        /// Displays the card in a nice format
        /// </summary>
        /// <returns>The card in string format</returns>
        public override string ToString()
        {
            return RankExtensions.RankToString(this.rank) + " of " + SuitExtensions.SuitToString(this.suit);
        }
    }
}
