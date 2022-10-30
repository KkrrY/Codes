//TODO : TR-11 Kirill Schust

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


internal class Lab1 {
        
        public static void Main(string[] args)
        {
            IGameEngine player1 = new GameAccount("KI");
            IGameEngine player2 = new GameAccount("NI");
            
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
            int gamesCount { get; }
            int currentRating { get; set; }
            int diceValue { get; set; }
            
            List<GameHistory> playerData { get; }

        }
        
        class GameAccount : IGameEngine {
            public string userName { get; }
            public string opponentName { get; set; }
            public string gameStatus { get; set; }
            public int currentRating { get; set; }
            public int gamesCount { get; private set; }
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
            

            public GameAccount(string userName) {
                this.userName = userName;
                currentRating = 1;
                gamesCount = 0;

                playerData = new List<GameHistory>();
                
            }
            public void winGame() {
                gameStatus = "Win" ;
                increaseCurrentRating();
                increaseGameCounter();
            }

            public void loseGame() {
                gameStatus = "Lose" ;
                decreaseCurrentRating();
                increaseGameCounter();
            }

            public void invokeDraw() {
                gameStatus = "Draw" ;
                increaseGameCounter();
            }

            public void getStats()
            {
                foreach (var data in playerData)
                {
                    Console.WriteLine("Game id:" + data.gameID);
                    Console.WriteLine("Player name: " + data.userName + " | "
                                      + " Game status: " + data.gameStatus + " | "
                                      + " Rating count (+/-): " + data.rating + " | "
                                      + " Played against: " + data.opponentName + " | "
                                      + " Total games: " + data.gamesCount);
                }
            }
            

            public static void printSessionLogs(IGameEngine player1, IGameEngine player2) 
            {
                Console.WriteLine("Player1: " + player1.userName + " VS Player 2: " + player2.userName);
                Console.WriteLine("Player's dice value is: "+ player1.diceValue +", " + player2.userName + " dice value is "+ player2.diceValue);
                Console.WriteLine("Player 1 : " + player1.userName + " won the game");

                if (player1.diceValue != player2.diceValue)
                {
                    Console.WriteLine("Player's 1 rating increased by " + player1.ratingValue + " and equals : " + player1.currentRating);
                    Console.WriteLine("Player's 2 rating decreased by " + player2.ratingValue + " and equals : " + player2.currentRating);
                }
                else
                {
                    Console.WriteLine("It's draw, dices are equal");
                }
            }
        }

        class GameSession : DiceGame
        {
            public void playDiceGame(IGameEngine player1, IGameEngine player2)
            {

                Random random = new Random();
                
                int value1 = random.Next(1, 6);
                int value2 = random.Next(1, 6);

                player1.diceValue = value1;
                player2.diceValue = value2;

                if (player1.diceValue > player2.diceValue) {
                    player1.winGame();
                    player2.loseGame();
                    if (player2. currentRating <= 0) {player2.currentRating = 1;}
                    GameAccount.printSessionLogs(player1, player2);

                }
                else if (player1.diceValue == player2.diceValue) {
                    player1.invokeDraw();
                    player2.invokeDraw();
                    GameAccount.printSessionLogs(player1, player2);

                }
                else {
                    player2.winGame();
                    player1.loseGame();
                    if (player1.currentRating <= 0){player1.currentRating = 1;}
                    GameAccount.printSessionLogs(player1, player2);
                    
                }
                GameHistory.recordData(player1, player2);
                
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
            
            private static int idSeed = 2022;
            public string gameID { get; set; }
            private int i = 1;
            public string userName { get; }
            public string opponentName { get; }
            public int rating { get; }
            public int gamesCount { get; }
            public string gameStatus { get; }

            private GameHistory(string userName, string opponentName, int rating, int gamesCount, string gameStatus)
            {
                this.userName = userName;
                this.opponentName = opponentName;
                this.rating = rating;
                this.gamesCount = gamesCount;
                this.gameStatus = gameStatus;

                gameID = idSeed.ToString();
                idSeed++;
            }

            public static void recordData(IGameEngine player1, IGameEngine player2)
            {
                player1.playerData.Add(new GameHistory(player1.userName, player2.userName, player1.currentRating, player1.gamesCount, player1.gameStatus));
                changeSeed();
                player2.playerData.Add(new GameHistory(player2.userName, player1.userName, player2.currentRating, player2.gamesCount, player2.gameStatus ));
            }
            
            private static void changeSeed()
            {
                idSeed--;
            }
            
        }
        
    }
    
