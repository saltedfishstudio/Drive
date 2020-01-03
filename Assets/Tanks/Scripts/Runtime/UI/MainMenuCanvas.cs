using UnityEngine;
using UnityEngine.UI;

namespace SaltedFishStudio.RoadKill.UI
{
    public class MainMenuCanvas : MonoBehaviour, ICanvas
    {
        public Button playButton;
        public Button exitButton;

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}

