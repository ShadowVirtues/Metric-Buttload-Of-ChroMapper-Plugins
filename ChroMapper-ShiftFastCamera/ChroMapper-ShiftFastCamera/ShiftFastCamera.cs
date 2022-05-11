using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChroMapper_ShiftFastCamera
{
    [Plugin("ShiftFastCamera")]
    public class ShiftFastCamera
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
                GameObject obj = new GameObject("FastCamera", typeof(MB));
            }
        }

        [Exit]
        private void Exit()
        {

        }
    }
}
