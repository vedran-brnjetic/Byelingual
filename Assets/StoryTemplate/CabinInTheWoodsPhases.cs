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
        private Dictionary<int, Action> _phases;

        private void InitializePhases()
        {
            _phases = new Dictionary<int, Action>
            {
                [0] = () =>
                {
                    //Intro start
                    var textPanel = GetTextPanel(true);
                    //Find the Text component in the panel
                    var text1 = textPanel.GetComponentInChildren<Text>();

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
                    _gc.ActiveCanvas.gameObject.AddComponent<AdvancePhase>();

                },
                [1] = () =>
                {//hide panels and fade out the game title (reusing the same text box for the main text)
                    var text2 = GetTextPanel().transform.Find("Intro02").GetComponent<Text>();
                    DisplayText(text2);
                    
                    //

                    
                },
                [2] = () =>
                    {
                        var text3 = GetTextPanel().transform.Find("Intro03").GetComponent<Text>();
                        DisplayText(text3);
                    },
                [3] = () =>
                {
                    var text4 = GetTextPanel().transform.Find("Intro04").GetComponent<Text>();
                    DisplayText(text4);
                },
                [4] = () =>
                    {
                        _mainText = _gc.ActiveCanvas.transform.Find("GameTitle").gameObject.GetComponent<Text>();
                        _mainText.gameObject.AddComponent<TextPartial>();
                        
                        Impress.Crossfade(_mainText.gameObject);
                        _gc.HideAllPanels();
                    },
                [5] = () =>
                {//this phase starts automatically during crossfade script once the title fades out completely
                    _mainText.color = Color.white;
                    _mainText.GetComponent<TextPartial>().FinalText = _storyPrompts["Intro05"];
                    _mainText.text = _storyPrompts["Intro05"];
                },
                [6] = () =>
                {//phase 3 fade out the main text
                    Impress.FadeOut(_mainText.gameObject, true);
                },
                [7] = () =>
                {//clear the text from the textbox
                    _mainText.text = "";

                    AdvancePhase();
                },
                [8] = () =>
                {//show the hands
                    _impressionImage = _gc.ActiveCanvas.transform.Find("SingleImageLeft").gameObject
                        .GetComponent<Image>();
                    _impressionImage.sprite = _handsSprite;
                    Impress.FadeIn(_impressionImage.gameObject, advanceGame: true);
                },
                [9] = () =>
                {//show the text with hands
                    Impress.FadeIn(_mainText.gameObject, advanceGame: true);
                    _mainText.resizeTextMaxSize = 20;
                    _mainText.text = _storyPrompts["Intro06"];
                    _mainText.alignment = TextAnchor.UpperRight;
                },
                [10] = () =>
                {//fade out hands and text
                    Impress.FadeOut(_mainText.gameObject);
                    Impress.FadeOut(_impressionImage.gameObject, advance: true);
                },
                [11] = () =>
                {//show the fire
                    _impressionImage = _gc.ActiveCanvas.transform.Find("SingleImageLeft").gameObject
                        .GetComponent<Image>();
                    _impressionImage.sprite = _fireSprite;
                    Impress.FadeIn(_impressionImage.gameObject, advanceGame: true);
                },
                [12] = () =>
                {//show the text with fire
                    Impress.FadeIn(_mainText.gameObject, advanceGame: true);
                    _mainText.text = _storyPrompts["Intro07"];
                    _mainText.alignment = TextAnchor.UpperLeft;
                },
                [13] = () =>
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
                [14] = () =>
                {
                    _gc.HideAllPanels();
                    Impress.FadeOut(_mainText.gameObject);
                    Impress.FadeOut(_impressionImage.gameObject, true);
                },
                [15] = () =>
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
                [16] = () =>
                {
                    _gc.HideAllPanels();
                    AdvancePhase();
                },
                [17] = () =>
                    {
                        //Impress.Crossfade(_gc.ActiveCanvas.gameObject);

                        _gc.ActiveCanvas.GetComponent<Image>().sprite = FindSprite.InResources("CabinInterior1");
                        _gc.ActiveCanvas.GetComponent<Image>().color = Color.white;

                    },
                [18] = () =>
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
                [19] = () =>
                {

                }
            };


            

            

            

            

            

            

            

            

            

        }
    }
}
