using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace PokeParadise
{
    public class GameVariables
    {
        public string lastCheck;
        public string lastEggCheck;
        public string status;

        public void SaveGameVariables()
        {
            string appData = Application.persistentDataPath;
            if (!Directory.Exists($"{appData}/PokeParadise_Data"))
            {
                Directory.CreateDirectory($"{appData}/PokeParadise_Data");
            }
            XmlSerializer x = new XmlSerializer(GetType());
            File.WriteAllText($"{appData}/PokeParadise_Data/GameVariables.xml", "");
            using FileStream fs = new FileStream($"{appData}/PokeParadise_Data/GameVariables.xml", FileMode.OpenOrCreate);
            x.Serialize(fs, this);
        }

        public static GameVariables FetchGameVariables()
        {
            GameVariables variables = new GameVariables();
            string appData = Application.persistentDataPath;
            if (!Directory.Exists($"{appData}/PokeParadise_Data"))
            {
                Directory.CreateDirectory($"{appData}/PokeParadise_Data");
            }
            if (File.Exists($"{appData}/PokeParadise_Data/GameVariables.xml"))
            {
                XmlSerializer x = new XmlSerializer(variables.GetType());
                using (FileStream fs = new FileStream($"{appData}/PokeParadise_Data/GameVariables.xml", FileMode.OpenOrCreate))
                {
                    variables = (GameVariables)x.Deserialize(fs);
                }
            }
            return variables;
        }
    }
}
