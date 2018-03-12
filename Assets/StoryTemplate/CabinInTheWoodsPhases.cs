using System;
using System.Collections.Generic;
using Assets.StoryTemplate.Infrastructure;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Assets.StoryTemplate
{
    public partial class CabinInTheWoods : Story
    {
        private Dictionary<string, Action> _phases;
        private Dictionary<string, string> _storyPrompts;

        private void InitializeStoryPrompts(){
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

              //intro conversation
              ["Intro05.0"] = "“They can track our phones through GPS!”\n\n",
              ["Intro05.1"] = "“Turn it off!”\n\n",
              ["Intro05.2"] = "“Bah! There are no search parties yet. They’re going to wait for us to come back, then deny us dessert as punishment – maybe eat it loudly in front of us to drive the message across.”\n\n",
              ["Intro05.3"] = "“I’ve already thrown mine in the lake.”\n\n",
              ["Intro05.4"] = "“What!?”",

              //hands
              ["Intro06"] = "...whereas you never even had a phone.",

              //fire
              ["Intro07"] = "There is a fire in the wood burner. Looking at the fire, you feel...",

              //options
              ["Intro08"] = "...excited.",
              ["Intro08.1"] = "You worked for this fire. You earned its warmth and comfort. You created a place for yourself where there was none.You could not be prouder of yourself in this moment. Finally, it’s all up to you.",

              ["Intro09"] = "...relieved.",
              ["Intro09.1"] = "This is a place where it does not matter that you have less than others.Here everyone has little.Maybe even too little.We are going to need everyone’s effort and cooperation to make it, which means that you are needed. At last there exists common ground where you stand as one among equals.",

              ["Intro10"] = "...skeptical.",
              ["Intro10.1"] = "That fire is not going to last long, and neither is the spirit of all these hopefuls basking in its short-lived heat. They dream big but are only now getting their first taste of true deprivation. The hunger, the cold, the lack of privacy, illness... the last of which will be a guaranteed breaking point for this unlikely uprising.",

              //intro end
              ["Intro11"] = "...it was a good dream",
              ["Intro12"] = "You dreamt of flying. From tree to tree you leapt, and a gust of wind swept you up beyond the canopy, from where you were gliding down gracefully, admiring the views, until your descent ended in a dip in ice-cold, dark, murky water. The shock woke you up.",
              ["Intro13"] = "...the fire has gone out.",

          };

        }

        private void InitializePhases()
        {
            /*0. First screen
             *1. Dialog in the dark
             *2. Hands
             *3. Fireplace+choice
             *4. First Room - ACT1
             */
            _phases = new Dictionary<string, Action>
            {
                ["0"] = () =>
                {
                    //Intro start
                    var textPanel = GetTextPanel(true);
                    //Find the Text component in the panel
                    var text1 = textPanel.GetComponentInChildren<Text>();
                    text1.color = Color.black;
                    var panelImage = textPanel.GetComponent<Image>();
                    panelImage.sprite = FindSprite.InResources("UI_no_arrows");
                    var canvasBg = _gc.ActiveCanvas.GetComponent<Image>();

                    //canvasBg.sprite = FindSprite.InResources("placeholder_hands");
                    canvasBg.color= Color.black;


                    var text2 = Object.Instantiate<Text>(text1, textPanel.transform, true);
                    text2.transform.Translate(0f, -text1.preferredHeight * 1.05f, 0f);

                    var text3 = Object.Instantiate(text2, textPanel.transform, true);
                    text3.transform.Translate(0f, -(text2.preferredHeight * 1.666f), 0f);

                    var text4 = Object.Instantiate(text3, textPanel.transform, true);
                    text4.transform.Translate(0f, -text3.preferredHeight * 1.05f, 0f);

                    text1.name = "Intro01";
                    text2.name = "Intro02";
                    text3.name = "Intro03";
                    text4.name = "Intro04";


                    DisplayText(text1);


                    text2.alignment = TextAnchor.MiddleRight;
                    text4.alignment = TextAnchor.MiddleRight;
                    //_gc.ActiveCanvas.gameObject.AddComponent<AdvancePhase>();

                },
                ["0.1"] = () =>
                {//hide panels and fade out the game title (reusing the same text box for the main text)
                    var text2 = GetTextPanel().transform.Find("Intro02").GetComponent<Text>();
                    DisplayText(text2);

                    //


                },
                ["0.2"] = () =>
                    {
                        var text3 = GetTextPanel().transform.Find("Intro03").GetComponent<Text>();
                        DisplayText(text3);
                    },
                ["0.3"] = () =>
                {
                    var text4 = GetTextPanel().transform.Find("Intro04").GetComponent<Text>();
                    DisplayText(text4);
                },
                ["0.4"] = () => 
                {
                    Impress.FadeOut(GetTextPanel(), true);
                },
                ["1"] = () =>
                    {
                        _mainText = _gc.ActiveCanvas.transform.Find("GameTitle").gameObject.GetComponent<Text>();
                        _mainText.gameObject.AddComponent<TextPartial>();
                        _mainText.GetComponent<TextPartial>().FinalText = _storyPrompts["Intro05.0"];
                        _mainText.resizeTextForBestFit = false;
                        _mainText.color = Color.white;
                        _mainText.fontSize = 20;
                        Impress.FadeIn(_mainText.gameObject);
                        _gc.HideAllPanels();
                    },
                ["1.1"] = () =>
                {
                    _mainText.GetComponent<TextPartial>().FinalText += _storyPrompts["Intro05.1"];
                    Impress.FadeIn(_mainText.gameObject);
                },
                ["1.2"] = () =>
                {
                    _mainText.GetComponent<TextPartial>().FinalText += _storyPrompts["Intro05.2"];
                    Impress.FadeIn(_mainText.gameObject);
                },
                ["1.3"] = () =>
                {
                    _mainText.GetComponent<TextPartial>().FinalText += _storyPrompts["Intro05.3"];
                    Impress.FadeIn(_mainText.gameObject);
                },
                ["1.4"] = () =>
                {//this phase starts automatically during crossfade script once the title fades out completely
                  _mainText.GetComponent<TextPartial>().FinalText += _storyPrompts["Intro05.4"];
                  Impress.FadeIn(_mainText.gameObject);
                  

                },
                ["1.5"] = () =>
                {//phase 3 fade out the main text
                    Impress.FadeOut(_mainText.gameObject, true);
                },
                ["2"] = () =>
                {//show the hands
                    _impressionImage = _gc.ActiveCanvas.gameObject.transform.Find("ImpressionImage").GetComponent<Image>();
                    _impressionImage.sprite = _handsSprite;
                    _mainText.text = "";
                    _mainText.color = Color.white;
                    Impress.FadeIn(_impressionImage.gameObject);
                    _currentPhase = "2.8";
                },
                ["2.1"] = () =>
                {//show the text with hands
                    _mainText.resizeTextMaxSize = 20;
                    
                    _mainText.GetComponent<TextPartial>().CurrentText = "";
                    _mainText.GetComponent<TextPartial>().FinalText = _storyPrompts["Intro06"];
                    _mainText.alignment = TextAnchor.LowerRight;
                    Impress.FadeIn(_mainText.gameObject);
                },
                ["2.2"] = () =>
                    {
                        //Impress.FadeOut(_mainText.gameObject);
                        _mainText.text = "";
                        Impress.Crossfade(_impressionImage.gameObject);
                    },
                ["2.3"] = () =>
                {//show the fire
                    _impressionImage = _gc.ActiveCanvas.transform.Find("SingleImageLeft").gameObject.GetComponent<Image>();
                    _impressionImage.sprite = _fireSprite;
                    Impress.FadeIn(_impressionImage.gameObject, advanceGame: true);
                },
                ["2.4"] = () =>
                {//show the text with fire

                    _mainText.GetComponent<TextPartial>().CurrentText = "";
                    _mainText.GetComponent<TextPartial>().FinalText = _storyPrompts["Intro07"];

                    //_mainText.text = _storyPrompts["Intro07"];

                    _mainText.alignment = TextAnchor.UpperLeft;
                    Impress.FadeIn(_mainText.gameObject, advanceGame: true);
                },
                ["2.5"] = () =>
                {// display the text choices
                    var textPanel = GetTextPanel(true);
                    //Find the Text component in the panel
                    var text1 = textPanel.GetComponentInChildren<Text>();
                    text1.resizeTextForBestFit = true;

                    var text2 = Object.Instantiate(text1, textPanel.transform, true);
                    text2.transform.Translate(0f, -text1.preferredHeight * 1.66f, 0f);

                    var text3 = Object.Instantiate(text2, textPanel.transform, true);
                    text3.transform.Translate(0f, -text2.preferredHeight * 1.66f, 0f);

                    text1.name = "Intro08";
                    text2.name = "Intro09";
                    text3.name = "Intro10";

                    DisplayText(text1, "IntroChoice_1");
                    DisplayText(text2, "IntroChoice_2");
                    DisplayText(text3, "IntroChoice_3");


                    _gc.ActiveCanvas.gameObject.AddComponent<AdvancePhase>();
                },
                ["2.6"] = () =>
                {
                    _gc.HideAllPanels();
                    Impress.FadeOut(_mainText.gameObject);
                    Impress.FadeOut(_impressionImage.gameObject, true);
                },
                ["3"] = () =>
                {
                    var textPanel = GetTextPanel(true);
                    //Find the Text component in the panel
                    var text1 = textPanel.GetComponentInChildren<Text>();
                    text1.resizeTextForBestFit = true;

                    var text2 = Object.Instantiate(text1, textPanel.transform, true);
                    text2.transform.Translate(0f, -text1.preferredHeight * 1.66f, 0f);

                    var text3 = Object.Instantiate(text2, textPanel.transform, true);
                    text3.transform.Translate(0f, -text2.preferredHeight * 1.66f, 0f);

                    DisplayText(text1, "Intro11");
                    DisplayText(text2, "Intro12");
                    DisplayText(text3, "Intro13");

                    text1.resizeTextForBestFit = false;

                    text3.resizeTextForBestFit = false;
                    text1.alignment = TextAnchor.UpperRight;
                    text3.alignment = TextAnchor.UpperRight;

                    _gc.ActiveCanvas.gameObject.AddComponent<AdvancePhase>();
                },
                ["3.1"] = () =>
                {
                    _gc.HideAllPanels();
                    AdvancePhase();
                },
                ["4"] = () =>
                    {
                        //Impress.Crossfade(_gc.ActiveCanvas.gameObject);

                        _gc.ActiveCanvas.GetComponent<Image>().sprite = FindSprite.InResources("CabinInterior1");
                        _gc.ActiveCanvas.GetComponent<Image>().color = Color.white;

                    },
                ["4.1"] = () =>
                {
                    var textPanel = GetTextPanel(true);
                    textPanel.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.66f);
                    //Find the Text component in the panel
                    var text1 = textPanel.GetComponentInChildren<Text>();
                    text1.color = Color.grey;
                    //text1.resizeTextForBestFit = true;
                    text1.alignment = TextAnchor.UpperLeft;
                    text1.fontStyle = FontStyle.Italic;

                    var text2 = Object.Instantiate(text1, textPanel.transform, true);
                    text2.transform.Translate(0f, -text1.preferredHeight * 1.35f, 0f);

                    var text3 = Object.Instantiate(text2, textPanel.transform, true);
                    text3.transform.Translate(0f, -text2.preferredHeight * 1.35f, 0f);

                    text1.name = "Location1";
                    text2.name = "Location2";
                    text3.name = "Location3";

                    DisplayText(text1, "AfterIntroLocation1");
                    DisplayText(text2, "AfterIntroLocation2");
                    DisplayText(text3, "AfterIntroLocation3");
                },
                ["4.2"] = () =>
                {

                }
            };

        }
    }
}
