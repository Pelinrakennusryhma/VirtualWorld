using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerTabletopInvaders : MonoBehaviour
{
    public static GameManagerTabletopInvaders instance;
    public static bool RelaunchingCustomScene;

    public Dictionary<string, int> keilatAsetettuPaikoilleen =
    new Dictionary<string, int>();

    public string s;
    private void Awake() {
        // SceneManager.sceneLoaded += LoadState;
        Debug.Log("Created gamemanager");
        DontDestroyOnLoad(gameObject);
        instance = this;    
    }


    // Keilat
    // public int[] keilanNumero;
    // public List<int> keilanNumero = new List<int>();

    // Keilojen paikka

    public GameObject[] keilojenPaikat;
    public GameObject[] keilat;


/*     public void SaveState() {
        
        for(int i = 0; i < keilojenPaikat.Length; i++) {
            s += keilanNumero[i].ToString() + "|";
        }
        s += " ";
        PlayerPrefs.SetString("SaveState", s);

    } */

    public void SaveState() {
        
        string msg = "";
        int count = 0;
        foreach(var kvp in keilatAsetettuPaikoilleen) {
            msg += kvp.Key + "|" + kvp.Value;
            if(count < keilatAsetettuPaikoilleen.Count - 1)
            {
                msg+="|";
            }
            count++;
            
        }
        Debug.Log(msg);
        s += msg;
        // s += " ";
        PlayerPrefs.SetString("SaveState", s);

    }

    public void LoadState() {

        if(!PlayerPrefs.HasKey("SaveState"))
            return;

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');


        Debug.Log("LOAD");
        for(int i = 0; i < data.Length; i=i+2) {

            var name = data[i];
            var keila = int.Parse(data[i+1]);

            foreach(var keilaPaikka in keilojenPaikat)
            {
                if(keilaPaikka.name == name)
                {
                    keilaPaikka.SetActive(false);
                    Debug.Log(name);
                    // keilaappear.instance.keilat[keila].SetActive(true);
                    keilat[keila].SetActive(true);
                    Debug.Log(keila);
                    break;
                }
                
            }
        }
    }
}
