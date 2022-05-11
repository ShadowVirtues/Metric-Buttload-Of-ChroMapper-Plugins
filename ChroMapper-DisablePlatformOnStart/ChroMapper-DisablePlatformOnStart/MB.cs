using System.Collections;
using UnityEngine;

namespace ChroMapper_DisablePlatformOnStart
{
    public class MB : MonoBehaviour
    {
        IEnumerator Start()
        {
            yield return new WaitForSeconds(0.1f);  //Otherwise doesn't work

            GetComponent<PlatformToggleDisableableObjects>().UpdateDisableableObjects();
        }
        
    }
}
