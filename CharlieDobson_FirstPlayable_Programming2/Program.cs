using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;

namespace CharlieDobson_FirstPlayable_Programming2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int width = 100;
            int height = 25;
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);

            Console.WriteLine("Before we begin, I have the window set to a speficic size for a reason.\nIf you resize it, and you break it, its on you.");
            Console.ReadKey(true);
            Console.Clear();

            //Lets you make a name for the player
            Console.WriteLine("PLEASE INSERT A NAME FOR YOUR JOURNEY");
            GameManager.Instance._writtenName = Console.ReadLine();

            
            Console.Clear();

            Console.WriteLine($"{GameManager.Instance._writtenName}'s Game");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            CharacterSelection();
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Tutorial();
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            StartMenu();
            ConsoleKey gameStart = ConsoleKey.NoName;
            while(gameStart == ConsoleKey.NoName)
            {
                gameStart = Console.ReadKey(true).Key;
                if (gameStart == ConsoleKey.E)
                {
                    Console.Clear();
                    GameManager.Instance._gameStateCheck = 0;
                    break;
                }
                else if (gameStart == ConsoleKey.A)
                {
                    Console.Clear();
                    GameManager.Instance._gameStateCheck = 1;
                    break;
                }
                else
                {
                    Console.WriteLine("NOT ACCEPTABLE. PLEASE PICK ACCEPTABLE CHOICE!");
                    gameStart = ConsoleKey.NoName;
                }
            }
            
            Console.Clear();

            //Startin stuff, just loads the map and does stuff. 
            GameManager.Instance.GamestateChecker();

        }

        public static void Tutorial()
        {
            Console.WriteLine("Use WSAD to control the * around.");
            Console.WriteLine("Collect gold from $ collectables or from enemies dying.");
            Console.WriteLine("Kill a certain amount of enemies(the red guys) to progress");
            Console.WriteLine("Tiles with a + will heal you.");
            Console.WriteLine("Water (blue) will damage you,\nMountains (light grey) are impassable,\nand forests (dark green) have a chance to heal you or harm you.");
        }

        //Start Menu
        public static void StartMenu()
        {
            Console.WriteLine("~~~~~~~~~");
            Console.WriteLine("HIT A TO PLAY ADVENTURE MODE!");
            Console.WriteLine();
            Console.WriteLine("HIT E TO PLAY ENDLESS MODE!");
            Console.WriteLine("~~~~~~~~~");
        }

        //Lets the player pick which character class they want to play as
        public static void CharacterSelection()
        {
            Console.WriteLine("PLEASE PICK YOUR CHARACTER");
            Console.WriteLine("1 - Standard (a normal, balanced character)");
            Console.WriteLine("2 - Assassin (deals double damage, but has a 15% chance to take double damage back from enemies)");
            Console.WriteLine("3 - Gambler (gold gained can be anywhere from -1.5x to 4x the normal amount)");
            Console.WriteLine("4 - Vampire (loses 1 HP per move, but heals a random amount (1-10) whenever it attacks an enemy)");

            ConsoleKey choice = ConsoleKey.NoName;
            while (choice == ConsoleKey.NoName)
            {
                choice = Console.ReadKey(true).Key;

                if (choice == ConsoleKey.D1 || choice == ConsoleKey.NumPad1)
                {
                    GameManager.Instance._selectedCharacterClass = CharacterClass.Standard;
                }
                else if (choice == ConsoleKey.D2 || choice == ConsoleKey.NumPad2)
                {
                    GameManager.Instance._selectedCharacterClass = CharacterClass.Assassin;
                }
                else if (choice == ConsoleKey.D3 || choice == ConsoleKey.NumPad3)
                {
                    GameManager.Instance._selectedCharacterClass = CharacterClass.Gambler;
                }
                else if (choice == ConsoleKey.D4 || choice == ConsoleKey.NumPad4)
                {
                    GameManager.Instance._selectedCharacterClass = CharacterClass.Vampire;
                }
                else
                {
                    choice = ConsoleKey.NoName;
                }
            }
        }

    }
}
