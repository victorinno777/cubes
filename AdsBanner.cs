using UnityEngine;
using GoogleMobileAds.Api;

public class AdsBanner : MonoBehaviour
{
	BannerView _bannerAd;
	string _bannerId = "ca-app-pub-3940256099942544/6300978111";
	
	private void OnEnable()
	{
		_bannerAd = new BannerView(_bannerId, AdSize.Banner, AdPosition.Bottom);
		AdRequest adRequest = new AdRequest.Builder().Build();
		_bannerAd.LoadAd(adRequest);
	}
	
	public void ShowAd()
	{
		_bannerAd.Show();
	}
}
