using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class Stories : PropertyChanger
    {
        List<StoryPart> stories = new List<StoryPart>();

        public bool TryAddStory(StoryPart story)
        {
            if (!story)
                return false;

            if (!findInStories(story))
            {
                stories.Add(story);
                Debug.LogFormat("Story '{0}' added to stories", story.index);
                Destroy(story.gameObject);
            }
            OnPropertyChanged("stories");
            return true;
        }

        public List<StoryPart> GetStories()
        {
            return new List<StoryPart>(stories);
        }

        public void RemoveStory(StoryPart story)
        {
            stories.Remove(story);
            OnPropertyChanged("stories");
        }

        bool findInStories(StoryPart story)
        {
            bool _res = false;

            if (stories.Count == 0) return _res;

            foreach (StoryPart _story in stories)
            {
                if (_story.index == story.index)
                {
                    _res = true;
                    break;
                }
                
            }
            return _res;
        }
    }
}
