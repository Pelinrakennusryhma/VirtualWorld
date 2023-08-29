using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace DiceMinigame
{
    public class DepthOfFieldController : MonoBehaviour
    {
        public VolumeProfile volumeProfile;
        public DepthOfField depthOfField;
        public DiceManager diceManager;
        public CamMover camMover;
        public Toggle dofToggle;
        float smoothSpeed = 5f;

        float distance;
        // Start is called before the first frame update
        void Start()
        {
            if (!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
            if (!volumeProfile.TryGet(out depthOfField)) throw new System.NullReferenceException(nameof(depthOfField));

            camMover = GetComponent<CamMover>();

            dofToggle.onValueChanged.AddListener(delegate { OnDofSettingChanged(dofToggle); });

            GetStartDoF();
        }

        void GetStartDoF()
        {
            int dofSetting = 1;

            if (PlayerPrefs.HasKey("DoF"))
            {
                dofSetting = PlayerPrefs.GetInt("DoF");
            }

            bool enabled = dofSetting > 0 ? true : false;
            dofToggle.isOn = enabled;

            ToggleDoF(enabled);
        }

        void OnDofSettingChanged(Toggle toggle)
        {
            int dofSetting = toggle.isOn ? 1 : 0;
            PlayerPrefs.SetInt("DoF", dofSetting);

            ToggleDoF(toggle.isOn);
        }

        void ToggleDoF(bool enabled)
        {
            depthOfField.active = enabled;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 targetPos = Vector3.zero;

            if (camMover.targetPos != Vector3.zero)
            {
                targetPos = camMover.targetPos;
            }
            else
            {
                targetPos = GetDiceMean();
            }
            distance = Vector3.Distance(transform.position, targetPos);
            float targetStart = distance + 10f;
            float targetEnd = distance + 30f;

            float currStart = Mathf.Lerp(depthOfField.gaussianStart.value, targetStart, smoothSpeed);
            depthOfField.gaussianStart.Override(currStart);

            float currEnd = Mathf.Lerp(depthOfField.gaussianEnd.value, targetEnd, smoothSpeed);
            depthOfField.gaussianEnd.Override(currEnd);
        }

        private Vector3 GetDiceMean()
        {
            List<Vector3> positions = new List<Vector3>();
            foreach (GameObject dice in diceManager.activeDice)
            {
                positions.Add(dice.transform.position);
            }
            if (positions.Count == 0)
            {
                return Vector3.zero;
            }

            Vector3 meanVector = Vector3.zero;

            foreach (Vector3 pos in positions)
            {
                meanVector += pos;
            }

            return (meanVector / positions.Count);
        }
    }
}