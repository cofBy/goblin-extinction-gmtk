using System;
using System.Collections.Generic;
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
            curseObjects[i].onClick.AddListener(() => addCurse(index));

            curseObjects[i].gameObject.SetActive(index > curses.Count);
        }

        playButton.onClick.AddListener(showCurses);
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

        amount += increase;
        spawner.spawn(amount);
        planet.generate();
    }
}
