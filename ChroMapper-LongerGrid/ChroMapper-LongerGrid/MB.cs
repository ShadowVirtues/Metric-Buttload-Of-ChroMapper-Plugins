using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChroMapper_LongerGrid
{
    public class MB : MonoBehaviour
    {
        IEnumerator Start()
        {
            yield return new WaitForSeconds(0.1f);  //Otherwise doesn't work

            SetScaleZ(transform.Find("Note Grid/Note Grid Front Scaling Offset"));
            SetScaleZ(transform.Find("BPM Changes Grid/BPM Changes Grid Front Scaling Offset"));
            SetScaleZ(transform.Find("Event Grid/Event Grid Front Scaling Offset"));
            SetScaleZ(transform.Find("Spectrogram Grid/Spectrogram Grid Front Scaling Offset"));

            ObstaclesContainer obstaclesContainer = FindObjectOfType<ObstaclesContainer>();
            InvokePrivateMethod(obstaclesContainer, "UnsubscribeToCallbacks");
            obstaclesContainer.UseChunkLoadingWhenPlaying = false;
            //All this breaks Ctrl+H walls spawning close tho D:
        }

        private void SetScaleZ(Transform t)
        {
            Vector3 localScale = t.localScale;
            localScale = new Vector3(localScale.x, localScale.y, 436);    //I just saved 0.000001s yaaay
            t.localScale = localScale;
        }


        /// <summary>
        /// Uses reflection to invoke any private method from passed class instance
        /// </summary>
        /// <param name="instance">Instance from which to call the private method</param>
        /// <param name="methodName">Name of the method to call</param>
        /// <param name="parameters">Parameters to pass to method</param>
        public static void InvokePrivateMethod(object instance, string methodName, params object[] parameters)
        {
            instance.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance).Invoke(instance, parameters);
        }

    }
}
