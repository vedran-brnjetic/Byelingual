using System.Collections;
using Assets.StoryTemplate.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class GameController : MonoBehaviour
    {
        
        private Dictionary<string, Story> _stories;
        private Dictionary<string, Canvas> _canvases;
        private Dictionary<string, Image> _storyLaunchers;
        private bool _init = true;
        private Story _currentStory;
        private List<GameObject> _panels;


        public Story CurrentStory
        {
            get { return _currentStory; }
            set { _currentStory = value; }
        }

        public Dictionary<string, Story> Stories
        {
            get { return _stories; }
        }


        // Use this for initialization
        private void Start()
        {
            _canvases = new Dictionary<string, Canvas>();
            _stories = new Dictionary<string, Story>();

            _panels = new List<GameObject>();

            FillPanels();
            
            ShowPanel(FindPanel.GO("ControlBar"));

            _stories = Resources.GetStoriesFromInternet();

            FindButton.Named("ExitButton").onClick.AddListener(ExitGame);
            

            //Canvas initialization
            var mainMenuCanvas = FindCanvas.Named("MainMenuCanvas");
            
            mainMenuCanvas.transform.SetAsLastSibling();
            _canvases["mainMenuCanvas"]=mainMenuCanvas;

            

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
            var mm = FindCanvas.Named("MainMenuCanvas");
            EnableCanvas(mm);
            var panel = FindPanel.GO("ControlBar");
            panel.transform.SetParent(mm.transform);
            ShowPanel(panel,Color.grey);
            Destroy(FindButton.Named("BackButton").gameObject);
        }

        private async void LoadButtons()
        {
            
            if (Stories.Count > 0) //a hack, have to refactor at some point
            {
                var a = IMG2Sprite.Instance(Stories.Values.ElementAt(0).SnakeCase() + "spriter");
                var b = IMG2Sprite.Instance(Stories.Values.ElementAt(1).SnakeCase() + "spriter");
                FindImage.Named("ImageStory1").sprite = await a.LoadNewSprite(Stories.Values.ElementAt(0).ImageUrl);
                FindImage.Named("ImageStory1").name = Stories.Values.ElementAt(0).SnakeCase();

                FindImage.Named("ImageStory2").sprite = await b.LoadNewSprite(Stories.Values.ElementAt(1).ImageUrl);
                FindImage.Named("ImageStory2").name = Stories.Values.ElementAt(1).SnakeCase();

                foreach (var story in Stories.Values)
                {

                    FindImage.Named(story.SnakeCase()).gameObject.AddComponent<LaunchGame>();
                }

            }

           
        }

        // Update is called once per frame
        private void Update()
        {

            if (_init)
            {
                EnableCanvas(FindCanvas.Named("MainMenuCanvas"));
                LoadButtons();
                _init = false;
            }
            
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
            }
        }

        public void ShowPanel(GameObject panel)
        {
           ShowPanel(panel, Color.black);
        }

        public void ShowPanel(GameObject panel, Color color)
        {
            HideAllPanels();
            panel.transform.SetAsLastSibling();
            panel.transform.GetComponent<Image>().color = color;
        }

        public void EnableCanvasByName(string name)
        {
            EnableCanvas(FindCanvas.Named(name));
        }

        public void EnableCanvas(Canvas canvas)
        {
            DisableAllCanvases();
            canvas.enabled = true;
        }
    }
}