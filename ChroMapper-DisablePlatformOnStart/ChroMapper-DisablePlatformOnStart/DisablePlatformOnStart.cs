using UnityEngine.SceneManagement;

namespace ChroMapper_DisablePlatformOnStart
{
    [Plugin("DisablePlatformOnStart")]
    public class DisablePlatformOnStart
    {
        [Init]
        private void Init()
        {
            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex == 3)  //Only in the scene where you actually edit the map
            {
                UnityEngine.Object.FindObjectOfType<PlatformToggleDisableableObjects>().gameObject.AddComponent<MB>();
            }
        }

        [Exit]
        private void Exit()
        {

        }
    }
}
