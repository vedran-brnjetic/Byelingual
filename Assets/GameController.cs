using Assets.StoryTemplate;
using Assets.StoryTemplate.Infrastructure;
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class GameController : MonoBehaviour
    {
        private bool _internetWorking = false;


        private Dictionary<string, Story> _stories; //list of stories available in the platform
        private bool _init = true; //flag for async methods that run on first update frame
        private Story _currentStory; //currently active story
        private Dictionary<string, Canvas> _canvases; //list of canvases to loop through when disabling them
        private List<GameObject> _panels; //list of game panels
        public Dictionary<string, List<GameObject>> UIElementEffects; //list of elements for visual transition
        private List<string> _effectMode;
        public GameObject ActivePanel; //currently active control panel
        public Canvas ActiveCanvas;
        public Canvas PreviousCanvas;

        private StoryTemplate.CabinInTheWoods _cabinInTheWoods;
        public bool _advance=false;

        //Current story property
        public Story CurrentStory
        {
            get { return _currentStory; }
            set { _currentStory = value; }
        }

        //Stories property - returns the list of locally available stories
        public Dictionary<string, Story> Stories
        {
            get { return _stories; }
        }

        public StoryTemplate.CabinInTheWoods CabinInTheWoods
        {
            get { return _cabinInTheWoods; }
        }


        // Use this for initialization
        private void Start()
        {
            //Initialazing lists
            _canvases = new Dictionary<string, Canvas>();
            _stories = new Dictionary<string, Story>();
            _panels = new List<GameObject>();

            //initialize effect modes
            _effectMode = new List<string> {"cross","in","out","black", "white"};
            UIElementEffects = new Dictionary<string, List<GameObject>>();

            foreach (var mode in _effectMode)
            {
                UIElementEffects[mode] = new List<GameObject>();
            }

            //add panels to the Panels list
            FillPanels();

            //show the main menu control bar
            ShowControlBar(FindPanel.GO("ControlBar"));

            //get stories from internet - needed when online
            _stories = BLResources.GetStoriesFromInternet();
            if (_stories.Count < 2) {
                _stories.Add("cabin_in_the_woods",new Story("Cabin In The Woods","A story by Sontra Samela", ""));
                _stories.Add("yojijukugo:_a_play_in_four_characters", new Story("Yojijukugo:A Play in Four Characters","A story by Sontra Samela", ""));
            }
            else
            {
                _internetWorking = true;
            }
            // add ExitGame callback to ExitButton listener
            FindButton.Named("ExitButton").onClick.AddListener(ExitGame);
            FindButton.Named("QuitButton").onClick.AddListener(ExitGame);

            FindButton.Named("MainMenuButton").onClick.AddListener(BackToMainMenu);
            FindButton.Named("SaveButton").onClick.AddListener(SaveGame);
            FindButton.Named("LoadButton").onClick.AddListener(LoadGame);

            FindButton.Named("MenuButton").onClick.AddListener(()=>{
                EnableCanvasByName("PauseCanvas");
            });
            FindButton.Named("ReturnButton").onClick.AddListener(()=>{
                EnableCanvas(PreviousCanvas);
            });
            //Testing text transition (fade in)
            var text = FindText.Named("TextGameTitle");
            //text.gameObject.AddComponent<TextPartial>();
            //text.GetComponent<TextPartial>().FinalText = "Byelingual";
            text.text = "Byelingual";
            VisualEffects.SetTextTransparent(text);
            UIElementEffects["in"].Add(text.gameObject);



            //Canvas initialization
            var mainMenuCanvas = FindCanvas.Named("MainMenuCanvas");

            mainMenuCanvas.transform.SetAsLastSibling();
            _canvases["mainMenuCanvas"] = mainMenuCanvas;



            foreach (var story in Stories.Values)
            {
                var cnv = Instantiate(FindCanvas.Named("StoryCanvas"));
                cnv.name = story.SnakeCase() + "_canvas";
                _canvases[story.SnakeCase()] = cnv;

            }






            /*Button initialization
            _exitButton = GameObject.Find("btnExit").GetComponent<Button>();


            //Assigning Methods to Unity actions
            _exit += ExitGame;


            //Assigning Unity actions to button Events
            _exitButton.onClick.AddListener(_exit);
            */

        }

        private void FillPanels()
        {
            _panels.Add(FindPanel.GO("ControlBar"));
            _panels.Add(FindPanel.GO("ControlBarText"));
            _panels.Add(FindPanel.GO("ControlBarImage"));
            _panels.Add(FindPanel.GO("ControlBarImageDragDrop"));
        }

        public void BackToMainMenu()
        {
            DisableAllCanvases();
            var canvas = FindCanvas.Named("MainMenuCanvas");
            EnableCanvas(canvas);
            var panel = FindPanel.GO("ControlBar");
            panel.transform.SetParent(canvas.transform);
            ShowControlBar(panel, Color.grey);
            Destroy(FindButton.Named("BackButton").gameObject);
        }




        // Update is called once per frame
        private void Update()
        {

            if (_init)
            {
                EnableCanvasByName("MainMenuCanvas");
                if(_internetWorking) LoadButtons(_internetWorking);
                else LoadButtons();

                _init = false;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
                EnableCanvas(PreviousCanvas);

            CrossFadeElements();

        }

        private void LoadButtons()
        {
            if (Stories.Count > 0) //a hack, have to refactor at some point
            {
                var a = FindSprite.InResources("Byelingual1");
                var b = FindSprite.InResources("YojijukugoTitleScreen");

                var ImageStory1 = FindImage.Named("ImageStory1");
                ImageStory1.name = Stories.Values.ElementAt(0).SnakeCase();
                ImageStory1.sprite = a;
                var ImageStory2 = FindImage.Named("ImageStory2");
                ImageStory2.name = Stories.Values.ElementAt(1).SnakeCase();
                ImageStory2.sprite = b;

                Impress.FadeIn(ImageStory1.gameObject);
                Impress.FadeIn(ImageStory2.gameObject);

                var x = true;
                foreach (var story in Stories.Values)
                {
                    if (x)
                    {

                        _cabinInTheWoods = new StoryTemplate.CabinInTheWoods(story.ToString(), story.Description, story.ImageUrl);
                        x = false;
                    }

                    FindImage.Named(story.SnakeCase()).gameObject.AddComponent<LaunchGame>();
                }

            }
        }

        /*// LoadButtons() async from internet.*/
        private async void LoadButtons(object s)
        {

            if (Stories.Count > 0) //a hack, have to refactor at some point
            {
                var a = IMG2Sprite.Instance(Stories.Values.ElementAt(0).SnakeCase() + "spriter");
                var b = IMG2Sprite.Instance(Stories.Values.ElementAt(1).SnakeCase() + "spriter");

                var ImageStory1 = FindImage.Named("ImageStory1");
                ImageStory1.sprite = await a.LoadNewSprite(Stories.Values.ElementAt(0).ImageUrl);
                ImageStory1.name = Stories.Values.ElementAt(0).SnakeCase();
                VisualEffects.SetImageTransparent(ImageStory1);
                UIElementEffects["in"].Add(ImageStory1.gameObject);



                var ImageStory2 = FindImage.Named("ImageStory2");
                ImageStory2.sprite = await b.LoadNewSprite(Stories.Values.ElementAt(1).ImageUrl);
                ImageStory2.name = Stories.Values.ElementAt(1).SnakeCase();
                VisualEffects.SetImageTransparent(ImageStory2);
                UIElementEffects["in"].Add(ImageStory2.gameObject);

                var x = true;
                foreach (var story in Stories.Values)
                {
                    if (x)
                    {

                        _cabinInTheWoods = new StoryTemplate.CabinInTheWoods(story.ToString(), story.Description, story.ImageUrl);
                        x = false;
                    }

                    FindImage.Named(story.SnakeCase()).gameObject.AddComponent<LaunchGame>();
                }

            }


        }
        //*/

        private void CrossFadeElements()
        {
            //container of items to remove from crossfade list once the item completes the transition


            var itemsToRemove = new Dictionary<string, List<GameObject>>();
            foreach (var mode in _effectMode)
            {
                itemsToRemove[mode] = new List<GameObject>();
            }



            //go through the crossfade lists
            var transitionComplete = false;
            var transtionsDirty = "";
            foreach (var mode in _effectMode)
            {
                var targetAlpha = 0.0f;
                if (mode == "in" || mode == "black" || mode == "white") targetAlpha = 1.0f;


                foreach (var element in UIElementEffects[mode])
                {
                    //try to get an image from the gameObject element
                    //if there is no image, it will be null, therefore false in the if statement
                    var image = element.GetComponent<Image>();
                    //try to get a text from the gameObject element
                    var text = element.GetComponent<Text>();

                    //test if the element is image
                    if (image)
                    {
                        switch (mode)
                        {
                                case "in":
                                    VisualEffects.ImageFadeIn(image);
                                    break;
                                case "out":
                                    VisualEffects.ImageFadeOut(image);
                                    break;
                                case "cross":
                                    VisualEffects.ImageFadeOut(image);
                                    break;
                                case "black":
                                    VisualEffects.ImageFadeOut(image, Color.black);
                                    break;
                                case "white":
                                    VisualEffects.ImageFadeOut(image, Color.white);
                                    break;
                        }

                        if (mode == "white")
                        {
                            if (Math.Abs(image.color.a - 1) > 0.0001 ||
                                Math.Abs(image.color.r - 1) > 0.0001 ||
                                Math.Abs(image.color.g - 1) > 0.0001 ||
                                Math.Abs(image.color.b - 1) > 0.0001)
                            {
                                transtionsDirty += " white ";
                                continue;

                            }
                        }
                        if (mode == "black")
                        {
                            if (Math.Abs(image.color.a - 1) > 0.0001 ||
                                Math.Abs(image.color.r - 0) > 0.0001 ||
                                Math.Abs(image.color.g - 0) > 0.0001 ||
                                Math.Abs(image.color.b - 0) > 0.0001)
                            {
                                transtionsDirty += " black ";
                                continue;

                            }
                        }
                        if (Math.Abs(image.color.a - targetAlpha) > 0.0001)
                        {
                            transtionsDirty += " alpha ";
                            continue;
                        }

                        if (mode == "cross") Impress.FadeIn(element);

                        itemsToRemove[mode].Add(element);
                        //transitionComplete = true;
                    } //test if the element is text
                    else if (text)
                    {
                        switch (mode)
                        {
                            case "in":
                                VisualEffects.TextFadeIn(text);
                                break;
                            case "out":
                                VisualEffects.TextFadeOut(text);
                                break;
                            case "cross":
                                VisualEffects.TextFadeOut(text);
                                break;
                        }

                        if (mode == "in")
                        {
                            if(UIElementEffects["in"].Contains(element))
                                if (Math.Abs(text.color.a - targetAlpha) > 0.0001)
                                {
                                    transtionsDirty += " txt_alpha ";
                                    continue;
                                }
                        }
                        else
                        {
                            if (Math.Abs(text.color.a - targetAlpha) > 0.0001)
                            {
                                transtionsDirty += " txt_alpha ";

                                continue;
                            }

                        }

                        itemsToRemove[mode].Add(element);
                        if(mode=="cross") Impress.FadeIn(element);

                        //transitionComplete = true;

                    }


                }
            }

            if (string.IsNullOrEmpty(transtionsDirty))
            {
                transitionComplete = true;
            }

            //cleanup the elements which completed transition
            foreach (var mode in _effectMode)
            {
                foreach (var item in itemsToRemove[mode])
                {
                    UIElementEffects[mode].Remove(item);

                }
            }

            if (transitionComplete)
                Advance();

        }

        private void Advance()
        {
            if (!_advance) return;
            _advance = false;
            _cabinInTheWoods.AdvancePhase();

            var ap = ActiveCanvas.GetComponent<AdvancePhase>();
            if (!ap) ActiveCanvas.gameObject.AddComponent<AdvancePhase>();
        }

        private static void ExitGame()
        {
            Debug.Log("Exiting game");
            Application.Quit();
        }



        public void DisableAllCanvases()
        {
            foreach (var canvas in _canvases.Values)
            {
                canvas.enabled = false;
            }
        }

        public void HideAllPanels()
        {
            foreach (var panel in _panels)
            {
                panel.transform.SetAsFirstSibling();
                //Debug.Log(panel.name + " " + panel.transform.position );
                if(panel.transform.position.y > 0)
                    panel.transform.Translate(0f, -panel.GetComponentInChildren<RectTransform>().rect.height*4f, 0f);
            }
        }

        public void ShowControlBar(GameObject panel)
        {
            ShowControlBar(panel, Color.white);

        }

        public void ShowControlBar(GameObject panel, Color color)
        {
            HideAllPanels();
            ActivePanel = panel;
            panel.transform.SetAsLastSibling();
            panel.transform.GetComponent<Image>().color = color;
            panel.transform.Translate(0f, panel.GetComponentInChildren<RectTransform>().rect.height*4f, 0f);
        }

        public void EnableCanvasByName(string canvasName)
        {
            EnableCanvas(FindCanvas.Named(canvasName));
        }

        public void EnableCanvas(Canvas canvas)
        {
            PreviousCanvas = ActiveCanvas;
            DisableAllCanvases();
            canvas.enabled = true;
            ActiveCanvas = canvas;
        }

        public void DelayLoad(int i)
        {
            System.Threading.Thread.Sleep(i * 1000);
        }

        public void SaveGame()
        {
            var savegame = JsonUtility.ToJson(CabinInTheWoods);
            TextSave.WriteString(savegame);
        }

        public void LoadGame()
        {
            var savegame = TextSave.ReadString();
            var json = JSON.Parse(savegame);
            var count = json["Choices"].Count;
            if (count <= 0) return;
            for (var i = 0; i < count; i++)
            {
                CabinInTheWoods.Choices.Add(json["Choices"][i]);
                Debug.Log(json["Choices"][i]);
            }

        }
    }
}
