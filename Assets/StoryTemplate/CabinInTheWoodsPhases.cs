using Assets.StoryTemplate.Infrastructure;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Assets.StoryTemplate
{
    /*! \class CabinInTheWoods
    \brief Our first story within the platform

    All the scripting is done within the class
    */
    public partial class CabinInTheWoods : Story
    {
        private Dictionary<string, Action> _phases; // dictionary of all phases (scripting)
        private Dictionary<string, string> _storyPrompts; // dictionary of all ingame strings
        private string _previousPhase; // not sure for what yet, but this might be useful
        private string _nextPhase; // controls where to go next
        private bool _phaseTransition; // flag to signal reinitialisation of the gameroom and fade out screen between non-linear phase jumps
        private Dictionary<string, Image> _characters;

        //rooms
        private string _currentRoom; // current room for decision-making purposes and selecting the UI elements
        private string _previousRoom; // current room for decision-making purposes and selecting the UI elements
        private string _nextRoom; // current room for decision-making purposes and selecting the UI elements
        public List<string> Rooms;
        private Button _nextRoomButton;
        private Button _previousRoomButton;

        private void InitializeStoryElements()
        {
            _characters = new Dictionary<string, Image>();


            //Room initializaton
            Rooms = new List<string>
            {
                "CabinInterior2",
                "CabinInterior1",
                "Outside",
                "Pond"
            };

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


              //Situational
              ["Act1_Pond_Arrival"] = "You arrive at the pond. Everyone is commenting on Aate’s attempt at fishing with a homemade fishing rod. Some comments are positive, some skeptical.",
              ["Act1_Aate_Intro"] = "Aate asks how you are and whether you like fishing or not.",
              ["Act1_Elina_Intro"] = "Elina hints that you might not want to go inside the cabin.",
              ["Act1_Tuomo_Intro"] = "Tuomo enters small talk with you while he is chopping wood.",
              ["Act1_Interior2_After_Pond"] = "Go away!",
              ["Act1_Interior1_After_Pond"] = "Get outta here!",
              ["Act2_Juhani_Intro"] = "Juhani greets you casually with a ‘What’s up’.",

              //Questions for Aate
              ["Player_Aate_1"] = "You tell him that you are fine.",
              ["Player_Aate_2"] = "You tell him that you like fishing.",
              ["Player_Aate_3"] = "You ask him if he is not hungry.",
              ["Player_Aate_4"] = "You ask him about Juhani and the Seven Brothers",
              ["Player_Aate_5"] = "You tell him he is running away from his father.",

              //Aate's replies
              ["Aate_reaction_1"] = "He nods",
              ["Aate_reaction_2"] = "He seems pleased and tells you a fishing story. Then he starts whistling.",
              ["Aate_reaction_3"] = "He talks to you excitedly about the hare trap he has laid in the woods.",
              ["Aate_reaction_4"] = "He tells you that the Juhani he knows is more interested in ice hockey.",
              ["Aate_reaction_5"] = "-",

              //Questions for Elina
              ["Player_Elina_1"] = "You ask her if she is not hungry.",
              ["Player_Elina_2"] = "You tell her that society is escapism.",
              ["Player_Elina_3"] = "You tell her that this is escapism.",
              ["Player_Elina_4"] = "You ask her about where the blankets came from.",
              ["Player_Elina_5"] = "You ask her if she does not want to become a writer.",

              //Elina's Reactions
              ["Elina_reaction_1"] = "She responds curtly by saying she is used to not eating.",
              ["Elina_reaction_2"] = "–",
              ["Elina_reaction_3"] = "–",
              ["Elina_reaction_4"] = "She shares with you the origin of the blankets: Tuomo robbing houses in the area.",
              ["Elina_reaction_5"] = "-",

              //Questions for Annika
              ["Player_Annika_1"] = "You thank her for the blueberries.",
              ["Player_Annika_2"] = "You make a face and say you are sick of eating nothing but berries.",
              ["Player_Annika_3"] = "You ask her why Elina is used to eating little.",

              //Annika's Reactions
              ["Annika_reaction_1"] = "She smiles sweetly.",
              ["Annika_reaction_2"] = "She raises her eyebrows with some exaggeration.",
              ["Annika_reaction_3"] = "She enters gossip mode, saying Elina is an anorectic martyr whose biggest problem is that she wants to be a writer.",

              //Questions for Juhani
              ["Player_Juhani_1"] = "You ask him if he is not hungry.",
              ["Player_Juhani_2"] = "You ask him to come speak with you in the cabin for a moment.",
              ["Player_Juhani_3"] = "You persuade him to alert the camp authorities to our location.",
              ["Player_Juhani_4"] = "You provoke him by inquiring about the hockey game he was supposed to partake in today.",

              //Juhani's reactions
              ["Juhani_reaction_1"] = "He waves it off by saying that Aate will figure everything out.",
              ["Juhani_reaction_2"] = "He feigns a casual shrug and follows you.",
              ["Juhani_reaction_2_alt"] = "He laughs your suggestion off.",
              ["Juhani_reaction_3"] = "He looks at you funny, asks you to sit down and have some blueberries.",
              ["Juhani_reaction_4"] = "He is clearly caught off guard, as he freezes and looks around himself nervously.",

              //Questions for Tuomo
              ["Player_Tuomo_1"] = "You ask him if he is not hungry.",
              ["Player_Tuomo_2"] = "You ask him to teach you how to do it.",
              ["Player_Tuomo_3"] = "You ask him to show you the way back to the camp.",
              ["Player_Tuomo_4"] = "You tell him that you think you spotted the Prodigal Son story.",
              ["Player_Tuomo_5"] = "You remind him that the cabin belongs to all of us and none of us.",

              ["Tuomo_reaction_1"] = "He divulges that he has broken into the camp’s pantry.",
              ["Tuomo_reaction_2"] = "He guffaws at your mischief but does not want you to get into trouble.",
              ["Tuomo_reaction_3"] = "He says we would likely get caught going near the camp now.",
              ["Tuomo_reaction_4"] = "He thinks you mean Aate. Unlike the Prodigal Son, he says, Aate has an abusive father.",

          };

        }

        private void InitializePhases()
        {
            /** 0. First screen
             * 1. Dialog in the dark
             * 2. Hands
             * 3. Fireplace+choice
             * 4. First Room - ACT1
             * 4.2 Control Point
             */

            _phases = new Dictionary<string, Action>
            {
                ["-1"] = () =>
                {
                    _phaseTransition = false;
                    GetTextPanel(true);
                    _gc.HideAllPanels();
                    _gc.ActiveCanvas.GetComponent<Image>().color = Color.gray;
                    //Impress.FadeOut(_mainText.gameObject);
                    _mainText.text = "Loading...";
                    _mainText.color = Color.white;
                    _mainText.fontSize = 40;
                    Impress.FadeToBlack(_gc.ActiveCanvas.GetComponent<Image>().gameObject, true);



                },
                ["0"] = () =>
                {
                    //initialization
                    _mainText = _gc.ActiveCanvas.transform.Find("GameTitle").gameObject.GetComponent<Text>();
                    _mainText.gameObject.AddComponent<TextPartial>();
                    _impressionImage = _gc.ActiveCanvas.gameObject.transform.Find("ImpressionImage").GetComponent<Image>();
                    _nextPhase = "4";
                    _phaseTransition = true;
                    AdvancePhase();

                    /*//Intro start
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
                    //*/
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
                    _phaseTransition = true;
                    _nextPhase = "2";
                    Impress.FadeOut(GetTextPanel(), true);

                },
                ["1"] = () =>
                    {

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
                        _mainText.text = "";
                        _gc.ShowControlBar(_gc.ActivePanel.gameObject);
                        _canvasBackground.sprite=FindSprite.InResources("UI_arrows");

                        _gc.ActiveCanvas.GetComponent<Image>().sprite = FindSprite.InResources("CabinInterior1");
                        //_gc.ActiveCanvas.GetComponent<Image>().color = Color.white;



                        Impress.FadeToWhite(_gc.ActiveCanvas.gameObject);
                        _nextRoom = "CabinInterior1";

                        _previousRoomButton.onClick.AddListener(() =>
                        {
                            GoToRoom("bck");
                        });


                        _nextRoomButton.onClick.AddListener(() =>
                        {
                            GoToRoom("fwd");
                        });
                        DisableClickToContinue();
                        GoToRoom("fwd");
                    },
                ["4.1"] = () =>
                {   //This is the free-movement phase in Act 1 until the player arrives at the Pond
                   
                    EnableRoomMovement();


                    if(_currentRoom != "Pond")
                    {
                        _currentPhase = "4";
                    }
                    else
                    {
                        /*we have arrived at the pond
                         *1. Initialize characters
                         *2. Move the characters to their places
                         *3. Add characters to fade in
                         *4. Disable the movement buttons
                         *5. AdvancePhase
                         */
                        var Aate = _gc.ActiveCanvas.transform.Find("StoryCharacter").GetComponent<Image>();
                        Aate.color = Color.black;

                        var Elina = Object.Instantiate(Aate, _gc.ActiveCanvas.transform, true);
                        Elina.color = Color.magenta;

                        var Juhani = Object.Instantiate(Aate, _gc.ActiveCanvas.transform, true);
                        Juhani.color = Color.white;

                        var Annika = Object.Instantiate(Aate, _gc.ActiveCanvas.transform, true);
                        Annika.color = Color.yellow;

                        var Tuomo = Object.Instantiate(Aate, _gc.ActiveCanvas.transform, true);
                        Tuomo.color = Color.cyan;

                        Aate.transform.Translate(100f, 50f, 0f);
                        Elina.transform.Translate(-100f, 40f, 0f);
                        Juhani.transform.Translate(-220f, 0f, 0f);
                        Annika.transform.Translate(210f, -30f, 0f);

                        _characters["Aate"] = Aate;
                        _characters["Annika"] = Annika;
                        _characters["Elina"] = Elina;
                        _characters["Juhani"] = Juhani;
                        _characters["Tuomo"] = Tuomo;

                        DisableRoomMovement();


                        EnableClickToContinue();

                        ControlBarDisplayText("Act1_Pond_Arrival");
                    }


                },
                ["4.2"] = () =>
                {
                    DisableClickToContinue();

                    Impress.FadeOut(_characters["Annika"]);
                    Impress.FadeOut(_characters["Elina"]);
                    Impress.FadeOut(_characters["Juhani"]);
                    Impress.FadeOut(_characters["Tuomo"]);

                    _characters["Aate"].name = "Aate_Click";
                    _characters["Aate"].gameObject.AddComponent<SaveChoice>();

                },
                ["4.3"] = () =>
                {
                    Object.Destroy(_characters["Aate"].GetComponent<SaveChoice>());

                    ShowCharacterDialogBox();
                    SnapDialogBoxNextToCharacter(_characters["Aate"]);
                    SetTextToDialogBox(_storyPrompts["Act1_Aate_Intro"]);


                    var texts = new List<string>
                    {
                        "Player_Aate_1",
                        "Player_Aate_2",
                        "Player_Aate_3",
                    };

                    ControlBarDisplayText(texts, texts);
                },
                ["4.4"] = () =>
                {
                    var prompt = "Aate_reaction_" + Choices[Choices.Count - 1].Split('_')[2];
                    SetTextToDialogBox(_storyPrompts[prompt]);
                    GetTextPanel(true);
                    
                    EnableRoomMovement();
                },
                ["4.5"] = () => 
                {

                }
            };
        }
    }
}
