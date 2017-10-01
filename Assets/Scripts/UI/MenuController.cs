using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	public Text tutorialText;
	public Animation buttonAnimation;
	public TutorialString[] tutorialStrings;

	[Serializable]
	public struct TutorialString
	{
		public string text;
		public float delay;
	}

	private void Awake()
	{
		buttonAnimation.gameObject.SetActive(true);
		tutorialText.gameObject.SetActive(false);
	}

	public void OnGameStart()
	{
		buttonAnimation.gameObject.SetActive(false);
		tutorialText.gameObject.SetActive(true);

		StartCoroutine(StartTutorial());
	}

	IEnumerator StartTutorial()
	{
		foreach (var data in tutorialStrings)
		{
			tutorialText.text = data.text;
			yield return new WaitForSeconds(data.delay);
		}

		StartGame();
	}

	void StartGame()
	{
		SceneManager.LoadScene("Game");
	}
}
