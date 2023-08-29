using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    [SerializeField]
    public GameObject Jetpack;
    public GameObject SprayBottle;
    public GameObject Food;
    public GameObject Battery;

    public GameObject powerup;

    public static bool test;

    public static float which;

    // Start is called before the first frame update
    void Start()
    {
        test = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (test)
        {
            Debug.Log("here");
            test = false;
            which = Random.Range(1.1f, 4.0f);
            if (which <= 1.0f)
            {
                GameObject powerup = Instantiate(SprayBottle);
            }
            else if (which <= 2.0f)
            {
                GameObject powerup = Instantiate(Battery);
            }
            else if (which <= 3.0f)
            {
                GameObject powerup = Instantiate(Food);
            }
            else if (which <= 4.0f)
            {
                GameObject powerup = Instantiate(Jetpack);
            }
            powerup.transform.position = new Vector3(0, 2.5f);
        }
    }
    /*public static void spawn()
    {
        which = Random.Range(0.0f, 4.0f);
        if (which <= 1.0f)
        {
            GameObject powerup = Instantiate(SprayBottle);
        }
        else if (which <= 2.0f)
        {
            GameObject powerup = Instantiate(Battery);
        }
        else if (which <= 3.0f)
        {
            GameObject powerup = Instantiate(Food);
        }
        else if (which <= 4.0f)
        {
            GameObject powerup = Instantiate(Jetpack);
        }
        powerup.transform.position = new Vector3(0, 0, 0);
        //StartCoroutine("countdown");
    }*/

}

