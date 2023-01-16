using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCursor : MonoBehaviour
{
    GameMaster gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameMaster>();
    }

    // Update is called once per frame
    void Update()
    {
        CursorFollow();
    }

    public void CursorFollow()
    {
        if (gm.selectedUnit != null && gm.movingUnit)
        {
            transform.position = gm.selectedUnit.transform.position;
        }

        if (gm.selectedUnit != null && gm.targetedUnit != null && gm.attackingUnit)
        {
            transform.position = gm.targetedUnit.transform.position;
        }
    }
}
