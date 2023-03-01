using UnityEngine;

namespace Scenes.Scripts
{
    public class GameObjectToggle : MonoBehaviour
    {
        public GameObject objectToEnable;
        public bool shouldDisableSelf;

        public void Toggle()
        {
            objectToEnable.SetActive(true);
            if (shouldDisableSelf)
                gameObject.SetActive(false);
        }
    }
}