using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideManager : MonoBehaviour
{
    [SerializeField] GameObject guideObject;

    bool is_displayed = false;
    
    void Start()
    {
        guideObject.SetActive(false);
    }

    public void DisplayGuide()
    {
        is_displayed = true;
        StartCoroutine(nameof(DisplayObject));
    }

    public void HideGuide()
    {
        is_displayed = false;
        guideObject.SetActive(false);
    }

    IEnumerator DisplayObject()
    {
        yield return new WaitForSeconds(0.5f);  // 0.5秒後に実行
        if (is_displayed)
        {
            guideObject.SetActive(true);
        }
    }
}
