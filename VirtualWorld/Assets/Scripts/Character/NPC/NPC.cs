using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

namespace Characters
{
    public class NPC : NetworkBehaviour
    {
        [field: SerializeField] public NPCData Data { get; private set; }
        [SerializeField] TMP_Text nameplate;
        [SerializeField] TMP_Text titleplate;

        [SyncVar]
        private string npcName;
        [SyncVar]
        private string npcTitle;

        public override void OnStartServer()
        {
            base.OnStartServer();

            InitData();

            SetNamePlates();
        }

        void InitData()
        {
            npcName = Data.fullName;
            npcTitle = Data.title;
        }

        void SetNamePlates()
        {
            nameplate.text = npcName;
            titleplate.text = npcTitle;
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            SetNamePlates();
        }
    }
}

