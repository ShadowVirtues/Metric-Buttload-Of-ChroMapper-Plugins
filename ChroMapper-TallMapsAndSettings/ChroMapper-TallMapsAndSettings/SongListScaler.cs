using UnityEngine;

namespace ChroMapper_TallMapsAndSettings
{
    public class SongListScaler : MonoBehaviour
    {
        //This script is sponsored by VerticalLayoutGroup on SongInfoPanel

        public RectTransform ParentRect;

        private RectTransform rect;

        void Start()
        {
            rect = GetComponent<RectTransform>();
        }

        void Update()
        {
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, ParentRect.rect.size.y - 34);    //So it scales for all window sizes and resolutions
        }
    }
}
