using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestClickDetector : MonoBehaviour, IPointerClickHandler
{
    public bool IsHoveringAbove;
    public int FramesPassedSinceHOveringAbove;

    public MeshRenderer Renderer;

    private void Awake()
    {
        Renderer = GetComponent<MeshRenderer>();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Debug.Log("Clicked an object " + Time.time);
    }

    public void OnHoverAbove()
    {
        FramesPassedSinceHOveringAbove = 2;
        IsHoveringAbove = true;
    }

    public void Update()
    {
        if (!IsHoveringAbove)
        {
            Renderer.material.color = Color.magenta;
        }

        else
        {
            float pingPong = Mathf.PingPong(Time.time, 1);
            Renderer.material.color = Color.Lerp(Color.red, Color.green, pingPong);

            FramesPassedSinceHOveringAbove--;

            if (FramesPassedSinceHOveringAbove <= 0)
            {
                IsHoveringAbove = false;
            }
        }
    }
}
