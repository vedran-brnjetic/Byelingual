using System.Collections.Generic;
using Assets.StoryTemplate.Infrastructure;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class Story
    {
        private string _name;
        private string _description;
        private List<Sprite> _sprites;
        private Image _introImage;
        private Canvas _titleCanvas;
        private string _imageUrl;

        public string ImageUrl
        {
            get { return _imageUrl;}
            set {_imageUrl = value; }
        }



        public Story(string name, string description, string imageUrl)
        {
            _name = name;
            _description = description;
            _imageUrl = imageUrl;
            
        }

        public Story WithANewCanvas(string name)
        {
            _titleCanvas = A.Canvas().Named(name);
            return this;
        }

        public Story WithCanvas(Canvas canvas)
        {
            _titleCanvas = canvas;
            return this;
        }

        public Story WithImage(Image image)
        {
            _introImage = image;
            _introImage.color = Color.black;
            return this;
        }

        public Story WithSprite(Sprite sprite)
        {
            _sprites.Add(sprite);
            return this;
        }


        public string Description
        {
            get { return _description; }
        }

        public Image IntroImage
        {
            get { return _introImage; }
            set { _introImage = value; }
        }


        public override string ToString()
        {
            return _name;
        }


        public string SnakeCase()
        {
            return ToString().Replace(' ', '_').ToLower();
        }
    }
}