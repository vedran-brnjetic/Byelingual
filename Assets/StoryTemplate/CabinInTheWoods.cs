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
        public string CurrentPhase;
        private Text _mainText;
        private Image _impressionImage;
        private readonly Image _canvasBackground;

        

        private void GoToRoom(string direction)
        {
            if (direction == "fwd")
            {
                if(!string.IsNullOrEmpty(NextRoom))
                CurrentRoom = NextRoom;

                if(Rooms.IndexOf(CurrentRoom) < Rooms.Count - 1)
                    NextRoom = Rooms[Rooms.IndexOf(CurrentRoom) + 1];
                else
                {
                    NextRoom = null;
                }

                if (Rooms.IndexOf(CurrentRoom) > 1)
                    PreviousRoom = Rooms[Rooms.IndexOf(CurrentRoom) - 1];
                else
                {
                    PreviousRoom = null;
                }


                Debug.Log(Rooms.IndexOf(CurrentRoom));
                Debug.Log(CurrentRoom);
                AdvancePhase();
            }
            if (direction == "bck")
            {
                if (!string.IsNullOrEmpty(PreviousRoom))
                    CurrentRoom = PreviousRoom;

                if (Rooms.IndexOf(CurrentRoom) >  1)
                    NextRoom = Rooms[Rooms.IndexOf(CurrentRoom) + 1];
                else
                {
                    NextRoom = null;
                }

                if (Rooms.IndexOf(CurrentRoom) > 0)
                    PreviousRoom = Rooms[Rooms.IndexOf(CurrentRoom) - 1];
                else
                {
                    PreviousRoom = null;
                }


                Debug.Log(Rooms.IndexOf(CurrentRoom));
                Debug.Log(CurrentRoom);
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
            if (!string.IsNullOrEmpty(NextPhase))
            {
                var next = NextPhase;
                NextPhase = null;

                PlayPhase(next);
                
                return;
            }

            var phase = (Convert.ToDouble(CurrentPhase) + 0.1).ToString("N" + 1);
            
            if (_phases.ContainsKey(phase))
            {
                PlayPhase(phase);
            }
            else
            {
                PlayPhase(
                    Convert.ToDouble(
                        Math.Floor(
                            Double.Parse(CurrentPhase) + 1
                )).ToString("N" + 0));
            }
        }

        public void PlayPhase(string phase)
        {
            CurrentPhase = phase;
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
