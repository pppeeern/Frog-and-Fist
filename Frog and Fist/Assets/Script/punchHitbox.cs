using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class punchHitbox : MonoBehaviour
{
    Transform me;
    GameObject him;
    PlayerControl2 myControl;
    PlayerControl2 hisControl;

    void Start(){
        me = transform.parent;
        myControl = me.GetComponent<PlayerControl2>();
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            him = other.gameObject;
            if(!him.GetComponent<PlayerControl2>().isAttack){
                Debug.Log($"{him.transform.name} was hit by {me.name}");
                
                hisControl = him.GetComponent<PlayerControl2>();
                hisControl.hurt(me, myControl.attackStrength);
                me.GetComponent<PlayerControl2>().ultimatePoint += 5;
            }
        }
    }
}
