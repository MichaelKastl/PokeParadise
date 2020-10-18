using PokeApiNet;
using PokeParadise.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

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
        public Classes.Item heldItem;
        public bool isLoaned;
        public DateTimeOffset whenLoaned;
        public TimeSpan timeToLoan;
        public Player trainer;
        public int originalId;
        public Classes.Move[] moveSet;

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
        public int evoLevel;
        public bool legendStatus;
        public bool mythicStatus;
        public string nextEvo;
        public string prevEvo;
        public string eggGroup1;
        public string eggGroup2;
        public string levelSpeed;
        public int genderThreshhold;
        public string blurb;
        public string evoMethod;
        public string evoMethodRequirement1;
        public string evoMethodRequirement2;
        public Dictionary<int, Classes.Move> moveList;

        //Fields for Contests
        public int sheen;
        public int coolness;
        public int cleverness;
        public int beauty;
        public int cuteness;
        public int toughness;

        public Pokemon()
        {

        }

        public Pokemon (int dexNo)
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
                evoLevel = p.evoLevel;
                speciesAbility1 = p.speciesAbility1;
                speciesAbility2 = p.speciesAbility2;
                legendStatus = p.legendStatus;
                mythicStatus = p.mythicStatus;
                nextEvo = p.nextEvo;
                prevEvo = p.prevEvo;
                genderThreshhold = p.genderThreshhold;
                eggGroup1 = p.eggGroup1;
                eggGroup2 = p.eggGroup2;
                levelSpeed = p.levelSpeed;
                genderThreshhold = p.genderThreshhold;
                blurb = p.blurb;
                evoMethod = p.evoMethod;
                evoMethodRequirement1 = p.evoMethodRequirement1;
                evoMethodRequirement2 = p.evoMethodRequirement2;
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
                evoLevel = p.evoLevel;
                speciesAbility1 = p.speciesAbility1;
                speciesAbility2 = p.speciesAbility2;
                legendStatus = p.legendStatus;
                mythicStatus = p.mythicStatus;
                nextEvo = p.nextEvo;
                prevEvo = p.prevEvo;
                genderThreshhold = p.genderThreshhold;
                eggGroup1 = p.eggGroup1;
                eggGroup2 = p.eggGroup2;
                levelSpeed = p.levelSpeed;
                genderThreshhold = p.genderThreshhold;
                blurb = p.blurb;
                evoMethod = p.evoMethod;
                evoMethodRequirement1 = p.evoMethodRequirement1;
                evoMethodRequirement2 = p.evoMethodRequirement2;
                moveList = new Dictionary<int, Classes.Move>();
                moveSet = new Classes.Move[4];
            }
        }

        public Pokemon SpeciesData(int dexNo)
        {
            Pokemon p = new Pokemon();
            Pokemon[] pokedex = FetchPokedex();
            if (dexNo == 0)
            {
                dexNo = 1;
            }
            if (dexNo <= pokedex.Count() && dexNo > 0)
            {
                p = pokedex[dexNo];
            }
            return p;
        }

        public Pokemon SpeciesDataByName(string species)
        {
            species = species.Substring(0, 1).ToUpper() + species.Substring(1);
            Pokemon p = new Pokemon();
            Pokemon[] pokedex = FetchPokedex();
            foreach (Pokemon x in pokedex)
            {
                if (x.pkmnName == species)
                {
                    p = x;
                }
            }
            return p;
        }

        public Pokemon[] FetchPokedex()
        {
            Pokemon[] pokedex = new Pokemon[891];
            int pokemonCount = 890;
            Pokemon empty = new Pokemon();
            pokedex[0] = empty;
            bool lastPokemon = false;
            int cnt = 0;
            using (var reader = new StreamReader(@"C:\Users\mkast\source\repos\PokeParadise\bin\Debug\netcoreapp3.1\pokedata.csv"))
            {
                while (!reader.EndOfStream && !lastPokemon)
                {
                    cnt++;
                    Pokemon p = new Pokemon();
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    string species = values[0];
                    string firstLet = species.Substring(0, 1).ToUpper();
                    species = firstLet + species.Substring(1);
                    p.pkmnName = species;
                    bool validDexNo = Int32.TryParse(values[1], out p.dexNo);
                    p.type1 = values[2];
                    p.type2 = values[3];
                    p.baseStatTotal = Convert.ToInt32(values[4]);
                    p.baseHP = Convert.ToInt32(values[5]);
                    p.baseAtk = Convert.ToInt32(values[6]);
                    p.baseDef = Convert.ToInt32(values[7]);
                    p.baseSpAtk = Convert.ToInt32(values[8]);
                    p.baseSpDef = Convert.ToInt32(values[9]);
                    p.baseSpd = Convert.ToInt32(values[10]);
                    p.eggGroup1 = values[11];
                    p.eggGroup2 = values[12];
                    if (values[13] != "")
                    {
                        p.evoLevel = Convert.ToInt32(values[13]);
                    }
                    else
                    {
                        p.evoLevel = 999;
                    }
                    p.speciesAbility1 = values[14];
                    p.speciesAbility2 = values[15];
                    p.levelSpeed = values[16];
                    p.genderThreshhold = Convert.ToInt32(values[17]);
                    p.legendStatus = false;
                    p.mythicStatus = false;
                    int statusIndicator = Convert.ToInt32(values[18]);
                    if (statusIndicator == 1)
                    {
                        p.mythicStatus = true;
                    }
                    else if (statusIndicator == 2)
                    {
                        p.legendStatus = true;
                    }
                    p.prevEvo = values[19];
                    p.nextEvo = values[20];
                    p.blurb = values[21];
                    if (values.Count() >= 22)
                    {
                        int count = 1;
                        while (count <= values.Count() - 22)
                        {
                            int index = count + 21;
                            p.blurb += "," + values[index];
                            count++;
                        }
                    }
                    pokedex[p.dexNo] = p;
                    if (cnt >= pokemonCount) 
                    { 
                        lastPokemon = true; 
                    } 
                    else 
                    { 
                        lastPokemon = false; 
                    }
                }
            }
            return pokedex;
        }

        public async Task<Classes.Move[]> GenerateMovesetAsync(int pokemonId)
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

        public async Task<Dictionary<int, Classes.Move>> GetPokemonMovesetAsync(int pokemonId)
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
        }
    }
}
