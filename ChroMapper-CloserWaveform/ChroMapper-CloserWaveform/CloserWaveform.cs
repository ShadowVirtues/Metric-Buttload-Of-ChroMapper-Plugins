using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChroMapper_CloserWaveform
{
    [Plugin("CloserWaveform")]
    public class CloserWaveform
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
                new GameObject("Mover", typeof(MB));
            }
        }

        [Exit]
        private void Exit()
        {

        }
    }
}
