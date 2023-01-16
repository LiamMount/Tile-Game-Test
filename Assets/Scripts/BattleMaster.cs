using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMaster : MonoBehaviour
{
    GameMaster gm;

    public bool runningFight = false;
    
    public GameObject display;

    //Animation End Timer Logic
    public float endClock = 0f;
    public bool endClockActive = false;

    //Sprite spots
    //Normal/Center
    public Transform leftSpot;
    public Transform rightSpot;

    //AOE
    public Transform leftSpotNorth;
    public Transform leftSpotEast;
    public Transform leftSpotSouth;
    public Transform leftSpotWest;
    public Transform rightSpotNorth;
    public Transform rightSpotEast;
    public Transform rightSpotSouth;
    public Transform rightSpotWest;



    //Sprite prefabs
    //Normal /Center
    public GameObject leftSprite;
    public GameObject rightSprite;

    //AOE
    private GameObject leftSpriteNorth;
    private GameObject leftSpriteEast;
    private GameObject leftSpriteSouth;
    private GameObject leftSpriteWest;
    private GameObject rightSpriteNorth;
    private GameObject rightSpriteEast;
    private GameObject rightSpriteSouth;
    private GameObject rightSpriteWest;

    //Active sprites
    //Normal/Center
    public GameObject leftSpriteInstance;
    public GameObject rightSpriteInstance;

    //AOE
    private GameObject leftSpriteNorthInstance;
    private GameObject leftSpriteEastInstance;
    private GameObject leftSpriteSouthInstance;
    private GameObject leftSpriteWestInstance;
    private GameObject rightSpriteNorthInstance;
    private GameObject rightSpriteEastInstance;
    private GameObject rightSpriteSouthInstance;
    private GameObject rightSpriteWestInstance;

    

    //Fight logic
    public bool usesStability; //What stat for damage resistance
    public string specialAttack; //Specifications for attacks done by units similar to Colin, Tim, Luke, etc.

    /*
     * Colin - "Hammer"
     * Luke - "Golf"
     * Tim - "Spear"
     * 
     * Anything else we think of
     */

    //Normal fight units
    Unit attackingUnit;
    Unit defendingUnit;

    //AOE fight units
    Unit defendingUnitNorth;
    Unit defendingUnitEast;
    Unit defendingUnitSouth;
    Unit defendingUnitWest;



    //public Image Background;

    //Backdrops Storage

    //Sprites storage
    public GameObject liamPrefab;
    public GameObject benPrefab;
    public GameObject timPrefab;
    public GameObject samPrefab;
    public GameObject colinPrefab;
    public GameObject lukePrefab;

    //Enemy
    public GameObject junkPilePrefab;

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

        //Get display and toggle off
        TryGetDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        TryGetDisplay();

        //Count down on the end clock to exit a fight
        if (endClock > 0 && endClockActive)
        {
            endClock -= Time.deltaTime;
            if (endClock <= 0)
            {
                endClockActive = false;
                EndFight();
            }
        }
    }

    void TryGetDisplay()
    {
        if (display == null)
        {
            //Get display and toggle off
            foreach (GameObject stupid in FindObjectsOfType<GameObject>())
            {
                if (stupid.name == "Battle Display")
                {
                    display = stupid;
                    //I had to call it that because "object" and "display" and stuff caused errors
                    display.SetActive(false);
                }
            }
        }
    }

    //Setup

    //The unit monolith
    public void SetNormalUnits(Unit aggressor, Unit defender)
    {
        
        attackingUnit = aggressor;
        defendingUnit = defender;

        //Sets up battle sprites
        //Friendlies
        if (aggressor.unitName == "Liam" || defender.unitName == "Liam")
        {
            leftSprite = liamPrefab;
        }
        if (aggressor.unitName == "Ben" || defender.unitName == "Ben")
        {
            leftSprite = benPrefab;
        }
        if (aggressor.unitName == "Tim" || defender.unitName == "Tim")
        {
            leftSprite = timPrefab;
        }
        if (aggressor.unitName == "Sam" || defender.unitName == "Sam")
        {
            leftSprite = samPrefab;
        }
        if (aggressor.unitName == "Colin" || defender.unitName == "Colin")
        {
            leftSprite = colinPrefab;
        }
        if (aggressor.unitName == "Luke" || defender.unitName == "Luke")
        {
            leftSprite = lukePrefab;
        }

        //Enemies
        if (aggressor.unitName == "Junk Pile" || defender.unitName == "Junk Pile")
        {
            rightSprite = junkPilePrefab;
        }
    }

    //The AOE unit monolith
    public void SetAOEUnits(Unit aggressor, Unit defender)
    {
        attackingUnit = aggressor;
        defendingUnit = defender;

        //Find units close to defender
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            //North
            if (unit.transform.position.x == defender.transform.position.x && unit.transform.position.y == defender.transform.position.y + 1)
            {
                defendingUnitNorth = unit;
            }
            //East
            if (unit.transform.position.x == defender.transform.position.x + 1 && unit.transform.position.y == defender.transform.position.y)
            {
                defendingUnitEast = unit;
            }
            //South
            if (unit.transform.position.x == defender.transform.position.x && unit.transform.position.y == defender.transform.position.y - 1)
            {
                defendingUnitSouth = unit;
            }
            //West
            if (unit.transform.position.x == defender.transform.position.x - 1 && unit.transform.position.y == defender.transform.position.y)
            {
                defendingUnitWest = unit;
            }
        }

        //Sets up battle sprites
        //Friendlies
        if (aggressor.unitName == "Liam" || defender.unitName == "Liam")
        {
            leftSprite = liamPrefab;
        }
        if (aggressor.unitName == "Ben" || defender.unitName == "Ben")
        {
            leftSprite = benPrefab;
        }
        if (aggressor.unitName == "Tim" || defender.unitName == "Tim")
        {
            leftSprite = timPrefab;
        }
        if (aggressor.unitName == "Sam" || defender.unitName == "Sam")
        {
            leftSprite = samPrefab;
        }
        if (aggressor.unitName == "Colin" || defender.unitName == "Colin")
        {
            leftSprite = colinPrefab;
        }
        if (aggressor.unitName == "Luke" || defender.unitName == "Luke")
        {
            leftSprite = lukePrefab;
        }

        //Enemies
        if (aggressor.unitName == "Junk Pile" || defender.unitName == "Junk Pile")
        {
            rightSprite = junkPilePrefab;
        }

        //The rest
        AOEUnitMonolith(defendingUnitNorth, defendingUnitEast, defendingUnitSouth, defendingUnitWest);
    }

    //Set up units in AOE
    //Sets up AOE battle sprites
    //This was the best alternative we had
    public void AOEUnitMonolith(Unit defendingUnitNorth, Unit defendingUnitEast, Unit DefendingUnitSouth, Unit defendingUnitWest)
    {
        //AOE Friendlies
        if (gm.playerTurn == 2)
        {
            //North
            if (defendingUnitNorth != null)
            {
                if (defendingUnitNorth.unitName == "Liam")
                    leftSpriteNorth = liamPrefab;
                if (defendingUnitNorth.unitName == "Ben")
                    leftSpriteNorth = benPrefab;
                if (defendingUnitNorth.unitName == "Tim")
                    leftSpriteNorth = timPrefab;
                if (defendingUnitNorth.unitName == "Sam")
                    leftSpriteNorth = samPrefab;
                if (defendingUnitNorth.unitName == "Colin")
                    leftSpriteNorth = colinPrefab;
                if (defendingUnitNorth.unitName == "Luke")
                    leftSpriteNorth = lukePrefab;
            }

            //East
            if (defendingUnitEast != null)
            {
                if (defendingUnitEast.unitName == "Liam")
                    leftSpriteEast = liamPrefab;
                if (defendingUnitEast.unitName == "Ben")
                    leftSpriteEast = benPrefab;
                if (defendingUnitEast.unitName == "Tim")
                    leftSpriteEast = timPrefab;
                if (defendingUnitEast.unitName == "Sam")
                    leftSpriteEast = samPrefab;
                if (defendingUnitEast.unitName == "Colin")
                    leftSpriteEast = colinPrefab;
                if (defendingUnitEast.unitName == "Luke")
                    leftSpriteEast = lukePrefab;
            }

            //South
            if (defendingUnitSouth != null)
            {
                if (defendingUnitSouth.unitName == "Liam")
                    leftSpriteSouth = liamPrefab;
                if (defendingUnitSouth.unitName == "Ben")
                    leftSpriteSouth = benPrefab;
                if (defendingUnitSouth.unitName == "Tim")
                    leftSpriteSouth = timPrefab;
                if (defendingUnitSouth.unitName == "Sam")
                    leftSpriteSouth = samPrefab;
                if (defendingUnitSouth.unitName == "Colin")
                    leftSpriteSouth = colinPrefab;
                if (defendingUnitSouth.unitName == "Luke")
                    leftSpriteSouth = lukePrefab;
            }

            //West
            if (defendingUnitWest != null)
            {
                if (defendingUnitWest.unitName == "Liam")
                    leftSpriteWest = liamPrefab;
                if (defendingUnitWest.unitName == "Ben")
                    leftSpriteWest = benPrefab;
                if (defendingUnitWest.unitName == "Tim")
                    leftSpriteWest = timPrefab;
                if (defendingUnitWest.unitName == "Sam")
                    leftSpriteWest = samPrefab;
                if (defendingUnitWest.unitName == "Colin")
                    leftSpriteWest = colinPrefab;
                if (defendingUnitWest.unitName == "Luke")
                    leftSpriteWest = lukePrefab;
            }
        }

        //AOE Enemies
        if (gm.playerTurn == 1)
        {
            //North
            if (defendingUnitNorth != null)
            {
                if (defendingUnitNorth.unitName == "Junk Pile")
                    rightSpriteNorth = junkPilePrefab;
            }

            //East
            if (defendingUnitEast != null)
            {
                if (defendingUnitEast.unitName == "Junk Pile")
                    rightSpriteEast = junkPilePrefab;
            }

            //South
            if (defendingUnitSouth != null)
            {
                if (defendingUnitSouth.unitName == "Junk Pile")
                    rightSpriteSouth = junkPilePrefab;
            }

            //West
            if (defendingUnitWest != null)
            {
                if (defendingUnitWest.unitName == "Junk Pile")
                    rightSpriteWest = junkPilePrefab;
            }
        }
    }

    //Battle setup
    public void SetupNormalBattle(Unit aggressor, Unit defender, string attackType)
    {
        runningFight = true;

        SetNormalUnits(aggressor, defender);

        leftSpriteInstance = Instantiate(leftSprite, leftSpot);
        rightSpriteInstance = Instantiate(rightSprite, rightSpot);

        display.SetActive(true);
        NormalFight(attackType);
    }

    public void SetupAOEBattle(Unit aggressor, Unit defender, string attackType)
    {
        runningFight = true;

        SetAOEUnits(aggressor, defender);

        leftSpriteInstance = Instantiate(leftSprite, leftSpot);
        rightSpriteInstance = Instantiate(rightSprite, rightSpot);

        if (gm.playerTurn == 1)
        {
            if (rightSpriteNorth != null)
                rightSpriteNorthInstance = Instantiate(rightSpriteNorth, rightSpotNorth);
            if (rightSpriteEast != null)
                rightSpriteEastInstance = Instantiate(rightSpriteEast, rightSpotEast);
            if (rightSpriteSouth != null)
                rightSpriteSouthInstance = Instantiate(rightSpriteSouth, rightSpotSouth);
            if (rightSpriteWest != null)
                rightSpriteWestInstance = Instantiate(rightSpriteWest, rightSpotWest);
        }
        if (gm.playerTurn == 2)
        {
            if (leftSpriteNorth != null)
                leftSpriteNorthInstance = Instantiate(leftSpriteNorth, leftSpotNorth);
            if (leftSpriteEast != null)
                leftSpriteEastInstance = Instantiate(leftSpriteEast, leftSpotEast);
            if (leftSpriteSouth != null)
                leftSpriteSouthInstance = Instantiate(leftSpriteSouth, leftSpotSouth);
            if (leftSpriteWest != null)
                leftSpriteWestInstance = Instantiate(leftSpriteWest, leftSpotWest);
        }

        display.SetActive(true);
        AOEFight(attackType);
    }

    //End and try to resume enemy turn

    //Battle Functions
    public void NormalFight(string attackType)
    {
        //Player attack
        if (gm.playerTurn == 1)
        {
            leftSpriteInstance.GetComponent<GenericSpriteScript>().attackType = attackType;
            leftSpriteInstance.GetComponent<GenericSpriteScript>().attackTrigger = true;
        }
        //Enemy attack
        if (gm.playerTurn == 2)
        {
            rightSpriteInstance.GetComponent<GenericSpriteScript>().attackType = attackType;
            rightSpriteInstance.GetComponent<GenericSpriteScript>().attackTrigger = true;
        }
    }

    public void AOEFight(string attackType) // Honestly this block is just the same code, I just like the organization
    {
        //Player attack
        if (gm.playerTurn == 1)
        {
            leftSpriteInstance.GetComponent<GenericSpriteScript>().attackType = attackType;
            leftSpriteInstance.GetComponent<GenericSpriteScript>().attackTrigger = true;
        }
        //Enemy attack
        if (gm.playerTurn == 2)
        {
            rightSpriteInstance.GetComponent<GenericSpriteScript>().attackType = attackType;
            rightSpriteInstance.GetComponent<GenericSpriteScript>().attackTrigger = true;
        }
    }

    //Attacker hits defender (Normal Fight)
    public void NormalHitOpponent()
    {
        //Have units deal and take damage (Try to spawn effects in here)
        //Special block
        if (specialAttack != "Normal")
        {
            //Not done yet
            //Tim's spear
            if (specialAttack == "Spear")
            {
                int pendingDamage = attackingUnit.attackPower - defendingUnit.defense;
                if (pendingDamage > 0)
                {
                    defendingUnit.health -= pendingDamage;
                }
                else
                {
                    defendingUnit.health -= 1;
                }
            } // Spear end

            //Not done yet
            //Luke's golf
            if (specialAttack == "Golf")
            {
                int pendingDamage = attackingUnit.attackPower - defendingUnit.stability;
                if (pendingDamage > 0)
                {
                    defendingUnit.health -= pendingDamage;
                }
                else
                {
                    defendingUnit.health -= 1;
                }
            } // Golf end

            //Put stuff here later
            //Maybe Sam's laptop?
        }

        //Regular
        else if (usesStability)
        {
            int pendingDamage = attackingUnit.attackPower - defendingUnit.stability;
            if (pendingDamage > 0)
            {
                defendingUnit.health -= pendingDamage;
            }
            else
            {
                defendingUnit.health -= 1;
            }
        }
        else
        {
            int pendingDamage = attackingUnit.attackPower - defendingUnit.defense;
            if (pendingDamage > 0)
            {
                defendingUnit.health -= pendingDamage;
            }
            else
            {
                defendingUnit.health -= 1;
            }
        }

        //Spawn in damage effects

        //If the player is attacking
        if (gm.playerTurn == 1)
        {
            //Have a death check
            rightSpriteInstance.GetComponent<GenericSpriteScript>().hurtTrigger = true;
        }
        //If the enemy is attacking
        if (gm.playerTurn == 2)
        {
            //Have a death check
            leftSpriteInstance.GetComponent<GenericSpriteScript>().hurtTrigger = true;
        }
    }

    //Attacker hits defenders (AOE Fight)
    public void AOEHitOpponent()
    {
        //Have units deal and take damage (Try to spawn effects in here)
        //Special block
        if (specialAttack != "Normal")
        {
            //Not done yet
            //Ben's grenade
            if (specialAttack == "Grenade")
            {
                int pendingDamage = attackingUnit.attackPower - defendingUnit.stability;
                if (pendingDamage > 0)
                {
                    defendingUnit.health -= pendingDamage;
                    //Splash
                    if (defendingUnitNorth != null)
                        defendingUnitNorth.health -= Mathf.RoundToInt(pendingDamage / 4.0f);
                    if (defendingUnitEast != null)
                        defendingUnitEast.health -= Mathf.RoundToInt(pendingDamage / 4.0f);
                    if (defendingUnitSouth != null)
                        defendingUnitSouth.health -= Mathf.RoundToInt(pendingDamage / 4.0f);
                    if (defendingUnitWest != null)
                        defendingUnitWest.health -= Mathf.RoundToInt(pendingDamage / 4.0f);
                }
                else
                {
                    defendingUnit.health -= 1;
                }
            } // Grenade end

            //Not done yet
            //Colin's Hammer
            if (specialAttack == "Hammer")
            {
                int pendingDamage = attackingUnit.attackPower - defendingUnit.defense;
                if (pendingDamage > 0)
                {
                    defendingUnit.health -= pendingDamage;
                    //Splash
                    if (defendingUnitNorth != null)
                        defendingUnitNorth.health -= Mathf.RoundToInt(pendingDamage / 4.0f);
                    if (defendingUnitEast != null)
                        defendingUnitEast.health -= Mathf.RoundToInt(pendingDamage / 4.0f);
                    if (defendingUnitSouth != null)
                        defendingUnitSouth.health -= Mathf.RoundToInt(pendingDamage / 4.0f);
                    if (defendingUnitWest != null)
                        defendingUnitWest.health -= Mathf.RoundToInt(pendingDamage / 4.0f);
                }
                else
                {
                    defendingUnit.health -= 1;
                }
            } // Hammer end

            //Maybe Tim's something
        }

        //Regular
        else if (usesStability)
        {
            int pendingDamage = attackingUnit.attackPower - defendingUnit.stability;
            if (pendingDamage > 0)
            {
                defendingUnit.health -= pendingDamage;
                //Splash
                if (defendingUnitNorth != null)
                    defendingUnitNorth.health -= Mathf.RoundToInt(pendingDamage / 4.0f);
                if (defendingUnitEast != null)
                    defendingUnitEast.health -= Mathf.RoundToInt(pendingDamage / 4.0f);
                if (defendingUnitSouth != null)
                    defendingUnitSouth.health -= Mathf.RoundToInt(pendingDamage / 4.0f);
                if (defendingUnitWest != null)
                    defendingUnitWest.health -= Mathf.RoundToInt(pendingDamage / 4.0f);
            }
            else
            {
                defendingUnit.health -= 1;
            }
        }
        else
        {
            int pendingDamage = attackingUnit.attackPower - defendingUnit.defense;
            if (pendingDamage > 0)
            {
                defendingUnit.health -= pendingDamage;
                //Splash
                if (defendingUnitNorth != null)
                    defendingUnitNorth.health -= Mathf.RoundToInt(pendingDamage / 4.0f);
                if (defendingUnitEast != null)
                    defendingUnitEast.health -= Mathf.RoundToInt(pendingDamage / 4.0f);
                if (defendingUnitSouth != null)
                    defendingUnitSouth.health -= Mathf.RoundToInt(pendingDamage / 4.0f);
                if (defendingUnitWest != null)
                    defendingUnitWest.health -= Mathf.RoundToInt(pendingDamage / 4.0f);
            }
            else
            {
                defendingUnit.health -= 1;
            }
        }

        //Spawn in damage effects

        //If the player is attacking
        if (gm.playerTurn == 1)
        {
            //Have a death check
            rightSpriteInstance.GetComponent<GenericSpriteScript>().hurtTrigger = true;
            //And the others
            if (rightSpriteNorthInstance != null)
                rightSpriteNorthInstance.GetComponent<GenericSpriteScript>().hurtTrigger = true;
            if (rightSpriteEastInstance != null)
                rightSpriteEastInstance.GetComponent<GenericSpriteScript>().hurtTrigger = true;
            if (rightSpriteSouthInstance != null)
                rightSpriteSouthInstance.GetComponent<GenericSpriteScript>().hurtTrigger = true;
            if (rightSpriteWestInstance != null)
                rightSpriteWestInstance.GetComponent<GenericSpriteScript>().hurtTrigger = true;
        }
        //If the enemy is attacking
        if (gm.playerTurn == 2)
        {
            //Have a death check
            leftSpriteInstance.GetComponent<GenericSpriteScript>().hurtTrigger = true;
        }
    }

    //Stop the fight (Should work for all types)
    public void EndFight()
    {
        runningFight = false;

        //Close and go back
        if (gm.playerTurn == 1)
        {
            gm.PlayerUnitSelect(attackingUnit);
            gm.playerCursor.transform.position = attackingUnit.transform.position;
        }

        if (gm.playerTurn == 2)
        {
            if (attackingUnit.isEnemy)
            {
                attackingUnit.StartEnemyMovement();
            }
        }

        //Reset Tiles
        gm.ResetTiles();

        //Reset visuals
        //Normal/Center
        Destroy(leftSpriteInstance);
        leftSpriteInstance = null;
        leftSprite = null;
        Destroy(rightSpriteInstance);
        rightSpriteInstance = null;
        rightSprite = null;

        //Reset defending units
        defendingUnitNorth = null;
        defendingUnitEast = null;
        defendingUnitSouth = null;
        defendingUnitWest = null;

        //AOE
        if (leftSpriteNorthInstance != null)
            Destroy(leftSpriteNorthInstance);
        leftSpriteNorthInstance = null;
        leftSpriteNorth = null;
        if (leftSpriteEastInstance != null)
            Destroy(leftSpriteEastInstance);
        leftSpriteEastInstance = null;
        leftSpriteEast = null;
        if (leftSpriteSouthInstance != null)
            Destroy(leftSpriteSouthInstance);
        leftSpriteSouthInstance = null;
        leftSpriteSouth = null;
        if (leftSpriteWestInstance != null)
            Destroy(leftSpriteWestInstance);
        leftSpriteWestInstance = null;
        leftSpriteWest = null;

        if (rightSpriteNorthInstance != null)
            Destroy(rightSpriteNorthInstance);
        rightSpriteNorthInstance = null;
        rightSpriteNorth = null;
        if (rightSpriteEastInstance != null)
            Destroy(rightSpriteEastInstance);
        rightSpriteEastInstance = null;
        rightSpriteEast = null;
        if (rightSpriteSouthInstance != null)
            Destroy(rightSpriteSouthInstance);
        rightSpriteSouthInstance = null;
        rightSpriteSouth = null;
        if (rightSpriteWestInstance != null)
            Destroy(rightSpriteWestInstance);
        rightSpriteWestInstance = null;
        rightSpriteWest = null;

        // FUCK

        display.SetActive(false);
    }
}
