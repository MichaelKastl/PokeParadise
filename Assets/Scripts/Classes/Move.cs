using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

namespace PokeParadise.Classes
{
    [Serializable]
    public class Move
    {
        public int id { get; set; }
        public string name { get; set; }
        public int learnedLevel { get; set; }
        public string flavorText { get; set; }
        public int appeal { get; set; }
        public string contestType { get; set; }

        public Move() { }

        public Move(int id)
        {
            Move m = FetchMovedex().Where(x => x.id == id).First();
            this.id = m.id;
            name = m.name;
            appeal = m.appeal;
            learnedLevel = m.learnedLevel;
            flavorText = m.flavorText;
            contestType = m.contestType;
        }

        public Move(string name)
        {
            name = name.ToLower();
            if (name.Contains(" "))
            {
                name = name.Replace(" ", "-");
            }
            Move m = FetchMovedex().Where(x => x.id == id).First();
            id = m.id;
            this.name = m.name;
            appeal = m.appeal;
            learnedLevel = m.learnedLevel;
            flavorText = m.flavorText;
            contestType = m.contestType;
        }

        public static List<Move> FetchMovedex()
        {
            List<Move> p = new List<Move>();
            XmlSerializer x = new XmlSerializer(p.GetType());
            string appData = Application.persistentDataPath;
            using (FileStream fs = new FileStream($"{appData}\\PokeParadise_Data\\moveDex.xml", FileMode.OpenOrCreate))
            {
                p = (List<Move>)x.Deserialize(fs);
            }
            return p;
        }
    }
}
