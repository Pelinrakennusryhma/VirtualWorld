using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceMinigame
{
    public class DiceMinigameGlobalSettings : MonoBehaviour
    {
        private static DiceMinigameGlobalSettings _instance;

        public static DiceMinigameGlobalSettings Instance { get { return _instance; } }
        public float zoomInTime;
        public float zoomOutTime;
        public float cameraStayOnDiceTime;
        public float cameraMoveToNextDiceTime;
        public float bonusDiceVanishTime;
        public int maxDice;
        public Material diceOpaqueMaterial;
        public Material diceTransparentMaterial;
        public bool isPortrait = false;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            GetScreenRatio();
        }

        private void GetScreenRatio()
        {
            int screenWidth = Camera.main.pixelWidth;
            int screenHeight = Camera.main.pixelHeight;

            if (screenWidth < screenHeight)
            {
                isPortrait = true;
            }
        }
    }
}
