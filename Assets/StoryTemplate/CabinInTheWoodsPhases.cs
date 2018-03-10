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
                    var textPanel = GetTextPanel();
                    //Find the Text component in the panel
                    var text1 = textPanel.GetComponentInChildren<Text>();

                    var text2 = Object.Instantiate<Text>(text1, textPanel.transform, true);
                    text2.transform.Translate(0f, -text1.preferredHeight * 1.05f, 0f);

                    var text3 = Object.Instantiate(text2, textPanel.transform, true);
                    text3.transform.Translate(0f, -(text2.preferredHeight * 1.666f), 0f);

                    var text4 = Object.Instantiate(text3, textPanel.transform, true);
                    text4.transform.Translate(0f, -text3.preferredHeight * 1.05f, 0f);

                    DisplayText(text1, "Intro01");
                    DisplayText(text2, "Intro02");
                    DisplayText(text3, "Intro03");
                    DisplayText(text4, "Intro04");

                    text2.alignment = TextAnchor.MiddleRight;
                    text4.alignment = TextAnchor.MiddleRight;
                    _gc.ActiveCanvas.gameObject.AddComponent<AdvancePhase>();

                },
                [1] = () =>
                {//hide panels and fade out the game title (reusing the same text box for the main text)
                    

                    _mainText = _gc.ActiveCanvas.transform.Find("GameTitle").gameObject.GetComponent<Text>();

                    Impress.Crossfade(_mainText.gameObject);
                    _gc.HideAllPanels();
                },
                [2] = () =>
                {//this phase starts automatically during crossfade script once the title fades out completely
                    _mainText.color = Color.white;
                    _mainText.text = _storyPrompts["Intro05"];
                },
                [3] = () =>
                {//phase 3 fade out the main text
                    Impress.FadeOut(_mainText.gameObject, true);
                },
                [4] = () =>
                {//clear the text from the textbox
                    _mainText.text = "";

                    AdvancePhase();
                },
                [5] = () =>
                {//show the hands
                    _impressionImage = _gc.ActiveCanvas.transform.Find("SingleImageLeft").gameObject
                        .GetComponent<Image>();
                    _impressionImage.sprite = _handsSprite;
                    Impress.FadeIn(_impressionImage.gameObject, advanceGame: true);
                },
                [6] = () =>
                {//show the text with hands
                    Impress.FadeIn(_mainText.gameObject, advanceGame: true);
                    _mainText.resizeTextMaxSize = 20;
                    _mainText.text = _storyPrompts["Intro06"];
                    _mainText.alignment = TextAnchor.UpperRight;
                },
                [7] = () =>
                {//fade out hands and text
                    Impress.FadeOut(_mainText.gameObject);
                    Impress.FadeOut(_impressionImage.gameObject, advance: true);
                },
                [8] = () =>
                {//show the fire
                    _impressionImage = _gc.ActiveCanvas.transform.Find("SingleImageLeft").gameObject
                        .GetComponent<Image>();
                    _impressionImage.sprite = _fireSprite;
                    Impress.FadeIn(_impressionImage.gameObject, advanceGame: true);
                },
                [9] = () =>
                {//show the text with fire
                    Impress.FadeIn(_mainText.gameObject, advanceGame: true);
                    _mainText.text = _storyPrompts["Intro07"];
                    _mainText.alignment = TextAnchor.UpperLeft;
                },
                [10] = () =>
                {// display the text choices
                    var textPanel = GetTextPanel();
                    //Find the Text component in the panel
                    var text1 = textPanel.GetComponentInChildren<Text>();
                    text1.resizeTextForBestFit = true;
                    
                    var text2 = Object.Instantiate(text1, textPanel.transform, true);
                    text2.transform.Translate(0f, -text1.preferredHeight * 1.47f, 0f);
                    
                    var text3 = Object.Instantiate(text2, textPanel.transform, true);
                    text3.transform.Translate(0f, -text2.preferredHeight * 1.47f, 0f);

                    DisplayText(text1, "Intro08", "IntroChoice_1");
                    DisplayText(text2, "Intro09", "IntroChoice_2");
                    DisplayText(text3, "Intro10", "IntroChoice_3");
                    

                    _gc.ActiveCanvas.gameObject.AddComponent<AdvancePhase>();
                },
                [11] = () =>
                {
                    _gc.HideAllPanels();
                    Impress.FadeOut(_mainText.gameObject);
                    Impress.FadeOut(_impressionImage.gameObject, true);
                },
                [12] = () =>
                {
                    var textPanel = GetTextPanel();
                    //Find the Text component in the panel
                    var text1 = textPanel.GetComponentInChildren<Text>();
                    text1.resizeTextForBestFit = true;

                    var text2 = Object.Instantiate(text1, textPanel.transform, true);
                    text2.transform.Translate(0f, -text1.preferredHeight * 1.35f, 0f);

                    var text3 = Object.Instantiate(text2, textPanel.transform, true);
                    text3.transform.Translate(0f, -text2.preferredHeight * 1.35f, 0f);

                    DisplayText(text1, "Intro11");
                    DisplayText(text2, "Intro12");
                    DisplayText(text3, "Intro13");

                    text1.resizeTextForBestFit = false;
                    
                    text3.resizeTextForBestFit = false;
                    text1.alignment = TextAnchor.UpperRight;
                    text3.alignment = TextAnchor.UpperRight;

                    _gc.ActiveCanvas.gameObject.AddComponent<AdvancePhase>();
                },
                [13] = () =>
                {
                    _gc.HideAllPanels();
                    AdvancePhase();
                },
                [14] = () =>
                    {
                        //Impress.Crossfade(_gc.ActiveCanvas.gameObject);

                        _gc.ActiveCanvas.GetComponent<Image>().sprite = FindSprite.InResources("CabinInterior1");
                        _gc.ActiveCanvas.GetComponent<Image>().color = Color.white;

                    },
                [15] = () =>
                {
                    var textPanel = GetTextPanel();
                    textPanel.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.66f);
                    //Find the Text component in the panel
                    var text1 = textPanel.GetComponentInChildren<Text>();
                    text1.color = Color.grey;
                    //text1.resizeTextForBestFit = true;
                    text1.alignment = TextAnchor.UpperLeft;
                    text1.fontStyle = FontStyle.Italic;

                    var text2 = Object.Instantiate(text1, textPanel.transform, true);
                    text2.transform.Translate(0f, -text1.preferredHeight * 1.15f, 0f);

                    var text3 = Object.Instantiate(text2, textPanel.transform, true);
                    text3.transform.Translate(0f, -text2.preferredHeight * 1.15f, 0f);

                    DisplayText(text1, "Location1", "AfterIntroLocation1");
                    DisplayText(text2, "Location2", "AfterIntroLocation2");
                    DisplayText(text3, "Location3", "AfterIntroLocation3");
                },
                [16] = () =>
                {

                }
            };


            

            

            

            

            

            

            

            

            

        }
    }
}
