using PokeParadise.Classes;
using System;
using System.Collections.Generic;

namespace PokeParadise
{
    [Serializable]
    public class Player
    {
        public string name;
        public int level;
        public int xp;
        public int coins;
        public List<Pokemon> pokemon;
        public Pokemon[] shownPokemon;
        public List<Egg> eggs;
        public List<Egg> breedingEggs;
        public List<Item> inventory;
        public int imgPref;
        public int backupPref;
        public int thumbnailPref;
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
        public string lastTrivia;

        public Player(string name)
        {
            this.name = name;
            level = 1;
            xp = 0;
            coins = 0;
            pokemon = new List<Pokemon>();
            shownPokemon = new Pokemon[21];
            eggs = new List<Egg>();
            breedingEggs = new List<Egg>();
            eggHatchSpeedMult = 0;
            legendaryMult = 0;
            eggMult = 0;
            trainingMult = 0;
            affectionMult = 0;
            breedingTimerMult = 0;
            breedingCenterUpgrades = 0;
            skillPoints = 0;
            breedingCenterName = "Daycare";
            isDoingTrivia = false;
            lastTrivia = DateTimeOffset.MinValue.ToString();
            inventory = new List<Item>();
        }
        public Player()
        {
        }
    }
}
