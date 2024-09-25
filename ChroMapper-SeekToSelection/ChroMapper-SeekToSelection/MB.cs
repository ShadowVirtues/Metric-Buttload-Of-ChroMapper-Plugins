using System.Linq;
using Beatmap.Base;
using UnityEngine;

namespace ChroMapper_SeekToSelection
{
    public class MB : MonoBehaviour
    {
        private AudioTimeSyncController controller;

        void Start()
        {
            controller = FindObjectOfType<AudioTimeSyncController>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (PersistentUI.Instance.DialogBoxIsEnabled || PersistentUI.Instance.InputBoxIsEnabled || PauseManager.IsPaused || SceneTransitionManager.IsLoading) return;

                BaseObject obj = SelectionController.SelectedObjects.FirstOrDefault();
                if (obj == null) return;
                controller.MoveToJsonTime(obj.JsonTime);
            }
        }
        
    }
}
