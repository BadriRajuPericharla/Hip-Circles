using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifes : MonoBehaviour
{
    [SerializeField]private GameObject life1;
    [SerializeField]private GameObject life2;
    [SerializeField]private GameObject life3;
    int count=0;
    public void lifes()
    {
        count++;
        if (count == 1)
        {
            life1.SetActive(false);
        }
        else if (count == 2)
        {
            life2.SetActive(false);
        }
        else if (count == 3)
        {
            life3.SetActive(false);
        }
    }
}
