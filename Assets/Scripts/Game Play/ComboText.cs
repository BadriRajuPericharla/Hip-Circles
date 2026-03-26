using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboText : MonoBehaviour
{
    [SerializeField]private GameObject comboPanel;
    [SerializeField]private TextMeshProUGUI comboText;
    int loopCount=0;
    public void increaseCount()
    {
        loopCount++;
        comboPanel.SetActive(true);
        if (loopCount == 1)
        {
            comboText.text="Good";
        }
        if (loopCount == 2)
        {
            comboText.text="Excellent";
        }
        if (loopCount == 3)
        {
            comboText.text="You Are Nailing It";
        }
        if (loopCount >= 4)
        {
            comboText.text="Marvellous";
        }
    }
    public void resetCount()
    {
        loopCount=0;
        comboPanel.SetActive(false);
    }
    
}
