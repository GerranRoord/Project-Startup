using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VariableToTMP : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI text;
    enum Display
    {
        LVL,
        XP,
        COINS
    }
    [SerializeField]Display display;
    [SerializeField] Slider slider;
    // Update is called once per frame
    void Update()
    {
        switch (display)
        {
            case Display.LVL:
               if(text!=null) text.text = GameManager.instance.lvl.ToString();
                break;
            case Display.XP:
                if (slider != null)
                {
                    slider.maxValue = GameManager.instance.expToNextLevel;
                    slider.value = GameManager.instance.exp;
                }
                break;
            case Display.COINS:
                if (text != null) text.text= GameManager.instance.coins.ToString();
                break;
        }
    }
}
