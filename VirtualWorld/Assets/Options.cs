using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public float MouseSensitivity = 2.0f;
    public bool InvertMouse = true;

    public Toggle InvertToggle;
    public Scrollbar SensitivityScrollbar;

    public FirstPersonPlayerController FirstPersonPlayerController;

    private void Awake()
    {
        //gameObject.SetActive(false);
    }

    public void OnGameStarted()
    {
        // GEt from player prefs

        Debug.Log("Game started on options");

        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            MouseSensitivity = PlayerPrefs.GetFloat("Sensitivity");
        }

        else
        {
            MouseSensitivity = 2.0f;
            PlayerPrefs.SetFloat("Sensitivity", MouseSensitivity);
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.HasKey("Invert"))
        {
            int invert = PlayerPrefs.GetInt("Invert");
            
            if (invert == 0)
            {
                InvertMouse = false;
            }

            else
            {
                InvertMouse = true;
            }
        }

        else
        {
            InvertMouse = false;
            PlayerPrefs.SetInt("Invert", 0);
            PlayerPrefs.Save();
        }

        InvertToggle.isOn = InvertMouse;        
        
        if (MouseSensitivity <= 0.5f)
        {
            MouseSensitivity = 0.5f;
        }

        SensitivityScrollbar.value = MouseSensitivity / 20.0f;



        //InvertMouse = true;
        //MouseSensitivity = 2.0f;
    }

    public void OnBecomeVisible()
    {
        // GEt from player prefs
        InvertToggle.isOn = InvertMouse;
    }

    public void OnBecomeHidden()
    {
        MouseSensitivity = ConvertSensitivityValueFromZeroToOne(SensitivityScrollbar.value);
        PlayerPrefs.SetFloat("Sensitivity", ConvertSensitivityValueFromZeroToOne(SensitivityScrollbar.value));

        int invert = 0;

        InvertMouse = InvertToggle.isOn;

        if (InvertMouse)
        {
            invert = 1;
        }

        PlayerPrefs.SetInt("Invert", invert);
        PlayerPrefs.Save();

        // Save to player prefs
    }

    public void OnInvertValueChanged(bool newValue)
    {
        //GameManager.Instance.SetMouseInvert(newValue);
        InvertMouse = newValue;


        //Debug.Log("invert Value changed");
    }

    public void OnSensitivityValueChanged(float newValue)
    {

        MouseSensitivity = ConvertSensitivityValueFromZeroToOne(newValue);
        //GameManager.Instance.SetMouseSensitivity(MouseSensitivity);


        //Debug.Log("mouse sens Value changed");

    }

    public float ConvertSensitivityValueFromZeroToOne(float fromZeroToOne)
    {
        return Mathf.Lerp(0.5f, 20.0f, fromZeroToOne);
    }
}
