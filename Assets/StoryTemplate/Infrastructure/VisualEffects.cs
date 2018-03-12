using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Assets.StoryTemplate.Infrastructure
{
    public static class Impress
    {

        public static void AdvanceGame(GameController gc, bool advanceGame){
          var x = gc.ActiveCanvas.GetComponent<AdvancePhase>();
          if (advanceGame)
          {
              gc._advance = true;

              if (x) Object.Destroy(x);
          }
          else
          {

              if (!x) gc.ActiveCanvas.gameObject.AddComponent<AdvancePhase>();
          }
        }

        public static void FadeIn(GameObject ob, bool advanceGame=false)
        {
            var gc = FindGameController.Named("GameController");
            //Debug.Log(advanceGame);
            AdvanceGame(gc, advanceGame);

            gc.UIElementEffects["in"].Add(ob);
        }

        public static void FadeOut(GameObject ob, bool advanceGame=false)
        {
            var gc = FindGameController.Named("GameController");
            AdvanceGame(gc, advanceGame);

            gc.UIElementEffects["out"].Add(ob);
        }

        public static void FadeToBlack(GameObject ob, bool advanceGame = false)
        {
            var gc = FindGameController.Named("GameController");
            AdvanceGame(gc, advanceGame);

            gc.UIElementEffects["black"].Add(ob);

        }

        public static void Crossfade(GameObject ob, bool advanceGame=true)
        {
            var gc = FindGameController.Named("GameController");
            //disable click-advance
            AdvanceGame(gc, advanceGame);

            gc.UIElementEffects["cross"].Add(ob);
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

        public static void ImageFadeIn(Image image, float targetAlpha=1f, float fadeRate=0.7f)
        {
            //SetImageTransparent(image);
            image.color = Blush(image.color, targetAlpha, fadeRate);

        }

        public static void TextFadeIn(Text text, float fadeRate=0.7f)
        {
            //SetTextTransparent(text);
            //text.color = Blush(text.color, targetAlpha, fadeRate);
            

            text.text = TextRoll(text.text.Trim(), text.GetComponent<TextPartial>(), fadeRate);

        }

        private static string TextRoll(string text, TextPartial tp, float fadeRate)
        {
            var fullText = tp.FinalText;
            if (tp.CurrentText==null) tp.CurrentText = "";
            if(tp.CurrentText.Length != text.Length) text = tp.CurrentText;
            var textRoll = text;

            var currentDiff = Mathf.Abs(text.Length - fullText.Length);
            if (currentDiff > 1)
            {
                var startIndex =  Mathf.CeilToInt(Mathf.Lerp(textRoll.Length, fullText.Length, fadeRate * Time.deltaTime));
                if(startIndex<fullText.Length) textRoll = fullText.Remove(startIndex);
                

            }
            else textRoll = fullText;

            tp.CurrentText = textRoll;
            
            return textRoll.PadRight(currentDiff);

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

        public static void ImageFadeOut(Image image, Color targetColor, float fadeRate=2.5f)
        {
            image.color = Blush(image.color, targetColor, fadeRate);
        }

        private static Color Blush(Color color, Color targetColor, float fadeRate)
        {
            Color curColor = color;
            float alphaDiff = Mathf.Abs(curColor.a - targetColor.a);
            if (alphaDiff > 0.0001f)
            {
                //Lerp linear interpolation
                curColor.a = Mathf.Lerp(curColor.a, targetColor.a, fadeRate * Time.deltaTime);

            }
            alphaDiff = Mathf.Abs(curColor.b - targetColor.b);
            if (alphaDiff > 0.0001f)
            {
                //Lerp linear interpolation
                curColor.b = Mathf.Lerp(curColor.b, targetColor.b, fadeRate * Time.deltaTime);

            }
            alphaDiff = Mathf.Abs(curColor.r - targetColor.r);
            if (alphaDiff > 0.0001f)
            {
                //Lerp linear interpolation
                curColor.r = Mathf.Lerp(curColor.r, targetColor.r, fadeRate * Time.deltaTime);

            }
            alphaDiff = Mathf.Abs(curColor.g - targetColor.g);
            if (alphaDiff > 0.0001f)
            {
                //Lerp linear interpolation
                curColor.g = Mathf.Lerp(curColor.g, targetColor.g, fadeRate * Time.deltaTime);

            }

            return curColor;
        }
    }
}
