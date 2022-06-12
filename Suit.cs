using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    /// <summary>
    /// Defines the suit of a card
    /// </summary>
    public enum Suit
    {
        DIAMONDS,
        HEARTS,
        SPADES,
        CLUBS
    }

    /// <summary>
    /// Extention methods for the suit of a card
    /// </summary>
    internal class SuitExtensions
    {
        /// <summary>
        /// Dispays the suit of a card in a nice format
        /// </summary>
        /// <param name="suit">The suit</param>
        /// <returns>The suit in string format</returns>
        internal static string SuitToString(Suit suit)
        {
            switch (suit)
            {
                case Suit.DIAMONDS: return "Diamonds";
                case Suit.HEARTS: return "Hearts";
                case Suit.SPADES: return "Spades";
                case Suit.CLUBS: return "Clubs";
                default: return String.Empty;
            }
        }
    }
}
