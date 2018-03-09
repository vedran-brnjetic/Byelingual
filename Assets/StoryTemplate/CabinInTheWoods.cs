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
        private int _currentPhase;
        private Text _mainText;
        private Image _impressionImage;

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
                ["Intro05"] = "“They can track our phones through GPS!”\n“Turn it off.”\n“Bah! There are no search parties yet. They’re going to wait for us to come back, then deny us dessert as punishment – maybe eat it loudly in front of us to drive the message across.”\n“I’ve already thrown mine in the lake.”\n“What?”",
                ["Intro06"] = "...whereas you never even had a phone.",
            };

            choiceToPhase = new Dictionary<string, int>
            {
                ["IntroChoice_1"] = 1,
                ["IntroChoice_2"] = 1,
                ["IntroChoice_3"] = 1,
            };
        }

        public void ProcessChoice(string choice)
        {
            choices.Add(choice);
            _gc.SaveGame();
            PlayPhase(choiceToPhase[choice]);

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

        public void AdvancePhase()
        {
            PlayPhase(_currentPhase + 1);
        }

        public void PlayPhase(int phase, int resume=0)
        {
            _currentPhase = phase;

            Debug.Log("phase " + phase);

            switch (phase)
            {
                case -1://delay execution for 2 seconds, resume to the next phase
                    {
                        _gc.DelayLoad(2);
                        PlayPhase(resume);
                        break;
                    }
                case 0:
                    {//Intro start
                        var textPanel = GetTextPanel();
                        //Find the Text component in the panel
                        var text1 = textPanel.GetComponentInChildren<Text>();

                        var text2 = Object.Instantiate(text1, textPanel.transform, true);
                        text2.transform.Translate(0f, -text1.preferredHeight, 0f);
                    
                        var text3 = Object.Instantiate(text2, textPanel.transform, true);
                        text3.transform.Translate(0f, -(text2.preferredHeight*1.666f), 0f);
                    
                        var text4 = Object.Instantiate(text3, textPanel.transform, true);
                        text4.transform.Translate(0f, -text3.preferredHeight, 0f);

                        
                        DisplayText(text1, "Intro01");
                        DisplayText(text2, "Intro02");
                        DisplayText(text3, "Intro03");
                        DisplayText(text4, "Intro04");
                        

                        _gc.ActiveCanvas.gameObject.AddComponent<AdvancePhase>();

                        text2.alignment = TextAnchor.MiddleRight;
                        text4.alignment = TextAnchor.MiddleRight;


                        break;
                    }
                case 1:
                    {//hide panels and fade out the game title (reusing the same text box for the main text)
                        _gc.HideAllPanels();
                        _mainText = _gc.ActiveCanvas.transform.Find("GameTitle").gameObject.GetComponent<Text>();

                        Impress.Crossfade(_mainText.gameObject);
                        //PlayIntro(-1, 2);
                        break;
                    }
                case 2:
                    {//this phase starts automatically during crossfade script once the title fades out completely
                        
                        
                        _mainText.color = Color.white;
                        _mainText.text = _storyPrompts["Intro05"];
                        //text.gameObject.GetComponent<RectTransform>().rect.y *= 2f;
                        
                    


                        /*
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

                    

                        var canvasBg = FindCanvas.Named(_gc.CurrentStory.SnakeCase() + "_canvas").GetComponent<Image>();
                        canvasBg.sprite = FindSprite.InResources("CabinInterior1");

                        //VisualEffects.SetImageTransparent(canvasBg);
                        VisualEffects.ImageFadeIn(canvasBg);

                        _gc.ElementsToCrossfade.Add(canvasBg.gameObject);
                        */
                        break;

                    }
                case 3:
                    {//phase 3 fade out the main text
                        Impress.FadeOutAndAdvanceGame(_mainText.gameObject);
                        break;
                    }
                case 4:
                    {//clear the text from the textbox
                        _mainText.text = "";
                        
                        //AdvancePhase();
                        break;
                    }
                case 5:
                    {//show the hands
                        var _impressionImage = _gc.ActiveCanvas.transform.Find("SingleImageLeft").gameObject.GetComponent<Image>();
                        _impressionImage.sprite = _handsSprite;
                        //Impress.FadeIn(_impressionImage.gameObject);
                        Impress.FadeOutAndAdvanceGame(_mainText.gameObject);
                        break;
                    }
                case 6:
                    {//show the text with hands
                        _mainText.text = _storyPrompts["Intro06"];
                        _mainText.alignment = TextAnchor.UpperRight;
                        //AdvancePhase();
                        break;
                    }
                default:
                    {
                        var s = "Phase " + phase + " not implemented yet.";
                        Debug.Log(s);
                        break;
                    }
                }
            
        }

        private void DisplayText(Text text1, string prompt, string choice="")
        {
            //Add visual effects - set transparent and fade in
            VisualEffects.SetTextTransparent(text1);
            //VisualEffects.TextFadeIn(text1);
            Impress.FadeIn(text1.gameObject);

            //change text and set up the choice click actions
            text1.text = _storyPrompts[prompt];

            if (choice.Length > 0)
            {
                text1.name = choice;
                text1.gameObject.AddComponent<SaveChoice>();
            }
        }
    }
}
