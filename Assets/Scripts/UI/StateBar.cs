using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateBar : MonoBehaviour
{
    [SerializeField] protected Image backImage;
    [SerializeField] protected Image frontImage;
    [SerializeField] float stateBarChangeDelay;
    [SerializeField] float fillSpeed;
    WaitForSeconds waitForStateBarChange;
    protected Coroutine stateBarFillCoroutine;
    protected Canvas parentCanvas;
    protected float targetFillAmount;
    private void Awake()
    {
        waitForStateBarChange = new WaitForSeconds(stateBarChangeDelay);
        parentCanvas = GetComponentInParent<Canvas>();
    }
    private void Start()
    {
        Initilize();
    }
    protected void Initilize()
    {
        backImage.fillAmount = 1f;
        frontImage.fillAmount = 1f;
        parentCanvas.worldCamera = Camera.main;
    }

    /// <summary>
    /// 更新状态条
    /// </summary>
    /// <param name="targetHP">目标数值</param>
    /// <param name="maxHP">满值</param>
    public void UpdateStateBar(float targetValue, float maxValue)
    {
        if (stateBarFillCoroutine != null)
        {
            StopCoroutine(stateBarFillCoroutine);
        }
        if (targetValue / maxValue <= frontImage.fillAmount)
        {
            frontImage.fillAmount = targetValue / maxValue;
            targetFillAmount = frontImage.fillAmount;
            stateBarFillCoroutine = StartCoroutine(StateBarFillCoroutine(backImage));
        }
        else if (targetValue / maxValue > frontImage.fillAmount)
        {
            backImage.fillAmount = targetValue / maxValue;
            targetFillAmount = backImage.fillAmount;
            stateBarFillCoroutine = StartCoroutine(StateBarFillCoroutine(frontImage));
        }
    }

    /// <summary>
    /// 根据状态数值进行缓冲式更新
    /// </summary>
    /// <param name="fillImage">缓冲更新状态图片</param>
    /// <returns></returns>
    IEnumerator StateBarFillCoroutine(Image fillImage)
    {
        yield return waitForStateBarChange;
        float t = 0;
        while (t < 1f)
        {
            t += (Time.deltaTime * fillSpeed);
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetFillAmount, t / 1f);
            print(fillImage.fillAmount);
            yield return null;
        }
        yield break;
    }
}
