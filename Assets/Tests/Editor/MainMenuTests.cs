using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Tests
{
    public class MainMenuTests
    {
        private static Button GetButton(string btnName)
        {
            return GameObject.Find(btnName).GetComponent<Button>();
        }

        [Test]
        public void Exit_Button_Present()
        {
            var btnName = "ExitButton";
            var testButton = GetButton(btnName);

            Assert.IsNotNull(testButton);
        }

        

    }
}