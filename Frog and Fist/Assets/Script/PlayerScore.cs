using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    PlayerControl playerControl;
    PlayerCombat playerCombat;
    bool isGrounded, alive;

    /*[Header("Score Configuration")]
    public int score;
    [SerializeField] int Knockout, Fall, AccidentFall;*/

    public int frogHeart = 3;
    bool minusHeart;

    [Header("UI")]
    [SerializeField] TMP_Text UI_Score;
    [SerializeField] TMP_Text UI_Out;

    void Start(){
        playerControl = GetComponent<PlayerControl>();
        playerCombat = GetComponent<PlayerCombat>();

    }

    void Update(){
        isGrounded = playerControl.isGrounded;
        alive = playerControl.alive;

        UI_Score.text = frogHeart.ToString();

        if(!alive){
            if(!minusHeart){
                minusHeart = true;
                frogHeart--;
                StartCoroutine("displayOut");
            }
        }
        else minusHeart = false;
    }

    IEnumerator displayOut(){
        UI_Out.gameObject.SetActive(true);
        UI_Out.text = transform.gameObject.name + " Out!";

        yield return new WaitForSeconds(2);
        UI_Out.gameObject.SetActive(false);
    }
}