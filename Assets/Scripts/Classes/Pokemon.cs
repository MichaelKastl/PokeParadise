using System;
using System.Collections.Generic;
using System.Linq;
using PokeParadise.Classes;

namespace PokeParadise
{
    [Serializable]
    public class Pokemon
    {
        // Fields set per-pokemon
        public int id;
        public string nickname;
        public string gender;
        public string ability;
        public string nature;
        public string favoriteFlavor;
        public string hatedFlavor;
        public uint personalityValue;
        public int level;
        public int xp;
        public int statTotal;
        public int statHP;
        public int statAtk;
        public int statDef;
        public int statSpAtk;
        public int statSpDef;
        public int statSpd;
        public bool isShiny;
        public int hpIv;
        public int atkIv;
        public int defIv;
        public int spAtkIv;
        public int spDefIv;
        public int spdIv;
        public Pokemon parent1;
        public Pokemon parent2;
        public int affection;
        public int friendship;
        public bool hasPoffin;
        public bool isBreeding;
        public bool isLoaned;
        public string whenLoaned;
        public string timeToLoan;
        public string trainer;
        public int originalId;
        public Move[] moves;

        // Fields set per-species
        public int dexNo;
        public string pkmnName;
        public string type1;
        public string type2;
        public string speciesAbility1;
        public string speciesAbility2;
        public int baseStatTotal;
        public int baseHP;
        public int baseAtk;
        public int baseDef;
        public int baseSpAtk;
        public int baseSpDef;
        public int baseSpd;
        public bool legendStatus;
        public bool mythicStatus;
        public List<Evolution> evolutions;
        public string eggGroup1;
        public string eggGroup2;
        public string levelSpeed;
        public int genderThreshhold;
        public string blurb;
        public List<MoveInfo> learnset;

        //Fields for Contests
        public int sheen;
        public int coolness;
        public int cleverness;
        public int beauty;
        public int cuteness;
        public int toughness;

        //Fields for Care
        public string lastPetCycle;
        public string lastFed;
        public string lastWalked;
        public int petsThisHour;
        public int feedsThisHour;
        public int walksThisHour;
        public int petsBeforeAnger;

        public Pokemon()
        {
        }

        public Pokemon(int dexNo)
        {
            Pokemon p = SpeciesData(dexNo);
            if (p.pkmnName != null)
            {
                string firstLet = p.pkmnName.Substring(0, 1).ToUpper();
                string name = firstLet + p.pkmnName.Substring(1);
                pkmnName = name;
                nickname = name;
                this.dexNo = p.dexNo;
                type1 = p.type1;
                type2 = p.type2;
                baseStatTotal = p.baseStatTotal;
                baseHP = p.baseHP;
                baseAtk = p.baseAtk;
                baseDef = p.baseDef;
                baseSpAtk = p.baseSpAtk;
                baseSpDef = p.baseSpDef;
                baseSpd = p.baseSpd;
                speciesAbility1 = p.speciesAbility1;
                speciesAbility2 = p.speciesAbility2;
                legendStatus = p.legendStatus;
                mythicStatus = p.mythicStatus;
                evolutions = p.evolutions;
                genderThreshhold = p.genderThreshhold;
                eggGroup1 = p.eggGroup1;
                eggGroup2 = p.eggGroup2;
                levelSpeed = p.levelSpeed;
                genderThreshhold = p.genderThreshhold;
                blurb = p.blurb;
                level = 1;
                moves = p.moves;
                learnset = p.learnset;
            }
        }

        public Pokemon(string pkmnName)
        {
            Pokemon p = SpeciesDataByName(pkmnName);
            if (p.dexNo != 0)
            {
                string firstLet = pkmnName.Substring(0, 1).ToUpper();
                string name = firstLet + pkmnName.Substring(1);
                this.pkmnName = name;
                nickname = name;
                dexNo = p.dexNo;
                type1 = p.type1;
                type2 = p.type2;
                baseStatTotal = p.baseStatTotal;
                baseHP = p.baseHP;
                baseAtk = p.baseAtk;
                baseDef = p.baseDef;
                baseSpAtk = p.baseSpAtk;
                baseSpDef = p.baseSpDef;
                baseSpd = p.baseSpd;
                speciesAbility1 = p.speciesAbility1;
                speciesAbility2 = p.speciesAbility2;
                legendStatus = p.legendStatus;
                mythicStatus = p.mythicStatus;
                evolutions = p.evolutions;
                genderThreshhold = p.genderThreshhold;
                eggGroup1 = p.eggGroup1;
                eggGroup2 = p.eggGroup2;
                levelSpeed = p.levelSpeed;
                genderThreshhold = p.genderThreshhold;
                blurb = p.blurb;
                level = 1;
                moves = p.moves;
                learnset = p.learnset;
            }
        }

        public static Pokemon SpeciesData(int dexNo)
        {
            Pokemon p = new Pokemon();
            Pokemon[] pokedex = PlayerData.FetchPokedex();
            if (dexNo == 0)
            {
                dexNo = 1;
            }
            if (dexNo <= pokedex.Length && dexNo > 0)
            {
                p = pokedex[dexNo];
            }
            return p;
        }

        public static Pokemon SpeciesDataByName(string species)
        {
            species = species.Substring(0, 1).ToUpper() + species.Substring(1);
            Pokemon p = new Pokemon();
            foreach (Pokemon x in PlayerData.FetchPokedex())
            {
                if (x.pkmnName == species)
                {
                    p = x;
                }
            }
            return p;
        }

        /*public Move[] GenerateMoveset(int pokemonId)
        {
            Classes.Move[] moves = new Classes.Move[4];
            Dictionary<int, Classes.Move> moveSet = await GetPokemonMovesetAsync(pokemonId);
            Dictionary<int, KeyValuePair<int, Classes.Move>> orderedMoveSet = new Dictionary<int, KeyValuePair<int, Classes.Move>>();
            int cnt = 1;
            foreach (KeyValuePair<int, Classes.Move> m in moveSet)
            {
                orderedMoveSet.Add(cnt, new KeyValuePair<int, Classes.Move>(m.Key, m.Value));
                cnt++;
            }
            Random rand = new Random();
            moves[0] = orderedMoveSet[rand.Next(1, orderedMoveSet.Count())].Value;
            moves[1] = orderedMoveSet[rand.Next(1, orderedMoveSet.Count())].Value;
            moves[2] = orderedMoveSet[rand.Next(1, orderedMoveSet.Count())].Value;
            moves[3] = orderedMoveSet[rand.Next(1, orderedMoveSet.Count())].Value;
            return moves;
        }

        public List<Move> GetPokemonMoveset(int pokemonId)
        {
            PokeApiClient pokeClient = new PokeApiClient();
            int cnt = 1;
            Dictionary<int, Classes.Move> moveSet = new Dictionary<int, Classes.Move>();
            try
            {
                PokeApiNet.Pokemon target = await pokeClient.GetResourceAsync<PokeApiNet.Pokemon>(pokemonId);
                List<PokemonMove> pokeApiMoveset = target.Moves;
                foreach (PokemonMove x in pokeApiMoveset)
                {
                    Classes.Move move = new Classes.Move(x.Move.Name);
                    move.name = x.Move.Name;
                    move.learnedLevel = x.VersionGroupDetails.LastOrDefault().LevelLearnedAt;
                    if (move.id != 0)
                    {
                        moveSet.Add(move.id, move);
                    }
                    cnt++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Pokemon not found. {ex}");
            }
            pokeClient.ClearCache();
            return moveSet;
        }*/
    }
}