using System.Collections.Generic;
using System.Linq;
using Assets.StoryTemplate.Infrastructure;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class CabinInTheWoods : Story
    {
        private Sprite _handsSprite;
        private Sprite _fireSprite;
        private GameController _gc;
        private Canvas _canvas;
        private Dictionary<string, string> _storyPrompts;
        public List<string> choices;
        public Dictionary<string, int> choiceToPhase;

        public CabinInTheWoods(string name, string description, string imageUrl) : base(name, description, imageUrl)
        {
            choices = new List<string>();
            

            _gc = FindGameController.Named("GameController");
            _handsSprite = FindSprite.InResources("placeholder_hands");
            _fireSprite = FindSprite.InResources("placeholder_wood-burning_stove");
            _canvas = FindCanvas.Named(_gc.Stories.Values.ElementAt(0).SnakeCase() + "_canvas");
            _storyPrompts = new Dictionary<string, string>
            {
                ["Intro01"] = "In the beginning, the place looked abandoned...",
                ["Intro02"] = "...now it doesn't.",
                ["Intro03"] = "In the beginning, there was no light source...",
                ["Intro04"] = "...except for our cellphones.",
            };

            choiceToPhase = new Dictionary<string, int>
            {
                ["IntroChoice_1"] = 1,
                ["IntroChoice_2"] = 1,
            };
        }

        public void ProcessChoice(string choice)
        {
            choices.Add(choice);
            _gc.SaveGame();
            PlayIntro(choiceToPhase[choice]);

        }

        

        private GameObject GetTextPanel()
        {
            //GET THE TEXT PANEL

            // find the text panel
            var textPanel = FindPanel.GO("ControlBarText");
            //move it to the game canvas
            textPanel.transform.SetParent(_canvas.transform);
            //push it to front
            _gc.ShowPanel(textPanel);

            return textPanel;
        }

        public void PlayIntro(int stage)
        {
            switch (stage)
            {
                case 0:
                {
                    var textPanel = GetTextPanel();
                    //Find the Text component in the panel
                    var text1 = textPanel.GetComponentInChildren<Text>();

                    var text2 = Object.Instantiate(text1, textPanel.transform, true);
                    text2.transform.Translate(0f, -text1.preferredHeight, 0f);
                    
                    var text3 = Object.Instantiate(text2, textPanel.transform, true);
                    text3.transform.Translate(0f, -(text2.preferredHeight*1.66f), 0f);
                    
                    var text4 = Object.Instantiate(text3, textPanel.transform, true);
                    text4.transform.Translate(0f, -text3.preferredHeight, 0f);
                    

                    DisplayTextualChoiceOption(text1, "Intro01", "IntroChoice_1");
                    DisplayTextualChoiceOption(text2, "Intro02", "IntroChoice_1");
                    DisplayTextualChoiceOption(text3, "Intro03", "IntroChoice_2");
                    DisplayTextualChoiceOption(text4, "Intro04", "IntroChoice_2");

                    text2.alignment = TextAnchor.UpperRight;
                    text4.alignment = TextAnchor.UpperRight;


                        break;
                }
                case 1:
                {
                    var imagePanel = FindPanel.GO("ControlBarImage");
                    imagePanel.transform.SetParent(_canvas.transform);
                    _gc.ShowPanel(imagePanel);

                    var image1 = FindImage.Named("Image1");
                    var image2 = FindImage.Named("Image2");


                    image1.sprite = _handsSprite;
                    image2.sprite = _fireSprite;

                    image1.name = "Hands";
                    image2.name = "Fire";

                    image1.gameObject.AddComponent<SaveChoice>();
                    image2.gameObject.AddComponent<SaveChoice>();

                    VisualEffects.SetImageTransparent(image1);
                    VisualEffects.SetImageTransparent(image2);

                    _gc.ElementsToCrossfade.Add(image1.gameObject);
                    _gc.ElementsToCrossfade.Add(image2.gameObject);

                    _gc.DelayLoad(3);

                    var canvasBg = FindCanvas.Named(_gc.CurrentStory.SnakeCase() + "_canvas").GetComponent<Image>();
                    canvasBg.sprite = FindSprite.InResources("CabinInterior1");

                    //VisualEffects.SetImageTransparent(canvasBg);
                    VisualEffects.ImageFadeIn(canvasBg);

                    _gc.ElementsToCrossfade.Add(canvasBg.gameObject);
                    break;

                }
            }
            
        }

        private void DisplayTextualChoiceOption(Text text1, string prompt, string choice)
        {
            //Add visual effects - set transparent and fade in
            VisualEffects.SetTextTransparent(text1);
            VisualEffects.TextFadeIn(text1);
            _gc.ElementsToCrossfade.Add(text1.gameObject);

            //change text and set up the choice click actions
            text1.text = _storyPrompts[prompt];

            text1.name = choice;
            text1.gameObject.AddComponent<SaveChoice>();
        }
    }
}
