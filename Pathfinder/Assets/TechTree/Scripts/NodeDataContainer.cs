using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NodeDataContainer : MonoBehaviour
{
    private Transform parent;
    public float animationDuration = 0.5f;
    public string title = "Display Title";
    public string flavorText = "Flavor Text";
    public string flavorText2 = "";
    public string flavorText3 = "";
    [HideInInspector]
    public int location = 0;
    [HideInInspector]
    public Sprite displayImage;
    private Image display;
    private RectTransform rTransform;

    private Button button;

    private bool visible = false;
    private bool active = false;
    private bool isEnabled = false;
    private bool animating = false;

    private Image halo;

	// Use this for initialization
	void Start () {
        display = GetComponent<Image>();
        halo = transform.FindChild("Halo").GetComponent<Image>();
        displayImage = display.sprite;
        rTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        parent = transform.parent;
        visible = true;
        isEnabled = true;
        //display.color = new Color(0.5f, 0.5f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
    }

    public bool IsMouseOver()
    {
        bool mouseOver = (Input.mousePosition.x > (transform.position.x - rTransform.rect.width / 2f) && Input.mousePosition.x < (transform.position.x + rTransform.rect.width / 2f)
                && Input.mousePosition.y > (transform.position.y - rTransform.rect.height / 2) && Input.mousePosition.y < (transform.position.y + rTransform.rect.height / 2f));
        /*
        if (mouseOver || active)
            display.color = Color.white;
        else
            display.color = new Color(0.5f, 0.5f, 0.5f);
         */
        return mouseOver;
    }

    public void Animate(bool direction, bool instant)
    {
        if (animating)
            return;
        if (instant)
        {
            if (direction)
            {
                parent.transform.localScale = Vector3.one;
                visible = true;
            }
            else
            {
                parent.transform.localScale = Vector3.zero;
                visible = false;
            }
        }
        else
        {
            animating = true;
            if (direction)
                StartCoroutine_Auto(AnimateOut());
            else
                StartCoroutine_Auto(AnimateIn());
        }
    }

    public bool IsActive()
    {
        return active;
    }

    public void SetActive(bool isActive)
    {
        active = isActive;
        halo.color = active ? Color.white : Color.clear;
        button.enabled = !active;
    }

    public bool IsInteractable()
    {
        return visible && isEnabled;
    }

    public bool IsAnimating()
    {
        return animating;
    }

    public void SetState(bool enable)
    {
        isEnabled = enable;
        button.interactable = enable;
        ///*
        if (isEnabled)
        {
            display.color = Color.white;
        }
        else
        {
            display.color = new Color(0.5f, 0.5f, 0.5f);
        }
         //* */
    }

    IEnumerator AnimateOut()
    {
        visible = true;
        float count = 0;
        parent.transform.localScale = Vector3.zero;
        while (count < animationDuration)
        {
            count += Time.deltaTime;
            parent.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, count / animationDuration);
            yield return null;
        }
        parent.transform.localScale = Vector3.one;
        animating = false;
    }

    IEnumerator AnimateIn()
    {
        visible = false;
        float count = 0;
        parent.transform.localScale = Vector3.one;
        while (count < animationDuration)
        {
            count += Time.deltaTime;
            parent.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, count / animationDuration);
            yield return null;
        }
        parent.transform.localScale = Vector3.zero;
        animating = false;
    }
}