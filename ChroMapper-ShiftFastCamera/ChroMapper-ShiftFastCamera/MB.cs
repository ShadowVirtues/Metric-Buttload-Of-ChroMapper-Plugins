using TMPro;
using UnityEngine;

namespace ChroMapper_ShiftFastCamera
{
    public class MB : MonoBehaviour
    {
        private float _initialSpeed;

        private float _modifiedSpeed;

        private TextMeshProUGUI _speedText;
        
        // Shift speed starts at 4x the normal speed, it is maxed at 350, and the minimum is 2x the normal speed.

        void Start()
        {
            _initialSpeed = Settings.Instance.Camera_MovementSpeed;
            _modifiedSpeed = _initialSpeed * 4f;
            
            _speedText = new GameObject("SpeedText", typeof(TextMeshProUGUI))
                .GetComponent<TextMeshProUGUI>();
            _speedText.transform.SetParent(GameObject.Find("Timeline Canvas").transform);
            _speedText.text = $"Speed: {_modifiedSpeed}";
            _speedText.rectTransform.anchoredPosition = new Vector2(0, 210);
            _speedText.fontSize = 32;
            _speedText.alignment = TextAlignmentOptions.Center;
            _speedText.horizontalAlignment = HorizontalAlignmentOptions.Center;
            _speedText.verticalAlignment = VerticalAlignmentOptions.Middle;
            _speedText.transform.gameObject.SetActive(false);
            
        }

        void Update()
        {
            // initialSpeed * 4f is the "default" shift speed before it was modified.
            if (Input.GetKey(KeyCode.LeftShift))
            {   
                // This fixes a very specific issue where if you are holding shift and hit esc (menu) and exit the map,
                // it will save the modified speed as the default speed, it will show 25 in the settings /
                // however its capped at 25, so its actually 60 if using the CM default cam speed.
                if (Input.GetKey(KeyCode.Escape)) return; 
                
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                if(scroll != 0)
                {
                    // Increment by -1 or 1.
                    if (scroll > 0)
                        _modifiedSpeed += 1f;
                    else
                        _modifiedSpeed -= 1f;
                    
                    if (_modifiedSpeed < _initialSpeed * 2f) _modifiedSpeed = _initialSpeed * 2f; // Prevent lower speed than normal / negative speed.
                    if (_modifiedSpeed > 350) _modifiedSpeed = 350; // Prevent super high speed, if you are going this high you need fucking help.
                }
                
                if (!_speedText.transform.gameObject.activeInHierarchy) _speedText.transform.gameObject.SetActive(true);
                _speedText.text = $"Camera Speed : {_modifiedSpeed}";
                
                Settings.Instance.Camera_MovementSpeed = _modifiedSpeed;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                Settings.Instance.Camera_MovementSpeed = _initialSpeed;
                _speedText.transform.gameObject.SetActive(false);
            }
        }

    }
}
