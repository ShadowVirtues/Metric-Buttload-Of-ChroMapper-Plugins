using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChroMapper_ThinBookmarks
{
    [Plugin("ThinBookmarks")]
    public class ThinBookmarks
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
                RectTransform rect = GetPrivateField<GameObject>(UnityEngine.Object.FindObjectOfType<BookmarkManager>(), "bookmarkContainerPrefab").GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(5, rect.sizeDelta.y);
            }
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

        [Exit]
        private void Exit()
        {

        }
    }
}
