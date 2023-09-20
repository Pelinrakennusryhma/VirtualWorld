using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToppleOverText : MonoBehaviour
{
    public TextMeshPro TextMeshPro;
    public AnimationCurve ScaleCurve;

    private Vector3 OriginalScale;
    private float SpawnTime;
    private float AliveLength = 1.2f;

    public ToppleOverAudio ToppleOverAudio;

    public void Spawn(string text)
    {
        TextMeshPro.material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Overlay;
        SpawnTime = Time.time;
        gameObject.SetActive(true);
        TextMeshPro.text = text;
        //transform.localPosition = Vector3.up * 2;
        transform.parent = null; // Unparent the text before spawning.

        //this implementation makes the text slightly rotated when at the edges of play field
        //Quaternion lookRot = Quaternion.LookRotation(transform.position - Camera.main.transform.position, Camera.main.transform.up);
        //transform.rotation = lookRot;

        transform.LookAt(Camera.main.transform);
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        OriginalScale = transform.localScale;

        ToppleOverAudio.Spawn();
        GameFlowManager.Instance.SoundManager.PlaySound("Topple over text spawned");
    }

    public void Spawn()
    {
        TextMeshPro.material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Overlay;
        SpawnTime = Time.time;
        gameObject.SetActive(true);
        //transform.localPosition = Vector3.up * 2;
        transform.parent = null; // Unparent the text before spawning.

        //this implementation makes the text slightly rotated when at the edges of play field
        //Quaternion lookRot = Quaternion.LookRotation(transform.position - Camera.main.transform.position, Camera.main.transform.up);
        //transform.rotation = lookRot;

        transform.LookAt(Camera.main.transform);
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);

        OriginalScale = transform.localScale;

        ToppleOverAudio.Spawn();
        GameFlowManager.Instance.SoundManager.PlaySound("Topple over text spawned");
    }

    private void Update()
    {
        float ratio = (Time.time - SpawnTime) / AliveLength;

        if (ratio >= 1.0f)
        {
            ratio = 1.0f;
            gameObject.SetActive(false);
        }

        float valueAtCurve = ScaleCurve.Evaluate(ratio);

        transform.localScale = new Vector3(OriginalScale.x * valueAtCurve * 2, 
                                           OriginalScale.y * valueAtCurve * 2, 
                                           OriginalScale.z);
    }
}
