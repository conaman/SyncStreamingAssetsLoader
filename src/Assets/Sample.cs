using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Sample : MonoBehaviour {
	public Text textUi;

	private SyncStreamingAssetsLoader streamingAssetsLoader = new SyncStreamingAssetsLoader();

	private void Start() {
		streamingAssetsLoader.Init();

		byte[] bytes = streamingAssetsLoader.Load("Test.txt");
		if (bytes != null) {
			textUi.text = Encoding.UTF8.GetString(bytes);
		}
	}
}
