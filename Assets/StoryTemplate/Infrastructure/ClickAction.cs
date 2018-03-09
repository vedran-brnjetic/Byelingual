using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.StoryTemplate.Infrastructure
{
    public class ClickAction : MonoBehaviour, IPointerClickHandler
    {

        public virtual void OnPointerClick(PointerEventData eventData)
        {
           
        }
    }

    public class SomeAction : ClickAction
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            //#CODE EXAMPLE
            //write your code here
            
            //this allows you to control the active game controller by accessing its public methods and properties
            var GameController = FindGameController.Named("GameController");


        }
    }

    public class LaunchGame : ClickAction
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            ////TODO: analyse the code to see what can be extracted and reused in other places

            //take control of the game controller
            var gc = FindGameController.Named("GameController");

            //find the current game canvas
            var gameCanvas = FindCanvas.Named(gameObject.name + "_canvas");
            
            //bring game canvas to front
            gc.EnableCanvas(gameCanvas);
            
            //set game controller's current story to the correct story from the Dictionary
            gc.CurrentStory = gc.Stories[gameObject.name];
            //and assign it to a local variable just for more readable code
            var currentStory = gc.CurrentStory;

            //Find GameTitle text in Game canvas, and set it to the currentStory name
            gameCanvas.transform.Find("GameTitle").GetComponent<Text>().text = currentStory.ToString();

            //set the canvas background image color to black
            gameCanvas.transform.GetComponent<Image>().color=Color.black;
            


            //move the ControlBar to the game canvas
            var panel = FindPanel.GO("ControlBar");
            
            //change panel transparency (and color) - rgba(0-1f, 0-1f, 0-1f, 0-1f)
            panel.transform.GetComponent<Image>().color = new Color(0f,0f,0f,0f);
            panel.transform.SetParent(gameCanvas.transform);
            panel.transform.SetAsLastSibling();

            //copy the exit button, place it on the panel, rename and relabel to create a "Back button" - this functionality we'll move to the game menu
            var backButton = Instantiate(panel.GetComponentInChildren<Button>(), panel.transform, true);
            backButton.name = "BackButton";
            backButton.transform.SetParent(panel.transform);
            backButton.gameObject.GetComponentInChildren<Text>().text = "Back";
            
            //move button a bit to the left
            backButton.transform.Translate(-1.5f * VisualEffects.GetDimension('x', backButton.gameObject), 0f, 0f);
            
            //apply the game controller action to the back button
            backButton.onClick.AddListener(gc.BackToMainMenu);
            ///////////////////////////Save Button
            var saveButton = Instantiate(panel.GetComponentInChildren<Button>(), panel.transform, true);
            saveButton.name = "SaveButton";
            saveButton.transform.SetParent(panel.transform);
            saveButton.gameObject.GetComponentInChildren<Text>().text = "Save";
            
            //move button a bit to the left
            saveButton.transform.Translate(-3f * VisualEffects.GetDimension('x',saveButton.gameObject),0f,0f);
            
            //apply the game controller action to the back button
            saveButton.onClick.AddListener(gc.SaveGame);


            ///////////////////////////Load Button
            var loadButton = Instantiate(panel.GetComponentInChildren<Button>(), panel.transform, true);
            loadButton.name = "LoadButton";
            loadButton.transform.SetParent(panel.transform);
            loadButton.gameObject.GetComponentInChildren<Text>().text = "Load";

            //move button a bit to the left
            loadButton.transform.Translate(-4.5f * VisualEffects.GetDimension('x', loadButton.gameObject), 0f, 0f);

            //apply the game controller action to the back button
            loadButton.onClick.AddListener(gc.LoadGame);

            if (gc.CurrentStory.SnakeCase() == "cabin_in_the_woods")
            {
                gc.CabinInTheWoods.PlayIntro(0);
            }
            
        }
    }

    public class SaveChoice : ClickAction
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            var gc = FindGameController.Named("GameController");

            //game controller / cabininthewoods / List<string> choices
            gc.CabinInTheWoods.ProcessChoice(gameObject.name);
            

        }

    }
   

}