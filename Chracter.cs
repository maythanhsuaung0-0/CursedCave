using System;
using System.Collections.Generic;

namespace MushroomPocket
{
    public class Character
    {

        private string name;
        private int hp;
        private int exp;
        private string skill;
        public int CharacterId { get; set; }
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }
        public int Hp
        {
            get
            {
                return this.hp;
            }
            set
            {
                this.hp = value;
            }
        }
        public int Exp
        {
            get
            {
                return this.exp;
            }
            set
            {
                this.exp = value;
            }
        }
        public string Skill
        {
            get
            {
                return this.skill;
            }
            set
            {
                this.skill = value;
            }
        }
        public Character()
        {
            this.Name = "";
            this.Hp = 100;
            this.Exp = 0;
            this.Skill = "";
        }
    }
}