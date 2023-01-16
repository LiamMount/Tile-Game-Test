using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    GameMaster gm;

    public SpriteRenderer rend;

    public LayerMask obstacleLayer;
    public Color moveColor;
    public Color attackColor;

    public bool isWalkable;

    public bool isRough;

    //For players
    public bool moveEvaluated;
    public int moveCount;

    //For AI
    public int gCost;
    public int hCost;
    public int fCost;

    public Tile parent;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameMaster>();
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsClear()
    {
        Collider2D obstacle = Physics2D.OverlapCircle(transform.position, 0.2f, obstacleLayer);
        if (obstacle != null)
        {
            return false;
        }
        else 
        {
            return true;
        }
    }

    public void MoveHighlight()
    {
        rend.color = moveColor;
    }

    public void AttackHighlight()
    {
        rend.color = attackColor;
    }

    public void Reset()
    {
        rend.color = Color.white;
        isWalkable = false;

        moveEvaluated = false;
        moveCount = 0;

        gCost = 0;
        hCost = 0;
        fCost = 0;

        parent = null;
    }
}
