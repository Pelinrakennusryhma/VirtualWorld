using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tallenna : MonoBehaviour
{
    public GameObject katto;
    bool paikallaan = false;
    public GameObject Button;
    public GameObject DropDown;

    public void asetaKatto() {
        if(!paikallaan) {
            TimerCountdown.peliVoiAlkaa = true;
            katto.SetActive(true);
            
            if (GameManagerTabletopInvaders.instance != null) 
            {
                GameManagerTabletopInvaders.instance.SaveState();
            }

            Button.SetActive(false);
            DropDown.SetActive(false);
            GameFlowManager.FinishSettingPinPositions();

            paikallaan = true;

            GameFlowManager.Instance.SoundManager.PlayUIPress();
            GameFlowManager.Instance.SoundManager.PlaySound("UI button pressed");

        } else if (paikallaan) {
            Debug.Log("MEnee");
            GameManagerTabletopInvaders.instance.LoadState();
            katto.SetActive(false);
            paikallaan = false;
        }
            
        
        //   katto.SetActive(false); 
        
            
    }
}
