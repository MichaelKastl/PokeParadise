using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace PokeParadise.Classes
{
    [Serializable]
    public class EvolutionRequirement
    {
        public string evolvesFrom;
        public string speciesName;
        public string evoMethod;
        public int dexNumber;
        public int gender;
        public string heldItem;
        public string item;
        public string knownMove;
        public string knownMoveType;
        public string location;
        public int minAffection;
        public int minBeauty;
        public int minHappiness;
        public int minLevel;
        public bool needsOverworldRain;
        public string partySpecies;
        public string partyType;
        public int relativePhysicalStats;
        public string timeOfDay;
        public string tradeSpecies;
        public bool turnUpsideDown;

        public EvolutionRequirement()
        {

        }

        public EvolutionRequirement FetchEvoRequirements(int dexNo)
        {
            EvolutionRequirement eReq = new EvolutionRequirement();
            EvolutionRequirement[] o = FetchRequirementsArray();
            eReq = o[dexNo];
            return eReq;
        }

        public EvolutionRequirement[] FetchRequirementsArray()
        {
            EvolutionRequirement[] reqArray = new EvolutionRequirement[891];
            EvolutionRequirement empty = new EvolutionRequirement();
            reqArray[0] = empty;
            bool lastPokemon = false;
            int cnt = 1;
            using (var reader = new StreamReader(@"C:\Users\mkast\source\repos\PokeParadise\bin\Debug\netcoreapp3.1\evoData.csv"))
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
                    bool validDexNo = Int32.TryParse(values[0], out int dexNo);
                    EvolutionRequirement eReq = new EvolutionRequirement();
                    if (validDexNo)
                    {
                        eReq.dexNumber = dexNo;
                        eReq.speciesName = values[1];
                        eReq.evolvesFrom = values[2];
                        eReq.evoMethod = values[3];
                        if (values[4] != "")
                        {
                            eReq.minLevel = Convert.ToInt32(values[4]);
                        }
                        else
                        {
                            eReq.minLevel = 999;
                        }
                        if (values[5] != "")
                        {
                            eReq.gender = Convert.ToInt32(values[5]);
                        }
                        else
                        {
                            eReq.gender = 0;
                        }
                        if (values[6] != "")
                        {
                            eReq.minAffection = Convert.ToInt32(values[6]);
                        }
                        else
                        {
                            eReq.minAffection = 999;
                        }
                        if (values[7] != "")
                        {
                            eReq.minBeauty = Convert.ToInt32(values[7]);
                        }
                        else
                        {
                            eReq.minBeauty = 999;
                        }
                        if (values[8] != "")
                        {
                            eReq.minHappiness = Convert.ToInt32(values[8]);
                        }
                        else
                        {
                            eReq.minHappiness = 999;
                        }
                        eReq.location = values[9];
                        eReq.item = values[10];
                        eReq.heldItem = values[11];
                        eReq.knownMove = values[12];
                        eReq.knownMoveType = values[13];
                        if (values[14] != "")
                        {
                            eReq.needsOverworldRain = Convert.ToBoolean(values[14]);
                        }
                        else
                        {
                            eReq.needsOverworldRain = false;
                        }
                        eReq.partySpecies = values[15];
                        eReq.partyType = values[16];
                        if (values[17] != "")
                        {
                            eReq.relativePhysicalStats = Convert.ToInt32(values[17]);
                        }
                        else
                        {
                            eReq.relativePhysicalStats = 999;
                        }
                        eReq.tradeSpecies = values[18];
                        if (values[15] != "")
                        {
                            eReq.turnUpsideDown = Convert.ToBoolean(values[19]);
                        }
                        else
                        {
                            eReq.turnUpsideDown = false;
                        }
                        eReq.timeOfDay = values[20];
                        reqArray[eReq.dexNumber] = eReq;
                        if (cnt >= 890)
                        {
                            lastPokemon = true;
                        }
                        else
                        {
                            lastPokemon = false;
                        }
                    }
                }
            }
            return reqArray;
        }
    }
}
