using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
	GameObject playButton, ua, en;
	public TextAsset jsonFile;
	LanguageSets languageSets;
	
    void Start()
    {
        playButton = transform.GetChild(0).gameObject;
        ua = transform.GetChild(1).gameObject;
        en = transform.GetChild(2).gameObject;
		playButton.GetComponent<Button>().onClick.AddListener(Play);
		ua.GetComponent<Button>().onClick.AddListener(delegate {ChangeLanguage("UA");});
		en.GetComponent<Button>().onClick.AddListener(delegate {ChangeLanguage("EN");});
		
        languageSets = JsonUtility.FromJson<LanguageSets>(jsonFile.text);
    }

    void ChangeLanguage(string language)
	{
		foreach (LanguageSet langSet in languageSets.languageSets)
		{
			if (langSet.language == language)
			{
				GameObject buttonText = playButton.transform.GetChild(0).gameObject;
				buttonText.GetComponent<Text>().text = langSet.play;
				break;
			}
		}
	}
	
	void Play()
	{
		SceneManager.LoadScene("Game");
	}
}

[System.Serializable]
public class LanguageSet
{
    public string language;
    public string play;
}

[System.Serializable]
public class LanguageSets
{
	public LanguageSet[] languageSets;
}