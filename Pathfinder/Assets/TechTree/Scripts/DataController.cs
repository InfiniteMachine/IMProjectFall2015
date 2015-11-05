using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DataController : MonoBehaviour {
    public MouseHover mouseHovery;

    public NodeController normalTree;
    public NodeController flyingTree;
    public NodeController diggingTree;

    public Color normal2Flying = new Color(0f, 152f / 255f, 222f / 255f);
    public Color normal2Digging = new Color(222f / 255f, 120f / 255f, 0f);
    public float lineWidth = 2;

    public float flyingFruit = 1;
    public float diggingFruit = 5;
    public float normalFriut = 2;

	// Use this for initialization
	void Start () {
        normalTree.SetCallBack(NormalButtonClickie);
        flyingTree.SetCallBack(FlyingButtonClickie);
        diggingTree.SetCallBack(DiggingButtonClickie);
        /***************\
          LINE CREATION
        \***************/
        Vector3 differenceVector;
        Image line;
        GameObject go;
        RectTransform rTrans;

        //Create Line From Normal 2 Flying Tree
        differenceVector = flyingTree.transform.position - normalTree.transform.position;
        go = new GameObject("Normal2Flying");
        rTrans = go.AddComponent<RectTransform>();
        rTrans.SetParent(transform);
        rTrans.SetSiblingIndex(0);
        rTrans.anchorMax = new Vector2(0.5f, 0.5f);
        line = go.AddComponent<Image>();
        line.color = normal2Flying;
        rTrans.sizeDelta = new Vector2(differenceVector.magnitude, lineWidth);
        rTrans.pivot = new Vector2(0, 0.5f);
        rTrans.position = normalTree.transform.position;
        rTrans.localRotation = Quaternion.Euler(0, 0,  Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg);

        //Create Line From Normal 2 Digging Tree
        differenceVector = diggingTree.transform.position - normalTree.transform.position;
        go = new GameObject("Normal2Digging");
        rTrans = go.AddComponent<RectTransform>();
        rTrans.SetParent(transform);
        rTrans.SetSiblingIndex(0);
        rTrans.anchorMax = new Vector2(0.5f, 0.5f);
        line = go.AddComponent<Image>();
        line.color = normal2Digging;
        rTrans.sizeDelta = new Vector2(differenceVector.magnitude, lineWidth);
        rTrans.pivot = new Vector2(0, 0.5f);
        rTrans.position = normalTree.transform.position;
        rTrans.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg);

        InvokeNextFrame(InitializeMenu);
	}
	
	// Update is called once per frame
	void Update () {
        bool found = false;
        NodeDataContainer ndc = normalTree.GetNodeMouseOver();
        if (ndc != null)
        {
            mouseHovery.OpenMenu(false, ndc);
            found = true;
        }
        if (!found)
        {
            ndc = diggingTree.GetNodeMouseOver();
            if (ndc != null)
            {
                mouseHovery.OpenMenu(false, ndc);
                found = true;
            }
        }
        if (!found)
        {
            ndc = flyingTree.GetNodeMouseOver();
            if (ndc != null)
            {
                mouseHovery.OpenMenu(false, ndc);
                found = true;
            }
        }
        if (!found)
            mouseHovery.CloseMenu(true);
	}

    public void NormalButtonClickie(NodeDataContainer ndc)
    {
        if (!ndc.IsInteractable() || ndc.IsActive())
            return;
        ndc.SetActive(true);
        normalFriut--;
        if (normalFriut <= 0)
        {
            for (int i = 1; i <= 4; i++)
            {
                if(!normalTree.IsSkillActive(normalTree.GetName(i))){
                    normalTree.SetSkillState(normalTree.GetName(i), false);
                }
            }
        }
    }

    public void FlyingButtonClickie(NodeDataContainer ndc)
    {
        if (!ndc.IsInteractable() || ndc.IsActive())
            return;
        ndc.SetActive(true);
        flyingFruit--;
        if (ndc.location == 7)
            return;
        int firstNew;
        if (ndc.location % 2 == 1 && ndc.location != 0)
        {
            firstNew = ndc.location + 2;
            flyingTree.SetSkillState(flyingTree.GetName(ndc.location + 1), false);
        }
        else
        {
            firstNew = ndc.location + 1;
            if(ndc.location != 0)
                flyingTree.SetSkillState(flyingTree.GetName(ndc.location - 1), false);
        }
        flyingTree.ShowSkill(flyingTree.GetName(firstNew), false);
        flyingTree.SetSkillState(flyingTree.GetName(firstNew), flyingFruit > 0);
        if (ndc.location < 5)
        {
            flyingTree.ShowSkill(flyingTree.GetName(firstNew + 1), false);
            flyingTree.SetSkillState(flyingTree.GetName(firstNew + 1), flyingFruit > 0);
        }
    }

    public void DiggingButtonClickie(NodeDataContainer ndc)
    {
        if (!ndc.IsInteractable() || ndc.IsActive())
            return;
        ndc.SetActive(true);
        diggingFruit--;
        if (ndc.location == 7)
            return;
        int firstNew;
        if (ndc.location % 2 == 1 && ndc.location != 0)
        {
            firstNew = ndc.location + 2;
            diggingTree.SetSkillState(diggingTree.GetName(ndc.location + 1), false);
        }
        else
        {
            firstNew = ndc.location + 1;
            if(ndc.location != 0)
                diggingTree.SetSkillState(diggingTree.GetName(ndc.location - 1), false);
        }
        diggingTree.ShowSkill(diggingTree.GetName(firstNew), false);
        diggingTree.SetSkillState(diggingTree.GetName(firstNew), diggingFruit > 0);
        if (ndc.location < 5)
        {
            diggingTree.ShowSkill(diggingTree.GetName(firstNew + 1), false);
            diggingTree.SetSkillState(diggingTree.GetName(firstNew + 1), diggingFruit > 0);
        }
    }

    public void InitializeMenu()
    {
        for (int i = 1; i < 8; i++)
        {
            flyingTree.HideSkill(flyingTree.GetName(i), true);
            diggingTree.HideSkill(diggingTree.GetName(i), true);
        }
        if (normalFriut <= 0)
        {
            for (int i = 1; i <= 4; i++)
            {
                normalTree.SetSkillState(normalTree.GetName(i), false);
            }
        }

    }

    public void UpdateMenu()
    {
        //UpdateNormal
        if (normalFriut > 0)
        {
            for (int i = 1; i <= 4; i++)
            {
                normalTree.SetSkillState(normalTree.GetName(i), true);
            }
        }
        //UpdateFlying
        if (flyingFruit > 0)
        {
            
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

    public void CloseTree()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().respawn();
    }
}