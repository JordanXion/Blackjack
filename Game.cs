using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    internal class Game
    {
        enum State { Exit, Entry, StartGame, PlayerTurn, DealerTurn }

        State _state;
        Deck _deck;
        Deck _player;
        Deck _dealer;

        Random _random;
        bool _debug = false;
        int _scorePlayer;
        int _scoreDealer;
        int _winsPlayer;
        int _winsDealer;
        bool _standingPlayer;
        bool _standingDealer;

        public Game()
        {
            _state = State.Entry;
            _random =  new Random();
            _deck = new Deck(_random);
            _player = new Deck(_random);
            _dealer = new Deck(_random);
            _winsPlayer = 0;
            _winsDealer = 0;
        }

        public bool Loop()
        {
            switch (_state)
            {
                case State.Exit:
                    return false;
                case State.Entry:
                    DisplayMainMenu();
                    break;
                case State.StartGame:
                    StartGame();
                    break;
                case State.PlayerTurn:
                case State.DealerTurn:
                    DoGameTurn();
                    break;
            }
            return true;
        }

        void PrintDeck(Deck deck)
        {
            if (deck == null || deck.Count() < 1) { return; }
            for (int i = 0; i < deck.Count(); i++)
            {
                Console.WriteLine(deck.GetCard(i).GetDisplayName());
            }
        }

        void StartGame()
        {
            _deck.Reset();
            _deck.FillStandardDeck();
            _player.Reset();
            _dealer.Reset();
            _standingDealer = false;
            _standingPlayer = false;

            _player.AddCard(_deck.DrawCard());
            _dealer.AddCard(_deck.DrawCard());
            _player.AddCard(_deck.DrawCard());
            _dealer.AddCard(_deck.DrawCard());

            _state = State.PlayerTurn;
        }

        void DisplayDebugGameState()
        {
            Console.WriteLine("DEBUG Dealer Hand: Score - " + _scoreDealer);
            PrintDeck(_dealer);
            Menu.DrawLine();
        }

        void DoGameTurn()
        {
            _scorePlayer = _player.CalculateScore();
            _scoreDealer = _dealer.CalculateScore();
            if (_scorePlayer >= 21 || _scoreDealer >= 21 || (_standingPlayer && _standingDealer))
            {
                EndGame();
                return;
            }

            if (_state == State.PlayerTurn)
            {
                DoPlayerTurn();
                _state = State.DealerTurn;
            }
            else
            {
                DoDealerTurn();
                _state = State.PlayerTurn;
            }

        }

        void DoDealerTurn()
        {
            if (_standingDealer) { return; }

            if (_scoreDealer < 17)
            {
                _dealer.AddCard(_deck.DrawCard());
            }
            else
            {
                _standingDealer = true;
            }
        }

        void DoPlayerTurn()
        {
            if (_standingPlayer) { return; }
            int choice = 0;

            while (true)
            {
                Console.Clear();

               
                Console.WriteLine($"Player Wins: {_winsPlayer} | Dealer Wins: {_winsDealer}");
                Menu.DrawLine();

                if (_debug)
                {
                    DisplayDebugGameState();
                }

                Console.WriteLine("Dealer's Hand");
                Console.WriteLine("????? of ?????");
                for (int i = 1; i < _dealer.Count(); i++)
                {
                    Console.WriteLine(_dealer.GetCard(i).GetDisplayName());
                }
                Menu.DrawLine();
                Console.WriteLine("Your Hand - Total: " + _scorePlayer);
                for (int i = 0; i < _player.Count(); i++)
                {
                    Console.WriteLine(_player.GetCard(i).GetDisplayName());
                }
                Menu.DrawLine();

                Console.WriteLine("\n\nYour turn - What will you do?");
                Console.WriteLine("1) Hit");
                Console.WriteLine("2) Stand");

                Console.Write("Choose an option: ");

                string? input = Console.ReadLine();

                if (input == "debug" || input == "dbg")
                {
                    _debug = !_debug;
                }
                else if (Int32.TryParse(input, out choice) && choice >= 1 && choice <= 2)
                {
                    break;
                }
                else
                {
                    Console.Write("Invalid choice, please try again.");
                    Console.ReadLine();
                }
            }

            switch (choice)
            {
                case 1:
                    _player.AddCard(_deck.DrawCard());
                    break;
                case 2:
                    _standingPlayer = true;
                    break;
            }
        }

        void EndGame()
        {
            int choice = 0;

            while (true)
            {
                Console.Clear();
                DisplayResult();
                Console.WriteLine("\n\nThanks for playing!");
                Console.WriteLine("1) Play again");
                Console.WriteLine("2) Main Menu");
                Console.WriteLine("3) Exit game");

                Console.Write("Choose an option: ");

                if (Int32.TryParse(Console.ReadLine(), out choice) && choice >= 1 && choice <= 3)
                {
                    break;
                }
                else
                {
                    Console.Write("Invalid choice, please try again.");
                    Console.ReadLine();
                }
            }

            switch (choice)
            {
                case 1:
                    _state = State.StartGame;
                    break;
                case 2:
                    _state = State.Entry;
                    break;
                case 3:
                    _state = State.Exit;
                    break;
            }

        }
        void DisplayResult()
        {
            Menu.DrawLine();
            Console.WriteLine("Dealer's Hand - Total: " + _scoreDealer);
            for (int i = 0; i < _dealer.Count(); i++)
            {
                Console.WriteLine(_dealer.GetCard(i).GetDisplayName());
            }
            Menu.DrawLine();
            Console.WriteLine("Your Hand - Total: " + _scorePlayer);
            for (int i = 0; i < _player.Count(); i++)
            {
                Console.WriteLine(_player.GetCard(i).GetDisplayName());
            }
            Menu.DrawLine();

            if (_scorePlayer == 21 && _scoreDealer == 21)
            {
                Console.WriteLine("Both have Blackjack! It's a tie.");
                _winsPlayer++;
                _winsDealer++;
                return;
            }
            if (_scorePlayer > 21)
            {
                Console.WriteLine("Player busts! Dealer wins.");
                _winsDealer++;
                return;
            }
            if (_scoreDealer > 21)
            {
                Console.WriteLine("Dealer busts! Player wins.");
                _winsPlayer++;
                return;
            }
            if (_scorePlayer == 21)
            {
                Console.WriteLine("Player has Blackjack! Player wins.");
                _winsPlayer++;
                return;
            }
            if (_scoreDealer == 21)
            {
                Console.WriteLine("Dealer has Blackjack! Dealer wins.");
                _winsDealer++;
                return;
            }

            if (_scorePlayer > _scoreDealer)
            {
                Console.WriteLine("Player wins with a higher score.");
                _winsPlayer++;
            }
            else if (_scorePlayer < _scoreDealer)
            {
                Console.WriteLine("Dealer wins with a higher score.");
                _winsDealer++;
            }
            else
            {
                Console.WriteLine("It's a tie!");
                _winsPlayer++;
                _winsDealer++;
            }
        }

        void DisplayMainMenu()
        {
            int choice = 0;

            while (true)
            {
                Console.Clear();

                Console.WriteLine("Welcome to Blackjack!");
                Console.WriteLine("1) Play game");
                Console.WriteLine("2) Test shuffling");
                Console.WriteLine("3) Exit game");

                Console.Write("Choose an option: ");

                if (Int32.TryParse(Console.ReadLine(), out choice) && choice >= 1 && choice <= 3)
                {
                    break;
                }
                else
                {
                    Console.Write("Invalid choice, please try again.");
                    Console.ReadLine();
                }
            }

            switch (choice)
            {
                case 1:
                    _state = State.StartGame;
                    break;
                case 2:
                    DisplayShuffleTest();
                    break;
                case 3:
                    _state = State.Exit;
                    break;
            }
        }

        void DisplayShuffleTest()
        {
            _deck.Reset();
            _deck.FillStandardDeck();

            Console.Clear();
            Console.WriteLine("Shuffle Test - Before");
            PrintDeck(_deck);
            Console.Write("\nPress Enter to Shuffle Deck");
            Console.ReadLine();

            _deck.Shuffle();

            Console.WriteLine("\nShuffle Test - After");
            PrintDeck(_deck);
            Console.Write("\nPress Enter to Return to Main Menu");
            Console.ReadLine();

            _deck.Reset();
        }
    }
}
