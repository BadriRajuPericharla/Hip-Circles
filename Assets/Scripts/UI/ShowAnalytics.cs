using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowAnalytics : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI compltetionTime; 
    [SerializeField] TextMeshProUGUI leftActions; 
    [SerializeField] TextMeshProUGUI rightActions; 
    [SerializeField] TextMeshProUGUI calories; 
    [SerializeField] TextMeshProUGUI score;

    [SerializeField] TextMeshProUGUI DummyData1;
    [SerializeField] TextMeshProUGUI DummyData2;
    [SerializeField] TextMeshProUGUI DummyData3;
    [SerializeField] TextMeshProUGUI DummyData4;
    [SerializeField] TextMeshProUGUI DummyData5;



    public void OnEnable()
    {
        //GameData gameData = SessionEndController.currentSession;
        GameData gameData = new GameData();


        if (gameData == null) return;

        compltetionTime.text = gameData.time;
        leftActions.text = gameData.leftActions.ToString();
        rightActions.text = gameData.rightActions.ToString();
        calories.text = gameData.calories.ToString();
        score.text = gameData.finalScore.ToString();

        //dummy data
        DummyData1.text = gameData.dummy1.ToString();
        DummyData2.text = gameData.dummy2.ToString();
        DummyData3.text = gameData.dummy3.ToString();
        DummyData4.text = gameData.dummy4.ToString();
        DummyData5.text = gameData.dummy5.ToString();
    }


}
