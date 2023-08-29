using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scoreCalc : MonoBehaviour
{
    static public int score;
    private int power;
    public int enemiesCrushed;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if((power - score) >= 1000)//bandaid
        {
            power = score;
            Powerups.test = true;
        }
    }
    public static void enemyKilled(int layer)
    {
        score += layer * 100;
        score += 100;
    }
    public static void powerUp(int level)
    {
        score += level * 200;
        score += 200;
    }
    public void rock()
    {
        if (enemiesCrushed == 0)
        {
            score += 1000;
        }
        else if (enemiesCrushed < 3)
        {
            score += 1500;
        }
        else
        {
            score += 2000;
        }
    }
    public static void ore()
    {
        score += 50;
    }
    public static void dig()
    {
        score += 10;
    }
}

