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


        private GameObject _dialogBox;
        private void ShowCharacterDialogBox()
        {
            _dialogBox = _gc.ActiveCanvas.transform.Find("CharacterDialogueBox").gameObject;
            var dialogBg = _dialogBox.GetComponent<Image>();
            var col = dialogBg.color;
            col.a = 0.76f;
            dialogBg.color = col;
        }

        private void HideCharacterDialogBox()
        {
            var dialogBg = _dialogBox.GetComponent<Image>();
            var col = dialogBg.color;
            col.a = 0.0f;
            dialogBg.color = col;
        }

        private void SnapDialogBoxNextToCharacter(Image character)
        {
            var rect = _dialogBox.transform.GetComponent<RectTransform>();
            
            _dialogBox.transform.position = character.transform.position;
            _dialogBox.transform.Translate((rect.rect.x - 10f), 27f, 0f);

        }

        private void SetTextToDialogBox(string text)
        {
            _dialogBox.transform.Find("CharacterDialogue").gameObject.GetComponent<Text>().text=text;
        }

        private void ControlBarDisplayText(string text)
        {
            var textPanel = GetTextPanel(true);
            var text1 = textPanel.GetComponentInChildren<Text>();

            var rect = text1.rectTransform.rect;
            
            text1.rectTransform.rect.Set(rect.x, rect.y, rect.width, rect.height*2);

            text1.color = Color.black;
            text1.name = text;
            DisplayText(text1);
        }
    }
}
