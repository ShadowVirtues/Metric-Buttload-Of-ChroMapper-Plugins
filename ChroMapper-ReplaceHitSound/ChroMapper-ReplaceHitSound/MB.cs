using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

namespace ChroMapper_ReplaceHitSound
{
    public class MB : MonoBehaviour
    {
        private List<AudioClip> shortClips = new List<AudioClip>();
        private List<AudioClip> longClips = new List<AudioClip>();
        private int totalCount;

        void Start()
        {
            shortClips.Clear();
            longClips.Clear();
            totalCount = 0;

            string dirPath = $"{Application.dataPath}/CustomHitSounds";
            if (!Directory.Exists(dirPath)) return;

            DirectoryInfo dir = new DirectoryInfo(dirPath);
            foreach (FileInfo file in dir.GetFiles())
            {
                if (!file.Name.EndsWith(".wav", StringComparison.InvariantCultureIgnoreCase)) continue;
                if (file.Name.StartsWith("short", StringComparison.InvariantCultureIgnoreCase))
                {
                    StartCoroutine(LoadTrack(file.FullName, true));
                    totalCount++;
                }
                else if (file.Name.StartsWith("long", StringComparison.InvariantCultureIgnoreCase))
                {
                    StartCoroutine(LoadTrack(file.FullName, false));
                    totalCount++;
                }
            }

            //Since all audio-tracks load through Coroutine, have to wait for them all to load before applying
            StartCoroutine(WaitForAllToLoad());

        }
        
        IEnumerator LoadTrack(string path, bool shortTrack)
        {
            UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip($"file:///{path}", AudioType.WAV); //That's how you load audio from folders, I guess
            yield return req.SendWebRequest();
            AudioClip track = DownloadHandlerAudioClip.GetContent(req);
            List<AudioClip> list = shortTrack ? shortClips : longClips;
            list.Add(track);
        }

        IEnumerator WaitForAllToLoad()
        {
            yield return new WaitWhile(() => shortClips.Count + longClips.Count < totalCount);

            //In case there are no files, or all are long or short
            if (shortClips.Count == 0 && longClips.Count == 0) yield break;
            if (shortClips.Count == 0) shortClips = longClips;
            if (longClips.Count == 0) longClips = shortClips;

            DingOnNotePassingGrid ding = FindObjectOfType<DingOnNotePassingGrid>();
            SoundList[] lists = GetPrivateField<SoundList[]>(ding, "soundLists");

            lists[1].ShortClips = shortClips.ToArray(); //Index 1 is "Slice" option in the settings
            lists[1].LongClips = longClips.ToArray();
        }

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
