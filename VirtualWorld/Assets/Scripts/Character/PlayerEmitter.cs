using Unity.Netcode;
using UnityEngine;

namespace Characters
{
    public class PlayerEmitter : NetworkBehaviour
    {
        void Start()
        {
            if (IsOwner)
            {
                Character.Instance.SetPlayerGameObject(this.gameObject);
            }
        }
    }
}
