using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevelSync : MonoBehaviour {
	public Slider indicator;
	public string sceneName;
	public float normalizer=0.01f;

	private AsyncOperation asyncop;
	float increaser;

	void Start(){
		asyncop = SceneManager.LoadSceneAsync(sceneName);
	}

	void Update(){
		if (indicator.value < 100) {
			indicator.value = asyncop.progress + increaser;
		}
		increaser += Time.deltaTime*normalizer;
	}
}
