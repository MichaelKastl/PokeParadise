using System;
using System.Collections.Generic;
using System.Text;

namespace PokeParadise.Classes
{
    public class ContestWrapper
    {
        public Player owner;
        public Pokemon pokemon;
        public Move move;
        public int turnOrder;
        public int pumpLevel;
        public int startleChance;
        public bool startlePrevent1;
        public bool startlePreventRound;
        public int nervousChance;
        public bool nervous;
        public bool moveLockRound;
        public bool moveLockContest;

        public ContestWrapper()
        {

        }

        public ContestWrapper(Player owner, Pokemon pokemon, Move move)
        {
            this.owner = owner;
            this.pokemon = pokemon;
            this.move = move;
            pumpLevel = 0;
            startleChance = 0;
            startlePrevent1 = false;
            startlePreventRound = false;
            nervous = false;
            nervousChance = 0;
            moveLockRound = false;
            moveLockContest = false;
        }
    }
}
