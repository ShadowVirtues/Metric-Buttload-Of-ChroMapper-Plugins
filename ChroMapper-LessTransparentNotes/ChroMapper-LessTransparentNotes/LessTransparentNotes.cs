using UnityEngine;

namespace ChroMapper_LessTransparentNotes
{
    [Plugin("LessTransparentNotes")]
    public class LessTransparentNotes
    {
        [Init]
        private void Init()
        {
            Resources.Load<GameObject>("Unassigned Note").transform.Find("SimpleBlock").GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_TranslucentAlpha", 0.7f);
        }
    }
}
