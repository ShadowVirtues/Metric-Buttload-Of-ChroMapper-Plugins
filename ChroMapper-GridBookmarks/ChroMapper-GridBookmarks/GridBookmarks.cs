using UnityEngine.SceneManagement;

namespace ChroMapper_GridBookmarks
{
    [Plugin("GridBookmarks")]
    public class GridBookmarks
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
                BookmarkManager manager = UnityEngine.Object.FindObjectOfType<BookmarkManager>();
                manager.gameObject.AddComponent<Controller>();
            }
        }

        [Exit]
        private void Exit()
        {

        }
    }
}
