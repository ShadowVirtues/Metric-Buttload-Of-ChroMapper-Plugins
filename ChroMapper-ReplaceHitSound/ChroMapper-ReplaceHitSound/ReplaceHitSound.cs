using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace ChroMapper_ReplaceHitSound
{
    [Plugin("ReplaceHitSound")]
    public class ReplaceHitSound
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
                new GameObject("SoundChanger", typeof(MB));
            }
        }

        [Exit]
        private void Exit()
        {

        }
    }
}
