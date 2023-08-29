using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class alienController : enemyController
{
    private bool paceStateOver = false;
    private bool attackOn = false;
    private bool attackReady = false;
    // Start is called before the first frame update
    void Start()
    {
        randomNumber = Random.Range(5, 10);
        animator = this.GetComponent<Animator>();
        startPosition = this.transform.position;
        translation = new Vector3(Time.deltaTime, 0, 0);
        tilemap = tilemapObject.GetComponent<Tilemap>();
        gridLayout = tilemap.GetComponentInParent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (attackOn)
        {
            StartCoroutine("ghostChaseState");
            if (attackReady)
            {
                StartCoroutine("attackState");
            }
        } else
        {
            paceAroundState();
        }

        RaycastHit2D meleeHit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, 0.5f, LayerMask.GetMask("Entities"));
        if (meleeHit.collider.gameObject.tag != "Alien_Enemy")
        { 
            StartCoroutine("attackState");
        }
    }

    protected void paceAroundState()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, Vector2.left, 5f, LayerMask.GetMask("Ground"));
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position, Vector2.right, 5f, LayerMask.GetMask("Ground"));
        if (rightHit.point.x - transform.position.x <= 0.5)
        {
            translation = new Vector3(-velocity * Time.deltaTime, 0);
        }
        else if (Mathf.Abs(leftHit.point.x - transform.position.x) <= 0.5)
        {
            translation = new Vector3(velocity * Time.deltaTime, 0);
        }
        transform.Translate(translation);
    }

    protected override IEnumerator ghostChaseState()
    {
        if (!isEnemyOnTile(transform.position))
        {
            animator.SetBool("isGhostChasing", false);
        }
        else
        {
            animator.SetBool("isGhostChasing", true);
        }
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, velocity * Time.deltaTime);
        yield return new WaitForSeconds(3);
    }

    protected override IEnumerator attackState()
    {
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isAttacking", false);
        RaycastHit2D meleeHit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, 0.5f, LayerMask.GetMask("Entities"));
        if(meleeHit && meleeHit.collider.gameObject.tag == "Player")
        {
            StartCoroutine(meleeHit.collider.gameObject.GetComponent<playerController>().deathSequence());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            attackOn = true;
            this.GetComponent<CircleCollider2D>().enabled = false;
        }
    }
}

