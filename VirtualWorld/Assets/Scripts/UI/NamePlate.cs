using Authentication;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Mirror;
using UnityEngine;

namespace Authentication
{
    //public class Nameplate : NetworkBehaviour
    //{
    //    [SerializeField] TMP_Text namePlate;
    //    [SerializeField] NetworkVariable<FixedString32Bytes> userName = 
    //        new NetworkVariable<FixedString32Bytes>
    //        (
    //            "", 
    //            NetworkVariableReadPermission.Everyone, 
    //            NetworkVariableWritePermission.Owner
    //        );

    //    private void Awake()
    //    {
    //        if(namePlate == null)
    //        {
    //            namePlate = GetComponent<TMP_Text>();
    //        }

    //        userName.OnValueChanged += OnUserNameSet;
    //    }

    //    void OnUserNameSet(FixedString32Bytes previous, FixedString32Bytes current)
    //    {
    //        namePlate.text = current.ToString();
    //    }

    //    public override void OnNetworkSpawn()
    //    {
    //        if(namePlate != null && UserSession.Instance.LoggedUserData.username != null)
    //        {
    //            if (IsOwner)
    //            {
    //                userName.Value = UserSession.Instance.LoggedUserData.username;
    //            } else
    //            {
    //                namePlate.text = userName.Value.ToString();
    //            }
    //        }
    //    }
    //}
}
