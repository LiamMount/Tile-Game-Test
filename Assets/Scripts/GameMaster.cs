using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    //Controls
    public BattleMaster bm;

    public PauseMenu pm;
    public bool gameIsPaused = false;

    public CameraFollow camScript;

    public PlayerCursor playerCursor;
    public EnemyCursor enemyCursor;

    //Units
    public Unit selectedUnit;
    public Unit targetedUnit;

    public int playerTurn = 1;

    public bool movingUnit;
    public bool attackingUnit;

    //Menus
    public LiamMenu liamMenu;
    public BenMenu benMenu;
    public TimMenu timMenu;
    public SamMenu samMenu;
    public ColinMenu colinMenu;
    public LukeMenu lukeMenu;
    /*
    public LukeMenu lukeMenu;
    public SamMenu samMenu;

    public FriendlyMenu friendlyMenu;
    public EnemyMenu enemyMenu;
    */

    //Enemy Management
    public int currentEnemy;
    public List<Unit> enemies = new List<Unit>();

    // Start is called before the first frame update
    void Start()
    {
        //Cams and controls
        bm = FindObjectOfType<BattleMaster>();
        pm = FindObjectOfType<PauseMenu>();

        playerCursor = FindObjectOfType<PlayerCursor>();
        playerCursor.gameObject.SetActive(true);

        enemyCursor = FindObjectOfType<EnemyCursor>();
        enemyCursor.gameObject.SetActive(false);

        camScript = FindObjectOfType<CameraFollow>();
        camScript.objectToFollow = playerCursor.gameObject;

        //Menus
        liamMenu = FindObjectOfType<LiamMenu>();
        liamMenu.gameObject.SetActive(false);

        benMenu = FindObjectOfType<BenMenu>();
        benMenu.gameObject.SetActive(false);

        timMenu = FindObjectOfType<TimMenu>();
        timMenu.gameObject.SetActive(false);

        samMenu = FindObjectOfType<SamMenu>();
        samMenu.gameObject.SetActive(false);

        colinMenu = FindObjectOfType<ColinMenu>();
        colinMenu.gameObject.SetActive(false);

        lukeMenu = FindObjectOfType<LukeMenu>();
        lukeMenu.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Temporary
        if (Input.GetKeyDown(KeyCode.P) && !playerCursor.locked && !movingUnit && !attackingUnit)
        {
            //Start Enemy Turn
            if (playerTurn == 1)
            {
                StartEnemyTurn();
            }
            //Start Player Turn
            else if (playerTurn == 2)
            {
                StartPlayerTurn();
            }
        }
        //Temporary

    }

    public void ResetTiles()
    {
        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            tile.Reset();
        }
    }

    //Turns
    //Player Turn
    public void StartPlayerTurn()
    {
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            if (unit.isFriendly)
            {
                unit.StartPlayerTurn();
            }
        }

        playerTurn = 1;
        playerCursor.gameObject.SetActive(true);
        enemyCursor.gameObject.SetActive(false);

        camScript.objectToFollow = playerCursor.gameObject;
    }

    //Enemy Turn
    public void StartEnemyTurn()
    {
        playerTurn = 2;
        playerCursor.gameObject.SetActive(false);
        enemyCursor.gameObject.SetActive(true);

        camScript.objectToFollow = enemyCursor.gameObject;

        currentEnemy = 0;
        enemies.Clear();
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            if (unit.isEnemy)
            {
                enemies.Add(unit);
            }
        }
        if (enemies.Count > 0)
        {
            selectedUnit = enemies[0];
            selectedUnit.StartEnemyTurn();
        }
    }

    //Unit selection
    public void PlayerUnitSelect(Unit unit)
    {
        selectedUnit = unit;
        playerCursor.locked = true;
        movingUnit = false;
        attackingUnit = false;

        if (unit.isFriendly && unit.unitName == "Liam")
        {
            liamMenu.gameObject.SetActive(true);
            liamMenu.ResetMenu();
        }
        else if (unit.isFriendly && unit.unitName == "Ben")
        {
            benMenu.gameObject.SetActive(true);
            benMenu.ResetMenu();
        }
        else if (unit.isFriendly && unit.unitName == "Tim")
        {
            timMenu.gameObject.SetActive(true);
            timMenu.ResetMenu();
        }
        else if (unit.isFriendly && unit.unitName == "Sam")
        {
            samMenu.gameObject.SetActive(true);
            samMenu.ResetMenu();
        }
        else if (unit.isFriendly && unit.unitName == "Colin")
        {
            colinMenu.gameObject.SetActive(true);
            colinMenu.ResetMenu();
        }
        else if (unit.isFriendly && unit.unitName == "Luke")
        {
            lukeMenu.gameObject.SetActive(true);
            lukeMenu.ResetMenu();
        }
        else if (unit.isFriendly)
        {
            //DataMinedMenu(); Or something
        }

        else if (unit.isEnemy)
        {
            //Temporary
            PlayerUnitDeselect();
            //Temporary
        }
    }

    public void PlayerUnitDeselect()
    {
        movingUnit = false;
        attackingUnit = false;

        selectedUnit = null;
        playerCursor.locked = false;
    }

    //Enemy selection
    public void EnemyPassPriority()
    {
        movingUnit = false;
        attackingUnit = false;

        selectedUnit = null;
        targetedUnit = null;

        currentEnemy += 1;
        if (currentEnemy >= enemies.Count)
        {
            currentEnemy = 0;
            StartPlayerTurn();
        }
        else
        {
            selectedUnit = enemies[currentEnemy];
            selectedUnit.StartEnemyTurn();
        }
    }
}
