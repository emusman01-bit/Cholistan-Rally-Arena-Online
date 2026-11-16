using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.HarmonicArts.CholistanRallyArena {
    public class CheckPoint : MonoBehaviour
    {
        public bool isMisser;
        [SerializeField]
        private bool isFinisher;
        ///
        GameMgrOffline gameMgrOffline;
        PunGameMgr punGameMgr;
        GameMgr gameMgrMain;
        int totalCps;
        ///
        void Start() {
            gameMgrMain = GameObject.FindWithTag("GameMgr").GetComponent<GameMgr>();
            gameMgrOffline = GameObject.FindWithTag("GameMgrOffline").GetComponent<GameMgrOffline>();
            punGameMgr = GameObject.FindWithTag("PunGameMgr").GetComponent<PunGameMgr>();
            if(!isMisser) totalCps = transform.parent.childCount;
        }
        
        private void OnTriggerEnter(Collider other) {
            if (!gameMgrMain.actOnline && gameMgrOffline && other.gameObject.tag == "Player") {
                if (!isMisser) {
                    gameMgrOffline.CheckPointPassed();
                    gameObject.SetActive(false);
                } else {
                    gameMgrOffline.CheckPointMissed();
                    gameObject.SetActive(false);
                }
            } else if (!gameMgrMain.actOnline && other.gameObject.tag == "OtherPlayer" && !isMisser) {
                PlayerMgr otherPlayer = other.gameObject.GetComponentInParent<PlayerMgr>();
                otherPlayer.checkPointsPassed++;
                if(otherPlayer.checkPointsPassed >= totalCps) {
                    gameMgrOffline.playersFinished.Add(otherPlayer.playerName);
                }
            }
            if (gameMgrMain.actOnline && punGameMgr && other.GetComponentInParent<VehicleFree>().photonView.IsMine) {
                if (!isMisser) {
                    punGameMgr.CheckPointPassed();
                    gameObject.SetActive(false);
                } else {
                    punGameMgr.CheckPointMissed();
                    gameObject.SetActive(false);
                }
            } else if (gameMgrMain.actOnline && punGameMgr && !isMisser) {
                PlayerMgr otherPlayer = other.gameObject.GetComponentInParent<PlayerMgr>();
                if(isFinisher && otherPlayer.checkPointsPassed>=totalCps-1)
                    otherPlayer.checkPointsPassed++;
                else if(!isFinisher) 
                    otherPlayer.checkPointsPassed++;
                if(otherPlayer.checkPointsPassed >= totalCps) {
                    string otherNick = other.GetComponentInParent<PhotonView>().Owner.NickName;
                    int lastIndex = punGameMgr.playersFinished.Count-1;
                    if((lastIndex>=0 && punGameMgr.playersFinished[lastIndex]!=otherNick) || lastIndex<0 && isFinisher) {
                        punGameMgr.playersFinished.Add(otherNick);
                    }
                }
            }
        }
    }

}