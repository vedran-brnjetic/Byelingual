﻿using System.Collections.Generic;
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
        private readonly Dictionary<string, string> _storyPrompts;
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
            _storyPrompts = new Dictionary<string, string>
            {
                //locations
                ["Location1"] = "Go to common room",
                ["Location2"] = "Go outside",
                ["Location3"] = "Go to Pond",
                
                //first intro text
                ["Intro01"] = "In the beginning, the place looked abandoned...",
                ["Intro02"] = "...now it doesn't.",
                ["Intro03"] = "In the beginning, there was no light source...",
                ["Intro04"] = "...except for our cellphones.",

                //intro main text
                ["Intro05"] = "“They can track our phones through GPS!”\n“Turn it off.”\n“Bah! There are no search parties yet. They’re going to wait for us to come back, then deny us dessert as punishment – maybe eat it loudly in front of us to drive the message across.”\n“I’ve already thrown mine in the lake.”\n“What?”",

                //hands
                ["Intro06"] = "...whereas you never even had a phone.",

                //fire
                ["Intro07"] = "There is a fire in the wood burner. Looking at the fire, you feel...",

                //options
                ["Intro08"] = "...excited. You worked for this fire. You earned its warmth and comfort. You created a place for yourself where there was none.You could not be prouder of yourself in this moment. Finally, it’s all up to you.",
                ["Intro09"] = "...relieved. This is a place where it does not matter that you have less than others.Here everyone has little.Maybe even too little.We are going to need everyone’s effort and cooperation to make it, which means that you are needed. At last there exists common ground where you stand as one among equals.",
                ["Intro10"] = "...skeptical. That fire is not going to last long, and neither is the spirit of all these hopefuls basking in its short-lived heat. They dream big but are only now getting their first taste of true deprivation. The hunger, the cold, the lack of privacy, illness... the last of which will be a guaranteed breaking point for this unlikely uprising.",

                //intro end
                ["Intro11"] = "...it was a good dream",
                ["Intro12"] = "You dreamt of flying. From tree to tree you leapt, and a gust of wind swept you up beyond the canopy, from where you were gliding down gracefully, admiring the views, until your descent ended in a dip in ice-cold, dark, murky water. The shock woke you up.",
                ["Intro13"] = "...the fire has gone out.",

            };

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
