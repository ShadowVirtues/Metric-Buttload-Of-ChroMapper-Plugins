using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ChroMapper_TimelineInputBlocker
{
    [Plugin("TimelineInputBlocker")]
    public class TimelineInputBlocker
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
                SongTimelineController controller = Object.FindObjectOfType<SongTimelineController>();
                GameObject obj = new GameObject();
                obj.transform.SetParent(controller.transform);

                Image img = obj.AddComponent<Image>();
                img.color = Color.clear;

                RectTransform rect = img.rectTransform;
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = new Vector2(1, 0);
                rect.pivot = new Vector2(0.5f, 0);
                rect.offsetMin = new Vector2(0, rect.offsetMin.y);
                rect.offsetMax = new Vector2(0, rect.offsetMax.y);
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, -6);
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, 42);
                rect.localScale = Vector3.one;
                rect.SetAsFirstSibling();
                
            }
        }

        [Exit]
        private void Exit()
        {

        }
    }
}
