using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
// using SixLabors.ImageSharp;
// using SixLabors.ImageSharp.PixelFormats;
// using SixLabors.ImageSharp.Processing;

namespace MushroomPocket
{
    class Program
    {
        static void Main(string[] args)
        {
            //MushroomMaster criteria list for checking character transformation availability.   
            /*************************************************************************
                PLEASE DO NOT CHANGE THE CODES FROM LINE 15-19
            *************************************************************************/
            List<MushroomMaster> mushroomMasters = new List<MushroomMaster>(){
            new MushroomMaster("Daisy", 2, "Peach"),
            new MushroomMaster("Wario", 3, "Mario"),
            new MushroomMaster("Waluigi", 1, "Luigi")
            };

            //Use "Environment.Exit(0);" if you want to implement an exit of the console program
            //Start your assignment 1 requirements below.

            var db = new MushroomPocketContext();
            Console.WriteLine($"Database path: {db.DbPath}.");

            List<Character> mushroom_pocket = new List<Character>();
            while (true)
            {
                // print the asterisks and welcome text
                string message = "Welcome to Mushroom Pocket Land";
                string border = new string('*', message.Length + 2);
                string dash = new string('-', message.Length + 2);


                Console.WriteLine(border);
                Console.WriteLine(message);
                Console.WriteLine(border);
                Console.Write("(1). Add mushrooms character to my pocket\n" +
                "(2). List character(s) in my pocket.\n" + "(3). Check if I can transform my characters\n" + "(4). Transform characters\n");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("(5). Play Legend of the Cursed Cave.");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("Please enter only [1,2,3,4,5] or q to quit: ");

                string kw_input = Console.ReadLine();
                Console.WriteLine(dash);
                if (kw_input.Equals("q", StringComparison.CurrentCultureIgnoreCase))
                {
                    Environment.Exit(0);
                }
                else
                {
                    try
                    {

                        int keyword = int.Parse(kw_input);
                        switch (keyword)
                        {

                            case 1:
                                {
                                    Character character = CharacterCRUD.AddCharacter();
                                    if (character != null)
                                    {
                                        db.Add(character);
                                        db.SaveChanges();
                                        Console.WriteLine($"{character.Name} has been added");
                                    }
                                }; break;
                            case 2:
                                {
                                    CharacterCRUD.showCharacterList(db, dash);
                                }; break;
                            case 3:
                                {
                                    var stored_characters = db.Characters.GroupBy(i => i.Name);
                                    List<string> transformables = new();
                                    // check if can transfer or not
                                    foreach (var storedCharacter in stored_characters)
                                    {
                                        foreach (var criteria in mushroomMasters)
                                        {
                                            if (storedCharacter.Key == criteria.Name)
                                            {
                                                if (storedCharacter.Count() >= criteria.NoToTransform)
                                                {
                                                    var count = storedCharacter.Count() / criteria.NoToTransform;
                                                    Console.WriteLine($"{storedCharacter.Key} --> {criteria.TransformTo} x{count}");
                                                    transformables.Add(storedCharacter.Key);
                                                }
                                            }
                                        }
                                    }
                                    if (transformables.Count <= 0)
                                    {
                                        Console.WriteLine("No characters can be transformed");
                                    }


                                }; break;
                            case 4:
                                {
                                    //  daisy,2; wario,3;waluigi,1; 
                                    var stored_characters = db.Characters.GroupBy(i => i.Name); //get count (daisy,3)(waluigi,1)(luigi,1)
                                    // check if can transfer or not
                                    List<string> transformables = new();
                                    foreach (var storedCharacter in stored_characters)
                                    {
                                        foreach (var criteria in mushroomMasters)
                                        {
                                            if (storedCharacter.Key == criteria.Name)
                                            {
                                                var count = storedCharacter.Count() / criteria.NoToTransform; //how many to add
                                                var noToDelete = count * criteria.NoToTransform;// how many to delete
                                                if (storedCharacter.Count() >= criteria.NoToTransform) //if have characters to transform
                                                {
                                                    if (storedCharacter.Count() % criteria.NoToTransform == 0) //if no exceed
                                                    {
                                                        var charactersToTransform = db.Characters.Where(i => i.Name == criteria.Name).ToList();
                                                        CharacterCRUD.UpgradeCharacter(criteria, db, charactersToTransform, noToDelete, count);
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine($"There is {storedCharacter.Count()} characters of {storedCharacter.Key} you can transform into {count} {criteria.TransformTo}");
                                                        Console.WriteLine("Press 1 to transform the characters with the least HP or 2 the least Exp: ");
                                                        while (true)
                                                        {
                                                            string opt = Console.ReadLine();
                                                            if (int.TryParse(opt, out int option))
                                                            {
                                                                if (option == 1)
                                                                {
                                                                    var charactersToTransformByHp = db.Characters.Where(i => i.Name == criteria.Name).OrderBy(i => i.Hp).ToList();
                                                                    CharacterCRUD.UpgradeCharacter(criteria, db, charactersToTransformByHp, noToDelete, count);
                                                                }
                                                                else if (option == 2)
                                                                {
                                                                    var charactersToTransformBySkill = db.Characters.Where(i => i.Name == criteria.Name).OrderBy(i => i.Exp).ToList();
                                                                    CharacterCRUD.UpgradeCharacter(criteria, db, charactersToTransformBySkill, noToDelete, count);
                                                                }
                                                                else
                                                                {
                                                                    Console.WriteLine("Please choose 1 or 2!");
                                                                }
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("Do not enter characters, only 1 or 2");
                                                            }
                                                        }
                                                    }
                                                    transformables.Add(storedCharacter.Key);
                                                }

                                            }
                                        }
                                    }
                                    if (transformables.Count == 0)
                                    {
                                        Console.WriteLine("No characters can be transformed");
                                    }

                                }; break;
                            case 5:
                                {
                                    // settle game code here
                                    var player = CursedCaveGame.Initialize(db, dash);
                                    if (player == null)
                                    {
                                        Console.WriteLine("No character to play, add more first");
                                        Environment.Exit(0);
                                    }
                                    else
                                    {
                                        if (player.Hp <= 0)
                                        {
                                            Console.WriteLine($"Your Hp is {player.Hp}, the program will auto restore your Hp for playing game");
                                            player.Hp = 100;
                                            Console.WriteLine($"Your Hp is now restored to {player.Hp}");
                                        }
                                        Console.WriteLine($"Welcome to the cursed cave ,{player.Name}");
                                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                                        Console.WriteLine($"\nYour Hp -> {player.Hp}\nYour Skill -> {player.SkillPoint[player.Skill]}");
                                        Console.ForegroundColor = ConsoleColor.Gray;
                                        var storyFile = @"story.txt";
                                        string[] stories = File.ReadAllLines(storyFile);
                                        // story starts
                                        bool done = true;
                                        while (done)
                                        {
                                            var choice = CursedCaveGame.StoryLine(stories[0]);
                                            switch (choice)
                                            {
                                                case 1:
                                                    {
                                                        while (true)
                                                        {
                                                            var open = CursedCaveGame.StoryLine(stories[1]);
                                                            if (open == 1)
                                                            {
                                                                while (true)
                                                                {
                                                                    var take = CursedCaveGame.StoryLine(stories[2]);
                                                                    if (take == 1)
                                                                    {
                                                                        player.AddItem(new Item("knife"));
                                                                        break;
                                                                    }
                                                                    else if (take == 2)
                                                                    {
                                                                        break;
                                                                    }
                                                                    else
                                                                    {
                                                                        Console.WriteLine("Please choose only 1 or 2");
                                                                    }
                                                                }
                                                                break;

                                                            }
                                                            else if (open == 2)
                                                            {
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("Please choose only 1 or 2");
                                                            }
                                                        }
                                                        done = false;
                                                    }; break;
                                                case 2:
                                                    {
                                                        while (true)
                                                        {
                                                            var fight = CursedCaveGame.StoryLine(stories[3]);
                                                            if (fight == 1)
                                                            {
                                                                //fight and lose hp here, smth happens if win, if lose
                                                                if (player.Hp > 0)
                                                                {
                                                                    CursedCaveGame.Battle(db, player, 0, 100, "Congratulations!! You won the bats, and earn a sword they have hidden! ", "bats");
                                                                }
                                                                else
                                                                {
                                                                    Console.WriteLine("You do not have enough Hp to play.");
                                                                }
                                                                break;
                                                            }
                                                            else if (fight == 2)
                                                            {
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("Please choose only 1 or 2");
                                                            }
                                                        }
                                                        done = false;

                                                    }; break;
                                                case 3:
                                                    {
                                                        while (true)
                                                        {
                                                            var bring = CursedCaveGame.StoryLine(stories[4]);
                                                            if (bring == 1)
                                                            {
                                                                player.AddItem(new Item("vessel"));
                                                                break;
                                                            }
                                                            else if (bring == 2)
                                                            {
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("Please choose only 1 or 2");
                                                            }
                                                        }
                                                        done = false;
                                                    }; break;
                                                default: Console.WriteLine("Please choose only 1,2,3"); break;
                                            }
                                        }


                                        // inner chamber adventure starts here
                                        Console.WriteLine(dash + "\nYou continue walking the cave, and you entered into an inner chamber\n" + dash);
                                        bool done2 = true;
                                        while (done2)
                                        {
                                            var decision = CursedCaveGame.StoryLine(stories[5]);
                                            if (decision == 1)
                                            {
                                                
                                                while (true)
                                                {
                                                    var choose = CursedCaveGame.StoryLine(stories[7]);
                                                    if (choose == 1)
                                                    {
                                                        player.AddItem(new Item("bomb"));
                                                        break;
                                                    }
                                                    else if (choose == 2)
                                                    {
                                                        player.AddItem(new Item("mace")); break;
                                                    }
                                                    else if (choose == 3)
                                                    {
                                                        player.AddItem(new Item("arrow")); break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Please choose only 1,2 or 3");
                                                    }
                                                }

                                                // break from outer while
                                                done2 = false;
                                            }
                                            else if (decision == 2)
                                            {
                                                Console.WriteLine("You walked into the other path with bird singing sound");
                                                try
                                                {
                                                    while (true)
                                                    {
                                                        var buy = CursedCaveGame.StoryLine(stories[6]);
                                                        if (buy == 1)
                                                        {
                                                            player.AddItem(new Item("apple"));
                                                            break;
                                                        }
                                                        else if (buy == 2)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("Please choose only 1 or 2");
                                                        }
                                                    }
                                                }
                                                catch (FormatException)
                                                {
                                                    Console.WriteLine("Only enter 1 or 2");
                                                }
                                                // break from while
                                                done2 = false;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Please choose only 1,2,3");
                                            }

                                        }
                                        bool done3 = true;
                                        while (done3)
                                        {
                                            var finalDecision = CursedCaveGame.StoryLine(stories[8]);
                                            if (finalDecision == 1)
                                            {
                                                if (player.Hp > 0)
                                                {
                                                    Console.WriteLine();
                                                    while (true)
                                                    {
                                                        var go = CursedCaveGame.StoryLine(stories[9]);
                                                        if (go == 1)
                                                        {
                                                            Console.WriteLine(stories[10]);
                                                            player.Hp = 100;
                                                            break;
                                                        }
                                                        else if (go == 2)
                                                        {
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("Please enter only 1 or 2");
                                                        }
                                                    }

                                                }
                                                done3 = false;
                                            }
                                            else if (finalDecision == 2)
                                            {
                                                Console.WriteLine("\nOops... your decision was wrong! You found a witch instead of a girl");
                                                if (player.Hp > 0)
                                                {
                                                    CursedCaveGame.Battle(db, player, 80, 100, "Congrats, you successfully slayed the witch, you survived!", "witch");

                                                }
                                                else
                                                {
                                                    Console.WriteLine("You do not have enough Hp to play.");
                                                }
                                                done3 = false;
                                            }

                                            else
                                            {
                                                Console.WriteLine("Please choose only 1 or 2");
                                            }

                                        }

                                        // last battle
                                        if (player.Hp > 0)
                                        {

                                            Console.WriteLine("\n" + dash);
                                            Console.WriteLine(stories[11]);
                                            if (player.Hp > 0)
                                            {
                                                CursedCaveGame.Battle(db, player, 90, 100, "Congratulations, Our Hero! You have bravely slain the dragon and rescued the princess. You are a true champion!", "dragon");
                                            }
                                            else
                                            {
                                                Console.WriteLine("You do not have enough Hp to play.");
                                            }
                                        }

                                    }

                                }; break;
                            default:
                                {
                                    Environment.Exit(0);
                                }; break;

                        }

                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please Enter a valid input");
                    }
                }
            }

        }

    }

}
