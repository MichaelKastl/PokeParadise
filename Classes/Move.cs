using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Text;

namespace PokeParadise.Classes
{
    [Serializable]
    public class Move
    {
        public int id { get; set; }
        public string name { get; set; }
        public int learnedLevel { get; set; }
        public string condition { get; set; }
        public string effect { get; set; }
        public int appeal { get; set; }
        public int jam { get; set; }

        public Move() { }

        public Move(int id)
        {
            Dictionary<int, Move> moveDict = FetchMoveDict();
            Move m = moveDict[id];
            this.id = m.id;
            name = m.name;
            condition = m.condition;
            effect = m.effect;
            appeal = m.appeal;
            jam = m.jam;
        }

        public Move(string name)
        {
            name = name.ToLower();
            if (name.Contains("-"))
            {
                name = name.Replace("-", " ");
            }
            Dictionary<int, Move> moveDict = FetchMoveDict();
            Move m = new Move();
            foreach (KeyValuePair<int, Move> kvp in moveDict)
            {
                if (name == kvp.Value.name)
                {
                    m = kvp.Value;
                    break;
                }
            }
            id = m.id;
            name = m.name;
            condition = m.condition;
            effect = m.effect;
            appeal = m.appeal;
            jam = m.jam;
        }

        public Dictionary<int, Move> FetchMoveDict()
        {
            Dictionary<int, Move> moveDict = new Dictionary<int, Move>();
            using (var reader = new StreamReader(@"C:\Users\mkast\source\repos\PokeParadise\bin\Debug\netcoreapp3.1\moveData.csv"))
            {
                int cnt = 0;
                while (!reader.EndOfStream && cnt <= 615)
                {
                    if (cnt >= 1)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        Move move = new Move();
                        move.id = Convert.ToInt32(values[0]);
                        move.name = values[1];
                        move.appeal = Convert.ToInt32(values[2]);
                        move.jam = Convert.ToInt32(values[3]);
                        move.condition = values[4];
                        move.effect = values[5];
                        if (values.Count() >= 6)
                        {
                            int count = 1;
                            while (count <= values.Count() - 6)
                            {
                                int index = count + 5;
                                move.effect += "," + values[index];
                                count++;
                            }
                        }
                        moveDict.Add(move.id, move);
                    }                   
                    cnt++;
                }
            }
            return moveDict;
        }       
    }
}
