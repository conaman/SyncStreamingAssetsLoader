using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Sample : MonoBehaviour {
	public Text textUi;

	private SyncStreamingAssetsLoader _streamingAssetsLoader = new SyncStreamingAssetsLoader();

	private void Start() {
		_streamingAssetsLoader.Init();

		byte[] bytes = _streamingAssetsLoader.Load("Test.txt");
		if (bytes != null) {
			textUi.text = Encoding.UTF8.GetString(bytes);
		}
	}

	private void OnDestroy() {
		_streamingAssetsLoader.Close();
	}
}
