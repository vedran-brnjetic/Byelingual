using System.Collections.Generic;
using System.Linq;
using Assets.StoryTemplate.Infrastructure;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.StoryTemplate
{
    public partial class CabinInTheWoods : Story
    {
        private readonly Sprite _handsSprite;
        private readonly Sprite _fireSprite;
        private readonly GameController _gc;
        private readonly Canvas _canvas;
        public List<string> Choices;
        private readonly Dictionary<string, int> _choiceToPhase;
        private int _currentPhase;
        private Text _mainText;
        private Image _impressionImage;


        public CabinInTheWoods(string name, string description, string imageUrl) : base(name, description, imageUrl)
        {
            Choices = new List<string>();


            _gc = FindGameController.Named("GameController");
            _handsSprite = FindSprite.InResources("placeholder_hands");
            _fireSprite = FindSprite.InResources("placeholder_wood-burning_stove");
            //_canvas = _gc.ActiveCanvas;
            _canvas = FindCanvas.Named(_gc.Stories.Values.ElementAt(0).SnakeCase() + "_canvas");

            InitializeStoryPrompts();

            _choiceToPhase = new Dictionary<string, int>
            {
                ["IntroChoice_1"] = 11,
                ["IntroChoice_2"] = 11,
                ["IntroChoice_3"] = 11,
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
            _gc.ShowPanel(textPanel);

            return textPanel;
        }

        public void AdvancePhase()
        {
            PlayPhase(_currentPhase + 1);
        }

        public void PlayPhase(int phase)
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
            text1.gameObject.AddComponent<TextPartial>();
            text1.GetComponent<TextPartial>().FinalText = _storyPrompts[text1.name];

            //change text and set up the choice click actions
            text1.text = "";

            if (choice.Length > 0)
            {
                text1.name = choice;
                text1.gameObject.AddComponent<SaveChoice>();
            }

        }
    }
}
