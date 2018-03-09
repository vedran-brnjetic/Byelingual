using UnityEngine;
using UnityEngine.UI;

namespace Assets.StoryTemplate.Infrastructure
{
    public static class Impress
    {
        public static void WithImage(Image image)
        {

        }

        public static void WithText(Text text)
        {

        }
    }

    public static class VisualEffects
    {
        public static float GetDimension(char dimension, GameObject gameObject)
        {
            var rect = gameObject.GetComponent<RectTransform>().rect;
            switch (dimension)
            {
                case 'x':
                    return rect.width;
                case 'y':
                    return rect.height;
                default:
                    return rect.width;
            }
        }

        public static Color Blush(Color color, float targetAlpha, float fadeRate)
        {
            Color curColor = color;
            float alphaDiff = Mathf.Abs(curColor.a - targetAlpha);
            if (alphaDiff > 0.0001f)
            {
                //Lerp linear interpolation
                curColor.a = Mathf.Lerp(curColor.a, targetAlpha, fadeRate * Time.deltaTime);
                
            }

            return curColor;
        }

        public static void ImageFadeIn(Image image, float targetAlpha=1f, float fadeRate=2.5f)
        {
            //SetImageTransparent(image);
            image.color = Blush(image.color, targetAlpha, fadeRate);
            
        }

        public static void TextFadeIn(Text text, float targetAlpha=1f, float fadeRate=2.5f)
        {
            //SetTextTransparent(text);
            text.color = Blush(text.color, targetAlpha, fadeRate);

        }


        public static void ImageFadeOut(Image image, float targetAlpha = 0f, float fadeRate = 2.5f)
        {
            
            image.color = Blush(image.color, targetAlpha, fadeRate);

        }

        public static void TextFadeOut(Text text, float targetAlpha = 0f, float fadeRate = 2.5f)
        {
            
            text.color = Blush(text.color, targetAlpha, fadeRate);

        }


        public static void SetImageTransparent(Image image)
        {
            var curColour = image.color;
            curColour.a = 0f;
            image.color = curColour;
        }

        public static void SetTextTransparent(Text text)
        {
            var curColour = text.color;
            curColour.a = 0f;
            text.color = curColour;
        }
    }
}
