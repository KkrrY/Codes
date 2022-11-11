//TODO : TR-11 Kirill Schust

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


internal class Lab1 {
        
        public static void Main(string[] args)
        {
            IGameEngine player1 = new GameAccount("KI", 100);
            IGameEngine player2 = new GameAccount("NI", 100);
            
            DiceGame game = new GameSession();
            game.playDiceGame(player1, player2);
        }

        interface DiceGame
        {
            void playDiceGame(IGameEngine player1, IGameEngine player2);
        }
        interface IGameEngine {
            void winGame();
            void loseGame();
            void invokeDraw();
            void getStats();

            void decreaseCurrentRating();
            void increaseCurrentRating();
            void increaseGameCounter();

            string userName { get; }
            string opponentName { get; set; }
            string gameStatus { get; set; }
            int ratingValue { get; set; }
            int gamesCount { get; set; }
            int currentRating { get; set; }
            int diceValue { get; set; }
            
            List<GameHistory> playerData { get; }

        }
        
        class GameAccount : IGameEngine {
            public string userName { get; }
            public string opponentName { get; set; }
            public string gameStatus { get; set; }

            private int _currentRating;

            public int currentRating
            {
                get
                {
                    return _currentRating;
                }
                set
                {
                    _currentRating = (_currentRating < 0) ? throw  new ArgumentOutOfRangeException(nameof(_currentRating), "Wrong setter value") : value;
                }
            }


            public int gamesCount { get; set; }
            public int diceValue { get; set; }
            public int ratingValue { get; set; }
            
            private Random random = new Random();
            public List<GameHistory> playerData { get; set; }
            


            public void increaseCurrentRating() {
                ratingValue = random.Next(1, 10);
                currentRating+= ratingValue;
            }

            public void decreaseCurrentRating() {
                ratingValue = random.Next(1, 10);
                currentRating-= ratingValue;
            }
            
            public  void increaseGameCounter (){ gamesCount++;}
            

            public GameAccount(string userName, int currentRating) {
                this.userName = userName;
                gamesCount = 0;
                this.currentRating = currentRating;

                playerData = new List<GameHistory>();
                
            }
            public void winGame() {
                gameStatus = "Win" ;
                increaseCurrentRating();
                increaseGameCounter();
                if (currentRating <= 0) {currentRating = 1;}
                playerData.Add(new GameHistory(userName, opponentName, ratingValue, currentRating, gamesCount, gameStatus, GameSession.id));
            }

            public void loseGame() {
                gameStatus = "Lose" ;
                decreaseCurrentRating();
                increaseGameCounter();
                if (currentRating <= 0) {currentRating = 1;}
                playerData.Add(new GameHistory(userName, opponentName, ratingValue, currentRating, gamesCount, gameStatus, GameSession.id));
            }

            public void invokeDraw() {
                gameStatus = "Draw" ;
                ratingValue = 0;
                increaseGameCounter();
                if (currentRating <= 0) {currentRating = 1;}
                playerData.Add(new GameHistory(userName, opponentName, ratingValue, currentRating, gamesCount, gameStatus, GameSession.id));
            }

            public void getStats()
            {
                foreach (var data in playerData)
                {
                    Console.WriteLine("Game id:" + data.gameID);
                    Console.WriteLine("Player name: " + data.userName + " | "
                                      + " Game status: " + data.gameStatus + " | "
                                      + " Rating count (+/-): " + data.rating + " | "
                                      + " Total rating: " + data.wholeRating + " | "
                                      + " Played against: " + data.opponentName + " | "
                                      + " Total games: " + data.gamesCount);
                }
            }
            

            public static void printSessionLogs(IGameEngine player1, IGameEngine player2) 
            {
                Console.WriteLine("Player1: " + player1.userName + " VS Player 2: " + player2.userName);
                Console.WriteLine("Player's dice value is: "+ player1.diceValue +", " + player2.userName + " dice value is "+ player2.diceValue);

                if (player1.diceValue > player2.diceValue)
                {
                    Console.WriteLine("Player 1 : " + player1.userName + " won the game");
                    Console.WriteLine("Player's 1 rating increased by " + player1.ratingValue + " and equals : " + player1.currentRating);
                    Console.WriteLine("Player's 2 rating decreased by " + player2.ratingValue + " and equals : " + player2.currentRating);
                }
                else if (player1.diceValue < player2.diceValue)
                {
                    Console.WriteLine("Player 2 : " + player2.userName + " won the game");
                    Console.WriteLine("Player's 2 rating increased by " + player2.ratingValue + " and equals : " + player2.currentRating);
                    Console.WriteLine("Player's 1 rating decreased by " + player1.ratingValue + " and equals : " + player1.currentRating);
                }
                else
                {
                    Console.WriteLine("It's draw, dices are equal");
                }
            }
        }

        class GameSession : DiceGame
        {
            Random random = new Random();
            public static int id { get; private set; } 
            public void playDiceGame(IGameEngine player1, IGameEngine player2)
            {

                int value1 = random.Next(1, 6);
                int value2 = random.Next(1, 6);

                player1.diceValue = value1;
                player2.diceValue = value2;

                if (player1.diceValue > player2.diceValue) {
                    player1.winGame();
                    player2.loseGame();

                }
                else if (player1.diceValue == player2.diceValue) {
                    player1.invokeDraw();
                    player2.invokeDraw();

                }
                else {
                    player2.winGame();
                    player1.loseGame();

                }
                GameAccount.printSessionLogs(player1,player2);
                incId();

                assignLoop(player1,player2);
                //currentRating = (currentRating < 1) ? throw  new ArgumentOutOfRangeException(nameof(currentRating), "Wrong setter value") : value;
            }

            void incId() { id++;}
            void assignLoop(IGameEngine player1, IGameEngine player2) {
               
                while (true) {
                    Console.WriteLine("Do you want to play again? Enter : \" yes \" or \" no \" ");
                    Console.WriteLine("You can print matches information using \" print player1 / player2 \" command");
                    string name = Console.ReadLine();
                    switch (name)
                    {
                        case "yes": playDiceGame(player1, player2);
                            break;
                        case "no": Environment.Exit(0);
                            break;
                        case "print player1" :
                            player1.getStats();
                            break;
                        case "print player2" :
                            player2.getStats();
                            break;
                        default: Console.WriteLine("Enter the right command, please");
                            break;
                    }

                }
            }
        }

        class GameHistory
        {
            
            public string gameID { get; set; } = "2022";
            private string i;
            public string userName { get; }
            public string opponentName { get; }
            public int rating { get; }
            public int wholeRating { get; }
            public int gamesCount { get; }
            public string gameStatus { get; }

            public GameHistory(string userName, string opponentName, int obtainedRating, int wholeRating, int gamesCount, string gameStatus, int id)
            {
                this.userName = userName;
                this.opponentName = opponentName;
                rating = obtainedRating;
                this.wholeRating = wholeRating;
                this.gamesCount = gamesCount;
                this.gameStatus = gameStatus;
                
                i = id.ToString();
                gameID += i;
            }
            
            
            
        }
        
    }
    

