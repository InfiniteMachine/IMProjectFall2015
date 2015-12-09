using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class UpgradeStorage : MonoBehaviour {
    public Dictionary<string, bool> normalUpgrades;
    public Dictionary<string, bool> flyingUpgrades;
    public Dictionary<string, bool> diggingUpgrades;

    private bool init = false;

    private DataController data;

    public int normalFruit = 4, flyingFruit = 5, diggingFruit = 5;

    void Start()
    {
        normalUpgrades = new Dictionary<string, bool>();
        flyingUpgrades = new Dictionary<string, bool>();
        diggingUpgrades = new Dictionary<string, bool>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Go to sleep
            GameObject.FindGameObjectWithTag("Player").GetComponent<SphereController>().active = false;
            // Start sleep animation
            GameObject.Find("GameManager").GetComponent<DelayedEndDay>().activate();
        }
    }

    void OnLevelWasLoaded(int level)
    {
        if (Application.loadedLevelName == "TechTree")
        {
            InvokeNextFrame(TechTreeLoad);
        }
    }

    public void TechTreeLoad()
    {
        data = GameObject.Find("/TechTreeCanvas/Display").GetComponent<DataController>();
        if (!init)
        {
            foreach (string s in data.normalTree.GetNames())
            {
                normalUpgrades[s] = false;
            }
            foreach (string s in data.flyingTree.GetNames())
            {
                flyingUpgrades[s] = false;
            }
            foreach (string s in data.diggingTree.GetNames())
            {
                diggingUpgrades[s] = false;
            }
            init = true;
            data.InitializeMenu(normalFruit, diggingFruit, flyingFruit);
        }
        else
        {
            data.UpdateMenu(normalUpgrades, flyingUpgrades, diggingUpgrades, normalFruit, flyingFruit, diggingFruit);
        }
    }

    public void CloseTechTree()
    {
        for(int i = 1; i <= normalUpgrades.Count; i++)
        {
            normalUpgrades[data.normalTree.GetName(i)] = data.normalTree.IsSkillActive(data.normalTree.GetName(i));
        }
        for (int i = 0; i < flyingUpgrades.Count; i++)
        {
            flyingUpgrades[data.flyingTree.GetName(i)] = data.flyingTree.IsSkillActive(data.flyingTree.GetName(i));
            
        }
        foreach(KeyValuePair<string, bool> p in flyingUpgrades)
        {
            Debug.Log(p);
        }
        for (int i = 0; i < diggingUpgrades.Count; i++)
        {
            diggingUpgrades[data.diggingTree.GetName(i)] = data.diggingTree.IsSkillActive(data.diggingTree.GetName(i));
        }
    }

    public delegate void Function();

    public void InvokeNextFrame(Function function)
    {
        try
        {
            StartCoroutine(_InvokeNextFrame(function));
        }
        catch
        {
            Debug.Log("Trying to invoke " + function.ToString() + " but it doesnt seem to exist");
        }
    }

    private IEnumerator _InvokeNextFrame(Function function)
    {
        yield return null;
        function();
    }
}