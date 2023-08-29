using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class enemyController : MonoBehaviour
{
    [SerializeField]
    public GameObject player;
    public float velocity = playerController.velocity / 2;
    public GameObject tilemapObject;
    public static bool frozen;

    protected Animator animator;
    protected Vector3 startPosition;
    protected Vector3 translation;
    protected float randomNumber;
    protected float timeElapsed = 0;
    protected Tilemap tilemap;
    protected GridLayout gridLayout;
    // Start is called before the first frame update
    void Start()
    {
        translation = new Vector3(1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /* NOTE:
     * The enemy will always be in one of these 3 states. Since we don't have a navmesh, we are going to have to implement an A-Star algorithm for the enemy*/
    /* Enemy death is NOT HANDLED in this script, but is handled in the player script.*/
    protected virtual IEnumerator ghostChaseState()
    {
        if (!isEnemyOnTile(transform.position))
        {
            animator.SetBool("isGhostChasing", false);
        } else
        {
            animator.SetBool("isGhostChasing", true);
        }
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, velocity * Time.deltaTime);
        yield return new WaitForSeconds(5);
    }

    protected virtual IEnumerator attackState()
    {
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isAttacking", false);
        RaycastHit2D meleeHit = Physics2D.Raycast(transform.position, Vector2.left, 1.5f, LayerMask.GetMask("Entities"));
        if (meleeHit.collider.gameObject.tag == "Player")
        {
            Time.timeScale = 0;
        } 
    }

    protected virtual IEnumerator paceAroundState()
    {
        float duration = Random.Range(4, 6);
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, Vector2.left, 5f, LayerMask.GetMask("Ground"));
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position, Vector2.right, 5f, LayerMask.GetMask("Ground"));
        if(rightHit.point.x - transform.position.x <= 0.5)
        {
            translation = new Vector3(-velocity * Time.deltaTime, 0);
        } else if(Mathf.Abs(leftHit.point.x - transform.position.x) <= 0.5)
        {
            translation = new Vector3(velocity * Time.deltaTime, 0);
        }
        transform.Translate(translation);
        yield return new WaitForSeconds(duration);
    }

    //A*STAR ALGORITHM
    /* 1. Determine best axis to move on
     * 3. Determine if a translation 1 unit in the direction of the axis will result in the starting position. (to prevent endless loops, the pacman team missed this. thats why the enemy never targetted the player, they kept looping)
     * 2. Determine if it can move 1 unit in that axis
            - If not, repeat step 1
            - If so, move 1 unit on that axis*/
    protected void mazeChaseState()
    {
        Vector3[] directionalQueue = { new Vector3(0, 1, 0), new Vector3(1, 0, 0), new Vector3(0, -1, 0), new Vector3(-1, 0, 0) };
        Ray ray = new Ray(transform.position, player.transform.position - transform.position);
        //Vector3[] bestDirectionalQueueArray = bestDirectionalQueue(directionalQueue, ray.direction);
        string x = "";
        foreach (int i in bestDirectionalQueueGenerator(ray.direction))
        {
            x += $"{i}, ";
        }
        Debug.Log(x);
        Debug.Log(Quaternion.FromToRotation(Vector3.right, ray.direction).eulerAngles.z);
    }

    //Helper methods
    protected bool isEnemyOnTile(Vector3 enemyPosition)
    {
        if(tilemap.GetTile(gridLayout.WorldToCell(enemyPosition)) == null)
        {
            return false;
        }
        return true;
    }

    //returns an array of z rotations sorted from closest to ray direction to furthest.
    protected int[] bestDirectionalQueueGenerator(Vector3 rayDirection)
    {
        int[] bestDirectionQueue = new int[4];
        int[] fourAxis = { 0, 90, 180, 270 };

        Quaternion rayQuaternion = Quaternion.FromToRotation(Vector3.right, rayDirection);
        float rayEuler = rayQuaternion.eulerAngles.z;

        for (int i = 0; i < bestDirectionQueue.Length; i++)
        {
            float smallestDifference = 1000;
            int probableAngle = fourAxis[i];

            for (int j = i+1; j < bestDirectionQueue.Length-1; j++)
            {
                if (Mathf.Abs(rayEuler - fourAxis[i]) < Mathf.Abs(rayEuler - fourAxis[j]))
                {
                    if (Mathf.Abs(rayEuler - fourAxis[i]) < smallestDifference)
                    {
                        smallestDifference = Mathf.Abs(rayEuler - fourAxis[i]);
                        probableAngle = fourAxis[i];
                    }
                }
                else if (Mathf.Abs(rayEuler - fourAxis[j]) < Mathf.Abs(rayEuler - fourAxis[i]))
                {
                    if (Mathf.Abs(rayEuler - fourAxis[j]) < smallestDifference)
                    {
                        smallestDifference = Mathf.Abs(rayEuler - fourAxis[j]);
                        probableAngle = fourAxis[j];
                    }
                }
            }
            bestDirectionQueue[i] = probableAngle;
        }
        return bestDirectionQueue;
    }
}
