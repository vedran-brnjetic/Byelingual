using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.StoryTemplate.Infrastructure
{
    public class IMG2Sprite : MonoBehaviour
    {
        // This script loads a PNG or JPEG image from disk and returns it as a Sprite
        // Drop it on any GameObject/Camera in your scene (singleton implementation)
        //
        // Usage from any other script:
        // MySprite = IMG2Sprite.Instance.LoadNewSprite(mediaUrl, [PixelsPerUnit (optional)])

        private static IMG2Sprite _instance;

        public static IMG2Sprite Instance(string name)
        {
            
                //If _instance hasn't been set yet, we grab it from the scene!
                //This will only happen the first time this reference is used.

                if (_instance == null)
                    _instance = new GameObject().AddComponent<IMG2Sprite>();
            _instance.name = name.Replace(' ','_').ToLower();
                return _instance;
            
        }

        public async Task<Sprite> LoadNewSprite(string mediaUrl, float PixelsPerUnit = 100.0f)
        {
            
            // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference

            Sprite NewSprite = new Sprite();
            var SpriteTexture = await LoadTexture(mediaUrl);
            NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit);

            return NewSprite;
        }

        public static async Task<Texture2D> LoadTexture(string url)
        {

            // Load a PNG or JPG file from disk to a Texture2D
            // Returns null if load fails

            var Tex2D = new Texture2D(2,2);
            var ms = new MemoryStream();

            
            {
                var bytes = await Resources.GetBitmapFromURL(url);
                

                
                if (Tex2D.LoadImage(bytes))           // Load the imagedata into the texture (size is set automatically)
                    return Tex2D;                 // If data = readable -> return texture
            }
            return null;                     // Return null if load failed
        }
    }
}
