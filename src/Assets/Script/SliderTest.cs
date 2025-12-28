using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderTest : MonoBehaviour
{
    [SerializeField]
    Slider slider;

    [SerializeField]
    TextSystem textSystem;

    [SerializeField]
    TextMeshProUGUI textMeshPro;

    int value;

    private void Start()
    {
        value = 0;
        slider.onValueChanged.AddListener(delegate {TextUpdate(); } );
    }


    // Update is called once per frame
    void TextUpdate()
    {
        value = (int)slider.value;
        textSystem.TextSetting(value);
        textMeshPro.SetText(((int)slider.value).ToString());
    }
}
