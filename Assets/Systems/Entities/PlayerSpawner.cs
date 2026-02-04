using Photon.Pun;
using UnityEngine;

namespace Entities
{
    public sealed class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private string _playerPrefabName;
        [SerializeField] private Transform _spawnPosition;
        private void Start()
        {
            PhotonNetwork.Instantiate(_playerPrefabName, _spawnPosition.position, Quaternion.identity);
        }

    }

}


