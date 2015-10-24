using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;

[System.Serializable]
[CustomEditor(typeof(NodeController))]
public class NodeControllerInspector : Editor {
    [SerializeField]
    private NodeController nCont;

    [SerializeField]
    private int node = 0;

    public void OnEnable()
    {
        nCont = (NodeController)target;
        Transform t = nCont.transform.FindChild("Node " + (node + 1) + "/Line");
        if (t != null)
        {
            Image line = t.GetComponent<Image>();
            line.color = Color.yellow;
            EditorUtility.SetDirty(line);
            EditorApplication.MarkSceneDirty();
        }
        if (nCont.prefab == null)
        {
            nCont.prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/TechTree/Prefabs/Node.prefab");
            //Add Center Node
            RectTransform currentNode = ((GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/TechTree/Prefabs/CentralNode.prefab"))).GetComponent<RectTransform>();
            currentNode.SetParent(nCont.transform, false);
            currentNode.gameObject.GetComponent<NodeDataContainer>().location = 0;
        }
        EditorApplication.playmodeStateChanged += ClearColors;
    }

    public void OnDisable()
    {
        if (nCont == null)
            return;
        ClearColors();
        EditorApplication.playmodeStateChanged -= ClearColors;
    }

    public override void OnInspectorGUI()
    {
        //Just to have the script field :P
        serializedObject.Update();
        SerializedProperty prop = serializedObject.FindProperty("m_Script");
        EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.ObjectField("Prefab", nCont.prefab, typeof(GameObject), true);

        if(nCont.numberOfNodes > 0){
            Color next = EditorGUILayout.ColorField("Color", nCont.lineColor);
            if (nCont.lineColor != next)
            {
                nCont.lineColor = next;
                UpdateColors(next);
            }
        }
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Nodes", GUILayout.MaxWidth(EditorGUIUtility.labelWidth));
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("-"))
        {
            RemoveNode();
            UpdateNodePositions();
        }
        int tempStorage = EditorGUILayout.IntField(nCont.numberOfNodes);
        if (GUILayout.Button("+"))
        {
            AddNode();
            UpdateNodePositions();
            tempStorage = nCont.numberOfNodes;
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
        if (nCont.hasCenter)
        {
            if (GUILayout.Button("Remove Center"))
            {
                DestroyImmediate(nCont.transform.FindChild("CentralNode").gameObject);
                nCont.hasCenter = false;
                if (node == -1)
                {
                    node = 0;
                }
            }
        }
        else
        {
            if (GUILayout.Button("Add Center"))
            {
                nCont.hasCenter = true;
                RectTransform currentNode = ((GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/TechTree/Prefabs/CentralNode.prefab"))).GetComponent<RectTransform>();
                currentNode.SetParent(nCont.transform, false);
                currentNode.gameObject.GetComponent<NodeDataContainer>().location = 0;
            }
        }
        if (nCont.numberOfNodes != tempStorage)
        {
            for (int i = nCont.numberOfNodes; i < tempStorage; i++)
                AddNode();
            for (int i = nCont.numberOfNodes; i > tempStorage; i--)
                RemoveNode();
            nCont.numberOfNodes = tempStorage;
            UpdateNodePositions();
        }
        tempStorage = EditorGUILayout.IntSlider("Angle", (int)nCont.angle, 0, nCont.numberOfNodes == 0 ? 360 : (int)Mathf.Ceil(360f / nCont.numberOfNodes));
        if (tempStorage != nCont.angle)
        {
            nCont.angle = tempStorage;
            UpdateNodePositions();
        }
        tempStorage = EditorGUILayout.IntSlider("Initial Angle", (int)nCont.initialAngle, 0, 360);
        if (nCont.initialAngle != tempStorage)
        {
            nCont.initialAngle = tempStorage;
            UpdateNodePositions();
        }
        bool isInverted = EditorGUILayout.Toggle("Inverted", nCont.inverted);
        if (isInverted != nCont.inverted)
        {
            nCont.inverted = isInverted;
            UpdateNodePositions();
        }
        if (!Application.isPlaying)
        {
            EditorGUILayout.LabelField("************EDITOR************", EditorStyles.boldLabel);
            int newNode = EditorGUILayout.IntSlider("Editing Node", node + 1, 0, nCont.numberOfNodes) - 1;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Previous"))
            {
                if (node == (nCont.hasCenter?-1:0))
                    newNode = nCont.numberOfNodes - 1;
                else
                    newNode--;
            }
            if (GUILayout.Button("Next"))
            {
                if (node == nCont.numberOfNodes - 1)
                    newNode = (nCont.hasCenter ? -1 : 0);
                else
                    newNode++;
            }
            EditorGUILayout.EndHorizontal();
            if (newNode != node)
            {
                Image line;
                if (node != -1)
                {
                    Transform t = nCont.transform.FindChild("Node " + (node + 1) + "/Image");
                    if (t != null)
                    {
                        line = t.parent.FindChild("Line").GetComponent<Image>();
                        line.color = nCont.lineColor;
                        EditorUtility.SetDirty(line);
                        EditorApplication.MarkSceneDirty();
                    }
                }
                node = newNode;
                if (node != -1)
                {
                    Transform t = nCont.transform.FindChild("Node " + (node + 1) + "/Image");
                    if (t != null)
                    {
                        line = t.parent.FindChild("Line").GetComponent<Image>();
                        line.color = Color.yellow;
                        EditorUtility.SetDirty(line.gameObject);
                        EditorApplication.MarkSceneDirty();
                    }
                }
            }
            if (node == -1)
            {
                EditorGUILayout.LabelField("**********Editing Central Node**********", EditorStyles.boldLabel);
            }
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            if (nCont != null)
            {
                NodeDataContainer ndc;
                if (node == -1)
                {
                    ndc = nCont.transform.FindChild("CentralNode").GetComponent<NodeDataContainer>();
                }
                else
                {
                    ndc = nCont.transform.FindChild("Node " + (node + 1) + "/Image").GetComponent<NodeDataContainer>();
                }
                Image display = ndc.GetComponent<Image>();
                ndc.title = EditorGUILayout.TextField("Title", ndc.title);
                ndc.flavorText = EditorGUILayout.TextField("Flavor 1", ndc.flavorText);
                ndc.flavorText2 = EditorGUILayout.TextField("Flavor 2", ndc.flavorText2);
                ndc.flavorText3 = EditorGUILayout.TextField("Flavor 3", ndc.flavorText3);
                display.sprite = (Sprite)EditorGUILayout.ObjectField("Display Image", ndc.GetComponent<Image>().sprite, typeof(Sprite), true);
                Image i = ndc.transform.GetChild(0).GetComponent<Image>();
                Sprite s = (Sprite)EditorGUILayout.ObjectField("Halo Image", i.sprite, typeof(Sprite), true);
                if (i.sprite != s)
                {
                    i.sprite = s;
                    EditorUtility.SetDirty(i);
                }
                EditorUtility.SetDirty(display);
                EditorUtility.SetDirty(ndc);
                EditorApplication.MarkSceneDirty();
            }
            EditorGUILayout.EndVertical();
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(nCont);
            EditorApplication.MarkSceneDirty();
        }
    }

    private void UpdateColors(Color c)
    {
        for (int i = 1; i <= nCont.numberOfNodes; i++)
        {
            Transform t = nCont.transform.FindChild("Node " + i + "/Line");
            Image newImage = t.GetComponent<Image>();
            newImage.color = c;
            EditorUtility.SetDirty(newImage);
            EditorApplication.MarkSceneDirty();
        }
    }

    private void UpdateNodePositions()
    {
        for (int i = 1; i <= nCont.numberOfNodes; i++)
        {
            float angle = ((nCont.inverted ? -1 : 1) * nCont.angle * i) + nCont.initialAngle;
            RectTransform currentNode = nCont.transform.FindChild("Node " + i).GetComponent<RectTransform>();
            currentNode.transform.rotation = Quaternion.identity;
            currentNode.transform.FindChild("Image").GetComponent<RectTransform>().rotation = Quaternion.identity;
            currentNode.transform.Rotate(new Vector3(0, 0, angle));
            currentNode.transform.FindChild("Image").GetComponent<RectTransform>().Rotate(new Vector3(0, 0, -angle));
        }
    }

    private void AddNode()
    {
        RectTransform currentNode = ((GameObject)PrefabUtility.InstantiatePrefab(nCont.prefab)).GetComponent<RectTransform>();
        currentNode.SetParent(nCont.transform, false);
        currentNode.SetSiblingIndex(nCont.numberOfNodes);
        nCont.numberOfNodes++;
        currentNode.name += " " + nCont.numberOfNodes;
        currentNode.FindChild("Image").GetComponent<NodeDataContainer>().location = nCont.numberOfNodes;
        Image newImage = currentNode.FindChild("Line").GetComponent<Image>();
        newImage.color = nCont.lineColor;
        EditorUtility.SetDirty(newImage);
        nCont.angle = (int)(360f / nCont.numberOfNodes);
        EditorApplication.MarkSceneDirty();
    }

    private void RemoveNode()
    {
        if (nCont.numberOfNodes > 0)
        {
            DestroyImmediate(nCont.transform.FindChild("Node " + (nCont.numberOfNodes)).gameObject);
            nCont.numberOfNodes--;
            nCont.angle = (int)Mathf.Ceil(360f / nCont.numberOfNodes);
        }
    }

    public void ClearColors()
    {
        UpdateColors(nCont.lineColor);
    }
}