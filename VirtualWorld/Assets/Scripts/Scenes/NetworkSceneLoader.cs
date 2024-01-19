using FishNet.Object;
using UnityEngine;
using FishNet.Managing.Scened;
using System.Collections.Generic;
using System.Linq;
using Dev;

namespace Scenes
{
    public class NetworkSceneLoader : NetworkBehaviour
    {
        [SerializeField] string mainSceneName;
        [SerializeField] List<string> otherSceneNames;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            LoadServerScenes();
        }

        void LoadServerScenes()
        {
            string launchSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            // Load main scene to be active on server and all clients.
            SceneLoadData mainSld = new SceneLoadData(mainSceneName);
            SceneManager.LoadGlobalScenes(mainSld);

            // Preload other scenes on server only so later on clients can enter those.
            SceneLoadData otherSld = new SceneLoadData(otherSceneNames);
            SceneManager.LoadConnectionScenes(otherSld);

            // Unload launch scene as it's no longer needed
            SceneManager.UnloadConnectionScenes(new SceneUnloadData(launchSceneName));
            Debug.Log("--- SERVER INIT END ---");
        }
    }
}

