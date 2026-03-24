using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManagement : MonoBehaviour
{
    [SerializeField] private GameObject[] Tabs;
    [SerializeField] private Image[] TabButtons;

    [SerializeField] private Sprite activeBtnBg, inActiveBtnBg;
    [SerializeField] private Vector2 activeBtnSize, inActiveBtnSize;



    public void SwitchTab(int TabId)
    {
        foreach(GameObject tab in Tabs)
        {
            tab.SetActive(false);
        }

        Tabs[TabId].SetActive(true);

        foreach (Image img in TabButtons)
        {
            img.sprite = inActiveBtnBg;
            img.rectTransform.sizeDelta = inActiveBtnSize;
        }

        TabButtons[TabId].sprite = activeBtnBg;
        TabButtons[TabId].rectTransform.sizeDelta = activeBtnSize;

    }


}
