using System;
using System.Linq;
using System.Collections.Generic;

namespace MushroomPocket
{

    class CharacterCRUD
    {
        // create
        public static Character AddCharacter()
        {
            try
            {

                Console.Write("Enter character's name (it must be one of Daisy, Waluigi or Wario): ");
                string name = Console.ReadLine().ToLower();
                switch (name)
                {
                    case "daisy":
                        return GetCharacterDetails<Daisy>("Daisy");
                    case "waluigi":
                        return GetCharacterDetails<Waluigi>("Waluigi");
                    case "wario":
                        return GetCharacterDetails<Wario>("Wario");
                    default:
                        {
                            Console.WriteLine("The name you created is not existed in characters list");
                            return null;
                        };
                }

            }
            catch (FormatException)
            {
                Console.WriteLine("Please Enter a valid input");
            }
            return null;
        }
        //get data function for create
        static Character GetCharacterDetails<T>(string name) where T : Character, new()
        {
            bool invalid = true;

            while (invalid)
            {
                Console.Write("Enter character's HP(must be between 0 and 100): ");
                int hp = int.Parse(Console.ReadLine());
                if (hp <= 100 && hp >= 0)
                {
                    invalid = false;
                    while (true)
                    {
                        Console.Write("Enter character's EXP(must be between 0 and 100): ");
                        int exp = int.Parse(Console.ReadLine());
                        if (exp <= 100 && exp >= 0)
                        {
                            return new T
                            {
                                Name = name,
                                Hp = hp,
                                Exp = exp
                            };
                        }
                    }
                }

            }
            return null;

        }
        //read
        public static void showCharacterList(MushroomPocketContext db, string separator)
        {
            var stored_characters = db.Characters.OrderByDescending(a => a.Hp).ToList();
            foreach (var avatar in stored_characters)
            {
                Console.WriteLine($"Character ID: {avatar.CharacterId}\nName: {avatar.Name}\nHP: {avatar.Hp}\nEXP: {avatar.Exp}\nSKILL: {avatar.Skill}");
                Console.WriteLine(separator);
                Console.WriteLine(separator);
            }
        }
        // update
        public static void updateCharacter<T>(MushroomPocketContext db, T gamePlayer) where T : Character
        {
            var originalPlayer = db.Characters.Where(i => i.CharacterId == gamePlayer.CharacterId).FirstOrDefault();
            if (originalPlayer != null)
            {
                if (gamePlayer.Hp > 0)
                {
                    originalPlayer.Hp = gamePlayer.Hp;
                }
                else
                {
                    originalPlayer.Hp = 0;

                }
                db.SaveChanges();
            }
            else
            {
                Console.WriteLine("Character not found");
            }
        }
        //upgrade
        public static void UpgradeCharacter(MushroomMaster criteria, MushroomPocketContext database, List<Character> charactersToTransform, int noToDelete, int noToAdd)
        {
            int index = 0;
            foreach (var c in charactersToTransform)
            {
                index++;
                Character newCharacter = null;
                switch (criteria.TransformTo)
                {
                    case "Peach":
                        {
                            newCharacter = new Peach { Hp = 100, Exp = 0 };
                        }; break;
                    case "Mario":
                        {
                            newCharacter = new Mario { Hp = 100, Exp = 0 };
                        }; break;
                    case "Luigi":
                        {
                            newCharacter = new Luigi { Hp = 100, Exp = 0 };
                        }; break;
                }
                if (newCharacter != null)
                {
                    if (index % criteria.NoToTransform == 0)
                    {
                        database.Add(newCharacter);
                        database.SaveChanges();
                    }
                    if (index <= noToDelete)
                    {
                        database.Remove(c);
                        database.SaveChanges();
                    }
                }

            }

            Console.WriteLine($"All {criteria.Name}s has been transformed to {noToAdd} {criteria.TransformTo}s");
        }
    }


}