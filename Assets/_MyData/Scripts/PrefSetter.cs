using UnityEngine;

namespace Com.HarmonicArts.CholistanRallyArena {
    public class PrefSetter : MonoBehaviour
    {
        public string key;
        ///
        public void SetValue(int value) {
            PlayerPrefs.SetInt(key, value);
        }
    }
}