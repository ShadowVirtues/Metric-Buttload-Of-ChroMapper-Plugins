using System.Collections;
using UnityEngine;

namespace ChroMapper_TallMapsAndSettings
{
    public class Coroutiner : MonoBehaviour
    {
        //This script instance is set to DontDestroyOnLoad, so we can run coroutines in any scene

        public void SetSettings()
        {
            StartCoroutine(Settings());
        }

        IEnumerator Settings()
        {
            //Waiting a frame, since OptionsController.OptionsLoadedEvent event procs right after the SceneManager.LoadScene("04_Options") line,
            //and we know it takes one frame to load a scene
            yield return null;  

            RectTransform rect = (RectTransform)FindObjectOfType<OptionsController>().transform.GetChild(0);
            rect.anchorMin = new Vector2(0.5f, 0);
            rect.anchorMax = new Vector2(0.5f, 1);
            rect.offsetMin = new Vector2(rect.offsetMin.x, 5);
            rect.offsetMax = new Vector2(rect.offsetMax.x, -5);
        }



        public void SetMapList()
        {
            StartCoroutine(MapList());
        }

        IEnumerator MapList()
        {
            RectTransform rect = GameObject.Find("SongSelectorCanvas/SongInfoPanel").GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 0);
            rect.anchorMax = new Vector2(0, 1);
            rect.offsetMin = new Vector2(rect.offsetMin.x, 5);
            rect.offsetMax = new Vector2(rect.offsetMax.x, -5);

            rect.Find("SongList").gameObject.AddComponent<SongListScaler>().ParentRect = rect;

            yield return null;
            FindObjectOfType<SongList>().TriggerRefresh();  //Doesn't work without waiting a frame
        }

    }
}
