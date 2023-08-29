//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class rockController : MonoBehaviour
//{

//    public LayerMask enemyMask;
//    public LayerMask Player;
//    public LayerMask ground;
//    bool onGround;
//    bool hitEnemy;
//    bool hitPerson;
//    bool fallen = false;

//    // Start is called before the first frame update
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        rockMovement();
//    }
//    public void rockMovement()
//    {
//        onGround = Physics2D.Raycast(this.transform.position, Vector2.down, 0.1f, ground);
//        Debug.DrawRay(this.transform.position, Vector2.down * 0.1f, Color.yellow);
//        if (!onGround && !fallen)
//        {
//            StartCoroutine("fallingSoon");
//            fallen = true;
//        }
//    }
//    IEnumerator fallingSoon()
//    {
//        //abt to fall animation?
//        yield return new WaitForSeconds(2.0f);
//        //fall or move down
//        hitPerson = Physics2D.Raycast(this.transform.position, Vector2.down, 0.1f, Player);
//        hitEnemy = Physics2D.Raycast(this.transform.position, Vector2.down, 0.1f, enemyMask);
//        if (hitPerson)
//        {
//            playerController.die();
//        }
//        if (hitEnemy)
//        {
//            //enemy death in enemy ai
//            scoreCalc.rock();
//        }
//        if (onGround)
//        {
//            //stop movement
//            //off trigger for player encounters
//            yield return new WaitForSeconds(3.0f);
//            //break animation
//            yield return new WaitForSeconds(2.0f);
//            //crumble animation
//            yield return new WaitForSeconds(1.0f);
//            Destroy(gameObject);
//        }
//    }
//}
