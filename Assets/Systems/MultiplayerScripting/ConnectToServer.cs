using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace MultiplayerScripting
{
    public class ConnectToServer : MonoBehaviourPunCallbacks
    {
        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        public override void OnErrorInfo(ErrorInfo errorInfo)
        {
            Debug.Log(errorInfo.Info);
        }
        public override void OnConnectedToMaster()
        {
            SceneManager.LoadScene("Menu");
            Debug.Log("Successfull connection to Master!");
        }
    }
}


