using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsShooting : MonoBehaviour
{
    public static float MouseSensitivity = 2.0f;
    public static bool InvertMouse = true;

    public Toggle InvertToggle;
    public Scrollbar SensitivityScrollbar;

    public static bool IsShowingOptions;

    public static void OnLaunch()
    {
        
        //Debug.Log("Options OnLaunch() called. Should probably get settings from database or something");

        //Debug.Log("On launch called in options");

        // GEt from player prefs
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

        if (MouseSensitivity <= 0.5f)
        {
            MouseSensitivity = 0.5f;
        }
    } 

    public void OnGameStarted()
    {
        IsShowingOptions = false;
        //Debug.Log("Options OnGameStarted() called. Should probably get settings from database or something");

        // GEt from player prefs
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


        SensitivityScrollbar.SetValueWithoutNotify(MouseSensitivity / 20.0f);
    }

    public void OnBecomeVisible()
    {
        IsShowingOptions = true;

        // GEt from player prefs
        InvertToggle.isOn = InvertMouse;

        gameObject.SetActive(true);
    }

    public void OnBecomeHidden()
    {
        IsShowingOptions = false;
        gameObject.SetActive(false);

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
        InvertMouse = newValue;
    }

    public void OnSensitivityValueChanged(float newValue)
    {
        MouseSensitivity = ConvertSensitivityValueFromZeroToOne(newValue);
    }

    public float ConvertSensitivityValueFromZeroToOne(float fromZeroToOne)
    {
        return Mathf.Lerp(0.5f, 20.0f, fromZeroToOne);
    }
}
