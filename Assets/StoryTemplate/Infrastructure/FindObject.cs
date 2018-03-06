using UnityEngine;
using UnityEngine.UI;

namespace Assets.StoryTemplate.Infrastructure
{
    public abstract class FindObject<T>
    {
        public static T Named(string name)
        {
            var obj = GameObject.Find(name).GetComponent<T>();
            return obj;
        }

        public static GameObject GO(string name)
        {
            var obj = GameObject.Find(name);
            return obj;
        }

        
    }

    public class FindCanvas : FindObject<Canvas>
    {

    }

    public class FindCamera : FindObject<Camera>
    {

    }

    public class FindSprite : FindObject<Sprite>
    {
        public static Sprite InResources(string sprite)
        {
            return UnityEngine.Resources.Load<Sprite>("CabinInTheWoods/Images/" + sprite);
        }
    }

    public class FindImage : FindObject<Image>
    {

    }

    public class FindButton : FindObject<Button>
    {

    }

    public class FindGameController : FindObject<GameController>
    {

    }

    public class FindPanel : FindObject<GameObject>
    {
        //for semantic purposes
    }

    public class FindText : FindObject<Text>
    {

    }
}
