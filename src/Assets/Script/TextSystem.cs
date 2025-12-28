using TMPro;
using UnityEngine;

public class TextSystem : MonoBehaviour
{
    [SerializeField] 
    string[] NumberText = new string[10] { "", "一", "二", "三", "四",                                                         
        "五", "六", "七", "八", "九"};

    [SerializeField]
    string[] DigitText = new string[10] { "", "十", "百", "千", "",                                                       
        "十", "百", "千", "", "十"};

    [SerializeField]
    string[] TierText = new string[10] { "","","","", "万",
        "","","", "億", ""};

    [SerializeField]
    TextMeshProUGUI TargetText;

    [SerializeField]
    string BackText = "鳥";

    public void TextSetting(int value)
    {
        string setText = string.Empty;

        int target = value <= int.MaxValue ? value : int.MaxValue;

        for(int digit = 0; digit < 10; digit++)
        {
            int pow = (int)Mathf.Pow(10, digit);
            
            if (target / pow <= 0)       
                break;

            int currentDigit = (target / pow) % 10;

            // Debug.Log(target +"/"+ pow + " " + currentDigit);

            string currentText = 
                ((currentDigit == 1 && digit % 4 != 0) ? string.Empty : NumberText[currentDigit]) +
                (currentDigit == 0 ? string.Empty : DigitText[digit]) +
                TierText[digit];

            setText = currentText + setText;
        }

        if (setText == string.Empty)
            setText = "　";

        TargetText.SetText(setText + BackText);
    }
}
