using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace ChroMapper_TallMapsAndSettings
{
    [Plugin("TallMapsAndSettings")]
    public class TallMapsAndSettings
    {
        private Coroutiner setter;

        [Init]
        private void Init()
        {
            GameObject settingsSetter = new GameObject("SettingsSetter", typeof(Coroutiner));
            Object.DontDestroyOnLoad(settingsSetter);   //So we can run coroutines in all scenes
            setter = settingsSetter.GetComponent<Coroutiner>();

            OptionsController.OptionsLoadedEvent += setter.SetSettings;
            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex == 1)  //Only in the map list scene
            {
                setter.SetMapList();
            }
        }

        [Exit]
        private void Exit()
        {

        }
    }
}
