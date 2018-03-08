using UnityEngine;
using UnityEngine.UI;

namespace Assets.StoryTemplate.Infrastructure
{
    public class ButtonBuilder : ComponentBuilder<Button>
    {
    

   
        public override Button Build()
        {

            var button = new GameObject().AddComponent<Button>();
        
            button.name = _name;
            return button;

        }
    }
}
