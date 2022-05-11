using System.Linq;
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
                BeatmapObject obj = SelectionController.SelectedObjects.FirstOrDefault();
                if (obj == null) return;
                controller.MoveToTimeInBeats(obj.Time);
            }
        }
        
    }
}
