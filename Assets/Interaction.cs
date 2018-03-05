using Assets.StoryTemplate.Infrastructure;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction
{
    private string _name;
    private string _interactionType;
    private Image _interactionBackground;
    private List<string> _dialogList;
    private List<Button> _controlButtons;
    private Interaction _nextInteraction;
    private int _timeUnitCost;
    private List<GameObject> _objects;
    private Canvas _canvas;

    public void PutButtonsIntoControlBar() { }

    public void GrabTheControlBar()
    {
        var panel = FindPanel.Named("ControlBar");
        panel.transform.SetParent(_canvas.transform);
        panel.transform.SetAsLastSibling();
        
    }
}