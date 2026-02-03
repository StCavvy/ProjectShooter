using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

namespace MultiplayerScripting
{
    public class MenuManager : MonoBehaviourPunCallbacks
    {

        [SerializeField] private TMP_InputField createInput;
        [SerializeField] private TMP_InputField joinInput;

        public void CreateRoom()
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 4;
            PhotonNetwork.CreateRoom(createInput.text, roomOptions);
        }
        public void JoinRoom()
        {
            PhotonNetwork.JoinRoom(joinInput.text);
        }
        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("Game");
        }
    }
}
