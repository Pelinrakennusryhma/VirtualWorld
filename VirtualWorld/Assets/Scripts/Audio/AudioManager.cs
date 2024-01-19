using FishNet;
using FMOD.Studio;
using FMODUnity;
using Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Audio
{
    enum MusicArea
    {
        MENU = 0,
        WORLD_MAP = 1,
        SPACESHIP_SCENE = 2,
        PLANET = 3
    }

    public enum FMODBus
    {
        MASTER,
        MUSIC,
        SFX,
        AMBIENCE
    }

    [System.Serializable]
    public class SavedVolumeData
    {
        public float master;
        public float music;
        public float sfx;
        public float ambience;
    }

    public class AudioManager : MonoBehaviour
    {
        [field: Header("Volume")]
        [field: Range(0, 1)]
        [field: SerializeField] public float MasterVolume { get; private set; }
        [field: Range(0, 1)]
        [field: SerializeField] public float MusicVolume { get; private set; }
        [field: Range(0, 1)]
        [field: SerializeField] public float SFXVolume { get; private set; }
        [field: Range(0, 1)]
        [field: SerializeField] public float AmbienceVolume { get; private set; }

        private Bus masterBus;
        private Bus musicBus;
        private Bus SFXBus;
        private Bus ambienceBus;

        private List<EventInstance> eventInstances = new List<EventInstance>();
        private EventInstance musicEventInstance;
        public static AudioManager Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }

            masterBus = RuntimeManager.GetBus("bus:/");
            musicBus = RuntimeManager.GetBus("bus:/Music");
            SFXBus = RuntimeManager.GetBus("bus:/SFX");
            ambienceBus = RuntimeManager.GetBus("bus:/Ambience");

            LoadData();
            SetInitialVolumes();

            //SceneManager.sceneLoaded -= OnSceneLoaded;
            //SceneManager.sceneLoaded += OnSceneLoaded;
        }

        //void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        //{
        //    if(SceneLoader.Instance == null)
        //    {
        //        return;
        //    }
        //    // Destroy on server when main scene loads
        //    if (scene.name == SceneLoader.Instance.MainSceneName)
        //    {
        //        if (InstanceFinder.IsServer && !InstanceFinder.IsHost)
        //        {
        //            Destroy(gameObject);
        //        }
        //    }
        //}


        void LoadData()
        {
            SavedVolumeData data = JsonUtility.FromJson<SavedVolumeData>(PlayerPrefs.GetString("VolumeData"));
            if (data != null)
            {
                MasterVolume = data.master;
                MusicVolume = data.music;
                SFXVolume = data.sfx;
                AmbienceVolume = data.ambience;
            }
            else
            {
                MasterVolume = 1f;
                MusicVolume = 1f;
                SFXVolume = 1f;
                AmbienceVolume = 1f;
            }
        }

        public void SaveData()
        {
            SavedVolumeData newVolumeData = new SavedVolumeData()
            {
                master = MasterVolume,
                music = MusicVolume,
                sfx = SFXVolume,
                ambience = AmbienceVolume,
            };
            PlayerPrefs.SetString("VolumeData", JsonUtility.ToJson(newVolumeData));
        }

        void SetInitialVolumes()
        {
            masterBus.setVolume(MasterVolume);
            musicBus.setVolume(MusicVolume);
            SFXBus.setVolume(SFXVolume);
            ambienceBus.setVolume(SFXVolume);
        }

        public float GetBusValue(FMODBus bus)
        {
            switch (bus)
            {
                case FMODBus.MASTER:
                    return MasterVolume;
                case FMODBus.MUSIC:
                    return MusicVolume;
                case FMODBus.SFX:
                    return SFXVolume;
                case FMODBus.AMBIENCE:
                    return AmbienceVolume;
                default:
                    return 1f;
            }
        }

        public void SetBusValue(FMODBus bus, float value)
        {
            switch (bus)
            {
                case FMODBus.MASTER:
                    masterBus.setVolume(value);
                    MasterVolume = value;
                    break;
                case FMODBus.MUSIC:
                    musicBus.setVolume(value);
                    MusicVolume = value;
                    break;
                case FMODBus.SFX:
                    SFXBus.setVolume(value);
                    SFXVolume = value;
                    break;
                case FMODBus.AMBIENCE:
                    ambienceBus.setVolume(value);
                    AmbienceVolume = value;
                    break;
                default:
                    break;
            }
        }

        private void InitializeMusic(EventReference musicEventReference)
        {
            musicEventInstance = CreateEventInstance(musicEventReference, false);
            musicEventInstance.start();
        }

        public void SetMusicAreaBySceneIndex(int index)
        {
            MusicArea musicArea = index <= 2 ? (MusicArea)index : MusicArea.PLANET;
            SetMusicArea(musicArea);
        }

        void SetMusicArea(MusicArea area)
        {
            musicEventInstance.setParameterByName("MusicArea", (float)area);
        }

        public void PlayOneShot(EventReference sound, Vector3 worldPos)
        {
            RuntimeManager.PlayOneShot(sound, worldPos);
        }

        public EventInstance CreateEventInstance(EventReference eventReference, bool addToList = true)
        {
            EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
            if (addToList)
            {
                eventInstances.Add(eventInstance);
            }

            return eventInstance;
        }

        private void CleanUp()
        {
            foreach (EventInstance eventInstance in eventInstances)
            {
                eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                eventInstance.release();
            }
        }

        private void OnDestroy()
        {
            //CleanUp();
        }
    }
}