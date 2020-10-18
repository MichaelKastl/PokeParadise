using PokeParadise.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PokeParadise
{
    [Serializable]
    public class Player
    {
        public string name;
        public ulong id;
        public int level;
        public int xp;
        public int coins;
        public List<Pokemon> pokemon;
        public List<Egg> eggs;
        public List<Egg> breedingEggs;
        public int imgPref;
        public int backupPref;
        public int thumbnailPref;
        public List<Item> inventory;
        public List<Berry> berryPouch;
        public List<Food> lunchBox;
        public int eggHatchSpeedMult;
        public int legendaryMult;
        public int eggMult;
        public int trainingMult;
        public int affectionMult;
        public int breedingTimerMult;
        public int cookingSuccessMult;
        public int breedingCenterUpgrades;
        public int skillPoints;
        public string breedingCenterName;
        public bool isDoingTrivia;
        public DateTimeOffset lastTrivia;

        public Player(string name, ulong id)
        {
            this.name = name;
            this.id = id;
            level = 1;
            xp = 0;
            coins = 0;
            pokemon = new List<Pokemon>();
            eggs = new List<Egg>();
            breedingEggs = new List<Egg>();
            inventory = new List<Item>();
            berryPouch = new List<Berry>();
            lunchBox = new List<Food>();
            eggHatchSpeedMult = 0;
            legendaryMult = 0;
            eggMult = 0;
            trainingMult = 0;
            affectionMult = 0;
            breedingTimerMult = 0;
            breedingCenterUpgrades = 0;
            skillPoints = 0;
            breedingCenterName = "The Farm";
            isDoingTrivia = false;
            lastTrivia = DateTimeOffset.MinValue;
        }
        public Player()
        {

        }
    }
}
