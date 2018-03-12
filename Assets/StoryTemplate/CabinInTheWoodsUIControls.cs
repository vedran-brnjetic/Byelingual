using Assets.StoryTemplate.Infrastructure;
using System;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Assets.StoryTemplate
{
    public partial class CabinInTheWoods
    {
        private void EnableClickToContinue()
        {
            var x = _gc.ActiveCanvas.GetComponent<AdvancePhase>();

            if (!x) _gc.ActiveCanvas.gameObject.AddComponent<AdvancePhase>();
        }

        private void DisableClickToContinue()
        {
            var x = _gc.ActiveCanvas.GetComponent<AdvancePhase>();

            if (x) Object.Destroy(x);

        }

        private void DisableRoomMovement()
        {
            _nextRoomButton.interactable = false;
            _previousRoomButton.interactable = false;
            _gc.ActivePanel.GetComponent<Image>().sprite = FindSprite.InResources("UI_no_arrows");
        }
    }
}
