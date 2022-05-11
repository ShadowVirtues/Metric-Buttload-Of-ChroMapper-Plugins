using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChroMapper_SeekToSelection
{
    [Plugin("SeekToSelection")]
    public class SeekToSelection
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
                new GameObject("Seeker", typeof(MB));
            }
        }

        [Exit]
        private void Exit()
        {

        }
    }
}
