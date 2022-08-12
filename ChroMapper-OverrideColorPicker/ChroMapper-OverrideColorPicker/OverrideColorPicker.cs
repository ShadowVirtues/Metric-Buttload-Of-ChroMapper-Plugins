using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChroMapper_OverrideColorPicker
{
    [Plugin("OverrideColorPicker")]
    public class OverrideColorPicker
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
                //Can't just FindObjectOfType, since there are the same two components on Persistent DontDestroyOnLoad object
                ColorPicker picker = GameObject.Find("MapEditorUI/Chroma Colour Selector/Chroma Colour Selector/Picker 2.0").GetComponent<ColorPicker>();
                CustomColorsUIController controller = picker.transform.Find("Custom Color Buttons").GetComponent<CustomColorsUIController>();
                for (int i = 0; i < 7; i++)     //Seven color override buttons
                {
                    RMBHook hook = controller.transform.GetChild(i).gameObject.AddComponent<RMBHook>();
                    hook.Init(picker);
                }
                
            }
        }

        [Exit]
        private void Exit()
        {

        }
    }
}
