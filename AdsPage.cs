using UnityEngine;
using GoogleMobileAds.Api;

public class AdsPage : ScriptableObject
{
    InterstitialAd _interAd;
    string _interstitialId = "ca-app-pub-3940256099942544/8691691433";
	
	void OnEnable()
	{
		_interAd = new InterstitialAd(_interstitialId);
		AdRequest adRequest = new AdRequest.Builder().Build();
		_interAd.LoadAd(adRequest);
	}
	
	public void ShowAd()
	{
		if (_interAd.IsLoaded())
		{
			_interAd.Show();
		}
	}
}
