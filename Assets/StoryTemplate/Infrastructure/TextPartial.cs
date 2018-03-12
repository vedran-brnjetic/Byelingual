using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.StoryTemplate.Infrastructure
{
    public class TextPartial : MonoBehaviour
    {
        public string FinalText;
        public string CurrentText;

        public TextPartial(string finalText)
        {
            FinalText = finalText;
        }

        public void UpdateCurrentText(string text)
        {
            CurrentText = text;
        }
    }
}
