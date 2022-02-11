using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileChecker : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        string appData = Application.persistentDataPath;
        if (!Directory.Exists($"{appData}/PokeParadise_Saves"))
        {
            Directory.CreateDirectory($"{appData}/PokeParadise_Saves");
        }
        if (!Directory.Exists($"{appData}/PokeParadise_Data"))
        {
            Directory.CreateDirectory($"{appData}/PokeParadise_Data");
        }
        if (!File.Exists($"{appData}/PokeParadise_Data/pokedex.xml"))
        {
            File.WriteAllText($"{appData}/PokeParadise_Data/pokedex.xml", Resources.Load<TextAsset>("pokedex").text);
        }
        if (!File.Exists($"{appData}/PokeParadise_Data/berries.xml"))
        {
            File.WriteAllText($"{appData}/PokeParadise_Data/berries.xml", Resources.Load<TextAsset>("berries").text);
        }
        if (!File.Exists($"{appData}/PokeParadise_Data/xpToLevel.xml"))
        {
            File.WriteAllText($"{appData}/PokeParadise_Data/xpToLevel.xml", Resources.Load<TextAsset>("xpToLevel").text);
        }
        if (!File.Exists($"{appData}/PokeParadise_Data/xpAtLevel.xml"))
        {
            File.WriteAllText($"{appData}/PokeParadise_Data/xpAtLevel.xml", Resources.Load<TextAsset>("xpAtLevel").text);
        }
        if (!File.Exists($"{appData}/PokeParadise_Data/moveDex.xml"))
        {
            File.WriteAllText($"{appData}/PokeParadise_Data/moveDex.xml", Resources.Load<TextAsset>("moveDex").text);
        }
    }
}
