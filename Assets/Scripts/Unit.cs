using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    GameMaster gm;
    BattleMaster bm;

    //Resetting Movement
    private Vector2 previousTransorm;

    //Names
    public string unitName;

    /*
     *  THE ATTRIBUTES BLOCK
     *  THE ATTRIBUTES BLOCK
     *  THE ATTRIBUTES BLOCK
     *  THE ATTRIBUTES BLOCK
     */

    //XP Stats
    //For players
    public int level;
    public int xp;

    //For enemies
    public int xpValue;

    //Stats
    public int health;
    public int attackPower;
    public int defense;
    public int stability;
    public int speed;
    public int luck;

    public int moves;

    //Attack stuff
    public int attackRange;
    List<Unit> enemiesInRange = new List<Unit>();
    public int attackCount;
    private int selectedEnemy;

    //Battle specifiers
    public string attackType;
    public bool aoeType;

    //Enemy attack stuff
    bool enemyHasAttacked = false;

    //Identification
    public bool isEnemy;
    public bool isFriendly;

    public bool isDataMined;

    /*
     *  THE FUNCTIONS BEGIN
     *  THE FUNCTIONS BEGIN
     *  THE FUNCTIONS BEGIN
     *  THE FUNCTIONS BEGIN
     */

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameMaster>();
        bm = FindObjectOfType<BattleMaster>();

        //Get current level if not datamined
        if (isFriendly && !isDataMined)
        {
            //Get level
        }

        moves = Mathf.Abs(speed / 2);
        attackCount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.gameIsPaused)
        {
            PlayerMovement();
            PlayerAttackDecide();
        }
    }



    //Stats
    public void AddXP(int xpAmount)
    {
        xp += xpAmount;
        //Try to level up
    }

    /*
     *  PLAYER FUNCTIONS
     *  PLAYER FUNCTIONS
     *  PLAYER FUNCTIONS
     *  PLAYER FUNCTIONS
     */

    //Player Moving
    public void GetPastPosition()
    {
        previousTransorm = transform.position;
    }

    public void GetPlayerMoves()
    {
        Debug.Log("GetPlayerMoves() Called");
        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            if (tile.transform.position == transform.position)
            {
                tile.moveEvaluated = true;
                tile.moveCount = moves;
                tile.MoveHighlight();
            }
        }

        for (int movesLeft = moves; movesLeft > 0; movesLeft -= 1)
        {
            foreach (Tile startTile in FindObjectsOfType<Tile>())
            {
                if (startTile.moveCount == movesLeft)
                {
                    foreach (Tile tile in FindObjectsOfType<Tile>())
                    {
                        if (Mathf.Abs(startTile.transform.position.x - tile.transform.position.x) + Mathf.Abs(startTile.transform.position.y - tile.transform.position.y) == 1 && !tile.moveEvaluated)
                        {
                            //Regular
                            if (tile.IsClear() && !tile.isRough && movesLeft >= 1)
                            {
                                tile.moveCount = movesLeft - 1;
                                tile.moveEvaluated = true;
                                tile.MoveHighlight();
                            }
                            //Rough
                            if (tile.IsClear() && tile.isRough && movesLeft >= 2)
                            {
                                tile.moveCount = movesLeft - 2;
                                tile.moveEvaluated = true;
                                tile.MoveHighlight();
                            }
                        }
                    }
                }
            }
        }   
    }

    public void PlayerMovement() 
    {
        if (isFriendly && gm.selectedUnit == this && gm.movingUnit)
        {
            //Moving
            //Up
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                foreach (Tile tile in FindObjectsOfType<Tile>())
                {
                    if (tile.transform.position.x == this.transform.position.x && tile.transform.position.y == this.transform.position.y + 1 && tile.IsClear() && tile.moveEvaluated)
                    {
                        transform.Translate(Vector2.up);
                        return;
                    }
                }
            }
            //Down
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                foreach (Tile tile in FindObjectsOfType<Tile>())
                {
                    if (tile.transform.position.x == this.transform.position.x && tile.transform.position.y == this.transform.position.y - 1 && tile.IsClear() && tile.moveEvaluated)
                    {
                        transform.Translate(Vector2.down);
                        return;
                    }
                }
            }
            //Left
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                foreach (Tile tile in FindObjectsOfType<Tile>())
                {
                    if (tile.transform.position.x == this.transform.position.x - 1 && tile.transform.position.y == this.transform.position.y && tile.IsClear() && tile.moveEvaluated)
                    {
                        transform.Translate(Vector2.left);
                        return;
                    }
                }
            }
            //Right
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                foreach (Tile tile in FindObjectsOfType<Tile>())
                {
                    if (tile.transform.position.x == this.transform.position.x + 1 && tile.transform.position.y == this.transform.position.y && tile.IsClear() && tile.moveEvaluated)
                    {
                        transform.Translate(Vector2.right);
                        return;
                    }
                }
            }
        }

        //Selection
        if (isFriendly && gm.selectedUnit == this && gm.movingUnit)
        {
            //Confirmation
            if (Input.GetKeyDown(KeyCode.Z))
            {
                foreach (Tile tile in FindObjectsOfType<Tile>())
                {
                    if (tile.transform.position == transform.position)
                    {
                        moves = tile.moveCount;
                    }
                }

                gm.ResetTiles();
                gm.PlayerUnitSelect(this);
            }

            //De-selection
            else if (Input.GetKeyDown(KeyCode.X))
            {
                transform.position = previousTransorm;
                gm.ResetTiles();
                gm.PlayerUnitSelect(this);
                gm.playerCursor.transform.position = transform.position;
            }
        }
    }


    //Player Attacking
    public void GetPlayerEnemies()
    {
        enemiesInRange.Clear();

        if (isFriendly)
        {
            foreach (Unit unit in FindObjectsOfType<Unit>())
            {
                if (Mathf.Abs(transform.position.x - unit.transform.position.x) + Mathf.Abs(transform.position.y - unit.transform.position.y) <= attackRange && unit.isEnemy)
                {
                    enemiesInRange.Add(unit);
                }
            }
            foreach (Tile tile in FindObjectsOfType<Tile>())
            {
                if (Mathf.Abs(transform.position.x - tile.transform.position.x) + Mathf.Abs(transform.position.y - tile.transform.position.y) <= attackRange && tile.transform.position != transform.position)
                {
                    tile.AttackHighlight();
                }
            }
        }
    }

    public void PlayerAttackDecide()
    {
        if (gm.attackingUnit && gm.selectedUnit == this)
        {
            if (enemiesInRange.Count > 0)
            {
                //Moving the cursor
                //Something if empty
                if (gm.attackingUnit && gm.targetedUnit == null)
                {
                    gm.targetedUnit = enemiesInRange[0];
                }

                //Inputs
                if (Input.GetKeyDown(KeyCode.E))
                {
                    selectedEnemy += 1;
                }
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    selectedEnemy -= 1;
                }

                //Over-and-under fixes
                if (selectedEnemy < 0)
                {
                    selectedEnemy = enemiesInRange.Count - 1;
                }
                if (selectedEnemy > enemiesInRange.Count - 1)
                {
                    selectedEnemy = 0;
                }

                gm.targetedUnit = enemiesInRange[selectedEnemy];

                //Selection
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    gm.ResetTiles();
                    attackCount -= 1;
                    if (!aoeType)
                    {
                        bm.SetupNormalBattle(this, gm.targetedUnit, attackType);
                    }
                    else if (aoeType)
                    {
                        bm.SetupAOEBattle(this, gm.targetedUnit, attackType);
                    }
                }

                //De-selection
                if (Input.GetKeyDown(KeyCode.X))
                {
                    gm.ResetTiles();
                    gm.PlayerUnitSelect(this);
                    gm.playerCursor.transform.position = transform.position;
                }
            }

            //De-selection
            if (Input.GetKeyDown(KeyCode.X))
            {
                gm.ResetTiles();
                gm.PlayerUnitSelect(this);
                gm.playerCursor.transform.position = transform.position;
            }
        }
    }

    /*
     *  TURN FUNCTIONS
     *  TURN FUNCTIONS
     *  TURN FUNCTIONS
     *  TURN FUNCTIONS
     */

    //Turns
    public void StartPlayerTurn()
    {
        moves = Mathf.Abs(speed / 2);
        attackCount = 1;
    }

    public void StartEnemyTurn()
    {
        moves = Mathf.Abs(speed / 2);
        attackCount = 1;

        StartEnemyMovement();
    }

    public void StartEnemyMovement()
    {
        //The unit monolith
        if (unitName == "Junk Pile")
        {
            StartCoroutine(DumbAggroEnemyMovement());
        }
    }

    /*
     *  ENEMY TILES
     *  ENEMY TILES
     *  ENEMY TILES
     *  ENEMY TILES
     */

    //Enemy Attack Tiles
    public void GetEnemyAttackTiles()
    {
        if (isEnemy)
        {
            foreach (Tile tile in FindObjectsOfType<Tile>())
            {
                if (Mathf.Abs(transform.position.x - tile.transform.position.x) + Mathf.Abs(transform.position.y - tile.transform.position.y) <= attackRange && tile.transform.position != transform.position)
                {
                    tile.AttackHighlight();
                }
            }
        }
    }

    /*
     *  ENEMY FUNCTIONS
     *  ENEMY FUNCTIONS
     *  ENEMY FUNCTIONS
     *  ENEMY FUNCTIONS
     */

    //Enemy Moving
    //Pathfinding
    public void FindPath(Tile startTile, Tile targetTile)
    {
        List<Tile> openTiles = new List<Tile>();
        List<Tile> closedTiles = new List<Tile>();
        openTiles.Add(startTile);

        while (openTiles.Count > 0)
        {
            Tile currentTile = openTiles[0];
            for (int i = 1; i < openTiles.Count; i += 1)
            {
                if (openTiles[i].fCost < currentTile.fCost || openTiles[i].fCost == currentTile.fCost && openTiles[i].hCost < currentTile.hCost)
                {
                    currentTile = openTiles[i];
                }
            }

            openTiles.Remove(currentTile);
            closedTiles.Add(currentTile);

            //Check neighbours
            List<Tile> neighbours = new List<Tile>();
            foreach (Tile tile in FindObjectsOfType<Tile>())
            {
                if (Mathf.Abs(currentTile.transform.position.x - tile.transform.position.x) + Mathf.Abs(currentTile.transform.position.y - tile.transform.position.y) == 1)
                {
                    neighbours.Add(tile);
                }
            }

            foreach (Tile neighbour in neighbours)
            {
                if ((!neighbour.IsClear() && neighbour != targetTile) || closedTiles.Contains(neighbour))
                {
                    continue;
                }

                int newNeighbourMovementCost = currentTile.gCost + GetTileDistance(currentTile, neighbour);
                if (newNeighbourMovementCost < neighbour.gCost || !openTiles.Contains(neighbour))
                {
                    neighbour.gCost = newNeighbourMovementCost;
                    neighbour.hCost = GetTileDistance(neighbour, targetTile);
                    neighbour.parent = currentTile;

                    if (!openTiles.Contains(neighbour))
                    {
                        openTiles.Add(neighbour);
                    }
                }
            }

            //Are we done?
            if (currentTile == targetTile)
            {
                return;
            }
        }
    }

    int GetTileDistance(Tile start, Tile end)
    {
        int distance = (int)(Mathf.Abs(start.transform.position.x - end.transform.position.x) + Mathf.Abs(start.transform.position.y - end.transform.position.y));
        return distance;
    }

    List<Tile> SortPath(Tile startTile, Tile endTile)
    {
        List<Tile> path = new List<Tile>();
        Tile currentTile = endTile;

        while (currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.parent;
        }
        path.Reverse();

        return path;
    }


    //Movement lists
    //Run straight at player and attack once in range
    IEnumerator DumbAggroEnemyMovement()
    {
        gm.movingUnit = true;

        //Variables
        Unit moveTarget = null;
        List<Unit> friendlyList = new List<Unit>();
        enemyHasAttacked = false;

        //List of player units
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            if (unit.isFriendly) 
            {
                friendlyList.Add(unit);
            }
        }

        //Find closest unit
        foreach (Unit unit in friendlyList)
        {
            if (moveTarget == null)
            {
                moveTarget = unit;
            }
            else if (Vector2.Distance(unit.transform.position, transform.position) < Vector2.Distance(moveTarget.transform.position, transform.position))
            {
                moveTarget = unit;
            }
        }

        //Get path start and end tiles
        Tile pathStartTile = null;
        Tile pathEndTile = null;
        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            if (tile.transform.position.x == transform.position.x && tile.transform.position.y == transform.position.y)
            {
                pathStartTile = tile;
            }
            else if (tile.transform.position == moveTarget.transform.position && tile.transform.position.y == moveTarget.transform.position.y)
            {
                pathEndTile = tile;
            }
        }

        //Pathfind
        FindPath(pathStartTile, pathEndTile);
        List<Tile> path = new List<Tile>();
        path = SortPath(pathStartTile, pathEndTile);

        //Wait a beat
        yield return new WaitForSeconds(0.75f);

        //Move
        int pathCounter = 0;

        while (moves > 0)
        {
            //Check if the target is within range
            if ((Mathf.Abs(transform.position.x - moveTarget.transform.position.x) + Mathf.Abs(transform.position.y - moveTarget.transform.position.y)) <= attackRange)
            {
                moves = 0;
                if (attackCount > 0)
                {
                    //Show attack range as well as target
                    GetEnemyAttackTiles();
                    gm.movingUnit = false;
                    gm.targetedUnit = moveTarget;
                    gm.attackingUnit = true;

                    //Wait a beat
                    yield return new WaitForSeconds(1f);

                    gm.targetedUnit = null;
                    gm.attackingUnit = false;

                    //Attack
                    enemyHasAttacked = true;
                    attackCount -= 1;

                    //Attacks depend on unit
                    if (unitName == "Junk Pile")
                    {
                        bm.SetupNormalBattle(this, moveTarget, "Normal");
                    }
                }
                yield break;
            }

            //Find the next part of the path            

            //Up
            if (path[pathCounter].transform.position.y > transform.position.y)
            {
                if (path[pathCounter].IsClear())
                {
                    if (path[pathCounter].isRough && moves >= 2)
                    {
                        transform.Translate(Vector2.up);
                        moves -= 2;
                    }
                    else if (!path[pathCounter].isRough && moves >= 1)
                    {
                        transform.Translate(Vector2.up);
                        moves -= 1;
                    }
                }
            }
            //Down
            if (path[pathCounter].transform.position.y < transform.position.y)
            {
                if (path[pathCounter].IsClear())
                {
                    if (path[pathCounter].isRough && moves >= 2)
                    {
                        transform.Translate(Vector2.down);
                        moves -= 2;
                    }
                    else if (!path[pathCounter].isRough && moves >= 1)
                    {
                        transform.Translate(Vector2.down);
                        moves -= 1;
                    }
                }
            }
            //Left
            else if (path[pathCounter].transform.position.x < transform.position.x)
            {
                if (path[pathCounter].IsClear())
                {
                    if (path[pathCounter].isRough && moves >= 2)
                    {
                        transform.Translate(Vector2.left);
                        moves -= 2;
                    }
                    else if (!path[pathCounter].isRough && moves >= 1)
                    {
                        transform.Translate(Vector2.left);
                        moves -= 1;
                    }
                }
            }
            //Right
            else if (path[pathCounter].transform.position.x > transform.position.x)
            {
                if (path[pathCounter].IsClear())
                {
                    if (path[pathCounter].isRough && moves >= 2)
                    {
                        transform.Translate(Vector2.right);
                        moves -= 2;
                    }
                    else if (!path[pathCounter].isRough && moves >= 1)
                    {
                        transform.Translate(Vector2.right);
                        moves -= 1;
                    }
                }
            }
            pathCounter += 1;

            yield return new WaitForSeconds(0.75f);

            //Check if the target is within range (again)
            if ((Mathf.Abs(transform.position.x - moveTarget.transform.position.x) + Mathf.Abs(transform.position.y - moveTarget.transform.position.y)) <= attackRange)
            {
                moves = 0;
                if (attackCount > 0)
                {
                    //Show attack range as well as target
                    GetEnemyAttackTiles();
                    gm.movingUnit = false;
                    gm.targetedUnit = moveTarget;
                    gm.attackingUnit = true;

                    //Wait a beat
                    yield return new WaitForSeconds(1f);

                    gm.targetedUnit = null;
                    gm.attackingUnit = false;

                    //Attack
                    enemyHasAttacked = true;
                    attackCount -= 1;
                    
                    //Attacks depend on unit
                    if (unitName == "Junk Pile")
                    {
                        bm.SetupNormalBattle(this, moveTarget, "Normal");
                    }
                }
                yield break;
            }

            //Wait a beat
            yield return new WaitForSeconds(0.75f);
        }

        //Check to see if we can continue
        if (!enemyHasAttacked)
        {
            gm.EnemyPassPriority();
        }
        //Debug.Log("Done");
    }
}
