using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.HarmonicArts.CholistanRallyArena
{
    public class PlayerMgr : MonoBehaviour
    {
        public Transform camTypes;
        public GameObject playerCanvas;
        public string playerName;
        public int checkPointsPassed;
        public ParticleSystem dustPs;

        private int camType;
        private bool canReset = true;
        
        VehicleFree vehicleFree;
        Transform cam;
        Rigidbody rb;

        void Start(){
            // PlayerPrefs.SetInt("CamType", 1);
            dustPs = GetComponentInChildren<ParticleSystem>();
            rb = GetComponent<Rigidbody>();
            camType = PlayerPrefs.GetInt("CamType");
            vehicleFree = GetComponent<VehicleFree>();
            if (vehicleFree.allowInput) {
                SetCameraToPlayer();
            }
            if(vehicleFree.isOnline && vehicleFree.photonView.IsMine) {
                SetCameraToPlayer();
                playerName = vehicleFree.photonView.Owner.NickName;
            }
        }
        void Update(){
            var emission = dustPs.emission;
            emission.rateOverTimeMultiplier = rb.velocity.magnitude + rb.angularVelocity.magnitude;
            if(Input.GetKeyDown(KeyCode.Return)){
                Reset();
            }
        }

        void SetCameraToPlayer() {
            cam = GameObject.FindWithTag("Cams").transform.GetChild(camType);
            GameObject.FindWithTag("Cams").transform.GetChild(2).gameObject.SetActive(false);
            cam.gameObject.SetActive(true);
            cam.GetComponent<Lerper>().target = camTypes.GetChild(camType);
            playerCanvas.SetActive(true);
        }
        private void OnDestroy() {
            if (vehicleFree.isOnline && vehicleFree.photonView.IsMine) {
                cam = GameObject.FindWithTag("Cams").transform.GetChild(camType);
                cam.gameObject.SetActive(false);
                cam.parent.GetChild(2).gameObject.SetActive(true);
            } else if(!vehicleFree.isOnline) {
                cam = GameObject.FindWithTag("Cams").transform.GetChild(camType);
                cam.gameObject.SetActive(false);
                cam.parent.GetChild(2).gameObject.SetActive(true);
            }
        }
        IEnumerator DisableReset() {
            canReset = false;
            yield return new WaitForSeconds(2);
            canReset = true;
        }

        public void Reset(){
            if (canReset) {
                transform.position = new Vector3(transform.position.x, transform.position.y+3,transform.position.z);
                transform.rotation = new Quaternion (0,transform.rotation.y,0,transform.rotation.w);
                StartCoroutine(DisableReset());
            }
        }
    }
}