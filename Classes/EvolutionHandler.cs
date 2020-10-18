using PokeParadise.Modules;
using PokeParadise.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Runtime;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace PokeParadise.Classes
{
    public class EvolutionHandler
    {
        public EvolvePkg EvolveCheck(Pokemon p, Player player, bool wasTraded = false, Pokemon tradedPokemon = null, string item = "empty")
        {
            EvolvePkg ep = new EvolvePkg();
            if (p.nextEvo != "" && p.nextEvo != " " && p.nextEvo != null)
            {
                Environment serverEnvironment = new Environment();
                string[] lines = File.ReadAllLines(@"C:\Users\mkast\source\repos\PokeParadise\bin\Debug\netcoreapp3.1\environment.txt");
                int cnt = 1;
                foreach (string line in lines)
                {
                    switch (cnt)
                    {
                        case 1: serverEnvironment.weather = line; break;
                        case 2: serverEnvironment.timeOfDay = line; break;
                    }
                }
                EvolutionRequirement eReq = new EvolutionRequirement();
                Pokemon evolvesTo = new Pokemon(p.nextEvo);
                eReq = eReq.FetchEvoRequirements(evolvesTo.dexNo);
                if (eReq.item != null && eReq.item != "" && eReq.item != " ")
                {
                    eReq.item = eReq.item.Replace("-", " ");
                }
                if (eReq.heldItem != null && eReq.heldItem != "" && eReq.heldItem != " ")
                {
                    eReq.heldItem = eReq.heldItem.Replace("-", " ");
                }
                if (eReq.knownMove != null && eReq.knownMove != "" && eReq.knownMove != " ")
                {
                    eReq.knownMove = eReq.knownMove.Replace("-", " ");
                }
                if (eReq.location != null && eReq.location != "" && eReq.location != " ")
                {
                    eReq.location = eReq.location.Replace("-", " ");
                }
                if (p.level >= eReq.minLevel && p.evoLevel > 0)
                {
                    string eReqGender = "";
                    if (eReq.gender == 1) { eReqGender = "female"; }
                    else if (eReq.gender == 2) { eReqGender = "male"; }

                    if (p.gender == eReqGender) { ep = new EvolvePkg(eReq.dexNumber, true); }

                    if (eReq.timeOfDay == serverEnvironment.timeOfDay) { ep = new EvolvePkg(eReq.dexNumber, true); }

                    foreach (Pokemon x in player.pokemon) { if (x.dexNo == 223) { ep = new EvolvePkg(eReq.dexNumber, true); } if (x.type1 == "Dark" || x.type2 == "Dark") { ep = new EvolvePkg(eReq.dexNumber, true); } }

                    if (p.dexNo == 236)
                    {
                        switch (eReq.relativePhysicalStats)
                        {
                            case -1:
                                if (p.statAtk < p.statDef) { ep = new EvolvePkg(eReq.dexNumber, true); }
                                break;
                            case 0:
                                if (p.statAtk > p.statDef) { ep = new EvolvePkg(eReq.dexNumber, true); }
                                break;
                            case 1:
                                if (p.statAtk == p.statDef) { ep = new EvolvePkg(eReq.dexNumber, true); }
                                break;
                        }
                    }

                    if (eReq.minAffection > 0)
                    {
                        if (p.affection >= eReq.minAffection) { ep = new EvolvePkg(eReq.dexNumber, true); }
                    }

                    if (eReq.minHappiness > 0)
                    {
                        if (p.friendship >= eReq.minHappiness) { ep = new EvolvePkg(eReq.dexNumber, true); }
                    }

                    if (eReq.needsOverworldRain && (serverEnvironment.weather == "rainy" || serverEnvironment.weather == "thunderstorm" || serverEnvironment.weather == "cloudy" || serverEnvironment.weather == "fog"))
                    { ep = new EvolvePkg(eReq.dexNumber, true); }

                    if (eReq.turnUpsideDown && item == "upside-down button") { ep = new EvolvePkg(eReq.dexNumber, true); }
                }

                if (p.level >= eReq.minLevel && (eReq.minLevel > 0 || eReq.minLevel != 999))
                {
                    ep = new EvolvePkg(eReq.dexNumber, true);
                }
                if (item.ToLower() == eReq.item)
                {
                    string eReqGender = "";
                    if (eReq.gender == 1) { eReqGender = "female"; }
                    else if (eReq.gender == 2) { eReqGender = "male"; }
                    if (p.gender == eReqGender || eReqGender == "") { ep = new EvolvePkg(eReq.dexNumber, true); }
                }
                if (wasTraded && eReq.evoMethod == "trade")
                {
                    if (eReq.tradeSpecies != null)
                    {
                        if (tradedPokemon.pkmnName == eReq.tradeSpecies)
                        {
                            if (eReq.heldItem != null)
                            {
                                if (eReq.heldItem == p.heldItem.name)
                                { ep = new EvolvePkg(eReq.dexNumber, true); }
                            }
                            else { ep = new EvolvePkg(eReq.dexNumber, true); }
                        }
                    }
                    else
                    {
                        if (eReq.heldItem != null)
                        {
                            if (eReq.heldItem == p.heldItem.name)
                            { ep = new EvolvePkg(eReq.dexNumber, true); }
                        }
                        else { ep = new EvolvePkg(eReq.dexNumber, true); }
                    }
                }
                //STILL NEED TO HANDLE LOCATIONS AND MOVES/MOVE TYPES
            }
            else
            {
                ep.evolvedTo = new Pokemon();
                ep.hasEvolved = false;
            }
            return ep;
        }

        public class EvolvePkg
        {
            public Pokemon evolvedTo;
            public bool hasEvolved;
            public EvolvePkg() { }
            public EvolvePkg(int species, bool hasEvolved) { evolvedTo = new Pokemon(species); this.hasEvolved = hasEvolved; }
        }
    }
}
