using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Kainat_UI_TextColorScript : MonoBehaviour
{
    [SerializeField] private string color1Hexa = "#EFD071";
    [SerializeField] private string color2Hexa = "#F9EECB";
    [SerializeField] private string color3Hexa = "#F9EECB";
    [SerializeField] private string color4Hexa = "#F9EECB";
    [SerializeField] private string color5Hexa = "#F9EECB";

    [SerializeField] private Color color1;
    [SerializeField] private Color color2;
    [SerializeField] private Color color3;
    [SerializeField] private Color color4;
    [SerializeField] private Color color5;

    public Color[] colors;

    [SerializeField] private Vector3 textScale;
    [SerializeField] private TextMeshProUGUI title;

    private void Start()
    {
        ColorUtility.TryParseHtmlString(color1Hexa, out color1);
        ColorUtility.TryParseHtmlString(color2Hexa, out color2);
        ColorUtility.TryParseHtmlString(color3Hexa, out color3);
        ColorUtility.TryParseHtmlString(color4Hexa, out color4);
        ColorUtility.TryParseHtmlString(color5Hexa, out color5);

        colors = new Color[5];
        colors[0] = color1;
        colors[1] = color2;
        colors[2] = color3;
        colors[3] = color4;
        colors[4] = color5;

        textScale = transform.localScale;
        transform.localScale = Vector3.zero;

        title = GetComponent<TextMeshProUGUI>();

        AnimateText();
    }

    private void AnimateText()
    {
        ColorAnimation();
        transform.DOScale(textScale, 2f).SetEase(Ease.InOutSine);
        //StartCoroutine(ColorAnimation());
        
    }

    void ColorAnimation()
    {
        //title.color = color1;
        //yield return new WaitForSeconds(0.4f);
        //title.color = color2;
        //yield return new WaitForSeconds(0.4f);
        //title.color = color3;
        //yield return new WaitForSeconds(0.4f);
        //title.color = color4;
        //yield return new WaitForSeconds(0.4f);
        //title.color = color5;

        //yield return new WaitForSeconds(0.4f);
        //title.color = color1;
        //yield return new WaitForSeconds(0.4f);
        //title.color = color2;
        //yield return new WaitForSeconds(0.4f);
        //title.color = color3;
        //yield return new WaitForSeconds(0.4f);
        //title.color = color4;
        //yield return new WaitForSeconds(0.4f);
        //title.color = color5;
        //yield return new WaitForSeconds(0.4f);

        Sequence colorSequence = DOTween.Sequence();

        foreach (Color color in colors)
        {
            // Add each color change to the sequence with a 0.4f delay
            colorSequence.Append(title.DOColor(color, 0.4f).SetEase(Ease.InSine));
        }

        // Loop the sequence indefinitely
        colorSequence.SetLoops(-1, LoopType.Restart);
    }
}
