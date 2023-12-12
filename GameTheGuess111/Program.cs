using System;
// check git
// hellos







namespace GuessTheNumberGame
{
    class Parameters
    {
        private int timeLimitHardLevel;
        private int timeLimitMediumLevel;
        private int guessesLimitHardLevel;
        private int guessesLimitMediumLevel;
        private int level;
        private int mode;
        private int limit;

        public int Level { get => level; set => level = value; }



        public int Mode { get => mode; set => mode = value; }
        public int Limit { get => limit; set => limit = value; }

        public Parameters()
        {
            timeLimitHardLevel = int.Parse(Environment.GetEnvironmentVariable("timeLimitHardLevel") ?? "1");
            timeLimitMediumLevel = int.Parse(Environment.GetEnvironmentVariable("timeLimitMediumLevel") ?? "3");
            guessesLimitHardLevel = int.Parse(Environment.GetEnvironmentVariable("guessesLimitHardLevel") ?? "10");
            guessesLimitMediumLevel = int.Parse(Environment.GetEnvironmentVariable("guessesLimitMediumLevel") ?? "30");
        }

        public void ChooseGameLevel()
        {
            Console.WriteLine("Choose game level (1: easy, 2: middle, 3: hard):");
            Level = int.Parse(Console.ReadLine());
        }

        public void ChooseGameMode()
        {
            Console.WriteLine("Choose game mode (1: for time, 2: for number of guesses):");
            Mode = int.Parse(Console.ReadLine());
        }

        public void SetUpLimit()

        {
            if (Level == 1)
            {
                Limit = 0;

            }
            else if (Level == 2)
            {
                if (Mode == 2)
                {
                    Limit = guessesLimitMediumLevel;
                }
                else
                {
                    Limit = timeLimitMediumLevel;
                }

            }
            else
            {
                if (Mode == 2)
                {
                    Limit = guessesLimitHardLevel;
                }
                else
                {
                    Limit = timeLimitHardLevel;
                }
            }


        }
    }

    abstract class Game
    {
        protected int randomNumber;

        public Game()
        {
            Random random = new Random();
            randomNumber = random.Next(1, 101);
        }

        public abstract void StartGame();

        protected bool MakeGuess()
        {
            Console.WriteLine("Make a guess");
            int guess = int.Parse(Console.ReadLine());

            if (guess == randomNumber)
            {
                Console.WriteLine("Great you are winner");
                return true;
            }
            else if (guess < randomNumber)
            {
                Console.WriteLine("MORE");
            }

            else
            {
                Console.WriteLine("LESS");
            }

            return false;
        }
    }

    class TimeLimitedGame : Game
    {
        private int timeLimit;
        private bool timeIsUp;

        public TimeLimitedGame(int limit)
        {
            timeLimit = limit;
        }

        public override void StartGame()
        {
            Console.WriteLine("Lets go start: " + timeLimit + " minutes.");

            Timer timer = new Timer(StopGame, null, timeLimit * 60000, Timeout.Infinite);

            while (!timeIsUp)
            {
                if (MakeGuess() || timeIsUp)
                {
                    break;
                }
            }

            if (timeIsUp)
            {
                Console.WriteLine("Time is up!");
            }
        }

        private void StopGame(object state)
        {
            timeIsUp = true;
        }
    }
    class GuessesLimitGame : Game
    {
        private int guessLimit;
        private int guessCount;

        public GuessesLimitGame(int limit)
        {
            guessLimit = limit;
            guessCount = 0;
        }

        public override void StartGame()
        {
            while (guessCount < guessLimit)
            {
                if (checkTruOrFalse())
                {
                    break;
                }

                guessCount++;
            }

            if (guessCount >= guessLimit)
            {
                Console.WriteLine("I have bad new, you dont have try!");
            }
        }

        private bool checkTruOrFalse()
        {
            return MakeGuess();
        }
    }

    class Program
    {
        static void Main()
        {
            Parameters parameters = new Parameters();
            parameters.ChooseGameLevel();
            parameters.ChooseGameMode();

            parameters.SetUpLimit();

            Game game;

            if (parameters.Mode == 1)
            {
                game = new TimeLimitedGame(parameters.Limit);
                game.StartGame();
            }
            else if (parameters.Mode == 2)
            {
                game = new GuessesLimitGame(parameters.Limit);
                game.StartGame();
            }
            else
            {
                Console.WriteLine("No correct");
            }


        }
    }
}
