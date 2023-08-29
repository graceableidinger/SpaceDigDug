using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; 

[System.Serializable]
public class batController : enemyController
{
    private bool paceStateOver = false;
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
        determineAttackDirection();
    }

    private void FixedUpdate()
    {
        if (!paceStateOver)
        {
            StartCoroutine("paceAroundState");
        } else
        {
            StartCoroutine("ghostChaseState");
        }

        if (attackReady)
        {
            StartCoroutine("attackState");
        }
    }

    protected override IEnumerator paceAroundState()
    {
        float duration = Random.Range(4, 6);
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
        yield return new WaitForSeconds(duration);
        paceStateOver = true;
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
        yield return new WaitForSeconds(5);
        attackReady = true;
    }

    public void determineAttackDirection()
    {
        Quaternion rayQuaternion = Quaternion.FromToRotation(Vector3.right, player.transform.position - transform.position);
        float rayEuler = rayQuaternion.eulerAngles.z;
        if(rayEuler <= 90 || rayEuler >= 270)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        } else if(rayEuler <= 180 || rayEuler <= 270)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }
}