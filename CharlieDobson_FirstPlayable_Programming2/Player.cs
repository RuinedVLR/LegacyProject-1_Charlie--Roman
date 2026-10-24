using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharlieDobson_FirstPlayable_Programming2
{
    internal class Player : Character
    {
        //You can name your player so I added this
        public string _playerName;
        public int _goldAmount;
        public CharacterClass _characterClass;

        public int _attackMultipler = 1;
        public int _goldMultipler = 1;

        public string _lastEventMessage = "";

        public Player(string name, int maxHealth, int startingXPos, int startingYPos, CharacterClass characterClass = CharacterClass.Standard) : base(maxHealth, startingXPos, startingYPos)
        {
            _playerName = name;
            _health = new Health(maxHealth);
            _position = new Position(startingXPos, startingYPos);
            _goldAmount = 0;
            _characterClass = characterClass;
        }

        //Shows the HUD of the player
        public override string GetHUDString()
        {
            string _upperCaseName = _playerName.ToUpper();

            _hudString = $"{_upperCaseName}'S HUD ({_characterClass})\n~~~HEALTH~~~\n   {_health.CurrentHealth}/{_health.MaxHealth}\n\n~~~POSITION~~~\n   ({_position._xPos},{_position._yPos})\n\n~~~GOLD~~~\n    {_goldAmount}\n\n~~~LAST EVENT~~~\n    {_lastEventMessage}";

            return _hudString;

        }
        public override void DrawCharacter(int xMarg, int yMarg)
        {
            base.DrawCharacter(xMarg, yMarg);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("*");
        }

        public void Attack(Enemy em)
        {
            int _attackPower = GameManager.Instance._random.Next(1, 6);
            int _damage = _attackPower * _attackMultipler;

            //Assassin deals double damage to enemies
            if (_characterClass == CharacterClass.Assassin)
            {
                _damage *= 2;
            }

            em._health.TakeDamage(_damage);

            //Assassin has a 15% chance of taking double damage back from the enemy
            if (_characterClass == CharacterClass.Assassin)
            {
                int _retaliationChance = GameManager.Instance._random.Next(1, 101);
                if (_retaliationChance <= 15)
                {
                    int _retaliationDamage = _attackPower * 2;
                    _health.TakeDamage(_retaliationDamage);
                    _lastEventMessage = $"Assassin took {_retaliationDamage} retaliation damage!";
                }
            }

            //Vampire heals a random amount whenever it attacks an enemy
            if (_characterClass == CharacterClass.Vampire)
            {
                int _healAmount = GameManager.Instance._random.Next(1, 11);
                _health.Heal(_healAmount);
                _lastEventMessage = $"Vampire healed {_healAmount} HP from attack!";
            }
        }
        public void GetGold(int amount)
        {
            int _finalAmount = amount * _goldMultipler;

            //Gambler can gain up to 4x the gold, or lose up to 1.5x the gold
            if (_characterClass == CharacterClass.Gambler)
            {
                double _gambleMultipler = (GameManager.Instance._random.NextDouble() * 5.5) - 1.5;
                _finalAmount = (int)(amount * _gambleMultipler);
                _lastEventMessage = $"Gambler multiplier x{_gambleMultipler:F2}: {(_finalAmount >= 0 ? "+" : "")}{_finalAmount} gold!";
            }

            _goldAmount = _goldAmount + _finalAmount;

            if (_goldAmount < 0)
            {
                _goldAmount = 0;
            }
        }

        public override void Movement()
        {
            int currentX = _position._xPos + _xMovement;
            int currentY = _position._yPos + _yMovement;

            if(currentX >= 0 && currentY >= 0)
            {
                if(currentX < GameManager.Instance._gameMap._mapWidth && currentY < GameManager.Instance._gameMap._mapHeight)
                {
                    if (GameManager.Instance._gameMap._isOccupied[currentY, currentX] == false)
                    {
                        GameManager.Instance._gameMap._isOccupied[_position._yPos, _position._xPos] = false;
                        UpdateTile(_position._xPos, _position._yPos);
                        ChangePosition(movementX: currentX, movementY: currentY);
                        GameManager.Instance._gameMap._isOccupied[_position._yPos, _position._xPos] = true;

                        //Vampire loses health with every movement
                        if (_characterClass == CharacterClass.Vampire)
                        {
                            _health.TakeDamage(1);
                        }
                    }

                    foreach(Enemy em in GameManager.Instance._enemies)
                    {
                        if(currentX == em._position._xPos && currentY == em._position._yPos)
                        {
                            Attack(em);
                        }
                    }

                    foreach(Collectables cl in GameManager.Instance._collectables)
                    {
                        if (currentX == cl._position._xPos && currentY == cl._position._yPos)
                        {
                            cl.PickUP();
                        }
                    }

                }
            }

            _xMovement = 0;
            _yMovement = 0;
        }

    }
}
