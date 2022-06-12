using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Blackjack
{
    /// <summary>
    /// Blackjack game
    /// Single deck
    /// No double down or split after split
    /// Blackjack pays 3 to 2 and no insurance or even money
    /// Dealer hits soft 17
    /// </summary>
    public class BlackjackGame
    {
        /* Declare global variables */
        private static readonly List<BlackjackHand> playerHand = new List<BlackjackHand>();
        private static readonly BlackjackHand dealerHand = new BlackjackHand();
        private static readonly Deck deck = Deck.GetDeck();

        private static Boolean hit = false;
        private static Boolean doubleDown = false;
        private static Boolean split = false;
        private static Boolean splitAces = false;
        private static Boolean playGame = true;

        /// <summary>
        /// Main method to play a blackjack game
        /// <param name="args">Command line arguments</param>
        public static void Main(String[] args)
        {
            /* Declare local variables */
            Decimal playerMoney = Decimal.Zero;
            Decimal betSize = Decimal.Zero;
            Boolean blackjack = false;

            /* Welcome player and get starting money */
            playerMoney = GetPlayerInput("InitialMoney", false, false, playerMoney);

            /* Loop to play the game until player wants to stop or runs out of money */
            while (playGame)
            {
                /* Reset flags */
                blackjack = false;
                hit = false;
                doubleDown = false;
                split = false;
                splitAces = false;

                /* Get player starting bet */
                betSize = GetPlayerInput("InitialBet", false, false, playerMoney);

                /* Deal a hand */
                Console.Write("Dealing hand");
                Thread.Sleep(750);
                Console.Write(".");
                Thread.Sleep(750);
                Console.Write(".");
                Thread.Sleep(750);
                Console.WriteLine(".\n");
                Thread.Sleep(750);
                Card playerCard1 = deck.DealCard();
                Card dealerCard1 = deck.DealCard();
                Card playerCard2 = deck.DealCard();
                Card dealerCard2 = deck.DealCard();

                playerHand.Add(new BlackjackHand(playerCard1, playerCard2));
                dealerHand.AddCard(dealerCard1);
                dealerHand.AddCard(dealerCard2);

                /* Display starting hands */
                Console.WriteLine("Cards in hand:\n" + playerHand.ElementAt(0).DisplayHand(false) + "\n");
                Thread.Sleep(2000);
                Console.WriteLine("Dealer shows:\n" + dealerHand.DisplayHand(true) + "\n");
                Thread.Sleep(2000);

                /* Check for player blackjack */
                if (playerHand.ElementAt(0).HandValue() == 21)
                {
                    Console.WriteLine("Blackjack!\n");
                    Console.WriteLine("Dealer's Hand: \n" + dealerHand.DisplayHand(false) + "\n");

                    if (dealerHand.HandValue() == 21)
                    {
                        Console.WriteLine("Push.\n");
                    }
                    else
                    {
                        Console.WriteLine("You win the hand.\n");
                        playerMoney += Decimal.Multiply(betSize, Convert.ToDecimal(1.5));
                    }
                    blackjack = true;
                }

                /* Check for dealer blackjack */
                if (!blackjack && dealerHand.HandValue() == 21)
                {
                    Console.WriteLine("Dealer's hand:\n" + dealerHand.DisplayHand(false) + "\n");
                    Console.WriteLine("Dealer has blackjack. You lose the hand.\n");
                    playerMoney -= betSize;
                    blackjack = true;
                }

                /* Continue with rest of hand if player or dealer didn't have blackjack */
                if (!blackjack)
                {
                    /* Get initial player action */
                    GetPlayerInput("Action", true, playerHand.ElementAt(0).CanSplit(), playerMoney);

                    /* Player splits */
                    if (split)
                    {
                        Decimal tempBetSize = Decimal.Multiply(betSize, Convert.ToDecimal(2.0));
                        if (tempBetSize > playerMoney)
                        {
                            Console.WriteLine("You do not have enough money to split. Adding " + (tempBetSize - playerMoney).ToString("C", CultureInfo.CurrentCulture) + ".\n");
                            playerMoney = tempBetSize;
                        }

                        splitAces = (playerCard1.GetRank() == Rank.ACE && playerCard2.GetRank() == Rank.ACE);
                        playerHand.Add(playerHand.ElementAt(0).Split());
                        for (int i = 0; i < playerHand.Count; i++)
                        {
                            Console.WriteLine("Playing hand " + (i + 1) + ":");
                            Console.WriteLine("Cards in hand:\n" + playerHand.ElementAt(i).DisplayHand(false) + "\n");

                            /* Get player action */
                            GetPlayerInput("Action", false, false, playerMoney);
                            CheckHit(playerMoney, i);

                            if (playerHand.ElementAt(i).CheckBust())
                            {
                                Console.WriteLine("Your hand: " + playerHand.ElementAt(i).DisplayHand(false) + "\nTotal: " + playerHand.ElementAt(i).HandValue() + "\n");
                                Console.WriteLine("Bust!\n");
                            }
                        }
                    }

                    /* Player doubles down - deal one card */
                    if (doubleDown)
                    {
                        betSize = Decimal.Multiply(betSize, Convert.ToDecimal(2.0));
                        if (betSize > playerMoney)
                        {
                            Console.WriteLine("You do not have enough money to double down. Adding " + (betSize - playerMoney).ToString("C", CultureInfo.CurrentCulture) + ".\n");
                            playerMoney = betSize;
                        }

                        playerHand.ElementAt(0).AddCard(deck.DealCard());

                        if (playerHand.ElementAt(0).HandValue() > 21)
                        {
                            playerHand.ElementAt(0).BustHand();
                        }
                    }

                    /* Keep dealing one card until player stops hitting or busts */
                    CheckHit(playerMoney, 0);

                    /* Check results and play dealers hand (if necessary) */
                    Boolean allHandsBusted = true;
                    for (int i = 0; i < playerHand.Count; i++)
                    {
                        if (!playerHand.ElementAt(i).CheckBust())
                        {
                            allHandsBusted = false;
                            break;
                        }
                    }

                    if (allHandsBusted)
                    {
                        for (int i = 0; i < playerHand.Count; i++)
                        {
                            String handVerbiage = "Your hand:\n";
                            if (split)
                            {
                                handVerbiage = "Hand " + (i + 1) + ":\n";
                            }
                            Console.WriteLine(handVerbiage + playerHand.ElementAt(i).DisplayHand(false) + "\nTotal: " + playerHand.ElementAt(i).HandValue() + "\n");;
                            playerMoney -= betSize;
                        }
                        Console.WriteLine("Bust! You lose!\n");
                    }
                    else //play dealers hand and then show finishing hands
                    {
                        Console.WriteLine("Dealer's hand:\n" + dealerHand.DisplayHand(false) + "\n");
                        Thread.Sleep(2000);

                        while (dealerHand.HandValue() < 17 || (dealerHand.HandSize() == 2 && dealerHand.HandValue() == 17 && (dealerCard1.GetRank() == Rank.ACE || dealerCard2.GetRank() == Rank.ACE)))
                        { //keep hitting until 17 or higher. Dealer hits soft 17.
                            Card dealerHitCard = deck.DealCard();
                            Console.WriteLine("Dealer hits a " + dealerHitCard + "\n");
                            dealerHand.AddCard(dealerHitCard);
                            Thread.Sleep(2000);
                        }

                        for (int i = 0; i < playerHand.Count; i++)
                        {
                            String handVerbiage = "Your hand:\n";
                            if (split)
                            {
                                handVerbiage = "Hand " + (i + 1) + ":\n";
                            }
                            Console.WriteLine(handVerbiage + playerHand.ElementAt(i).DisplayHand(false) + "\nTotal: " + playerHand.ElementAt(i).HandValue() + "\n");
                        }

                        Console.WriteLine("Dealer's hand:\n" + dealerHand.DisplayHand(false) + "\nTotal: " + dealerHand.HandValue() + "\n");

                        if (dealerHand.HandValue() > 21) //dealer busts
                        {
                            Console.WriteLine("Dealer busts!\n");
                            for (int i = 0; i < playerHand.Count; i++)
                            {
                                if (split)
                                {
                                    Console.WriteLine("Hand " + (i + 1) + ":");
                                }
                                if (!playerHand.ElementAt(i).CheckBust())
                                {
                                    Console.WriteLine("You win!\n");
                                    playerMoney += betSize;
                                }
                                else
                                {
                                    Console.WriteLine("Bust! You lose!\n");
                                    playerMoney -= betSize;
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < playerHand.Count; i++)
                            {
                                if (split)
                                {
                                    Console.WriteLine("Hand " + (i + 1) + ":");
                                }
                                if (playerHand.ElementAt(i).CheckBust())
                                { //hand busted
                                    Console.WriteLine("Bust! You lose!\n");
                                    playerMoney -= betSize;
                                }
                                else if (playerHand.ElementAt(i).HandValue() > dealerHand.HandValue())
                                { //player wins
                                    Console.WriteLine("You win!\n");
                                    playerMoney += betSize;
                                }
                                else if (playerHand.ElementAt(i).HandValue() == dealerHand.HandValue())
                                { //push
                                    Console.WriteLine("Push.\n");
                                }
                                else
                                { //dealer wins
                                    Console.WriteLine("You lose!\n");
                                    playerMoney -= betSize;
                                }
                            }
                        }
                    }
                }

                /* Player has no money left */
                if (playerMoney <= 0)
                {
                    Console.WriteLine("You have no money left!");
                    playGame = false;
                    Thread.Sleep(5000);
                }
                else
                {
                    /* See if the player wants to keep playing or not */
                    GetPlayerInput("KeepPlaying", false, false, playerMoney);
                }

                /* Shuffle the hands back into the deck */
                for (int i = 0; i < playerHand.Count; i++)
                {
                    deck.MoveHandToDeck(playerHand.ElementAt(i));
                }
                playerHand.Clear();
                deck.MoveHandToDeck(dealerHand);
                Console.Clear();
            }

            /* Display money the player left with */
            Console.WriteLine("You finished playing with " + playerMoney.ToString("C", CultureInfo.CurrentCulture) + " left.");
            Environment.Exit(0);
        } //end Main

        /// <summary>
        /// Gets the players input for a variety of actions
        /// </summary>
        /// <param name="inputType">Type of input we are getting from the player</param>
        /// <param name="canDoubleDown">True if doubling down is a valid option</param>
        /// <param name="canSplit">True is splitting is a valid option</param>
        /// <param name="playerMoney">The players current money</param>
        /// <returns>Either initial player money or bet size, depending on the input type</returns>
        private static Decimal GetPlayerInput(String inputType, Boolean canDoubleDown, Boolean canSplit, Decimal playerMoney)
        {
            Decimal returnMoney = Decimal.Zero;
            int playerInput = 0;
            String input = String.Empty;

            const String standOption = "(1) Stand";
            const String hitOption = ", (2) Hit";
            const String doubleOption = ", (3) Double Down";
            const String splitOption = ", (4) Split";

            String playerOptions = standOption + hitOption;

            if (canDoubleDown)
            {
                playerOptions = playerOptions + doubleOption;
            }

            if (canSplit)
            {
                playerOptions = playerOptions + splitOption;
            }

            switch (inputType)
            {
                case "InitialMoney":
                    Console.WriteLine("Welcome to blackjack! How much money are you going to be starting with?");

                    while (returnMoney <= Decimal.Zero)
                    {
                        input = Console.ReadLine();

                        if (!Decimal.TryParse(input, out returnMoney))
                        {
                            Console.WriteLine("Please enter an amount of money only (no $):");
                        }
                        else if (returnMoney <= Decimal.Zero)
                        {
                            Console.WriteLine("Initial buy in must be greater than 0. Please enter a valid amount:");
                        }
                    }

                    break;

                case "InitialBet":
                    Console.WriteLine("You have " + playerMoney.ToString("C", CultureInfo.CurrentCulture) + ".");
                    Console.WriteLine("How much do you want to bet?");

                    while (returnMoney <= Decimal.Zero || returnMoney > playerMoney)
                    {
                        input = Console.ReadLine();

                        if (!Decimal.TryParse(input, out returnMoney))
                        {
                            Console.WriteLine("Please enter a valid bet size (no $):");
                        }
                        else if (returnMoney <= Decimal.Zero || returnMoney > playerMoney)
                        {
                            Console.WriteLine("Bet size must be greater than 0 and cannot be more than you have. Please enter a valid bet size:");
                        }
                    }

                    break;

                case "Action":
                    Console.WriteLine("What would you like to do?\n" + playerOptions);

                    while (playerInput == 0)
                    {
                        input = Console.ReadLine();

                        if (int.TryParse(input, out playerInput))
                        {
                            if (playerInput == 1) //stand
                            {
                                //do nothing
                            }
                            else if (playerInput == 2) //hit
                            {
                                hit = true;
                            }
                            else if (playerInput == 3 && canDoubleDown) //double down
                            {
                                doubleDown = true;
                            }
                            else if (playerInput == 4 && canSplit) //split
                            {
                                split = true;
                            }
                            else
                            {
                                Console.WriteLine("Please enter a valid option:\n" + playerOptions);
                                playerInput = 0;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Please enter a valid option:\n" + playerOptions);
                        }
                    }

                    break;

                case "KeepPlaying":
                    Console.WriteLine("You have " + playerMoney.ToString("C", CultureInfo.CurrentCulture) + ".\n");
                    Console.WriteLine("Would you like to play another hand?\n(1) Yes, (2) No");

                    while (playerInput == 0)
                    {
                        input = Console.ReadLine();

                        if (int.TryParse(input, out playerInput))
                        {
                            if (playerInput == 1)
                            {
                                //do nothing
                            } 
                            else if (playerInput == 2)
                            {
                                playGame = false;
                            }
                            else
                            {
                                Console.WriteLine("Please enter a valid option:\n(1) Yes, (2) No");
                                playerInput = 0;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Please enter a valid option:\n(1) Yes, (2) No");
                        }
                    }

                    break;

                default: break;
            }

            return Math.Round(returnMoney, 2);
        } //end GetPlayerInput

        /// <summary>
        /// Hits a card if a player wants to
        /// </summary>
        /// <param name="playerMoney">the player's current money</param>
        /// <param name="handNumber">the hand number we are playing, in the case the player has split</param>
        private static void CheckHit(Decimal playerMoney, int handNumber)
        {
            while (hit)
            {
                hit = false;
                Card hitCard = deck.DealCard();
                Console.WriteLine("You hit a " + hitCard + "\n");
                playerHand.ElementAt(handNumber).AddCard(hitCard);

                if (playerHand.ElementAt(handNumber).HandValue() > 21)
                {
                    playerHand.ElementAt(handNumber).BustHand();
                    break;
                }

                if (!splitAces)
                {
                    Console.WriteLine("Your hand:\n" + playerHand.ElementAt(handNumber).DisplayHand(false) + "\nTotal: " + playerHand.ElementAt(handNumber).HandValue() + "\n");
                    Console.WriteLine("Dealer shows:\n" + dealerHand.DisplayHand(true) + "\n");
                    GetPlayerInput("Action", false, false, playerMoney);
                }
            }
        } //end CheckHit
    } //end class
} //end namespace
