using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LiamMenu : MonoBehaviour
{
    GameMaster gm;

    public GameObject arrow;

    public Transform moveTransform;
    public Transform attackTransform;

    public Text menuMovesText;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameMaster>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.gameIsPaused)
        {
            gm.PlayerUnitDeselect();
            this.gameObject.SetActive(false);
        }
        if (!gm.gameIsPaused)
        {
            MenuUsage();
        }
    }

    public void MenuUsage()
    {
        //Arrow movement
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //On move button, up to attack
            if (arrow.transform.position == moveTransform.position)
            {
                arrow.transform.position = attackTransform.position;
            }
            //On attack button, up to move
            else if (arrow.transform.position == attackTransform.position)
            {
                arrow.transform.position = moveTransform.position;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //On move button, down to attack
            if (arrow.transform.position == moveTransform.position)
            {
                arrow.transform.position = attackTransform.position;
            }
            //On attack button, down to move
            else if (arrow.transform.position == attackTransform.position)
            {
                arrow.transform.position = moveTransform.position;
            }
        }

        //Selection
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //Move Button Select
            if (arrow.transform.position == moveTransform.position)
            {
                gm.selectedUnit.GetPastPosition();
                gm.selectedUnit.GetPlayerMoves();
                gm.movingUnit = true;

                this.gameObject.SetActive(false);
            }

            //Attack Button Select
            else if (arrow.transform.position == attackTransform.position)
            {
                if (gm.selectedUnit.attackCount > 0)
                {
                    gm.selectedUnit.attackType = "Sword"; // Change later when we have multiple attack types
                    gm.selectedUnit.aoeType = false;

                    gm.selectedUnit.GetPlayerEnemies();
                    gm.attackingUnit = true;

                    this.gameObject.SetActive(false);
                }
            }
        }
        //De-selection
        if (Input.GetKeyDown(KeyCode.X))
        {
            gm.PlayerUnitDeselect();
            this.gameObject.SetActive(false);
        }
    }

    public void ResetMenu()
    {
        arrow.transform.position = moveTransform.position;
        UpdateText();
    }

    //Text
    public void UpdateText()
    {
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            if (unit.unitName == "Liam")
            {
                menuMovesText.text = unit.moves.ToString();
            }
        }
    }
}
