using UnityEngine;
using UnityEngine.UI;

namespace SaltedFishStudio.RoadKill.UI
{
	public class EndCanvas : MonoBehaviour, ICanvas
	{
		public Button mainMenuButton;
		
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