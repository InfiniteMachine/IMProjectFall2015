using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MouseHover : MonoBehaviour {
    public NodeController techTree;
    public float animationDuration = 0.5f;
    public float animationDelay = 0.5f;
    private bool open = false;

    private List<NodeDataContainer> nodes;
    //private CanvasGroup alphaController;
    private RectTransform rTransform;

    private Text title;
    private Text flavor1;
    private Text flavor2;
    private Text flavor3;
    private Image display;

    private bool isEnabled = false;

    // Use this for initialization
    void Start()
    {
        rTransform = GetComponent<RectTransform>();

        flavor1 = transform.FindChild("Flavor1").GetComponent<Text>();
        flavor2 = transform.FindChild("Flavor2").GetComponent<Text>();
        flavor3 = transform.FindChild("Flavor3").GetComponent<Text>();
        display = transform.FindChild("Image").GetComponent<Image>();
        title = transform.FindChild("Title").GetComponent<Text>();

        transform.SetAsLastSibling();

        transform.localScale = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = (Vector2)Input.mousePosition;// +(new Vector2((flipX ? -1 : 1) * rTransform.rect.size.x, (flipY ? 1 : -1) * rTransform.rect.size.y) / 2f);
        Vector2 pivot = new Vector2();
        if (Screen.width - Input.mousePosition.x < rTransform.rect.width)
        {
            pivot.x = 1;
        }
        else
        {
            pivot.x = 0;
        }
        if (Input.mousePosition.y < rTransform.rect.height)
        {
            pivot.y = 0;
        }
        else
        {
            pivot.y = 1;
        }
        rTransform.pivot = pivot;
	}

    public bool IsEnabled()
    {
        return isEnabled;
    }

    public void OpenMenu(bool instant, NodeDataContainer ndc)
    {
        StopCoroutine("AnimateClose");
        title.text = ndc.title;
        flavor1.text = ndc.flavorText;
        flavor2.text = ndc.flavorText2;
        flavor3.text = ndc.flavorText3;
        display.sprite = ndc.displayImage;

        if (!isEnabled)
        {
            if (instant)
                rTransform.localScale = Vector3.zero;
            else
                StartCoroutine("AnimateOpen");
        }
        isEnabled = true;
    }

    public void CloseMenu(bool instant)
    {
        StopCoroutine("AnimateOpen");
        if (isEnabled)
        {
            if (instant)
                rTransform.localScale = Vector3.zero;
            else
                StartCoroutine("AnimateClose");
        }
        isEnabled = false;
    }

    IEnumerator AnimateOpen()
    {
        float count = 0;
        Vector3 scale = rTransform.localScale;
        while (count < animationDelay)
        {
            count += Time.deltaTime;
            yield return null;
        }
        count = 0;
        while (count <= animationDuration)
        {
            transform.localScale = Vector3.Lerp(scale, Vector3.one, count / animationDuration);
            count += Time.deltaTime;
            yield return null;
        }
        transform.localScale = Vector3.one;
    }

    IEnumerator AnimateClose()
    {
        StopCoroutine("AnimateOpen");
        open = false;
        float count = 0;
        Vector3 scale = rTransform.localScale;
        while (count <= animationDuration)
        {
            transform.localScale = Vector3.Lerp(scale, Vector3.zero, count / animationDuration);
            count += Time.deltaTime;
            yield return null;
        }
        transform.localScale = Vector3.zero;
    }
}