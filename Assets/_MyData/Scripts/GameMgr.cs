using UnityEngine.UI;
using UnityEngine;

namespace Com.HarmonicArts.CholistanRallyArena {
    public class GameMgr : MonoBehaviour
    {
        public Transform checkPointsContainer;
        public GameObject endUIMain;
        public Text earnedMoneyTxt;
        public Text earnedExpTxt;
        public StatsMgrX statsMgr;
        public bool actOnline;
        public GameObject hud;
        ///
        private void Awake() {
            Application.runInBackground = true;
        }
        ///
        public void QuitGame() {
            Application.Quit();
        }
        public void DestroyPlayers() {
            PlayerMgr[] players = GameObject.FindObjectsOfType<PlayerMgr>();
            foreach (PlayerMgr player in players) {
                Destroy(player.gameObject);
            }
        }
        public void EnableCheckpoints() {
            for (int i=0; i<checkPointsContainer.childCount; i++) {
                checkPointsContainer.GetChild(i).gameObject.SetActive(true);
                checkPointsContainer.GetChild(i).GetChild(0).gameObject.SetActive(true);
            }
        }
        public void UpdateEarnings() {
            earnedMoneyTxt.text = statsMgr.earnedMoney.ToString();
            earnedExpTxt.text = statsMgr.earnedExp.ToString();
        }
        public void SetOnline(bool b) {
            actOnline = b;
        }
        public void SetHUD(bool b) {
            hud.SetActive(b);
        }
    }
}