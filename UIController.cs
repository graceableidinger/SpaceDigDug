using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //public GameObject playButton;
    //public GameObject title;
    //public GameObject backGroundUI;
    //public Text Score_UIText;
    public GameObject spaceText;
    //public GameObject tiles;
    //int scoreCounter;
    

    bool time = true;
    bool gone = false;
    bool firstTime = true;

    void Start()
    {
        spaceText.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Space)&&!gone)
        {
            playGame();
            gone = true;
        }
        if (!gone && time)
        {
            StartCoroutine("blinkSpace");
        }
        //scoreCounter = scoreCalc.score;
        //string score2 = scoreCounter.ToString();
        //Score_UIText.text = "Score:" + score2;
    }
    IEnumerator blinkSpace()
    {
        time = false;
        if (firstTime)
        {
            yield return new WaitForSeconds(2.0f);
            firstTime = false;
        }
        spaceText.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        spaceText.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        time = true;
    }
    public void playGame()
    {
        //Debug.Log("here");
        //playButton.SetActive(false);
        //title.SetActive(false);
        //backGroundUI.SetActive(false);
        spaceText.SetActive(false);
        //tiles.SetActive(true);
    }
}
