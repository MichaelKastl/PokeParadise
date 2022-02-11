using PokeParadise.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace PokeParadise
{
    public static class MovesOpen
    {
        public static void Open(int id)
        {
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            PlayerData.ClosePanels();
            panels["PkmnMovesWrapper"].transform.GetChild(0).gameObject.SetActive(true);
            Player player = PlayerData.FetchCurrentPlayerData();
            Pokemon p = player.pokemon.Where(x => x.id == id).First();
            GameObject.Find("MovesSprite").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(p.dexNo.ToString() + "_menu");
            GameObject.Find("MovesDexNoText").GetComponent<Text>().text = p.dexNo.ToString();
            GameObject.Find("MovesNameText").GetComponent<Text>().text = p.pkmnName;
            GameObject.Find("MovesIDText").GetComponent<Text>().text = p.id.ToString();
            GameObject.Find("MovesLevelText").GetComponent<Text>().text = p.level.ToString();
            GameObject.Find("MovesAbilityText").GetComponent<Text>().text = p.ability;
            for (int i = 1; i <= 4; i++)
            {
                GameObject panel = GameObject.Find("Moves" + i + "Background");
                foreach (Transform obj in panel.transform)
                {
                    GameObject.Destroy(obj.gameObject);
                }
                if (p.moves[i - 1] != null)
                {
                    Move m = p.moves[i - 1];
                    panel.GetComponent<Image>().color = Color.white;
                    GameObject moveName = new GameObject("Moves1Name");
                    moveName.AddComponent<RectTransform>().SetParent(panel.transform);
                    string mName = m.name[0].ToString().ToUpper() + m.name.Substring(1);
                    if (mName.Contains("-"))
                    {
                        mName = mName.Replace("-", " ");
                        mName = mName.Substring(0, mName.IndexOf(" ") + 1) + mName[mName.IndexOf(" ") + 1].ToString().ToUpper() + mName.Substring(mName.IndexOf(" ") + 2);
                    }
                    Text moveNameTxt = moveName.AddComponent<Text>();
                    moveNameTxt.text = mName;
                    moveNameTxt.fontSize = 16;
                    moveNameTxt.font = Resources.Load<Font>("Fonts/PKMN RBYGSC");
                    moveNameTxt.color = Color.black;
                    moveNameTxt.alignment = TextAnchor.MiddleCenter;
                    RectTransform moveNameRect = moveName.GetComponent<RectTransform>();
                    moveNameRect.anchorMin = new Vector2(0.5f, 0.5f);
                    moveNameRect.anchorMax = new Vector2(0.5f, 0.5f);
                    moveNameRect.sizeDelta = new Vector2(190.6f, 60);
                    moveNameRect.localPosition = new Vector2(8.34f, 0);
                    moveNameRect.localScale = new Vector2(1, 1);
                    GameObject moveType = new GameObject("Moves1Type");
                    RectTransform moveTypeRect = moveType.AddComponent<RectTransform>();
                    moveType.AddComponent<Image>().sprite = Resources.Load<Sprite>(m.contestType);
                    moveTypeRect.SetParent(panel.transform);
                    moveTypeRect.sizeDelta = new Vector2(82.75f, 20);
                    moveTypeRect.localScale = new Vector2(1, 1);
                    moveTypeRect.localPosition = new Vector2(-138.53f, 0);
                    GameObject button = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("MoveInfoButton"));
                    RectTransform buttonRect = button.GetComponent<RectTransform>();
                    buttonRect.SetParent(panel.transform);
                    buttonRect.sizeDelta = new Vector2(72.42f, 30);
                    buttonRect.localPosition = new Vector2(148.7f, 15);
                    button.GetComponent<Button>().onClick.AddListener(delegate { MoveInfoOpen(m); });
                    buttonRect.localScale = new Vector2(1, 1);
                    GameObject deleteButton = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("MoveDeleteButton"));
                    RectTransform deleteButtonRect = deleteButton.GetComponent<RectTransform>();
                    deleteButtonRect.SetParent(panel.transform);
                    deleteButtonRect.sizeDelta = new Vector2(72.42f, 30);
                    deleteButtonRect.localPosition = new Vector2(148.7f, -15);
                    deleteButton.GetComponent<Button>().onClick.AddListener(delegate { MoveDelete(m, p.id); });
                    deleteButtonRect.localScale = new Vector2(1, 1);
                }
                else
                {
                    panel.GetComponent<Image>().color = new Color(.850980392f, .843137255f, .843137255f, 1);
                    GameObject button = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("AddMoveButton"));
                    RectTransform buttonRect = button.GetComponent<RectTransform>();
                    buttonRect.SetParent(panel.transform);
                    buttonRect.sizeDelta = new Vector2(160, 30);
                    buttonRect.localPosition = new Vector2(0, 0);
                    button.GetComponent<Button>().onClick.AddListener(delegate { MoveSelectOpen(p.id); });
                    buttonRect.localScale = new Vector2(1, 1);
                }
            }
        }

        public static void MoveDelete(Move m, int pokeId)
        {
            Player player = PlayerData.FetchCurrentPlayerData();
            foreach (Pokemon p in player.pokemon)
            {
                if (p.id == pokeId)
                {
                    p.moves[Array.IndexOf(p.moves, Array.Find(p.moves, x => x.id == m.id))] = null;
                    Move[] newMoves = new Move[4];
                    int validIndex = 0;
                    for (int i = 0; i <= 3; i++)
                    {
                        if (p.moves[i] != null)
                        {
                            newMoves[validIndex] = p.moves[i];
                            validIndex++;
                        }
                    }
                    p.moves = newMoves;
                }
            }
            foreach (Pokemon p in player.shownPokemon)
            {
                if (p.id == pokeId)
                {
                    p.moves[Array.IndexOf(p.moves, Array.Find(p.moves, x => x.id == m.id))] = null;
                    Move[] newMoves = new Move[4];
                    int validIndex = 0;
                    for (int i = 0; i <= 3; i++)
                    {
                        if (p.moves[i] != null)
                        {
                            newMoves[validIndex] = p.moves[i];
                            validIndex++;
                        }
                    }
                    p.moves = newMoves;
                }
            }
            PlayerData.SavePlayerData(player);
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            panels["PkmnMovesWrapper"].transform.GetChild(0).gameObject.SetActive(false);
            Open(pokeId);
        }

        public static void MoveInfoOpen(Move move)
        {
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            foreach (Transform obj in panels["PkmnMovesWrapper"].transform.GetChild(0).transform)
            {
                if (obj.gameObject.name == "MoveInfoPanel")
                {
                    obj.gameObject.SetActive(true);
                }
                else if (obj.gameObject.name == "MoveSelectPanel")
                {
                    obj.gameObject.SetActive(false);
                }
            }
            string moveName = move.name[0].ToString().ToUpper() + move.name.Substring(1);
            if (moveName.Contains("-"))
            {
                moveName = moveName.Replace("-", " ");
                moveName = moveName.Substring(0, moveName.IndexOf(" ") + 1) + moveName[moveName.IndexOf(" ") + 1].ToString().ToUpper() + moveName.Substring(moveName.IndexOf(" ") + 2);
            }
            GameObject.Find("MoveInfoName").GetComponent<Text>().text = moveName;
            GameObject.Find("MoveInfoContestType").GetComponent<Image>().sprite = Resources.Load<Sprite>(move.contestType);
            GameObject.Find("MoveInfoDesc").GetComponent<Text>().text = move.flavorText;
        }

        public static void MoveSelectOpen(int id)
        {
            Player player = PlayerData.FetchCurrentPlayerData();
            Pokemon p = player.pokemon.Where(x => x.id == id).First();
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            foreach (Transform obj in panels["PkmnMovesWrapper"].transform.GetChild(0))
            {
                if (obj.gameObject.name == "MoveInfoPanel")
                {
                    obj.gameObject.SetActive(false);
                }
                else if (obj.gameObject.name == "MoveSelectPanel")
                {
                    obj.gameObject.SetActive(true);
                    GameObject panel = GameObject.Find("MoveSelectMovesPanel");
                    foreach (Transform o in panel.transform)
                    {
                        UnityEngine.Object.Destroy(o.gameObject);
                    }
                    int openIndex = 0;
                    List<int> alreadyLearned = new List<int>();
                    for (int i = 0; i <= 3; i++)
                    {
                        if (p.moves[i] == null)
                        {
                            openIndex = i;
                            break;
                        }
                        else
                        {
                            alreadyLearned.Add(p.moves[i].id);
                        }
                    }
                    //-44.855, get to -67.5
                    float x = 80.6f;
                    float y = -15f;
                    int cnt = 1;
                    MoveInfo[] movesLearnable = p.learnset.Where(x => x.learnedLevel <= p.level && x.learnedLevel != 0).ToArray();
                    List<int> learnsetIDs = movesLearnable.Select(x => x.id).ToList();
                    List<Move> learnset = PlayerData.FetchMoveDex().Where(x => learnsetIDs.Contains(x.id)).ToList();
                    int totalMoves = learnset.Count(x => !alreadyLearned.Contains(x.id));
                    if (totalMoves >= 6)
                    {
                        obj.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(273.43f, (totalMoves * 25) + 15);
                    }
                    foreach (Move mv in learnset.Where(x => !alreadyLearned.Contains(x.id)))
                    {
                        GameObject name = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("MoveSelectName"));
                        name.GetComponent<RectTransform>().SetParent(panel.transform);
                        name.name = "MoveSelectName" + mv.id;
                        string moveName = mv.name[0].ToString().ToUpper() + mv.name.Substring(1);
                        if (moveName.Contains("-"))
                        {
                            moveName = moveName.Replace("-", " ");
                            moveName = moveName.Substring(0, moveName.IndexOf(" ") + 1) + moveName[moveName.IndexOf(" ") + 1].ToString().ToUpper() + moveName.Substring(moveName.IndexOf(" ") + 2);
                        }
                        name.GetComponent<Text>().text = moveName;
                        RectTransform nameRect = name.GetComponent<RectTransform>();
                        if (cnt == 1)
                        {
                            nameRect.localPosition = new Vector2(x, y);
                        }
                        else
                        {
                            y -= 25;
                            nameRect.localPosition = new Vector2(x, y);
                        }
                        nameRect.sizeDelta = new Vector2(147, 14.76f);
                        nameRect.localScale = new Vector2(1, 1);
                        GameObject addButton = GameObject.Instantiate(Resources.Load<GameObject>("MoveSelectAdd"));
                        addButton.name = "MoveSelectAdd" + mv.id;
                        RectTransform addRect = addButton.GetComponent<RectTransform>();
                        addRect.SetParent(name.transform);
                        addButton.GetComponent<Button>().onClick.AddListener(delegate { AddMove(p.id, mv.id, openIndex, learnset); });
                        addRect.localPosition = new Vector2(164.1f, 0);
                        addRect.localScale = new Vector2(1, 1);
                        GameObject line = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("MoveSelectLine"));
                        RectTransform lineRect = line.GetComponent<RectTransform>();
                        line.name = "MoveSelectLine" + mv.id;
                        lineRect.SetParent(name.transform);
                        lineRect.localPosition = new Vector2(57.63f, -12.84f);
                        lineRect.localScale = new Vector2(1, 1);
                        lineRect.sizeDelta = new Vector2(265.21f, 0.67f);
                        GameObject typeImage = new GameObject("TypeImage");
                        typeImage.AddComponent<Image>().sprite = Resources.Load<Sprite>(mv.contestType);
                        RectTransform imgRect = typeImage.GetComponent<RectTransform>();
                        imgRect.SetParent(name.transform);
                        imgRect.sizeDelta = new Vector2(50, 20);
                        imgRect.localScale = new Vector2(1, 1);
                        imgRect.localPosition = new Vector2(105, 0);
                        cnt++;
                    }
                }
            }
        }

        public static void AddMove(int pokeId, int mvId, int openIndex, List<Move> learnset)
        {
            Player player = PlayerData.FetchCurrentPlayerData();
            foreach (Pokemon p in player.pokemon)
            {
                if (p.id == pokeId)
                {
                    p.moves[openIndex] = learnset.Where(x => x.id == mvId).First();
                }
            }
            foreach (Pokemon p in player.shownPokemon)
            {
                if (p.id == pokeId)
                {
                    p.moves[openIndex] = learnset.Where(x => x.id == mvId).First();
                }
            }
            PlayerData.SavePlayerData(player);
            GameObject.Find("MoveSelectPanel").SetActive(false);
            Dictionary<string, GameObject> panels = GameObject.Find("PanelHolder").GetComponent<PanelHolder>().panels;
            panels["PkmnMovesWrapper"].transform.GetChild(0).gameObject.SetActive(false);
            Open(pokeId);
        }
    }
}