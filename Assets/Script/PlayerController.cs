using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public float fadeDuration = 2f; // Duur van de fade (in seconden)
    public Transform player;
    public float currency = 1;
    public TextMeshProUGUI currencyAmount, noFundsWarning, CurrencyText;


    void Awake() => Instance = this;
 
     private void Update()
    {
        currencyAmount.text = currency.ToString();
    }

    public void NoFund()
    {
        //noFundsWarning.gameObject.SetActive(true);
        StartCoroutine(FadeTextToFullAlpha(.5f, .2f, noFundsWarning, CurrencyText, currencyAmount));
        StartCoroutine(Wait(noFundsWarning));
    }

    public IEnumerator Wait(TMP_Text i)
    {
        yield return new WaitForSeconds(.5f);
        StartCoroutine(FadeTextToZeroAlpha(1,.2f, i, CurrencyText, currencyAmount));
    }
    public IEnumerator FadeTextToFullAlpha(float t, float r, TMP_Text i, TMP_Text CurrencyText, TMP_Text CurrencyA)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        CurrencyText.color = new Color(CurrencyText.color.r, CurrencyText.color.g, CurrencyText.color.b, CurrencyText.color.a);
        CurrencyA.color = new Color(CurrencyA.color.r, CurrencyA.color.g, CurrencyA.color.b, CurrencyA.color.a);

        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r + (Time.deltaTime / r), i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            //fade to red
            CurrencyText.color = new Color(CurrencyText.color.r, CurrencyText.color.g - (Time.deltaTime / r), CurrencyText.color.b - (Time.deltaTime / r), CurrencyText.color.a);
            CurrencyA.color = new Color(CurrencyA.color.r, CurrencyA.color.g - (Time.deltaTime / r), CurrencyA.color.b - (Time.deltaTime / r), CurrencyA.color.a);

            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, float r, TMP_Text i, TMP_Text CurrencyText, TMP_Text CurrencyA)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        CurrencyText.color = new Color(CurrencyText.color.r, CurrencyText.color.g, CurrencyText.color.b, CurrencyText.color.a);
        CurrencyA.color = new Color(CurrencyA.color.r, CurrencyA.color.g, CurrencyA.color.b, CurrencyA.color.a);

        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            //fade back to white
            CurrencyText.color = new Color(CurrencyText.color.r, CurrencyText.color.g + (Time.deltaTime / r), CurrencyText.color.b + (Time.deltaTime / r), CurrencyText.color.a);
            CurrencyA.color = new Color(CurrencyA.color.r, CurrencyA.color.g + (Time.deltaTime / r), CurrencyA.color.b + (Time.deltaTime / r), CurrencyA.color.a);
            yield return null;
        }
    }
}
