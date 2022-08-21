using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
	[SerializeField]
	private int _sceneIndex = 0;

	public void OnButtonPressed()
	{
		SceneManager.LoadScene(_sceneIndex);
	}
}
