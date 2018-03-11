using System;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Assets.StoryTemplate.Infrastructure
{
    public static class Impress
    {
        
        public static void FadeIn(GameObject ob, bool advanceGame=false)
        {
            var gc = FindGameController.Named("GameController");
            //Debug.Log(advanceGame);
            if (advanceGame)
            {
                gc._advance = true;
                var x = gc.ActiveCanvas.GetComponent<AdvancePhase>();
                if (x) Object.Destroy(x);
            }

            gc.ElementsToCrossfade["in"].Add(ob);
        }

        public static void FadeOut(GameObject ob, bool advance=false)
        {
            var gc = FindGameController.Named("GameController");
            if (advance)
            {
                gc._advance = true;
                var x = gc.ActiveCanvas.GetComponent<AdvancePhase>();
                if (x) Object.Destroy(x);
            }
            gc.ElementsToCrossfade["out"].Add(ob);
        }

        
        public static void Crossfade(GameObject ob)
        {
            var gc = FindGameController.Named("GameController");
            //disable click-advance
            var x = gc.ActiveCanvas.GetComponent<AdvancePhase>();
            if (x) Object.Destroy(x);

            gc._advance = true;
            gc.ElementsToCrossfade["cross"].Add(ob);
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

        public static void ImageFadeIn(Image image, float targetAlpha=1f, float fadeRate=2f)
        {
            //SetImageTransparent(image);
            image.color = Blush(image.color, targetAlpha, fadeRate);
            
        }

        public static void TextFadeIn(Text text, float fadeRate=0.03f)
        {
            //SetTextTransparent(text);
            //text.color = Blush(text.color, targetAlpha, fadeRate);
            text.text = TextRoll(text.text, text.GetComponent<TextPartial>().FinalText, fadeRate);

        }

        private static string TextRoll(string text, string fullText, float fadeRate)
        {
            var Text = text;
            
            var currentDiff = Mathf.Abs(text.Length - fullText.Length);
            if (currentDiff > 1)
            {
                var startIndex =  Mathf.CeilToInt(Mathf.Lerp(Text.Length, fullText.Length, fadeRate * Time.deltaTime));
                if(startIndex<fullText.Length) Text = fullText.Remove(startIndex);
                Debug.Log(startIndex);
            }
            else Text = fullText;

            return Text;

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
