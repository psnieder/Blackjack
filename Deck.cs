using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    internal class Deck
    {
        private static Deck? instance;
        private Stack<Card> deck;

        /// <summary>
        /// Private Constructor
        /// Adds all cards to a deck and shuffles it
        /// </summary>
        private Deck()
        {
            this.deck = new Stack<Card>();
            foreach (Rank rank in Rank.GetValues(typeof(Rank)))
            {
                foreach (Suit suit in Suit.GetValues(typeof(Suit)))
                {
                    this.deck.Push(new Card(rank, suit));
                }
            }
            this.deck = Shuffle(this.deck);
        }

        /// <summary>
        /// Shuffles a deck
        /// </summary>
        /// <param name="deck">The deck to shuffle</param>
        /// <returns>The shuffled deck</returns>
        private static Stack<Card> Shuffle(Stack<Card> deck)
        {
            return new Stack<Card>(deck.OrderBy(x => new Random().Next()));
        }

        /// <summary>
        /// Gets the singleton deck instance
        /// </summary>
        /// <returns>The deck instance</returns>
        internal static Deck GetDeck()
        {
            if (instance == null) instance = new Deck();
            return instance;
        }

        /// <summary>
        /// Gets the number of cards in the deck
        /// </summary>
        /// <returns>The number of cards in the deck</returns>
        internal int CardsRemaining()
        {
            return this.deck.Count;
        }

        /// <summary>
        /// Deals a card
        /// </summary>
        /// <returns>The card dealt</returns>
        internal Card DealCard()
        {
            return this.deck.Pop();
        }

        /// <summary>
        /// Combines a hand with the deck and shuffles it
        /// </summary>
        /// <param name="hand">The hand to shuffle into the deck</param>
        internal void MoveHandToDeck(BlackjackHand hand)
        {
            int handSize = hand.HandSize();
            for (int i = 0; i < handSize; i++)
            {
                this.deck.Push(hand.RemoveCard());
            }
            this.deck = Shuffle(this.deck);
        }

        /// <summary>
        /// Returns the cards in the deck
        /// </summary>
        /// <returns>The cards in the deck</returns>
        public override String ToString()
        {
            String cardsInDeck = String.Empty;
            for (int i = 0; i < this.deck.Count; i++)
            {
                cardsInDeck += this.deck.ElementAt(i) + "\n";
            }
            return cardsInDeck.Trim();
        }
    }
}
