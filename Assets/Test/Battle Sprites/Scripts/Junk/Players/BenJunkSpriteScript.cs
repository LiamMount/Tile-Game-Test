using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenJunkSpriteScript : MonoBehaviour
{
    public BattleMaster bm;
    public Animator animator;
    public GenericSpriteScript triggerScript;

    // Start is called before the first frame update
    void Start()
    {
        bm = FindObjectOfType<BattleMaster>();
        animator = GetComponent<Animator>();
        triggerScript = GetComponent<GenericSpriteScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //Test if it is time to attack
        if (triggerScript.attackTrigger)
        {
            triggerScript.attackTrigger = false;
            StartAttack(triggerScript.attackType);
        }

        //test if it is time to get smacked
        if (triggerScript.hurtTrigger)
        {
            triggerScript.hurtTrigger = false;
            GetHit();
        }
    }

    public void StartAttack(string attackType)
    {
        if (attackType == "Grenade")
        {
            bm.specialAttack = "Grenade";

            bm.usesStability = true;
            animator.SetTrigger("GrenadeAttack");
        }
    }

    public void HitOpponent()
    {
        //Probably check for crits here
        //Also check attack type
        bm.AOEHitOpponent();
    }

    public void GetHit()
    {
        animator.SetTrigger("Hurt");
        bm.endClock = 2f;
        bm.endClockActive = true;
    }
}
