using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    GameMaster gm;

    public bool locked;

    public LayerMask cursorBoundary;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameMaster>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.gameIsPaused)
        {
            CursorMove();
            CursorFollow();
            UnitSelect();
        }
    }

    //Selection
    public void UnitSelect()
    {
        if (!locked && gm.selectedUnit == null && Input.GetKeyDown(KeyCode.Z))
        {
            foreach (Unit unit in FindObjectsOfType<Unit>())
            {
                if (unit.transform.position == transform.position)
                {
                    gm.PlayerUnitSelect(unit);
                }
            }
        }
    }

    //Movement
    public void CursorMove()
    {
        if (!locked && gm.selectedUnit == null)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Collider2D col = Physics2D.OverlapCircle(this.transform.position + Vector3.up, 0.15f, cursorBoundary);
                if (col == null)
                {
                    transform.Translate(Vector2.up);
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Collider2D col = Physics2D.OverlapCircle(this.transform.position + Vector3.down, 0.15f, cursorBoundary);
                if (col == null)
                {
                    transform.Translate(Vector2.down);
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Collider2D col = Physics2D.OverlapCircle(this.transform.position + Vector3.left, 0.15f, cursorBoundary);
                if (col == null)
                {
                    transform.Translate(Vector2.left);
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Collider2D col = Physics2D.OverlapCircle(this.transform.position + Vector3.right, 0.15f, cursorBoundary);
                if (col == null)
                {
                    transform.Translate(Vector2.right);
                }
            }
        }
    }

    public void CursorFollow()
    {
        if (locked && gm.selectedUnit != null && gm.movingUnit)
        {
            transform.position = gm.selectedUnit.transform.position;
        }

        if (locked && gm.selectedUnit != null && gm.targetedUnit != null && gm.attackingUnit)
        {
            transform.position = gm.targetedUnit.transform.position;
        }
    }
}
