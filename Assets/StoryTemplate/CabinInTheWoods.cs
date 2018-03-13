using System;
using System.Collections.Generic;
using System.Linq;
using Assets.StoryTemplate.Infrastructure;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Assets.StoryTemplate
{
    public partial class CabinInTheWoods
    {
        private readonly Sprite _handsSprite;
        private readonly Sprite _fireSprite;
        private readonly GameController _gc;
        private readonly Canvas _canvas;
        public List<string> Choices;
        private readonly Dictionary<string, string> _choiceToPhase;
        private string _currentPhase;
        private Text _mainText;
        private Image _impressionImage;
        private readonly Image _canvasBackground;

        public string CurrentPhase
        {
            get
            {
                return _currentPhase;
            }

            set
            {
                _currentPhase = value;
            }
        }

        private void GoToRoom(string direction)
        {
            if (direction == "fwd")
            {
                if(!string.IsNullOrEmpty(_nextRoom))
                _currentRoom = _nextRoom;

                if(Rooms.IndexOf(_currentRoom) < Rooms.Count - 1)
                    _nextRoom = Rooms[Rooms.IndexOf(_currentRoom) + 1];
                else
                {
                    _nextRoom = null;
                }

                if (Rooms.IndexOf(_currentRoom) > 1)
                    _previousRoom = Rooms[Rooms.IndexOf(_currentRoom) - 1];
                else
                {
                    _previousRoom = null;
                }


                Debug.Log(Rooms.IndexOf(_currentRoom));
                Debug.Log(_currentRoom);
                AdvancePhase();
            }
            if (direction == "bck")
            {
                if (!string.IsNullOrEmpty(_previousRoom))
                    _currentRoom = _previousRoom;

                if (Rooms.IndexOf(_currentRoom) >  1)
                    _nextRoom = Rooms[Rooms.IndexOf(_currentRoom) + 1];
                else
                {
                    _nextRoom = null;
                }

                if (Rooms.IndexOf(_currentRoom) > 0)
                    _previousRoom = Rooms[Rooms.IndexOf(_currentRoom) - 1];
                else
                {
                    _previousRoom = null;
                }


                Debug.Log(Rooms.IndexOf(_currentRoom));
                Debug.Log(_currentRoom);
                AdvancePhase();
            }

        }


        public CabinInTheWoods(string name, string description, string imageUrl) : base(name, description, imageUrl)
        {
            Choices = new List<string>();

            
            _gc = FindGameController.Named("GameController");
            
            _handsSprite = FindSprite.InResources("placeholder_hands");
            _fireSprite = FindSprite.InResources("placeholder_wood-burning_stove");
            //_canvas = _gc.ActiveCanvas;
            _canvas = FindCanvas.Named(_gc.Stories.Values.ElementAt(0).SnakeCase() + "_canvas");
            _canvasBackground = _canvas.GetComponent<Image>();
            _nextRoomButton = FindButton.Named("RightControlButton");
            _previousRoomButton = FindButton.Named("LeftControlButton");

            InitializeStoryElements();

            _choiceToPhase = new Dictionary<string, string>
            {
                ["Aate_Click"] = "4.3",
                ["Player_Aate_1"] = "4.4",
                ["Player_Aate_2"] = "4.4",
                ["Player_Aate_3"] = "4.4",
                ["IntroChoice_2"] = "11",
                ["IntroChoice_3"] = "11",
            };
            InitializePhases();
        }

        public void ProcessChoice(string choice)
        {
            Choices.Add(choice);
            _gc.SaveGame();
            PlayPhase(_choiceToPhase[choice]);

        }



        private GameObject GetTextPanel(bool clean=false)
        {
            //GET THE TEXT PANEL
            //Cleanup from previous stage
            //remove the all but first text components

            var textPanel = FindPanel.GO("ControlBarText");
            var textfields = textPanel.GetComponentsInChildren<Text>();

            if (clean) {
                var kill = false;
                foreach (var textfield in textfields)
                {

                    if (kill)
                    {
                        Object.Destroy(textfield);
                    }
                    else {
                        kill = true;
                        textfield.text = "";
                        textfield.resizeTextForBestFit = true;
                        var choice = textfield.GetComponentInChildren<SaveChoice>();
                        if (choice) Object.Destroy(choice);
                    }
                }
                    // find the text panel
            }
            //move it to the game canvas
            textPanel.transform.SetParent(_canvas.transform);
            //push it to front
            _gc.ShowControlBar(textPanel);

            return textPanel;
        }

        public void AdvancePhase()
        {
            if (_phaseTransition)
            {
               
                _phaseTransition = false;
                PlayPhase("-1");
                return;
            }
            if (!string.IsNullOrEmpty(_nextPhase))
            {
                var next = _nextPhase;
                _nextPhase = null;

                PlayPhase(next);
                
                return;
            }

            var phase = (Convert.ToDouble(_currentPhase) + 0.1).ToString("N" + 1);
            
            if (_phases.ContainsKey(phase))
            {
                PlayPhase(phase);
            }
            else
            {
                PlayPhase(
                    Convert.ToDouble(
                        Math.Floor(
                            Double.Parse(_currentPhase) + 1
                )).ToString("N" + 0));
            }
        }

        public void PlayPhase(string phase)
        {
            _currentPhase = phase;
            _phases[phase]();
            Debug.Log("Phase " + phase);
        }

        private void DisplayText(Text text1, string choice="")
        {
            //Add visual effects - set transparent and fade in
            //VisualEffects.SetTextTransparent(text1);
            //VisualEffects.TextFadeIn(text1);

            Impress.FadeIn(text1.gameObject);
            //text1.gameObject.AddComponent<TextPartial>();
            //text1.GetComponent<TextPartial>().FinalText = _storyPrompts[text1.name];

            //change text and set up the choice click actions
            text1.text = _storyPrompts[text1.name];

            if (choice.Length > 0)
            {
                text1.name = choice;
                text1.gameObject.AddComponent<SaveChoice>();
            }

        }
    }
}
