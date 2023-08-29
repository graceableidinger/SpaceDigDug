using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class rockController : MonoBehaviour
{
    [SerializeField]
    public GameObject tilemapObject;
    public Animator animator;

    [HideInInspector]
    private Tilemap tilemap;
    private GridLayout gridLayout;
    private Rigidbody2D rb;
    private bool movementDisabled = false;
    private List<Vector3Int> dugPositions;
    // Start is called before the first frame update
    void Start()
    {
        tilemap = tilemapObject.GetComponent<Tilemap>();
        gridLayout = tilemap.GetComponentInParent<Grid>();
        animator = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rockMovement();
    }

    public void rockMovement()
    {
        RaycastHit2D tilehit = Physics2D.Raycast(transform.position, Vector3.down, 1f, LayerMask.GetMask("Ground"));
        Vector3Int rockTilePosition = gridLayout.WorldToCell(tilehit.point);
        if (tilemap.GetTile(new Vector3Int(rockTilePosition.x, rockTilePosition.y-1, 0)) == null)
        {
            StartCoroutine(rockFallingDelay());
        } 
    }

    private IEnumerator rockFallingDelay()
    {
        yield return new WaitForSeconds(1f);
        transform.Translate(Vector3.down * Time.deltaTime * 2f);
    }
}