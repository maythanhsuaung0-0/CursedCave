using System.Collections.Generic;
using System;

namespace MushroomPocket
{
    public class CursedCaveCharacter : Character
    {
        public List<Item> Inventory { get; set; }
        public Dictionary<string, double> SkillPoint { get; set; }
        public int ATK{get;set;}
        public CursedCaveCharacter(string name) : base()
        {
            this.Name = name;
            this.Inventory = new List<Item>();
            this.SkillPoint = new Dictionary<string, double>();
            InitializeSkillPoints();
        }

        private void InitializeSkillPoints()
        {
            // Initialize skill points with default values
            this.SkillPoint["Agility"] = 0.2;
            this.SkillPoint["Leadership"] = 0.2;
            this.SkillPoint["Strength"] = 0.4;
            this.SkillPoint["Precision and Accuracy"] = 0.4;
            this.SkillPoint["Magic Abilities"] = 0.5;
            this.SkillPoint["Combat Skills"] = 0.6;
        }
        public void AddItem(Item item)
        {
            this.Inventory.Add(item);
            Console.WriteLine($"You have obtained: {item.Name}");
        }

        public void AddSkillPoint(string key, double value)
        {
            // Update skill points
            if (this.SkillPoint.ContainsKey(key))
            {
                this.SkillPoint[key] += value;
            }
            else
            {
                Console.WriteLine($"Invalid skill: {key}");
            }
        }
        public int Damage(double sp){
              return Convert.ToInt32(this.ATK*sp);
        }
    }

    public class Item
    {
        public string Name { get; set; }

        public Item(string name)
        {
            this.Name = name;
        }
    }
}
