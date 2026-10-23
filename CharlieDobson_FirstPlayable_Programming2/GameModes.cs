using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharlieDobson_FirstPlayable_Programming2
{
    abstract class GameMode
    {
        public string _path = "";
        public static int _xMarg = 1;
        public static int _yMarg = 1;

        public virtual void Intialize(string path)
        {
            _path = path;
            GameManager.Instance._gameMap = new Map(_path);
            GameManager.Instance._gameMap.LoadMap();
            GameManager.Instance._gameMap.DrawMap();
            Console.Clear();

            GameManager.Instance._gameMap.DrawMapButAnimated();

            GameManager.Instance.spawner.EnemySpawning(1, 26);
            GameManager.Instance.spawner.CollectablesSpawning(1, 26);
        }

        public virtual void PlayerSpawning()
        {
            GameManager.Instance._gamePlayer = default;
            while (true)
            {
                GameManager.Instance._randomYPosition = GameManager.Instance._random.Next(1, GameManager.Instance._gameMap._mapHeight);
                GameManager.Instance._randomXPosition = GameManager.Instance._random.Next(1, GameManager.Instance._gameMap._mapWidth);

                if (GameManager.Instance._gameMap._isOccupied[GameManager.Instance._randomYPosition, GameManager.Instance._randomXPosition] == false)
                {
                    break;
                }
            }
            //Creates player properly, then sets the spot as occupied amd shows your hud
            GameManager.Instance._gamePlayer = new Player(name: GameManager.Instance._writtenName, maxHealth: 100, startingXPos: GameManager.Instance._randomXPosition, startingYPos: GameManager.Instance._randomYPosition, characterClass: GameManager.Instance._selectedCharacterClass);
            GameManager.Instance._gameMap._isOccupied[GameManager.Instance._gamePlayer._position._yPos, GameManager.Instance._gamePlayer._position._xPos] = true;
        }

        public void ProcessInput()
        {
            if (GameManager.Instance._currentTurn == 0) return;

            ConsoleKey input = ConsoleKey.NoName;

            while (input == ConsoleKey.NoName)
            {
                input = Console.ReadKey(true).Key;

                if (input != ConsoleKey.W && input != ConsoleKey.S && input != ConsoleKey.A && input != ConsoleKey.D)
                {
                    input = ConsoleKey.NoName;
                }
            }

            if (input == ConsoleKey.W)
            {
                GameManager.Instance._gamePlayer._yMovement--;
            }
            if (input == ConsoleKey.S)
            {
                GameManager.Instance._gamePlayer._yMovement++;
            }
            if (input == ConsoleKey.A)
            {
                GameManager.Instance._gamePlayer._xMovement--;
            }
            if (input == ConsoleKey.D)
            {
                GameManager.Instance._gamePlayer._xMovement++;
            }

            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }





        }

        //Updates various things based off of stuff actions
        public virtual void Update()
        {
            GameManager.Instance._gamePlayer.Movement();
            GameManager.Instance.EnemyMovement();
            GameManager.Instance.CheckTile();
            GameManager.Instance.DeathCheck();
            GameManager.Instance._currentTurn++;
        }

        //Draws the game
        public virtual void Draw()
        {
            Console.SetCursorPosition(0, GameManager.Instance._gameMap._mapHeight + 1);
            Console.WriteLine();
            Console.WriteLine($"Current Turn: {GameManager.Instance._currentTurn}");
            Console.WriteLine($"Current Goal:{GameManager.Instance.currentEnemiesKilled}/{GameManager.Instance.totalEnemiesBoundary}");
            GameManager.Instance.huds.PrintHUDStrings();
            Console.WriteLine();

            Console.SetCursorPosition(0, 0);
            GameManager.Instance._gamePlayer.DrawCharacter(_xMarg, _yMarg);
            foreach (Enemy em in GameManager.Instance._enemies)
            {
                em.DrawCharacter(_xMarg, _yMarg);
            }
            foreach (Collectables cl in GameManager.Instance._collectables)
            {
                cl.DrawCollectable(_xMarg, _yMarg);
            }

            Console.ResetColor();
        }
    }
}
