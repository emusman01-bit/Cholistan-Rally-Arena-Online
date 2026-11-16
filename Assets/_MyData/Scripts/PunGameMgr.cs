using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

using Photon.Pun;

namespace Com.HarmonicArts.CholistanRallyArena
{
    public class PunGameMgr : MonoBehaviourPunCallbacks
    {
        public StatsMgrX statsMgr;
        public GameObject[] vehiclePrefabsOnline;
        public Transform playersParent;
        public Transform spawnsParent;
        public bool allowControl = false;
        public List<string> playersFinished;
        public Transform checkPointsContainer;

        [Header("UI")]
        public Text startCountdownTxt;
        public Text CPMissTxt;
        public Text levelTxt;
        public GameObject punGui;
        public GameObject endUI;
        public Text waitPlayersTxt;
        ///
        private Transform[] spawns;
        private int carChoice;
        private string mainPlayerName;
        private float startCountdown = 3f;
        private GameMgr gameMgrMain;
        private int checkPointsRemain;
        private int startMatchAfter;
        int i=0;
        ///
        private void Awake() {
            gameMgrMain = GameObject.FindWithTag("GameMgr").GetComponent<GameMgr>();
            spawns = spawnsParent.GetComponentsInChildren<Transform>();
            checkPointsRemain = checkPointsContainer.childCount;
        }

        void SetSpawn() {
            // for (int i = 0; i < spawns.Length; i++) {
            //     int rnd = Random.Range(0, spawns.Length);
            //     Transform temp = spawns[rnd];
            //     spawns[rnd] = spawns[i];
            //     spawns[i] = temp;
            // }
            // if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(spawns[0].position)) {
            //     ExitGames.Client.Photon.Hashtable hashTablePrev = PhotonNetwork.CurrentRoom.CustomProperties;
            //     hashTablePrev.Add(spawns[0].position, 1);
            //     PhotonNetwork.CurrentRoom.SetCustomProperties(hashTablePrev);
            // }
            // ExitGames.Client.Photon.Hashtable hashTablePrev = PhotonNetwork.CurrentRoom.CustomProperties;
            // hashTablePrev.Add(spawns[PhotonNetwork.CurrentRoom.PlayerCount-1].position, 1);
            // PhotonNetwork.CurrentRoom.SetCustomProperties(hashTablePrev);
        }

        void SpawnPlayer() {
            // if(PhotonNetwork.CurrentRoom.CustomProperties.Count > 0) {
            //     for (i=0; i<spawns.Length; i++) {
            //         if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(spawns[i].position)) {
            //             break;
            //         } else {
            //             print("Spawner key found");
            //         }
            //     }
            // }
            carChoice = PlayerPrefs.GetInt("CarChoice");
            GameObject spawnedPlayer = PhotonNetwork.Instantiate(vehiclePrefabsOnline[carChoice].name, spawns[i].position, spawns[i].rotation);
            spawnedPlayer.transform.parent = playersParent;
            VehicleFree v = spawnedPlayer.GetComponent<VehicleFree>();
            v.allowInput = true;
            v.mainCollider.tag = "Player";
            spawnedPlayer.GetComponent<PlayerMgr>().playerName = PhotonNetwork.NickName;
            mainPlayerName = PhotonNetwork.NickName;
        }

        void EndLevel(){
            gameMgrMain.SetHUD(false);
            playersFinished.Add(mainPlayerName);
            Debug.Log(playersFinished);
            if(playersFinished[0]==mainPlayerName) {
                startCountdownTxt.text = "You Won!";
                AddPlayerStats(false);
            } else {
                startCountdownTxt.text = "You Lost!";
                AddPlayerStats(true);
            }
            StartCoroutine(ShowEndUI());
        }

        IEnumerator ShowEndUI() {
            gameMgrMain.UpdateEarnings();
            yield return new WaitForSeconds(3);
            gameMgrMain.DestroyPlayers();
            startCountdownTxt.text = "";
            gameMgrMain.endUIMain.SetActive(true);
            endUI.SetActive(true);
            endUI.transform.GetChild(0).gameObject.SetActive(true);
        }

