using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace ChroMapper_GridBookmarks
{
    //Since ChroMapper doesn't expose bookmark events, we have to check changes to these in 'Update' every frame, while storing previous state of bookmarks with CachedBookmark's

    public class CachedBookmark
    {
        public string Name;
        public Color Color;
        public readonly BeatmapBookmark MapBookmark;    //Instance of ChroMapper's bookmark
        public readonly TextMeshProUGUI Text;           //Text object that grid bookmark is implemented with

        public CachedBookmark(BeatmapBookmark bookmark, TextMeshProUGUI text) => (Name, Color, MapBookmark, Text) = (bookmark.Name, bookmark.Color, bookmark, text);
    }

    public class Controller : MonoBehaviour
    {
        private readonly List<CachedBookmark> cachedBookmarks = new List<CachedBookmark>();
        private Transform gridBookmarksParent;
        private BeatSaberMap map;
        private BookmarkManager manager;
        private AudioTimeSyncController timeController;

        private int prevObjectCount;    //Count of objects in transform hierarchy where bookmarks reside, so we can SetLastSibling to them when it changes

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.15f);     //Since ChroMapper waits 0.1s before loading bookmarks

            cachedBookmarks.Clear();
            gridBookmarksParent = FindObjectOfType<MeasureLinesRenderingOrderController>().transform;
            map = BeatSaberSongContainer.Instance.Map;
            manager = FindObjectOfType<BookmarkManager>();
            timeController = FindObjectOfType<AudioTimeSyncController>();

            List<BookmarkContainer> bookmarks = GetPrivateField<List<BookmarkContainer>>(manager, "bookmarkContainers");

            //Caching initial bookmarks
            foreach (BeatmapBookmark bookmark in map.Bookmarks)
            {
                TextMeshProUGUI text = CreateGridBookmark(bookmark);
                cachedBookmarks.Add(new CachedBookmark(bookmark, text));

                SetTooltip(bookmarks, bookmark);
            }

            //Since changing EditorScale affects bookmark position
            EditorScaleController.EditorScaleChangedEvent += OnEditorScaleChange;
            prevObjectCount = gridBookmarksParent.childCount;

            //Making main beat lines thinner and setting order so bookmarks get drawn on top of them
            MeshRenderer r = GameObject.Find("Editor/Rotating/Note Grid/Note Grid Front Scaling Offset").transform.GetChild(3).Find("One Measure Grid").GetComponent<MeshRenderer>();    //
            r.sharedMaterial.SetFloat("_GridThickness", 0.07f);
            r.sharedMaterial.renderQueue = 2999;
        }

        private void OnEditorScaleChange(float newScale)
        {
            foreach (CachedBookmark bookmark in cachedBookmarks)
            {
                SetBookmarkPos(bookmark.Text.rectTransform, bookmark.MapBookmark.Time);
            }
        }

        private void OnDestroy()
        {
            EditorScaleController.EditorScaleChangedEvent -= OnEditorScaleChange;
        }

        void Update()
        {
            if (map?.Bookmarks == null) return; //Since Update runs before stuff in IEnumerator Start procs

            if (map.Bookmarks.Count != cachedBookmarks.Count)   //Checking if removed or added a bookmark
            {
                for (int i = cachedBookmarks.Count - 1; i >= 0; i--)    //Reversely, since modifying the list
                {
                    CachedBookmark bookmark = cachedBookmarks[i];
                    if (!map.Bookmarks.Contains(bookmark.MapBookmark)) //If cached bookmark reference isn't contained in list anymore (got removed)
                    {
                        Destroy(bookmark.Text.gameObject);
                        cachedBookmarks.Remove(bookmark);
                    }
                }

                if (map.Bookmarks.Count > cachedBookmarks.Count)    //If there are more bookmarks in ChroMapper than cached (got added)
                {
                    //Can't cache this one since it gets REASSIGNED every time it gets changed! Nice coding over there.
                    List<BookmarkContainer> bookmarks = GetPrivateField<List<BookmarkContainer>>(manager, "bookmarkContainers");

                    foreach (BeatmapBookmark bookmark in map.Bookmarks)
                    {
                        if (cachedBookmarks.All(x => x.MapBookmark != bookmark))    //If found ChroMapper bookmark that is not cached
                        {
                            TextMeshProUGUI text = CreateGridBookmark(bookmark);
                            cachedBookmarks.Add(new CachedBookmark(bookmark, text));

                            SetTooltip(bookmarks, bookmark);
                        }
                    }
                }
            }

            foreach (CachedBookmark cachedBookmark in cachedBookmarks)  //Checking all cached against ChroMapper bookmarks to see Text/Color changes
            {
                string mapBookmarkName = cachedBookmark.MapBookmark.Name;
                Color mapBookmarkColor = cachedBookmark.MapBookmark.Color;

                if (cachedBookmark.Name != mapBookmarkName || cachedBookmark.Color != mapBookmarkColor)
                {
                    List<BookmarkContainer> bookmarks = GetPrivateField<List<BookmarkContainer>>(manager, "bookmarkContainers");

                    SetGridBookmarkNameColor(cachedBookmark.Text, mapBookmarkColor, mapBookmarkName);

                    cachedBookmark.Name = mapBookmarkName;
                    cachedBookmark.Color = mapBookmarkColor;

                    SetTooltip(bookmarks, cachedBookmark.MapBookmark);
                }
            }

            //There was some bs with grid bookmarks appearing under beat numbers, but it was most likely from low delay in IEnumerator Start
            //Anyway, instead we make a totally robust solution where we put bookmarks in the bottom of (UI Canvas) hierarchy every time anything gets added to it
            //(and yes, adding a new bookmark self-procs it, but it doesn't matter)
            if (gridBookmarksParent.childCount != prevObjectCount)
            {
                prevObjectCount = gridBookmarksParent.childCount;
                foreach (CachedBookmark bookmark in cachedBookmarks)
                {
                    bookmark.Text.transform.SetAsLastSibling();
                }
            }
        }

        private TextMeshProUGUI CreateGridBookmark(BeatmapBookmark bookmark)
        {
            GameObject obj = new GameObject("GridBookmark", typeof(TextMeshProUGUI));
            RectTransform rect = (RectTransform)obj.transform;
            rect.SetParent(gridBookmarksParent);
            SetBookmarkPos(rect, bookmark.Time);
            rect.sizeDelta = Vector2.one;
            rect.localRotation = Quaternion.identity;

            TextMeshProUGUI text = obj.GetComponent<TextMeshProUGUI>();
            text.font = PersistentUI.Instance.ButtonPrefab.Text.font;
            text.alignment = TextAlignmentOptions.Left;
            text.fontSize = 0.4f;
            text.enableWordWrapping = false;
            text.raycastTarget = false;
            SetGridBookmarkNameColor(text, bookmark.Color, bookmark.Name);

            return text;
        }

        private void SetBookmarkPos(RectTransform rect, float time)
        {
            //Need anchoredPosition3D, so Z gets precisely set, otherwise text might get under lighting grid
            rect.anchoredPosition3D = new Vector3(-4.5f, time * EditorScaleController.EditorScale, 0);
        }

        private void SetGridBookmarkNameColor(TextMeshProUGUI text, Color color, string s)
        {
            string hex = HEXFromColor(color, false);
            
            SetText();
            text.ForceMeshUpdate();

            //Here making so bookmarks with short name have still long colored rectangle on the right to the grid
            if (text.textBounds.size.x < 2) //2 is distance between notes and lighting grid
            {
                SetText((int)((2 - text.textBounds.size.x) / 0.0642f)); //Divided by 'space' character width for chosen fontSize
            }

            void SetText(int spaceNumber = 0)
            {
                string spaces = spaceNumber == 0 ? null : new string(' ', spaceNumber);
                //<voffset> to align the bumped up text to grid, <s> to draw a line across the grid, in the end putting transparent dot, so trailing spaces don't get trimmed, 
                text.text = $"<mark={hex}50><voffset=0.06><s> <indent=3.92> </s></voffset> {s}{spaces}<color=#00000000>.</color>"; 
            }
        }

        private void SetTooltip(List<BookmarkContainer> bookmarks, BeatmapBookmark bookmark)
        {
            BookmarkContainer container = bookmarks.FirstOrDefault(x => x.Data == bookmark);
            if (container == null) return;
            
            float timeInBeats = container.Data.Time;
            TimeSpan span = TimeSpan.FromSeconds(SecondsFromBeats(timeInBeats));
            container.GetComponent<Tooltip>().TooltipOverride = $"{container.Data.Name} [{Math.Round(timeInBeats, 2)} | {span:mm':'ss}]";
        }

        //==========================================

        /// <summary> Returned string starts with # </summary>
        private string HEXFromColor(Color color, bool inclAlpha = true) => $"#{(inclAlpha ? ColorUtility.ToHtmlStringRGBA(color) : ColorUtility.ToHtmlStringRGB(color))}";

        private float SecondsFromBeats(float beats) => timeController.GetSecondsFromBeat(beats);

        /// <summary>
        /// Uses reflection to get any private field from the class instance
        /// </summary>
        /// <typeparam name="T">Return type of the field you are getting</typeparam>
        /// <param name="instance">Instance from which you get the field with 'fieldName'</param>
        /// <param name="fieldName">String name of the field of 'instance'</param>
        /// <param name="baseType">If the field belongs to base class for passed instance of inherited class</param>
        public static T GetPrivateField<T>(object instance, string fieldName, bool baseType = false)
        {
            const BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            Type type = instance.GetType();
            if (baseType) type = type.BaseType;
            FieldInfo field = type.GetField(fieldName, bindFlags);
            return (T)field.GetValue(instance);
        }
    }
}
