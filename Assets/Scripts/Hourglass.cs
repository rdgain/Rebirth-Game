using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Hourglass : MonoBehaviour {
	public Temple temple;
	private GameObject topSand;
	private GameObject bottomSand;
	private Image topSandImage;
	private Image bottomSandImage;

	void Awake() {
		topSand = GameObject.Find("TopSand");
		bottomSand = GameObject.Find("BottomSand");
		topSandImage = topSand.GetComponent<Image>();
		bottomSandImage = bottomSand.GetComponent<Image>();
	}

	// Update is called once per frame
	void Update () {
		topSandImage.fillAmount = temple.SecondsLeft()/120;
		bottomSandImage.fillAmount = (120-temple.SecondsLeft())/120;
	}
}
