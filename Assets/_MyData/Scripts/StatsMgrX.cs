using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.HarmonicArts.CholistanRallyArena {
    public class StatsMgrX : MonoBehaviour
    {
        [SerializeField]
        private bool clearStats = true;
        public int setExp=60;
        public int setMoney=100000;
        public int earnedExp=0;
        public int earnedMoney=0;
        ///
        private int money;
        private int levelExp;
        private int playerLevel=1;
        int lvChanger=50;
        ///
        void Awake(){
            if(clearStats) PlayerPrefs.DeleteAll();
            money = PlayerPrefs.GetInt("Money");
            if(setExp>0) {
                levelExp=setExp;
                PlayerPrefs.SetInt("LevelExp", levelExp);
            }
            if(setMoney>0) {
                money=setMoney;
                PlayerPrefs.SetInt("Money", setMoney);
            }
            levelExp = PlayerPrefs.GetInt("LevelExp");
            CalculateLevel();
        }

        void CalculateLevel() {
            for (int i=0; i<levelExp; i+=10) {
                if(i>=lvChanger) {
                    playerLevel++;
                    lvChanger += (int) (lvChanger*1.05f);
                }
            }
            PlayerPrefs.SetInt("PlayerLevel", playerLevel);
        }
        ///
        public void AddMoney(int amount) {
            money+=amount;
            PlayerPrefs.SetInt("Money", money);
            earnedMoney = amount;
        }

        public void SubMoney(int amount) {
            money-=amount;
            PlayerPrefs.SetInt("Money", money);
        }

        public int GetMoney() {
            return money;
        }

        public void AddLevelExp(int amount) {
            levelExp+=amount;
            PlayerPrefs.SetInt("LevelExp", levelExp);
            earnedExp = amount;
        }

        public int GetPlayerLevel() {
            CalculateLevel();
            return playerLevel;
        }
    }

}