        void AddPlayerStats(bool calculate){
            int amountToAdd=750;
            int lvExpToAdd=15;
            if(!calculate){
                statsMgr.AddMoney(amountToAdd);
                statsMgr.AddLevelExp(lvExpToAdd);
            } else {
                amountToAdd -= 100;
                lvExpToAdd -= 3;
                for (int i=1;i<playersFinished.Count;i++) {
                    print (playersFinished[i]+" "+(i+1));
                    if (playersFinished[i]==mainPlayerName) {
                        startCountdownTxt.text = "You Lost!\nYou finished on " + (i+1);
                        statsMgr.AddMoney(amountToAdd);
                        statsMgr.AddLevelExp(lvExpToAdd);
                    } else {
                        amountToAdd -= 100;
                        lvExpToAdd -= 3;
                    }
                }
            }
        }

        IEnumerator CheckPointMissedRoutine() {
            CPMissTxt.text = "Check Point Missed!";
            yield return new WaitForSeconds(3);
            CPMissTxt.text = "";
        }

        IEnumerator WaitForPlayersRoutine() {
            if (PhotonNetwork.IsConnected) {
                byte players = PhotonNetwork.CurrentRoom.PlayerCount;
                if (players >= 2) {
                    startMatchAfter = 10;
                    StartCoroutine(WaitForMorePlayers());
                    StartCoroutine(WaitStartKey());
                } else {
                    yield return new WaitForSeconds(0.05f);
                    StartCoroutine(WaitForPlayersRoutine());
                }
            }
        }

        IEnumerator WaitForMorePlayers() {
            waitPlayersTxt.text = "Player(s) found! Match starting in "+startMatchAfter+" seconds.";
            if (startMatchAfter<=0) {
                DisablePunGui();
                StartMatch();
            } else {
                yield return new WaitForSeconds(1);
                startMatchAfter--;
                StartCoroutine(WaitForMorePlayers());
            }
        }
        IEnumerator WaitStartKey() {
            if(PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("StartMatch")) {
                startMatchAfter=0;
                print("StartMatch key found");
            } else {
                yield return new WaitForSeconds(0.05f);
                StartCoroutine(WaitStartKey());
            }
        }

        void ResetMatch() {
            allowControl = false;
            startCountdown = 3;
            checkPointsRemain = checkPointsContainer.childCount;
            gameMgrMain.SetHUD(true);
        }

        void StartMatch() {
            if(!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("StartMatch")) {
                ExitGames.Client.Photon.Hashtable hashTablePrev = PhotonNetwork.CurrentRoom.CustomProperties;
                hashTablePrev.Add("StartMatch", "StartMatch");
                // ExitGames.Client.Photon.Hashtable hashTable = new ExitGames.Client.Photon.Hashtable() {{"StartMatch", 1}};
                PhotonNetwork.CurrentRoom.SetCustomProperties(hashTablePrev);
            }
            waitPlayersTxt.text = "";
            ResetMatch();
            SpawnPlayer();
            startCountdownTxt.text = startCountdown.ToString();
            StartCoroutine(StartCountdown());
        }

        IEnumerator StartCountdown () {
            yield return new WaitForSeconds(1);
            startCountdown--;
            startCountdownTxt.text = startCountdown.ToString();
            if(startCountdown <= 0) {
                startCountdownTxt.text = "Go!";
                allowControl = true;
                startCountdownTxt.text = "";
            } else 
                StartCoroutine(StartCountdown());
        }

        void DisablePunGui() {
            punGui.SetActive(false);
        }

        public void CheckPointPassed() {
            checkPointsRemain--;
            if(checkPointsRemain<=0) EndLevel();
        }
        public void CheckPointMissed() {
            StartCoroutine(CheckPointMissedRoutine());
        }

        public void LeaveRoom() {
            PhotonNetwork.LeaveRoom();
        }

        ///
        public override void OnJoinedRoom() {
            playersFinished.Clear();
            i = PhotonNetwork.CurrentRoom.PlayerCount-1;
            print("OnJoinedRoom called");
            waitPlayersTxt.text = "Waiting for players to join...";
            StartCoroutine(WaitForPlayersRoutine());
        }
    }
}