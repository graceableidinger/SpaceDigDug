using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class playerController : MonoBehaviour
{
    public static float velocity = 3;
    [SerializeField]
    public Tilemap world;
    public GridLayout gridLayout;

    [HideInInspector]
    private bool rotate90Clockwise = false;
    private Animator animator;
    private Vector3 currentRotation = new Vector3(0, 0, 0);
    public static bool movementDisabled = false;
    private Rigidbody2D rb;
    private bool win = false;
    private bool lose = false;

    public static int layer = 1;

    // Start is called before the first frame update
    void Start()
    {
        transform.localEulerAngles = new Vector3(0, 0, 0);
        animator = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        transform.position = new Vector3(6.27f, 2.39f);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y >= -3)
        {
            layer = 1;
        }
        else if (transform.position.y >= -9)
        {
            layer = 2;
        }
        else if(transform.position.y >= -15)
        {
            layer = 3;
        }
        else if(transform.position.y >= -21)
        {
            layer = 4;
        }
        else
        {
            layer = 5;
        }
        if (Input.GetMouseButtonDown(0))
        {
            attack();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            shoot();
        }
        if (transform.localRotation.eulerAngles.z == 180)
        {
            rotate90Clockwise = true;
        }
        GetComponent<Rigidbody2D>().freezeRotation = true;

        if (movementDisabled)
        {
            this.GetComponent<playerController>().enabled = false;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(miningSequence());
        }

        if(transform.position.y < -31)
        {
            win = true;
        }

        if (win)
        {

        } else if (lose)
        {

        }
    }

    private void FixedUpdate()
    {
        movement(movementDisabled);

    }

    private IEnumerator miningSequence()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, getRayDirection(transform.localEulerAngles), 0.5f, LayerMask.GetMask("Ground"));
        Vector3 miningPosition = transform.position;
        Vector3Int deleteTilePosition = new Vector3Int(gridLayout.WorldToCell(hit.point).x, gridLayout.WorldToCell(hit.point).y - 1, gridLayout.WorldToCell(hit.point).z);
        if (hit && hit.transform.gameObject.tag != "Player")
        {
            if (approximately(Mathf.Abs(transform.localEulerAngles.z), 90, 1))
            {
                deleteTilePosition = new Vector3Int(gridLayout.WorldToCell(hit.point).x, gridLayout.WorldToCell(hit.point).y, gridLayout.WorldToCell(hit.point).z);
            }
            else if (approximately(Mathf.Abs(transform.localEulerAngles.z), 180, 1))
            {
                deleteTilePosition = new Vector3Int(gridLayout.WorldToCell(hit.point).x - 1, gridLayout.WorldToCell(hit.point).y, gridLayout.WorldToCell(hit.point).z);
            }
            else if (approximately(Mathf.Abs(transform.localEulerAngles.z), 0, 1))
            {
                deleteTilePosition = new Vector3Int(gridLayout.WorldToCell(hit.point).x, gridLayout.WorldToCell(hit.point).y, gridLayout.WorldToCell(hit.point).z);
            }
        }
        animator.SetBool("isMining", true);
        yield return new WaitForSeconds(2f);
        if(world.GetTile(deleteTilePosition).ToString().IndexOf("ore") != -1 && transform.position == miningPosition)
        {
            AudioManagerDigdug.Instance.PlayMiningSound();
            scoreCalc.ore();
            world.SetTile(deleteTilePosition, null);
        }
        animator.SetBool("isMining", false);
    }

    private bool approximately(float number2, float number1, float threshold)
    {
        return ((number2 - number1) < 0 ? ((number2 - number1) * -1) : (number2 - number1)) <= threshold;
    }

    private Vector2 getRayDirection(Vector3 currentEulerAngles)
    {
        Vector2 rayCastDirection = new Vector2(1, 0);
        if (approximately(currentEulerAngles.z, 90, 1))
        {
            rayCastDirection = new Vector2(0, 1);
        }
        else if (approximately(currentEulerAngles.z, 180, 1))
        {
            rayCastDirection = new Vector2(-1, 0);
        }
        else if (approximately(currentEulerAngles.z, 270, 1))
        {
            rayCastDirection = new Vector2(0, -1);
        }
        return rayCastDirection;
    }

    IEnumerator attackSequence()
    {
        animator.SetBool("isAttacking", true);
        AudioManagerDigdug.Instance.PlaySaberSound();
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("isAttacking", false);
    }

    private void attack()
    {
        StartCoroutine("attackSequence");
        RaycastHit2D meleeHit = Physics2D.Raycast(transform.position, currentRotation, 2f, LayerMask.GetMask("Entities"));
        if(meleeHit && meleeHit.collider.gameObject.tag != "Player")
        {
            StartCoroutine(playEnemyDeath(meleeHit.collider.gameObject));
        }
    }

    IEnumerator playEnemyDeath(GameObject enemyGameObject)
    {
        Animator enemyAnimator = enemyGameObject.GetComponent<Animator>();
        enemyAnimator.SetBool("isDead", true);
        AudioManagerDigdug.Instance.PlayEnemyDeathSound();
        scoreCalc.enemyKilled(layer);
        yield return new WaitForSeconds(1f);
        Destroy(enemyGameObject);
    }

    IEnumerator shootSequence()
    {
        animator.SetBool("isShooting", true);
        AudioManagerDigdug.Instance.PlayLaserSound();
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isShooting", false);
    }

    private void shoot()
    {
        StartCoroutine("shootSequence");
        RaycastHit2D shootHit = Physics2D.Raycast(transform.position, currentRotation, 3f, LayerMask.GetMask("Entities"));
        if (shootHit && shootHit.collider.gameObject.tag != "Player")
        {
            StartCoroutine(playEnemyDeath(shootHit.collider.gameObject));
        }
    }
    
    private void movement(bool movementDisabled)
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) && !movementDisabled)
        {
            animator.SetBool("isRunning", true);
            transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.y));
            if (Input.GetKey(KeyCode.W))
            {
                transform.localEulerAngles = new Vector3(0, 0, 90);
                if (rotate90Clockwise)
                {
                    transform.localScale = new Vector3(transform.localScale.x, -Mathf.Abs(transform.localScale.y));
                }
                currentRotation = new Vector3(0, 1, 0);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.localEulerAngles = new Vector3(0, 0, 270);
                if (rotate90Clockwise)
                {
                    transform.localScale = new Vector3(transform.localScale.x, -Mathf.Abs(transform.localScale.y));
                }
                currentRotation = new Vector3(0, -1, 0);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.localEulerAngles = new Vector3(0, 0, 180);
                transform.localScale = new Vector3(transform.localScale.x, -Mathf.Abs(transform.localScale.y));
                currentRotation = new Vector3(-1, 0, 0);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                rotate90Clockwise = false;
                transform.localEulerAngles = new Vector3(0, 0, 0);
                currentRotation = new Vector3(1, 0, 0);
            }
            transform.Translate(new Vector3(velocity * Time.deltaTime, 0, 0));
        } else
        {
            animator.SetBool("isRunning", false);
        }
    }

    public IEnumerator deathSequence()
    {
        movementDisabled = true;
        if (currentRotation != new Vector3(1, 0, 0) || currentRotation != new Vector3(-1, 0, 0))
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y));
        }
        animator.SetBool("isDead", true);
        AudioManagerDigdug.Instance.PlayPlayerDeathSound();
        yield return new WaitForSeconds(0.5f);
        lose = true;
    }

    private void OnCollision2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Alien_Enemy" || collision.gameObject.tag == "Bat_Enemy")
        {
            StartCoroutine(deathSequence()); 
        }
    }
}