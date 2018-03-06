﻿using System.Linq;
using Assets.StoryTemplate.Infrastructure;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class CabinInTheWoods : Story
    {
        private Sprite _handsSprite;
        private Sprite _fireSprite;
        private GameController _gc;
        private Canvas _canvas;

        public CabinInTheWoods(string name, string description, string imageUrl) : base(name, description, imageUrl)
        {
            _gc = FindGameController.Named("GameController");
            _handsSprite = FindSprite.InResources("placeholder_hands");
            _fireSprite = FindSprite.InResources("placeholder_wood-burning_stove");
            _canvas = FindCanvas.Named(_gc.Stories.Values.ElementAt(0).SnakeCase() + "_canvas");

        }

        public void PlayIntro()
        {
            var imagePanel = FindPanel.GO("ControlBarImage");
            imagePanel.transform.SetParent(_canvas.transform);
            _gc.ShowPanel(imagePanel);

            var image1 = FindImage.Named("Image1");
            var image2 = FindImage.Named("Image2");


            image1.sprite = _handsSprite;
            image2.sprite = _fireSprite;

            VisualEffects.SetImageTransparent(image1);
            VisualEffects.SetImageTransparent(image2);

            _gc.ElementsToCrossfade.Add(image1.gameObject);
            _gc.ElementsToCrossfade.Add(image2.gameObject);

        }
    }
}
