using Assets.Scripts.Common.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryViewer : SingletonMonoBehaviour<HistoryViewer>
{

    public GameObject index_text;
    public GameObject view_text;
    public GameObject count_text;
    public TextAsset[] story;

    public bool[] bStoryOpen;

    private int storyIndex;
    private int lastOpened;
    private int openedCount;

    void Awake()
    {
        bStoryOpen = new bool[story.Length];
        openedCount = story.Length;
        lastOpened = -1;
        count_text.GetComponent<Text>().text = "0";
        gameObject.SetActive(false);
    }

    public void openHistory()
    {
        if (lastOpened < 0) return;

        storyIndex = lastOpened;
        setText(storyIndex);
        gameObject.SetActive(true);
    }

    public void closeHistory()
    {
        gameObject.SetActive(false);
    }

    public void nextHistory()
    {

        setText(findOpen(storyIndex + 1));
    }

    public void prevHistory()
    {
        setText(findOpen(storyIndex - 1));
    }

    private void setText(int _index)
    {
        if (_index < story.Length && _index >= 0)
        {
            storyIndex = _index;
            lastOpened = storyIndex;
            view_text.GetComponent<Text>().text = story[storyIndex].text;
            index_text.GetComponent<Text>().text = storyIndex.ToString() + "/" + openedCount.ToString();
        }
    }

    int findOpen(int _index)
    {
        int _res = storyIndex;
        if (_index < story.Length && _index >= 0)
        {
            if (bStoryOpen[_index])
            {
                _res = _index;
            }
            else
            {
                for(int i = _index; ; i += _index - storyIndex)
                {
                    if (i >= bStoryOpen.Length || i < 0) break;
                    if(bStoryOpen[ i ])
                    {
                        _res = i;
                        break;
                    }
                }
            }
        }

        return _res;
    }

    public void unlockStory(int _index)
    {
        if(_index < story.Length && _index >= 0)
        {
            bStoryOpen[_index] = true;
            openedCount += 1;
            count_text.GetComponent<Text>().text = openedCount.ToString();
        }
    }
}
