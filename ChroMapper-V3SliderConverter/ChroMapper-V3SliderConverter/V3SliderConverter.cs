using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChroMapper_V3SliderConverter
{
    [Plugin("V3SliderConverter")]
    public class V3SliderConverter
    {
        [Init]
        private void Init()
        {
            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex == 3)
            {
                GameObject obj = new GameObject("converter", typeof(Converter));
            }
        }

    }
}
