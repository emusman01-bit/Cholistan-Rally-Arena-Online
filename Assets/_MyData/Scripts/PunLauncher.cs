using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

namespace Com.HarmonicArts.CholistanRallyArena
{
    public class PunLauncher : MonoBehaviourPunCallbacks
    {
        /// <summary>
        /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
        /// </summary>
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField] private byte maxPlayersPerRoom = 4;

        /// <summary>
        /// Client's version number. Users are separated by game version (which allows to make breaking changes)
        /// </summary>
        string gameVersion = "1";

        /// <summary>
        /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
        /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
        /// Typically this is used for the OnConnectedToMaster() callback.
        /// </summary>
        bool isConnecting;
        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField] private GameObject controlPanel;
        RoomOptions roomOptions;

        public Text punLogText;
        public Text punPlayersTxt;
        public InputField punPlayerName;
        public Dropdown regionDropdown;
        public GameObject playBtn;

        ExitGames.Client.Photon.Hashtable hashTable;

        void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        void Start()
        {
            controlPanel.SetActive(true);
        }
        void Update () {
            if (punLogText) punLogText.text = PhotonNetwork.NetworkClientState.ToString();
            if (punPlayersTxt) punPlayersTxt.text = PhotonNetwork.IsConnected ? PhotonNetwork.CountOfPlayers.ToString() : "Not connected, Please connnect.";
        }
        
        public void Connect()
        {
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = regionDropdown.options[regionDropdown.value].text;
            isConnecting = PhotonNetwork.ConnectUsingSettings();
        }
        public void Disconnect() {
            PhotonNetwork.Disconnect();
        }
        public void mJoinRandom() {
            PhotonNetwork.NickName = punPlayerName.text;
            PhotonNetwork.JoinRandomRoom();
        }

        #region MonoBehaviourPunCallbacks Callbacks
        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            playBtn.SetActive(true);
            isConnecting = false;
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            // controlPanel.SetActive(true);
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
            isConnecting = false;
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // RoomOptions roomOptions = new RoomOptions();
            // roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
            // hashTable = roomOptions.CustomRoomProperties;
            // roomOptions.MaxPlayers = 4;

            // PhotonNetwork.CreateRoom("SomeRoom", roomOptions);
            PhotonNetwork.CreateRoom("SomeRoom");
        }

        public override void OnCreatedRoom() {
            ExitGames.Client.Photon.Hashtable hashTable = new ExitGames.Client.Photon.Hashtable();
            PhotonNetwork.CurrentRoom.SetCustomProperties(hashTable);
        }
        #endregion
    }

}
