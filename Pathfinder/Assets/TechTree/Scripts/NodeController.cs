using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class NodeController : MonoBehaviour {
    #if UNITY_EDITOR
    //All Dem Fancy Editor Things
    public GameObject prefab = null;
    public GameObject hoverPrefab = null;
    public int numberOfNodes = 0;
    public float initialAngle = -90f;
    public float angle = 0f;
    public bool inverted = false;
    public Color lineColor = Color.red;
    #endif

    private List<string> names;
    private List<NodeDataContainer> nodes;

    public bool hasCenter = true;

    public delegate void CallBack(NodeDataContainer ndc);
    private CallBack cBack;

    void Start()
    {
        NodeDataContainer ndc;
        nodes = new List<NodeDataContainer>();
        names = new List<string>();
        int index;
        if (hasCenter)
        {
            ndc = transform.FindChild("CentralNode").GetComponent<NodeDataContainer>();
            if (ndc != null)
            {
                names.Add(ndc.title);
                nodes.Add(ndc);
                Button b = ndc.gameObject.GetComponent<Button>();
                int newIndex = ndc.location;
                b.onClick.AddListener(() => { ButtonClickie(newIndex); });
            }
            else
            {
                hasCenter = false;
            }
        }
        index = 1;
        Transform t = transform.FindChild("Node " + index);
        while (t != null)
        {
            ndc = t.FindChild("Image").GetComponent<NodeDataContainer>();
            names.Add(ndc.title);
            nodes.Add(ndc);
            Button b = ndc.gameObject.GetComponent<Button>();
            int newIndex = ndc.location;
            b.onClick.AddListener(() => { ButtonClickie(newIndex); });
            index++;
            t = transform.FindChild("Node " + index);
        }
    }

    public void SetCallBack(CallBack callback)
    {
        cBack = callback;
    }

    public void ButtonClickie(int location)
    {
        cBack(nodes[location - (hasCenter ? 0 : 1)]);
    }

    public string GetName(int location)
    {
        return names[location - (hasCenter ? 0 : 1)];
    }

    public bool IsSkillActive(string name)
    {
        int location = names.IndexOf(name);
        if (location == -1)
            return false;
        return nodes[location].IsActive();
    }

    public void ShowSkill(string name, bool instant)
    {
        int location = names.IndexOf(name);
        if (location == -1)
            return;
        nodes[location].Animate(true, instant);
    }

    public void HideSkill(string name, bool instant)
    {
        int location = names.IndexOf(name);
        if (location == -1)
            return;
        nodes[location].Animate(false, instant);
    }

    public void SetSkillState(string name, bool isEnabled)
    {
        int location = names.IndexOf(name);
        if (location == -1)
            return;
        nodes[location].SetState(isEnabled);
    }

    public void SetActive(string name, bool active)
    {
        int location = names.IndexOf(name);
        if (location != -1)
            return;
        nodes[location].SetActive(active);
    }

    public NodeDataContainer GetNodeMouseOver()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].IsInteractable() && nodes[i].IsMouseOver())
                return nodes[i];
        }
        return null;
    }
}