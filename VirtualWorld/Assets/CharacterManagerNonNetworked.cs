using UnityEngine;
using BackendConnection;
using Dev;
using System.Collections.Generic;
using Items;


namespace Characters
{
    public class CharacterManagerNonNetworked : MonoBehaviour
    {
        public static CharacterManagerNonNetworked Instance { get; private set; }
        public GAME_STATE gameState = GAME_STATE.FREE;
        [field: SerializeField] public GameObject OwnedCharacter { get; private set; }

        [SerializeField] CharacterData characterData;
        [SerializeField] public PlayerEmitter PlayerEmitter { get; private set; }

        // Antti's addition to help determine who is driving a car.
        public int ClientId;

        public Item[] DummyItems;


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                //DontDestroyOnLoad(gameObject);
            }
        }

        public void SetGameState(GAME_STATE newState)
        {
            //CharacterManager.Instance.gameState = newState;
            CharacterManagerNonNetworked.Instance.gameState = newState;
            PlayerEvents.Instance.CallEventGameStateChanged(newState);
        }

        public void SetOwnedCharacter(GameObject obj)
        {
            OwnedCharacter = obj;
        }

        // Disable and enable inputs depending on if we are driving a car.
        public void SetInputsEnabled(bool isEnabled)
        {
            OwnedCharacter.GetComponentInChildren<UnityEngine.InputSystem.PlayerInput>(true).enabled = isEnabled;
            OwnedCharacter.GetComponentInChildren<StarterAssets.StarterAssetsInputs>(true).enabled = isEnabled;
        }

        public void Start()
        {
            if (Instance == this)
            {
                DontDestroyOnLoad(gameObject);
            }
            //base.OnStartClient();

            //ClientId = LocalConnection.ClientId;

            //GetCharacterDataServerRpc(LocalConnection, UserSession.Instance.LoggedUserData.id);

            // Create some dummydata for development purposes
            CreateDummyCharacterData();
        }

        //[ServerRpc(RequireOwnership = false)]
        //public void GetCharacterDataServerRpc(NetworkConnection conn, string id)
        //{
        //    APICalls_Server.Instance.GetCharacterData(conn, id, AcceptCharacterData);
        //}

        //[TargetRpc]
        //void AcceptCharacterData(NetworkConnection conn, CharacterData characterData)
        //{
        //    Utils.DumpToConsole(characterData);
        //    PlayerEvents.Instance.CallEventCharacterDataSet(characterData);
        //}

        private void CreateDummyCharacterData()
        {
            Debug.LogWarning("Creating dummy data");

            CharacterData dummyData = new CharacterData();
            dummyData.inventory = new InventoryData();
            dummyData.inventory.items = new List<InventoryItemData>();

            InventoryItemData dummyData0 = new InventoryItemData();
            dummyData0.id = DummyItems[0].Id;
            dummyData0.amount = 10;

            InventoryItemData dummyData1 = new InventoryItemData();
            dummyData1.id = DummyItems[1].Id;
            dummyData1.amount = 10;

            InventoryItemData dummyData2 = new InventoryItemData();
            dummyData2.id = DummyItems[2].Id;
            dummyData2.amount = 10;

            InventoryItemData credit = new InventoryItemData();
            credit.id = DummyItems[3].Id;
            credit.amount = 1000;

            dummyData.inventory.items.Add(dummyData0);
            dummyData.inventory.items.Add(dummyData1);
            dummyData.inventory.items.Add(dummyData2);
            dummyData.inventory.items.Add(credit);

            characterData = dummyData;

            Utils.DumpToConsole(characterData);
            PlayerEvents.Instance.CallEventCharacterDataSet(characterData);
            Debug.Log("Called event of character data being set " + Time.time);
        }
    }
}
