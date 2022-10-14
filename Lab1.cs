//TODO : TR-11 Kirill Schust

using System;
using System.Collections.Generic;


internal class Lab1 {
        
        public static void Main(string[] args)
        {
            DiceGame game = new GameSession();
            game.playDiceGame();
        }

        interface DiceGame
        {
            void playDiceGame();
        }
        interface GameEngine {
            void winGame();
            void loseGame();
            void invokeDraw();
            void getStats();

            //void collectData(GameEngine player, GameEngine oppositePlayer);
            
            string getUserName();
            string getGameStatus();
            void setGameStatus(string gameStatus);
            int getCurrentRating();
            void decreaseCurrentRating();
            void increaseCurrentRating();
            void setCurrentRating(int currentRating);
            int getRatingValue();
            int getDiceValue();
            void setDiceValue(int diceValue);
            int getGamesCount();
            void increaseGameCounter();




        }
        
        class GameAccount : GameEngine
        {
            static List<string> list = new List<string>();
            private Random random = new Random();
            private string userName ;
            private string gameStatus;
            private int currentRating;
            private int gamesCount;
            private int diceValue;
            private int ratingValue;
            private static string id;
            private static int i = 1;

            public string getUserName() {
                return userName;
            }

            public string getGameStatus() {
                return gameStatus;
            }

            public void setGameStatus(string gameStatus) {
                this.gameStatus = gameStatus;
            }

            public int getCurrentRating() {
                return currentRating;
            }

            public int getDiceValue() {
                return diceValue;
            }

            public void setDiceValue(int diceValue) {
                this.diceValue = diceValue;
            }

            public void increaseCurrentRating()
            {
                ratingValue = random.Next(1, 10);
                currentRating+= ratingValue;
            }

            public void decreaseCurrentRating() {
                ratingValue = random.Next(1, 10);
                currentRating-= ratingValue;
            }

            public void setCurrentRating(int currentRating) {
                this.currentRating = currentRating;
            }

            public int getRatingValue()
            {
                return ratingValue;
            }

            public int getGamesCount() {
                return gamesCount;
            }
            public  void increaseGameCounter (){ gamesCount++;}

            public static string generateID()
            {
                id = "Game ID : 2022";
                id = id + i;
                list.Add(id);
                i += 1;
                return id;
            }

            private static string getID()
            {
                return id;
            }
            


            public GameAccount(string userName) {
                this.userName = userName;
                currentRating = 1;
                gamesCount = 0;
            }
            public void winGame() {
                setGameStatus("Win");
            }

            public void loseGame() {
                setGameStatus("Lose");
            }

            public void invokeDraw() {
                setGameStatus("Draw");
            }

            public void getStats()
            {
                foreach (var data in list)
                {
                    Console.WriteLine(data);
                }
            }

            public static void collectData(GameEngine player, GameEngine oppositePlayer)
            {
                string data = "Player name: " +  player.getUserName().ToString() + " | " 
                              +" Game status: " +  player.getGameStatus().ToString() + " | " 
                              +" Rating count (+/-): " +  player.getRatingValue().ToString() + " | " 
                              +" Played against: " + oppositePlayer.getUserName().ToString() + " | " 
                              +" Total games: " + player.getGamesCount().ToString(); 
                
                list.Add(data);
            }
        }

        class GameSession : DiceGame
        {
            private GameEngine player1 = new GameAccount("KI");
            private GameEngine player2 = new GameAccount("NI");
            
            public void playDiceGame() {
                Random random = new Random();
                int value1 = random.Next(1, 6);
                int value2 = random.Next(1, 6);

                player1.setDiceValue(value1);
                player2.setDiceValue(value2);

                if (player1.getDiceValue() > player2.getDiceValue()) {
                    Console.WriteLine("Player1: " + player1.getUserName() + " VS Player 2: " + player2.getUserName());
                    Console.WriteLine("Player's dice value is: "+ player1.getDiceValue() +", " + player2.getUserName() + " dice value is "+ player2.getDiceValue());
                    Console.WriteLine("Player 1 : " + player1.getUserName() + " won the game");
                    player1.increaseCurrentRating();
                    player2.decreaseCurrentRating();
                    if (player2. getCurrentRating() <= 0) {player2.setCurrentRating(1);}
                    Console.WriteLine("Player's 1 rating increased by " + player1.getRatingValue() + " and equals : " + player1.getCurrentRating());
                    Console.WriteLine("Player's 2 rating decreased by " + player2.getRatingValue() + " and equals : " + player2.getCurrentRating());
                    player1.winGame();
                    player2.loseGame();
                    
                    player1.increaseGameCounter();
                    player2.increaseGameCounter();
                    GameAccount.generateID();
                    GameAccount.collectData(player1, player2);
                    GameAccount.collectData(player2, player1);
                }
                else if (player1.getDiceValue() == player2.getDiceValue()) {
                    Console.WriteLine("Player1: " + player1.getUserName() + " VS Player 2: " + player2.getUserName());
                    Console.WriteLine("Player's dice value is: "+ player1.getDiceValue() +", " + player2.getUserName() + " dice value is "+ player2.getDiceValue());
                    Console.WriteLine("It's draw, dices are equal");
                    player1.invokeDraw();
                    player2.invokeDraw();
                    
                    player1.increaseGameCounter();
                    player2.increaseGameCounter();
                    GameAccount.generateID();
                    GameAccount.collectData(player1, player2);
                    GameAccount.collectData(player2, player1);
                }
                else {
                    Console.WriteLine("Player1: " + player1.getUserName() + " VS Player 2: " + player2.getUserName());
                    Console.WriteLine("Player's dice value is: "+ player1.getDiceValue() +", " + player2.getUserName() + " dice value is "+ player2.getDiceValue());
                    Console.WriteLine("Player 2 : " + player2.getUserName() + " won the game");
                    player2.increaseCurrentRating();
                    player1.decreaseCurrentRating();
                    if (player1.getCurrentRating() <= 0){player1.setCurrentRating(1);}
                    Console.WriteLine("Player's 2 rating increased by " + player2.getRatingValue() + " and equals : " + player2.getCurrentRating());
                    Console.WriteLine("Player's 1 rating decreased by " + player1.getRatingValue() + " and equals : " + player1.getCurrentRating());
                    player2.winGame();
                    player1.loseGame();
                    
                    player1.increaseGameCounter();
                    player2.increaseGameCounter();
                    GameAccount.generateID();
                    GameAccount.collectData(player2, player1);
                    GameAccount.collectData(player1, player2);
                }

                while (true) {
                    Console.WriteLine("Do you want to play again? Enter : \" yes \" or \" no \" ");
                    Console.WriteLine("You can print matches information using \" print \" command");
                    string name = Console.ReadLine();
                    switch (name)
                    {
                        case "yes": playDiceGame();
                            break;
                        case "no": Environment.Exit(0);
                            break;
                        case "print" :
                            player2.getStats(); //or player 1, doesn't matter 
                            break;
                        default: Console.WriteLine("Enter the right command, please");
                            break;
                    }

                }
                
            }
        }
        
    }
    
