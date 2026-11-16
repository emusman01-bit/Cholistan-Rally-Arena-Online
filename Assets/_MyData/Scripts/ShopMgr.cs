using UnityEngine.UI;
using UnityEngine;
using System.Collections;

namespace Com.HarmonicArts.CholistanRallyArena {
    public class ShopMgr : MonoBehaviour
    {
        public Transform vehiclesContainer;
        public StatsMgrX statsMgr;
        public int[] prices = new int[4];

        [Header("UI")]
        public Text priceTxt;
        public Text warnTxt;
        public Text succTxt;
        public GameObject buyBtn;
        public GameObject selectBtn;
        public GameObject selectedTxt;
        ///
        private int currentVehicle;
        private int boughtVehicle;
        ///
        private void Awake() {
            PlayerPrefs.SetInt("CarChoice0", 1);
            PlayerPrefs.SetInt("CarChoice", 0);
        }
        private void OnEnable() {
            boughtVehicle = PlayerPrefs.GetInt("CarChoice"+currentVehicle.ToString());
            currentVehicle = PlayerPrefs.GetInt("CarChoice");
            vehiclesContainer.GetChild(currentVehicle).gameObject.SetActive(true);
            UpdateSelection();
        }
        private void OnDisable() {
            vehiclesContainer.GetChild(currentVehicle).gameObject.SetActive(false);
        }

        IEnumerator CanBuy() {
            buyBtn.SetActive(false);
            PlayerPrefs.SetInt("CarChoice"+currentVehicle.ToString(), 1);
            statsMgr.SubMoney(prices[currentVehicle]);
            succTxt.gameObject.SetActive(true);
            succTxt.text = "Purchased!";
            yield return new WaitForSeconds(2);
            succTxt.gameObject.SetActive(false);
            UpdateSelection();
        }
        IEnumerator CannotBuy() {
            warnTxt.gameObject.SetActive(true);
            warnTxt.text = "Not enough Sikkay!";
            yield return new WaitForSeconds(2);
            warnTxt.gameObject.SetActive(false);
        }

        void UpdateSelection() {
            UpdatePurchase();
            int selected = PlayerPrefs.GetInt("CarChoice");
            if(selected==currentVehicle) {
                selectBtn.SetActive(false);
                selectedTxt.SetActive(true);
            } else {
                selectedTxt.SetActive(false);
            }
        }
        void UpdatePurchase() {
            boughtVehicle = PlayerPrefs.GetInt("CarChoice"+currentVehicle.ToString());
            if(boughtVehicle==1) {
                selectBtn.SetActive(true);
                buyBtn.SetActive(false);
                priceTxt.gameObject.SetActive(false);
            } else {
                selectBtn.SetActive(false);
                buyBtn.SetActive(true);
                priceTxt.gameObject.SetActive(true);
                priceTxt.text = prices[currentVehicle].ToString();
            }
        }
        ///
        public void NextCar(Button btn) {
            if (currentVehicle>=vehiclesContainer.childCount-1) {
                btn.enabled = false;
            } else {
                vehiclesContainer.GetChild(currentVehicle).gameObject.SetActive(false);
                currentVehicle++;
                vehiclesContainer.GetChild(currentVehicle).gameObject.SetActive(true);
                UpdateSelection();
            }
        }
        public void PrevCar(Button btn) {
            if (currentVehicle<=0) {
                btn.enabled = false;
            } else {
                vehiclesContainer.GetChild(currentVehicle).gameObject.SetActive(false);
                currentVehicle--;
                vehiclesContainer.GetChild(currentVehicle).gameObject.SetActive(true);
                UpdateSelection();
            }
        }
        public void EnableButton(Button otherBtn) {
            otherBtn.enabled=true;
        }
        public void BuyCar() {
            if (statsMgr.GetMoney() > prices[currentVehicle]) {
                StartCoroutine(CanBuy());
            } else {
                StartCoroutine(CannotBuy());
            }
        }
        public void SelectCar() {
            PlayerPrefs.SetInt("CarChoice", currentVehicle);
            UpdateSelection();
        }
    }
}