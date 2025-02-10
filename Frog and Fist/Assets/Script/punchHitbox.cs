using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchHitbox : MonoBehaviour
{
    Transform me;
    GameObject him;
    PlayerCombat myCombat;
    PlayerCombat hisCombat;

    void Start(){
        me = transform.parent;
        myCombat = me.GetComponent<PlayerCombat>();
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            him = other.gameObject;
            if(!him.GetComponent<PlayerCombat>().isAttack){
                Debug.Log($"{him.transform.name} was hit by {me.name}");
                
                hisCombat = him.GetComponent<PlayerCombat>();
                hisCombat.hurt(me, myCombat.attackStrength);
                myCombat.ultimatePoint += 5;
            }
        }
    }
}
