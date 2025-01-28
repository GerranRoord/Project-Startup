using TMPro;
using UnityEngine;

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
    // Update is called once per frame
    void Update()
    {
        switch (display)
        {
            case Display.LVL:
                text.text = GameManager.instance.lvl.ToString();
                break;
            case Display.XP:
                text.text = (GameManager.instance.exp / GameManager.instance.expToNextLevel).ToString(); 
                break;
            case Display.COINS:
                text.text= GameManager.instance.coins.ToString();
                break;
        }
    }
}
