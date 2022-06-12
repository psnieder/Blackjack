using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    /// <summary>
    /// Defines the rank of a card
    /// </summary>
    public enum Rank
    {
        TWO,
        THREE,
        FOUR,
        FIVE,
        SIX,
        SEVEN,
        EIGHT,
        NINE,
        TEN,
        JACK,
        QUEEN,
        KING,
        ACE
    }

    /// <summary>
    /// Extension methods for the rank of a card
    /// </summary>
    internal class RankExtensions
    {
        /// <summary>
        /// Returns the value of a rank
        /// </summary>
        /// <param name="rank">The rank</param>
        /// <returns>The value of the rank</returns>
        internal static int Value(Rank rank)
        {
            switch (rank)
            {
                case Rank.TWO: return 2;
                case Rank.THREE: return 3;
                case Rank.FOUR: return 4;
                case Rank.FIVE: return 5;
                case Rank.SIX: return 6;
                case Rank.SEVEN: return 7;
                case Rank.EIGHT: return 8;
                case Rank.NINE: return 9;
                case Rank.TEN: return 10;
                case Rank.JACK: return 10;
                case Rank.QUEEN: return 10;
                case Rank.KING: return 10;
                case Rank.ACE: return 11;
                default: return 0;
            }
        }

        /// <summary>
        /// Displays the rank of a card in a nice format
        /// </summary>
        /// <param name="rank">The rank</param>
        /// <returns>The rank in string format</returns>
        internal static string RankToString(Rank rank)
        {
            switch (rank)
            {
                case Rank.TWO: return "Two";
                case Rank.THREE: return "Three";
                case Rank.FOUR: return "Four";
                case Rank.FIVE: return "Five";
                case Rank.SIX: return "Six";
                case Rank.SEVEN: return "Seven";
                case Rank.EIGHT: return "Eight";
                case Rank.NINE: return "Nine";
                case Rank.TEN: return "Ten";
                case Rank.JACK: return "Jack";
                case Rank.QUEEN: return "Queen";
                case Rank.KING: return "King";
                case Rank.ACE: return "Ace";
                default: return "Invalid Rank";
            }
        }
    }
}
