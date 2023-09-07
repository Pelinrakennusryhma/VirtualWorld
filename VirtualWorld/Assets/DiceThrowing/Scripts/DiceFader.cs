using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace DiceMinigame
{
    public class DiceFader : MonoBehaviour
    {
        private MeshRenderer mRenderer;
        private List<Color> originalColors = new List<Color>();
        private List<Color> transparentColors = new List<Color>();

        void Start()
        {
            mRenderer = transform.GetChild(1).GetComponent<MeshRenderer>();

            // hide shadow cast by collider
            MeshRenderer colliderRenderer = GetComponent<MeshRenderer>();
            colliderRenderer.enabled = false;

            StoreColors();
            ChangeMaterials();
            ApplyColors();

            // in case of a d100, add same script to the child dice
            D100Score d100Score = GetComponent<D100Score>();
            if (d100Score)
            {
                d100Score.childDiceScore.AddComponent<DiceFader>();
            }


            StartCoroutine(FadeAway());
        }

        private void StoreColors()
        {
            foreach (Material material in mRenderer.materials)
            {
                Color color = material.color;
                originalColors.Add(color);
                Color transparentColor = new Color(color.r, color.g, color.b, 0f);
                transparentColors.Add(transparentColor);
            }
        }

        private void ChangeMaterials()
        {
            Material[] materials = mRenderer.materials;

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = DiceMinigameGlobalSettings.Instance.diceTransparentMaterial;
            }

            mRenderer.materials = materials;
        }

        private void ApplyColors()
        {
            for (int i = 0; i < mRenderer.materials.Length; i++)
            {
                mRenderer.materials[i].color = originalColors[i];
            }
        }

        IEnumerator FadeAway()
        {
            float fadeTime = DiceMinigameGlobalSettings.Instance.bonusDiceVanishTime;
            float elapsedTime = 0f;
            while (elapsedTime < fadeTime)
            {
                float step = elapsedTime / fadeTime;

                for (int i = 0; i < mRenderer.materials.Length; i++)
                {
                    mRenderer.materials[i].color = Color.Lerp(originalColors[i], transparentColors[i], step);
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}
