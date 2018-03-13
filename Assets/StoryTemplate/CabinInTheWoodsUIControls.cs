using Assets.StoryTemplate.Infrastructure;
using System;
using System.Collections.Generic;
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

        private float _oldBT_height;

        private void ControlBarDisplayText(string text, string choice="")
        {
            
            var textPanel = GetTextPanel(true);
            var text1 = textPanel.GetComponentInChildren<Text>();

            var rect = text1.rectTransform.rect;
            _oldBT_height = rect.height;

            text1.rectTransform.rect.Set(rect.x, rect.y, rect.width, rect.height*2);

            text1.color = Color.black;
            text1.name = text;

            if (!string.IsNullOrEmpty(choice))
                DisplayText(text1, choice);
            else
            {
                DisplayText(text1);
            }

            
        }

        private void ControlBarDisplayText(List<string> texts, List<string> choices)
        {
            
            var textPanel = GetTextPanel(true);
            var text1 = textPanel.GetComponentInChildren<Text>();
            var rect = text1.rectTransform.rect;
            text1.rectTransform.rect.Set(rect.x, rect.y, rect.width, _oldBT_height);
            text1.color = Color.black;

            text1.name = texts[0];

            if(!string.IsNullOrEmpty(choices[0]))
                DisplayText(text1, choices[0]);
            else
            {
                DisplayText(text1);
            }

            texts.RemoveAt(0);
            //choices.RemoveAt(0);
            foreach (var text in texts)
            {
                var txt = Object.Instantiate(text1, textPanel.transform, true);
                txt.transform.Translate(0f, -(text1.preferredHeight * (texts.IndexOf(text)+1)) * 1.33f, 0f);

                txt.name = text;

                if (!string.IsNullOrEmpty(choices[texts.IndexOf(text)]))
                    DisplayText(txt, choices[texts.IndexOf(text)]);
                else
                {
                    DisplayText(txt);
                }

            }
        }

        private void EnableRoomMovement()
        {
            _canvasBackground.sprite = FindSprite.InResources(_currentRoom);
            _gc.ActivePanel.GetComponent<Image>().sprite = FindSprite.InResources("UI_arrows");

            _previousRoomButton.interactable = true;

            _nextRoomButton.interactable = true;

            if (Rooms.IndexOf(_currentRoom) == 0)
            {
                _previousRoomButton.interactable = false;
                _gc.ActivePanel.GetComponent<Image>().sprite = FindSprite.InResources("UI_right_arrow");

            }

            if (Rooms.IndexOf(_currentRoom) == Rooms.Count - 1)
            {
                _nextRoomButton.interactable = false;
                _gc.ActivePanel.GetComponent<Image>().sprite = FindSprite.InResources("UI_left_arrow");
            }



            _gc.ShowControlBar(FindPanel.GO("ControlBarText"));
        }
    }
}
