using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightController : MonoBehaviour
{
    static public lightController instance;
    [SerializeField]
    public Rigidbody2D rb;
    public Light light1;

    GameObject player;
    float speed = 100.0f;

    public float natural;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDirection = (player.transform.position - transform.position).normalized;
        LightMove(moveDirection);
    }
    void LightMove(Vector3 direction)
    {
        rb.velocity = new Vector2(direction.x, direction.y) * speed;
    }

    IEnumerator lighting()
    {
        yield return new WaitForSeconds(5.0f);
        instance.light1.intensity = 1;
        instance.light1.range = instance.natural;
    }
    public void lightingEffect()
    {
        instance.natural = instance.light1.range;
        instance.light1.range = 150.0f;
        instance.light1.intensity = 2;
        Debug.Log(instance.light1.range);
        instance.StartCoroutine("lighting");
    }
}
