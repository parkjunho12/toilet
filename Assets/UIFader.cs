using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFader : MonoBehaviour
{
    public static UIFader _uniqueInstance;
    public CanvasGroup uiElement;

    void Awake()
    {
        _uniqueInstance = this;    
    }

    public void FadeIn(float _alpha)
    {
        StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, _alpha));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 0));
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 0.5f)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        if(end == 0)
        {
            while(true)
            {
                timeSinceStarted = Time.time - _timeStartedLerping;
                percentageComplete = timeSinceStarted / lerpTime;

                float currentValue = Mathf.Lerp(start, end, percentageComplete);

                cg.alpha = currentValue;

                if (percentageComplete >= 1)
                    break;

                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            cg.alpha = end;
        }
    }

}
