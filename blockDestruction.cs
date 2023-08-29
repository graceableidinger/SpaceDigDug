using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class blockDestruction : MonoBehaviour
{
    public GameObject tilemapObject;

    private Tilemap tilemap;

    private GridLayout gridLayout;

    private float timeElapsed;
    // Start is called before the first frame update
    void Start()
    {
        tilemap = tilemapObject.GetComponent<Tilemap>();
        gridLayout = tilemap.GetComponentInParent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if(timeElapsed > 0.5f)
        {
            destroyBlockInFront();
            timeElapsed = 0;
        } 
    }

    private void destroyBlockInFront()
    {    
        RaycastHit2D hit = Physics2D.Raycast(transform.position, getRayDirection(transform.localEulerAngles), 0.5f, LayerMask.GetMask("Ground"));
        if(hit && hit.transform.gameObject.tag != "Player")
        {
            Vector3Int deleteTilePosition = new Vector3Int(gridLayout.WorldToCell(hit.point).x, gridLayout.WorldToCell(hit.point).y-1, gridLayout.WorldToCell(hit.point).z);
            if (approximately(Mathf.Abs(transform.localEulerAngles.z), 90, 1))
            {
                deleteTilePosition = new Vector3Int(gridLayout.WorldToCell(hit.point).x, gridLayout.WorldToCell(hit.point).y, gridLayout.WorldToCell(hit.point).z);
            } else if(approximately(Mathf.Abs(transform.localEulerAngles.z), 180, 1))
            {
                deleteTilePosition = new Vector3Int(gridLayout.WorldToCell(hit.point).x-1, gridLayout.WorldToCell(hit.point).y, gridLayout.WorldToCell(hit.point).z);
            } else if(approximately(Mathf.Abs(transform.localEulerAngles.z), 0, 1))
            {
                deleteTilePosition = new Vector3Int(gridLayout.WorldToCell(hit.point).x, gridLayout.WorldToCell(hit.point).y, gridLayout.WorldToCell(hit.point).z);
            }

            if (tilemap.GetTile(deleteTilePosition).name.IndexOf("ore") == -1)
            {
                StartCoroutine(blockDestructionAnimation(deleteTilePosition));
            }
        }
    }

    private IEnumerator blockDestructionAnimation(Vector3Int deleteTilePosition)
    {
        scoreCalc.dig();
        yield return new WaitForSeconds(0.1f);
        tilemap.SetTile(deleteTilePosition, null);
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
        } else if(approximately(currentEulerAngles.z, 180, 1))
        {
            rayCastDirection = new Vector2(-1, 0);
        } else if(approximately(currentEulerAngles.z, 270, 1))
        {
            rayCastDirection = new Vector2(0, -1);
        }
        return rayCastDirection;
    }
}