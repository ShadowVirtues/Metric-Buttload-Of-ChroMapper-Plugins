using System.Collections;
using UnityEngine;

namespace ChroMapper_CloserWaveform
{
    public class MB : MonoBehaviour
    {
        private Transform waveformTransform;
        private readonly WaitForSeconds second = new WaitForSeconds(1);

        IEnumerator Start()
        {
            //Some weird shit happens sometimes where Waveform gets moved further for some reason, so waiting longer for a good measure
            //And then it gets further when you add BPM change, press P to toggle lighting stuff, so we just constantly reposition it to combat this
            yield return new WaitForSeconds(0.5f);

            waveformTransform = GameObject.Find("Editor/Moveable Grid/Waveform Chunks Grid").transform;
            GameObject.Find("Editor/Rotating/Spectrogram Grid").SetActive(false);

            while (true)
            {
                Vector3 localPosition = waveformTransform.localPosition;
                localPosition = new Vector3(-6, localPosition.y, localPosition.z);
                waveformTransform.localPosition = localPosition;
                yield return second;
            }
        }
        

    }
}
