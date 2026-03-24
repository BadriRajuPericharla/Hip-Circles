using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RotateDirDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rotateDirText;


    private void Start()
    {
        ShowRotateDir();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            ShowRotateDir();
        }


    }



    public string GetRotateDir()
    {
        int index = Random.Range(0, 2);
        string dir = index == 0 ? "Left" : "Right";

        return dir;
    }


    public void ShowRotateDir()
    {
        rotateDirText.text = "Rotate " + GetRotateDir();
    }
}
