using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class cursesSystem : MonoBehaviour
{
    [Header("saving curses")]
    public List<curse> curses;
    public List<curse> inUseCurses;

    int startingIndex;

    [Header("ui")]
    public Button playButton;
    public Button[] curseObjects;
    public GameObject curseParents;
    public GameObject curseDiscParent;
    public TextMeshProUGUI curseGood, curseBad;

    [Header("restarting")]
    public humanSpawner spawner;
    public planetGenerator planet;

    public int amount;
    public int increase;

    [Header("appling curses")]
    public Transform appliedCursePrefab;
    public Transform appliedCurseParent;

    [Serializable]public struct curse
    {
        public string name;
        public string good;
        public string bad;
    }

    public void win()
    {
        curseParents.SetActive(false);
        curseDiscParent.SetActive(false);

        startingIndex = UnityEngine.Random.Range(0, curses.Count);

        for (int i = 0; i < curseObjects.Length; i++)
        {
            int index = i;

            curseObjects[i].onClick.RemoveAllListeners();
            curseObjects[i].onClick.AddListener(() => addCurse(index));

            curseObjects[i].gameObject.SetActive(index < curses.Count);
        }

        if (curses.Count == 0)
        {
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(play);
        }
        else
        {
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(showCurses);
        }
    }

    void showCurses()
    {
        curseParents.SetActive(true);
        for (int i = 0; i < curseObjects.Length; i++)
        {
            string name = curses[(startingIndex + i) % curses.Count].name;
            curseObjects[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
        }
    }

    public void explainCurse(int i)
    {
        curseDiscParent.SetActive(true);

        int index = (startingIndex + i) % curses.Count;
        curseGood.text = curses[index].good;
        curseBad.text = curses[index].bad;
    }
    public void removeCurseDisc()
    {
        curseDiscParent.SetActive(false);
    }

    void addCurse(int i)
    {
        int index = (startingIndex + i) % curses.Count;

        inUseCurses.Add(curses[index]);
        curses.RemoveAt(index);

        Transform curseInstance = Instantiate(appliedCursePrefab, appliedCurseParent);
        curseInstance.GetChild(0).GetComponent<TextMeshProUGUI>().text = inUseCurses[inUseCurses.Count - 1].name;

        play();
    }

    void play()
    {
        StartCoroutine(FEEL.transitionWithoutScene());
        amount += increase;
        spawner.spawn(amount);
        planet.generate();
    }

    public bool hasCurse(string name)
    {
        return inUseCurses.Any(c => c.name == name);
    }
}
