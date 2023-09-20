using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class keilaappear : MonoBehaviour
{
    public int PositionID;
    public RespawnToPreviousPositions.PinType KeilaTyyppi;

    //public static keilaappear instance;
    // Start is called before the first frame update
    public GameObject[] keilat;

    public int index = 0;

    // public GameObject alusta;
    public Dropdown dropdown;
    /*     public Dictionary<string, int> keilatAsetettu =
        new Dictionary<string, int>(); */
    private void Awake()
    {
        // Miksi t‰llainen? Ei pit‰isi olla singleton objekti edes, kun n‰it‰ on pilvin pimein.
        //instance = this;


    }

    void Start()
    {
        SetupPin();
        // dropdown.gameObject.SetActive(true);
    }

    private void SetupPin()
    {
        KeilaTyyppi = RespawnToPreviousPositions.PinType.None;

        // Haetaan tallennettu keilatyyppi
        if (GameFlowManager.Instance != null)
        {
            RespawnToPreviousPositions.PinType pinType = GameFlowManager.Instance.GetPinTypeWithID(PositionID);
            KeilaTyyppi = pinType;
        }

        for (int i = 0; i < keilat.Length; i++)
        {
            keilat[i].SetActive(false);
        }

        if (KeilaTyyppi != RespawnToPreviousPositions.PinType.None)
        {
            int id = -1;

            switch (KeilaTyyppi)
            {
                case RespawnToPreviousPositions.PinType.None:
                    id = -1;
                    break;
                case RespawnToPreviousPositions.PinType.Minus2:
                    id = 0;
                    break;
                case RespawnToPreviousPositions.PinType.Minus5:
                    id = 1;
                    break;
                case RespawnToPreviousPositions.PinType.Plus1:
                    id = 2;
                    break;
                case RespawnToPreviousPositions.PinType.Plus2:
                    id = 3;
                    break;
                case RespawnToPreviousPositions.PinType.Plus5:
                    id = 4;
                    break;
                case RespawnToPreviousPositions.PinType.Plus10:
                    id = 5;
                    break;
                default:
                    id = -1;
                    break;
            }

            ActivatePin(id);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnMouseDown()
    {
        /*             keilaMiinus2.SetActive(true); // false to hide, true to show
             keilaMiinus5.SetActive(true); // false to hide, true to show
             keila1.SetActive(true); // false to hide, true to show
             keila2.SetActive(true); // false to hide, true to show
             keila5.SetActive(true); // false to hide, true to show */
        // lista.gameObject.SetActive(true);
        // alusta.SetActive(false);
        DropdownItemSelected(dropdown);
        // lista.onValueChanged.AddListener(delegate { DropdownItemSelected(lista);});
        //        keila10.SetActive(true); // false to hide, true to show 
    }



    public void DropdownItemSelected(Dropdown dropdown)
    {
        index = dropdown.value;

        //GameManager.instance.keilatAsetettuPaikoilleen.Add(gameObject.name.ToString(), index);

        /*         
                string msg = "";
                int count = 0;
                foreach(var kvp in keilatAsetettu) {
                    msg += kvp.Key + " " + kvp.Value;
                    if(count < keilatAsetettu.Count - 1)
                    {
                        msg+=",";
                    }
                    count++;
                }
                Debug.Log(msg);

         */
        // GameManager.instance.keilanNumero.Add(index);
        ActivatePin(index);
    }

    private void ActivatePin(int id)
    {            
        RespawnToPreviousPositions.PinType keilaTyyppi = RespawnToPreviousPositions.PinType.None;

        if (id >= 0)
        {



            switch (id)
            {
                case 0:
                    keilaTyyppi = RespawnToPreviousPositions.PinType.Minus2;
                    break;

                case 1:
                    keilaTyyppi = RespawnToPreviousPositions.PinType.Minus5;
                    break;

                case 2:
                    keilaTyyppi = RespawnToPreviousPositions.PinType.Plus1;
                    break;

                case 3:
                    keilaTyyppi = RespawnToPreviousPositions.PinType.Plus2;
                    break;

                case 4:
                    keilaTyyppi = RespawnToPreviousPositions.PinType.Plus5;
                    break;

                case 5:
                    keilaTyyppi = RespawnToPreviousPositions.PinType.Plus10;
                    break;

                default:
                    keilaTyyppi = RespawnToPreviousPositions.PinType.None;
                    break;
            }

            KeilaTyyppi = keilaTyyppi;

            keilat[id].SetActive(true);
            PinCollider collider = keilat[id].GetComponentInChildren<PinCollider>();
            collider.transform.parent.gameObject.AddComponent<PinDisappearer>().PlacePinOnAPlate(this);

            // Lis‰t‰‰n komponentti, jonka tarkoituksena on poistaa keila oikean hiiren napin klikkauksella.

            DisableMeshCollider();
        }

        else
        {
            KeilaTyyppi = RespawnToPreviousPositions.PinType.None;

            // Disable all pins just in case...
            for (int i = 0; i < keilat.Length; i++)
            {
                keilat[i].gameObject.SetActive(false);
            }
        }

        //Debug.Log("Saving pin at " + PositionID + " " + keilaTyyppi.ToString());
        GameFlowManager.SavePinPosition(PositionID, KeilaTyyppi);
    }

    private void DisableMeshCollider()
    {
        // Keilat voivat j‰‰d‰ jumiin aluslautaseen. Disabloidaaan lautasen collider, jotta n‰in ei tapahdu
        // Toisaalta t‰m‰n voisi tehd‰ vasta pelin alussa kaikille aluslautasille? Mutta toisaalta tykki toimii jo keiloja aseteltaessa
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.enabled = false;
    }

    private void EnableMeshCollider()
    {
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.enabled = true;
    }

    public void RemovePin()
    {
        ActivatePin(-1);
        EnableMeshCollider();
    }
}

