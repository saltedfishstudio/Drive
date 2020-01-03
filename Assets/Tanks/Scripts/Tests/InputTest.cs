using UnityEngine;

namespace SaltedFishStudio.RoadKill.Tests
{
	public class InputTest : MonoBehaviour
	{
		bool isTouching = false;
		[SerializeField] RectTransform horizontal = null;
		[SerializeField] RectTransform vertical = null;

		void Awake()
		{
			gameObject.SetActive(true);
		}

		void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				isTouching = true;
			}

			if (Input.GetMouseButtonUp(0))
			{
				isTouching = false;
			}

			if (isTouching)
			{
				horizontal.anchoredPosition = new Vector2(0, Input.mousePosition.y);
				vertical.anchoredPosition = new Vector2( Input.mousePosition.x,0);
			}
		}
	}
}