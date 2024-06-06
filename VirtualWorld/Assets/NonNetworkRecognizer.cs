using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonNetworkRecognizer : MonoBehaviour
{
    public static NonNetworkRecognizer Instance;

    public void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
