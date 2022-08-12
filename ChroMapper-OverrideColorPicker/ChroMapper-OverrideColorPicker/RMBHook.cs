using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ChroMapper_OverrideColorPicker
{
    public class RMBHook : MonoBehaviour, IPointerClickHandler
    {
        private ColorPicker picker;

        public void Init(ColorPicker p) => picker = p;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right) return;

            Color color = transform.GetChild(0).GetComponent<Image>().color;    //Image colored to override color
            picker.CurrentColor = color;
        }
    }
}
