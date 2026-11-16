using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.HarmonicArts.CholistanRallyArena
{
    public class GameMgrOffline : MonoBehaviour
    {
        [SerializeField]
        bool act=true;
        public StatsMgrX statsMgr;
        public Transform spawnsParent;
        public GameObject[] vehiclePrefabs;
        public bool allowControl = false;
        public int playerCount=4;
        public Transform playersParent;
        public string otherPlayerTag;
        public Transform checkPointsParent;
        public List<string> playersFinished;

        [Header("UI")]
        public Text startCountdownTxt;
        public Text CPMissTxt;
        public GameObject endUI;
        public Text timeCompletedTxt;
        ///
        private float startCountdown = 3f;
        private int carChoice;
        private Transform[] spawns;
        private string mainPlayerName;
        private int checkPointsRemain;
        GameMgr gameMgrMain;
        private float gameTimer;
        private bool gameStarted;
        ///
        void Awake() {
            if(!act) {
                allowControl=true;
                return;
            }
            gameMgrMain = GameObject.FindWithTag("GameMgr").GetComponent<GameMgr>();
            checkPointsRemain = checkPointsParent.childCount;
            playersFinished = new List<string>();
            checkPointsRemain = checkPointsParent.childCount;
            spawns = spawnsParent.GetComponentsInChildren<Transform>();
            carChoice = PlayerPrefs.GetInt("CarChoice");
        }

        private void Update() {
            if (gameStarted) gameTimer+=Time.deltaTime;
        }

        string GetNiceTime(float timer) {
            int minutes = Mathf.FloorToInt(timer / 60F);
            int seconds = Mathf.FloorToInt(timer - minutes * 60);
            return string.Format("{0:0}:{1:00}", minutes, seconds);
        }

        IEnumerator StartCountdown () {
            yield return new WaitForSeconds(1);
            startCountdown--;
            startCountdownTxt.text = startCountdown.ToString();
            if(startCountdown <= 0) {
                startCountdownTxt.text = "Go!";
                allowControl = true;
                startCountdownTxt.text = "";
                gameStarted = true;
            } else 
                StartCoroutine(StartCountdown());
        }

        void SpawnPlayer() {
            carChoice = PlayerPrefs.GetInt("CarChoice");
            GameObject spawnedPlayer = Instantiate(vehiclePrefabs[carChoice], spawns[0].position, spawns[0].rotation);
            spawnedPlayer.transform.parent = playersParent;
            VehicleFree v = spawnedPlayer.GetComponent<VehicleFree>();
            v.allowInput = true;
            v.mainCollider.tag = "Player";
            mainPlayerName = spawnedPlayer.GetComponent<PlayerMgr>().playerName;
        }

        void SpawnOpponents() {
            for (int i=1; i<playerCount;i++) {
                GameObject spawnedPlayer = Instantiate(vehiclePrefabs[i], spawns[i].position, spawns[i].rotation);
                VehicleFree v = spawnedPlayer.GetComponent<VehicleFree>();
                v.mainCollider.tag = otherPlayerTag;
            }
        }

        void ShuffleSpawns() {
            for (int i = 0; i < spawns.Length; i++) {
                int rnd = Random.Range(0, spawns.Length);
                Transform temp = spawns[rnd];
                spawns[rnd] = spawns[i];
                spawns[i] = temp;
            }
        }

        void EndLevel(){
            gameMgrMain.SetHUD(false);
            gameStarted = false;
            playersFinished.Add(mainPlayerName);
            Debug.Log(playersFinished);
            startCountdownTxt.text = "You finished at " + GetNiceTime(gameTimer);
            AddPlayerStats();
            StartCoroutine(ShowEndUI());
        }

        IEnumerator ShowEndUI() {
            gameMgrMain.UpdateEarnings();
            yield return new WaitForSeconds(3);
            gameMgrMain.DestroyPlayers();
            timeCompletedTxt.text = GetNiceTime(gameTimer);
            startCountdownTxt.text = "";
            gameMgrMain.endUIMain.SetActive(true);
            endUI.SetActive(true);
            endUI.transform.GetChild(0).gameObject.SetActive(true);
        }

        void AddPlayerStats(){
            int amountToAdd = Mathf.RoundToInt(30000/gameTimer);
            int lvExpToAdd = Mathf.RoundToInt(1000/gameTimer);
            statsMgr.AddMoney(amountToAdd);
            statsMgr.AddLevelExp(lvExpToAdd);
        }

        IEnumerator CheckPointMissedRoutine() {
            CPMissTxt.text = "Check Point Missed!";
            yield return new WaitForSeconds(3);
            CPMissTxt.text = "";
        }

        void ResetGame() {
            gameStarted = false;
            gameTimer = 0;
            startCountdown = 3;
            allowControl = false;
            checkPointsRemain = checkPointsParent.childCount;
        }

        public void CheckPointPassed() {
            checkPointsRemain--;
            if(checkPointsRemain<=0) EndLevel();
        }

        public void CheckPointMissed() {
            StartCoroutine(CheckPointMissedRoutine());
        }

        public void StartGame() {
            if(!act) return;
            ResetGame();
            startCountdownTxt.text = startCountdown.ToString();
            StartCoroutine(StartCountdown());
            // ShuffleSpawns();
            SpawnPlayer();
            gameMgrMain.SetHUD(true);
            startCountdownTxt.text = startCountdown.ToString();
        }
    }
}