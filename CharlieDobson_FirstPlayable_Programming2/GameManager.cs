using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static CharlieDobson_FirstPlayable_Programming2.GameManager;

namespace CharlieDobson_FirstPlayable_Programming2
{

    internal class GameManager
    {
        #region Variables
        public Map _gameMap;
        public PrintHUD huds = new PrintHUD();
        public EndlessMode endless = new EndlessMode();
        public AdventureMode adventure = new AdventureMode();
        public Spawner spawner = new Spawner();

        public int _currentTurn = 0;
        public List<Enemy> _enemies = new List<Enemy>();
        public List<Collectables> _collectables = new List<Collectables>();
        public Player _gamePlayer;
        public bool _isDead = false;
        public string _writtenName;
        public CharacterClass _selectedCharacterClass = CharacterClass.Standard;
        public int _gameStateCheck;
        public int currentEnemiesKilled;
        public int totalEnemiesBoundary;

        public Random _random = new Random();
        public int _randomXPosition;
        public int _randomYPosition;

        public bool replayAgain = true;
        #endregion

        //So You cannot create another game manager
        private GameManager() { }

        private static GameManager _instance;

        public static GameManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new GameManager();
                }

                return _instance;
            }
        }

        
        //Checks the game state selected, and will set the game to that
        public void GamestateChecker()
        {
            if (_gameStateCheck == 0)
            {
                endless.Intialize(endless._path);
                endless.PlayerSpawning();
                endless.Endless();
            }
            else if(_gameStateCheck == 1)
            {
                adventure.Intialize(adventure.levels[0]);
                adventure.PlayerSpawning();
                adventure.Adventure();
            }
            else
            {
                Console.WriteLine("Please tell me how you got here.");
            }
        }

        //Moves all enemies in the list
        public void EnemyMovement()
        {
            foreach(Enemy em in _enemies)
            {
                em.Movement();
            }
        }

        
        //Checks to see whats dead
        public void DeathCheck()
        {
            if(_gamePlayer._health.CurrentHealth == 0)
            {
                _isDead = true;
            }

            foreach(Enemy em in _enemies)
            {
                if(em._health.CurrentHealth == 0)
                {
                    GameManager.Instance.currentEnemiesKilled++;
                    int _goldAmount = _random.Next(1, 11);
                    GameManager.Instance._gamePlayer.GetGold(_goldAmount);

                    em._health.ResetHealth();

                    while (true)
                    {
                        _randomYPosition = _random.Next(1, _gameMap._mapHeight);
                        _randomXPosition = _random.Next(1, _gameMap._mapWidth);

                        if (_gameMap._isOccupied[_randomYPosition, _randomXPosition] == false)
                        {
                            break;
                        }
                    }

                    _gameMap._isOccupied[em._position._yPos, em._position._xPos] = false;
                    em.ChangePosition(movementX: _randomXPosition, movementY: _randomYPosition);
                    _gameMap._isOccupied[em._position._yPos, em._position._xPos] = true;
                }


            }
        }
        
        //Checks the tile to see what kind it is, and does an action based off of that
        public void CheckTile()
        {
            char _mapTile = _gameMap._inGameMap[_gamePlayer._position._yPos][_gamePlayer._position._xPos];

            if (_mapTile == '▒')
            {
                _gamePlayer._health.TakeDamage(1);
            }
            else if(_mapTile == '█')
            {
                int chance = _random.Next(1, 3);
                if(chance == 1)
                {
                    _gamePlayer._health.TakeDamage(5);
                }
                else
                {
                    _gamePlayer._health.Heal(5);
                }
            }

                foreach (Enemy em in _enemies)
                {
                    _mapTile = _gameMap._inGameMap[em._position._yPos][em._position._xPos];

                    if (_mapTile == '▒')
                    {
                        em._health.TakeDamage(1);
                    }
                else if (_mapTile == '█')
                {
                    int chance = _random.Next(1, 3);
                    if (chance == 1)
                    {
                        em._health.TakeDamage(5);
                    }
                    else
                    {
                        em._health.Heal(5);
                    }
                }

            }
        }
    }
}
