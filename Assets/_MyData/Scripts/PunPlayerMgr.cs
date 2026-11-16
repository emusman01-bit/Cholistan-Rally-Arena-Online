using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.HarmonicArts.CholistanRallyArena
{
    /// <summary>
    /// Requires PhotonView to check Pun related data.
    /// </summary>
    [RequireComponent(typeof(PhotonView))]
    public class PunPlayerMgr : MonoBehaviourPun
    {
        [SerializeField] private GameObject playerControls;
        [SerializeField] private Transform camTop;
        [SerializeField] private Transform camBack;
        GameObject playerCam;


        void Awake()
        {
            if(!photonView.IsMine && PhotonNetwork.IsConnected) {
                return;
            }
            if (camTop)
            {
                playerCam = GameObject.FindWithTag("Cams").transform.GetChild(0).gameObject;
                playerCam.GetComponent<Lerper>().target = camTop;
            }
            playerCam.SetActive(true);
            GameObject.FindWithTag("Cams").transform.GetChild(2).gameObject.SetActive(false);
            if (photonView.IsMine && PhotonNetwork.IsConnected)
            {
                playerControls.SetActive(true);
            }
        }
    }
}

