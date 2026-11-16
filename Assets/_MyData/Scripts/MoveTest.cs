using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    public Vector3 direction;
    public Vector3 maxDistance;
    public float moveSpeed;
    public float timer;
    public string keyInput;

    private bool enabler=false;
    private float timerOrig;

    void Start() {
        timerOrig = timer;
    }

    void Update(){
        if(keyInput=="1"){
            if(Input.GetKeyDown(KeyCode.Alpha1)){
                enabler=!enabler;
            }
            if(enabler){
                Move();
            }
        }
        if(keyInput=="2"){
            if(Input.GetKeyDown(KeyCode.Alpha2)){
                enabler=!enabler;
            }
            if(enabler){
                Move();
            }
        }
        if(keyInput=="3"){
            if(Input.GetKeyDown(KeyCode.Alpha3)){
                enabler=!enabler;
            }
            if(enabler){
                Move();
            }
        }
        if(keyInput=="4"){
            if(Input.GetKeyDown(KeyCode.Alpha4)){
                enabler=!enabler;
            }
            if(enabler){
                Move();
            }
        }
    }

    void Move(){
        timer-= Time.deltaTime;
        if(timer>0){
            transform.position += direction * moveSpeed * Time.deltaTime;
        } else if(timer>-timerOrig){
            transform.position += -direction * moveSpeed * Time.deltaTime;
        } else {
            timer=timerOrig;
        }
    }
}
