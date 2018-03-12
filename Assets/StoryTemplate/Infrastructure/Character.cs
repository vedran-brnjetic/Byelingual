using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.StoryTemplate.Infrastructure
{
    public class Character
    {
        private string _name;
        private string _imageSpriteName;
        private Dictionary<string, string> _queries;
        private Dictionary<string, string> _reactions;
        private Dictionary<string, string> _queryConditions;

        public Character(string name, string imageSpriteName)
        {
            _name = name;
            _imageSpriteName = imageSpriteName;
            _queries = new Dictionary<string, string>();
            _reactions = new Dictionary<string, string>();
            _queryConditions = new Dictionary<string, string>();
        }

        public Character(string name, string imageSpriteName, Dictionary<string, string> queries, Dictionary<string, string> reactions, Dictionary<string, string> queryConditions)
        {
            _name = name;
            _imageSpriteName = imageSpriteName;
            _queries = queries;
            _reactions = reactions;
            _queryConditions = queryConditions;
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public string ImageSpriteName
        {
            get
            {
                return _imageSpriteName;
            }

            set
            {
                _imageSpriteName = value;
            }
        }

        public Dictionary<string, string> Queries
        {
            get
            {
                return _queries;
            }

            set
            {
                _queries = value;
            }
        }

        public Dictionary<string, string> Reactions
        {
            get
            {
                return _reactions;
            }

            set
            {
                _reactions = value;
            }
        }

        public Dictionary<string, string> QueryConditions
        {
            get
            {
                return _queryConditions;
            }

            set
            {
                _queryConditions = value;
            }
        }

        public string GetReaction(string query)
        {
            return _reactions[query];
        }

        public List<string> GetAvailableQueries(List<string> choices)
        {
            var queries = new List<string>();
            foreach (var choice in choices)
            {   
                if(!string.IsNullOrEmpty(_queryConditions[choice]))
                queries.Add(_queries[_queryConditions[choice]]);
            }

            return queries;
        }

        public Sprite GetSprite()
        {
            return FindSprite.InResources(_imageSpriteName);
        }
    }
}
