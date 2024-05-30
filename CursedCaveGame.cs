using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
namespace MushroomPocket
{
    public class Enemy(string name, int hp, double skill)
    {
        public string Name { set; get; } = name;
        public int Hp { set; get; } = hp;
        public double Skill { set; get; } = skill;
        public int ATK { set; get; }
        public int Damage()
        {
            return Convert.ToInt32(this.ATK * this.Skill);
        }
    }

    class CursedCaveGame
    {
        public static CursedCaveCharacter Initialize(MushroomPocketContext db, string dash)
        {
            string intro = @"

                                        
 ▄████▄   █    ██  ██▀███    ██████ ▓█████ ▓█████▄     ▄████▄   ▄▄▄    ██▒   █▓▓█████ 
▒██▀ ▀█   ██  ▓██▒▓██ ▒ ██▒▒██    ▒ ▓█   ▀ ▒██▀ ██▌   ▒██▀ ▀█  ▒████▄ ▓██░   █▒▓█   ▀ 
▒▓█    ▄ ▓██  ▒██░▓██ ░▄█ ▒░ ▓██▄   ▒███   ░██   █▌   ▒▓█    ▄ ▒██  ▀█▄▓██  █▒░▒███   
▒▓▓▄ ▄██▒▓▓█  ░██░▒██▀▀█▄    ▒   ██▒▒▓█  ▄ ░▓█▄   ▌   ▒▓▓▄ ▄██▒░██▄▄▄▄██▒██ █░░▒▓█  ▄ 
▒ ▓███▀ ░▒▒█████▓ ░██▓ ▒██▒▒██████▒▒░▒████▒░▒████▓    ▒ ▓███▀ ░ ▓█   ▓██▒▒▀█░  ░▒████▒
░ ░▒ ▒  ░░▒▓▒ ▒ ▒ ░ ▒▓ ░▒▓░▒ ▒▓▒ ▒ ░░░ ▒░ ░ ▒▒▓  ▒    ░ ░▒ ▒  ░ ▒▒   ▓▒█░░ ▐░  ░░ ▒░ ░
  ░  ▒   ░░▒░ ░ ░   ░▒ ░ ▒░░ ░▒  ░ ░ ░ ░  ░ ░ ▒  ▒      ░  ▒     ▒   ▒▒ ░░ ░░   ░ ░  ░
░         ░░░ ░ ░   ░░   ░ ░  ░  ░     ░    ░ ░  ░    ░          ░   ▒     ░░     ░   
░ ░         ░        ░           ░     ░  ░   ░       ░ ░            ░  ░   ░     ░  ░
░                                           ░         ░                    ░          


                                    ";
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(intro);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Choose a character to play \n" + dash);
            CharacterCRUD.showCharacterList(db, dash);
            Console.Write("Enter the id of the character you want: ");
            while (true)
            {
                string kw = Console.ReadLine();
                if (int.TryParse(kw, out int option))
                {
                    var chosenCharacter = db.Characters.Where(i => i.CharacterId == option).FirstOrDefault();
                    // game starts
                    if (chosenCharacter != null)
                    {
                        var player = new CursedCaveCharacter(chosenCharacter.Name)
                        {
                            CharacterId = option,
                            Hp = chosenCharacter.Hp,
                            Skill = chosenCharacter.Skill
                        };
                        return player;
                    }
                    return null;
                }
                else
                {
                    Console.WriteLine("Please enter the character Id correctly");
                }
            }


        }
        public static void Battle(MushroomPocketContext database, CursedCaveCharacter player, int minDamage, int maxDamage, string info, string enemy)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"Your hp before the fight: {player.Hp}");
            Console.ForegroundColor = ConsoleColor.Gray;
            var skillPoint = player.SkillPoint[player.Skill];
            var weapon = ChooseWeapon(player.Inventory);
            // knife, apple, bomb, mace, arrow, vessel,sword
            if (weapon != null)
            {
                switch (weapon.Name)
                {
                    case "sword":
                        {
                            skillPoint += 0.3;
                            Console.WriteLine($"By using {weapon.Name}, your skill is boosted to {skillPoint}");
                        }; break;
                    case "knife": { skillPoint += 0.2; skillPoint = Math.Round(skillPoint, 2); Console.WriteLine($"By using {weapon.Name}, your skill is boosted to {skillPoint}"); }; break;
                    case "bomb": { skillPoint += 0.4; skillPoint = Math.Round(skillPoint, 2); Console.WriteLine($"By using {weapon.Name}, your skill is boosted to {skillPoint}"); }; break;
                    case "mace": { skillPoint += 0.25; skillPoint = Math.Round(skillPoint, 2); Console.WriteLine($"By using {weapon.Name}, your skill is boosted to {skillPoint}"); }; break;
                    case "arrow": { skillPoint += 0.2; skillPoint = Math.Round(skillPoint, 2); Console.WriteLine($"By using {weapon.Name}, your skill is boosted to {skillPoint}"); }; break;
                    case "apple":
                        {
                            player.Hp = 100;
                            Console.WriteLine($"You eat the magic apple, hence your HP is 100 now!! ");
                        }; break;
                    case "vessel":
                        {
                            if (player.Hp < 90)
                            {
                                player.Hp += 10;
                                Console.WriteLine($"You drink the holy water from vessel, so your HP is boosted!! ");
                            }
                            else
                            {
                                Console.WriteLine($"Vessel is only useful when your HP is lower than 80!! ");
                            }
                        }; break;
                    default: break;
                }

            }
            else{
                 Console.WriteLine("You did not choose any weapons, you have a high chance to lose!");
            }
            // enemy
            if (enemy != null)
            {
                switch (enemy)
                {
                    case "bats":
                        {
                            Enemy Bat = new("Bat", 100, 0.4);
                            Fight(database, Bat, minDamage, maxDamage, player, skillPoint, weapon, info);

                        }; break;
                    case "witch":
                        {
                            Enemy Witch = new("Witch", 100, 0.6);
                            Fight(database, Witch, minDamage, maxDamage, player, skillPoint, weapon, info);
                        }; break;
                    case "dragon":
                        {
                            Enemy Dragon = new("Dragon", 100, 0.9);
                            Fight(database, Dragon, minDamage, maxDamage, player, skillPoint, weapon, info);
                        }; break;
                    default: break;
                }
            }


        }

        public static void Fight<T>(MushroomPocketContext db, Enemy enemy, int minDamage, int maxDamage, CursedCaveCharacter player, double sp, T item, string info) where T : Item
        {
            if (player.Hp > 0)
            {
                var count = 1;
                Random rnd = new();
                while (enemy.Hp > 0 && player.Hp > 0)
                {
                    Console.WriteLine("-----------------");
                    Console.WriteLine($"{count} Round of the battle");
                    Console.WriteLine("-----------------");
                    var atk = rnd.Next(minDamage, maxDamage);
                    enemy.ATK = atk;
                    var damage = enemy.Damage();
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine($"The {enemy.Name} attacked you with the damage power {damage}");

                    var effectiveDamage = Convert.ToInt32(damage - (damage * sp));
                    // enemy attacks character
                    player.Hp -= effectiveDamage;


                    if (player.Hp <= 0)
                    {
                        Console.WriteLine("Your Hp -> 0");
                        player.Hp = 0;
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine($"Oops...you died from the {enemy.Name}");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        CharacterCRUD.updateCharacter(db, player);
                        Environment.Exit(0);
                    }
                    Console.WriteLine($"Your Hp -> {player.Hp}");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    if (item != null)
                    {
                        if (item.Name == "apple")
                        {
                            player.Hp = 100;
                            Console.WriteLine("You eat the magic apple, hence your HP is 100 now!!");
                        }
                        else if (item.Name == "vessel")
                        {
                            player.Hp += 10;
                            Console.WriteLine("You drink the holy water from vessel, so your HP is boosted!!");
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine($"Your Hp ->{player.Hp}");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                    }
                    // actual fight
                    Console.Write("Enter any key to fight, q to give up: ");
                    var fightOpt = Console.ReadLine();
                    if (fightOpt != "q")
                    {
                        switch (player.Name)
                        {
                            case "Waluigi":
                            case "Daisy":
                            case "Wario":
                                {
                                    var daisy_atk = rnd.Next(0, 80);
                                    player.ATK = daisy_atk;
                                }; break;
                            case "Luigi":
                            case "Mario":
                            case "Peach":
                                {
                                    var peach_atk = rnd.Next(50, 100);
                                    player.ATK = peach_atk;
                                }; break;
                            default: break;
                        }
                        var my_damage = player.Damage(sp);
                        var my_effectiveDamage = Convert.ToInt32(my_damage - (my_damage * enemy.Skill));
                        enemy.Hp -= my_effectiveDamage;

                        if (enemy.Hp <= 0)
                        {
                            
                            enemy.Hp = 0;
                            Console.WriteLine($"The {enemy.Name}'s Hp -> {enemy.Hp}");
                            Console.WriteLine(info);
                            Console.ForegroundColor = ConsoleColor.Gray;
                            if (enemy.Name == "Bat")
                            {
                                player.AddItem(new Item("sword"));
                            }
                            break;
                        }
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine($"You attacked the {enemy.Name} with the damage power {my_damage}");
                        Console.WriteLine($"The {enemy.Name}'s Hp -> {enemy.Hp}");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else if (fightOpt == "q")
                    {
                        Console.WriteLine("Are you sure you want to quit, you will not be able to save the princess and earn coins if you give up now!Enter y to confirm: ");
                        string confirm = Console.ReadLine();
                        if (confirm == "y")
                        {
                            Console.WriteLine("End of game");
                            // goto
                            CharacterCRUD.updateCharacter(db, player);
                            Environment.Exit(0);
                        }

                    }

                    count += 1;
                }
            }
            else
            {
                Console.WriteLine($"Oops...you died from the {enemy.Name}");
                Environment.Exit(0);
            }
        }
        public static Item ChooseWeapon(List<Item> inventory)
        {
            if (inventory.Count > 0)
            {
                Console.WriteLine($"You have {inventory.Count} items in your inventory\nEnter the first character of the item to choose: ");
                for (var i = 0; i < inventory.Count; i++)
                {
                    if (i == inventory.Count - 1)
                    {
                        Console.Write(inventory[i].Name + " :");
                    }
                    else
                    {
                        Console.Write(inventory[i].Name + ",");
                    }
                }
                var choose = Console.ReadLine();
                foreach (var item in inventory)
                {
                    if (choose == item.Name[0].ToString())
                    {
                        return item;
                    }

                }
            }

            return null;
        }

        public static int StoryLine(string question)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            string[] lines = question.Split(new[] { "\\n" }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            while (true)
            {
                string response = Console.ReadLine();
                if(int.TryParse(response,out int res)){
                    return res;
                }
                else{
                    Console.WriteLine("Please enter only numbers (1,2,3,..)");
                }
            }
            
        }
    }
}