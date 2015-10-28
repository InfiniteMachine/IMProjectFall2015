using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DayNNight : MonoBehaviour {
    [Tooltip("Duration of Day in Seconds")]
    public float dayDuration = 10f;
    [Tooltip("Change from day to night in seconds")]
    public float dayToNightDuration = 1f;
    [Tooltip("Duration of Night in Seconds")]
    public float nightDuration = 5f;
    [Tooltip("Change from night to day in seconds")]
    public float nightToDayDuration = 1f;
    [Tooltip("Alpha of the Canvas During the Day")]
    [Range(0, 1f)]
    public float dayAlpha = 0;
    [Tooltip("Alpha of the Canvas During the Night")]
    [Range(0, 1f)]
    public float nightAlpha = 0.75f;

    private Color dayColor;
    private Color nightColor;

    private float timer;
    
    public enum CycleState {Day, DayToNight, Night, NightToDay, Override};
    private CycleState state = CycleState.Day;

    public Image dayNightImage;

    // Use this for initialization
	void Start () {
        timer = 0;
        dayColor = new Color(0, 0, 0, dayAlpha);
        nightColor = new Color(0, 0, 0, nightAlpha);
        if (state == CycleState.DayToNight || state == CycleState.NightToDay || state == CycleState.Override)
            state = CycleState.Day;

        if (state == CycleState.Day)
        {
            dayNightImage.color = dayColor;
        }
        else
        {
            dayNightImage.color = nightColor;
        }
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        switch (state)
        {
            case CycleState.Day:
                if (timer >= dayDuration)
                {
                    timer = 0;
                    state = CycleState.DayToNight;
                }
                break;
            case CycleState.DayToNight:
                dayNightImage.color = Color.Lerp(dayColor, nightColor, timer / dayToNightDuration);
                if (timer >= dayToNightDuration)
                {
                    timer = 0;
                    state = CycleState.Night;
                    dayNightImage.color = nightColor;
                }
                break;
            case CycleState.Night:
                if (timer >= nightDuration)
                {
                    timer = 0;
                    state = CycleState.NightToDay;
                }
                break;
            case CycleState.NightToDay:
                dayNightImage.color = Color.Lerp(nightColor, dayColor, timer / nightToDayDuration);
                if (timer >= nightToDayDuration)
                {
                    timer = 0;
                    state = CycleState.Day;
                    dayNightImage.color = dayColor;
                }
                break;
            case CycleState.Override:

                break;
        }
	}

    public void SwapState(CycleState newState, bool force)
    {
        if (state == CycleState.Override)
        {
            StopAllCoroutines();
        }
        if(force){
            switch(newState){
                case CycleState.Day:
                    dayNightImage.color = dayColor;
                    break;
                case CycleState.Night:
                    dayNightImage.color = nightColor;
                    break;
                case CycleState.DayToNight:
                    dayNightImage.color = dayColor;
                    break;
                case CycleState.NightToDay:
                    dayNightImage.color = nightColor;
                    break;
            }
            timer = 0;
        }
        else
        {
            switch(newState){
                case CycleState.Day:
                    StartCoroutine(NightToDay(newState));
                    break;
                case CycleState.Night:
                    StartCoroutine(DayToNight(newState));
                    break;
                case CycleState.DayToNight:
                    StartCoroutine(NightToDay(newState));
                    break;
                case CycleState.NightToDay:
                    StartCoroutine(DayToNight(newState));
                    break;
            }
        }
    }

    IEnumerator NightToDay(CycleState newState)
    {
        float counter = 0;
        Color startColor = dayNightImage.color;
        while(counter < nightToDayDuration){
            counter += Time.deltaTime;
            dayNightImage.color = Color.Lerp(startColor, dayColor, counter / nightToDayDuration);
            yield return null;
        }
        dayNightImage.color = dayColor;
        timer = 0;
        state = newState;
    }

    IEnumerator DayToNight(CycleState newState)
    {
        float counter = 0;
        Color startColor = dayNightImage.color;
        while (counter < nightToDayDuration)
        {
            counter += Time.deltaTime;
            dayNightImage.color = Color.Lerp(startColor, nightColor, counter / dayToNightDuration);
            yield return null;
        }
        dayNightImage.color = nightColor;
        timer = 0;
        state = newState;
    }
}