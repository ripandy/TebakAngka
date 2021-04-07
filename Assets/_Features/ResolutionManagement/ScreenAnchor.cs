/* ScreenAnchor.cs
 * Author 		: Ripandy Adha (ripandy.adha@gmail.com)
 * Usage 		: Attach to an object to be anchored. Will also anchor children.
 * Requirement	: - ResolutionManager -> handles screen resolution and size to be the anchor position.
 *				  - Anchor enum from Enums.cs.
 **/

using System;
using Cysharp.Threading.Tasks.Linq;
using Kassets.VariableSystem;
using UnityEngine;

namespace Feature.ScreenManagement
{
	/// <summary>
	/// enums to define anchor positions
	/// </summary>
	[Flags]
	public enum Anchor {
		Center = 0,
		Top = 1 << 1,
		Bottom = 1 << 2,
		Left = 1 << 3,
		Right = 1 << 4
	};
	
	/// <summary>
	/// A class to anchor an object and its children to be bound to a certain position.
	/// </summary>
	public class ScreenAnchor : MonoBehaviour
	{
		[SerializeField] private Anchor _anchor;

		[SerializeField] private FloatVariable _screenWidth;
		[SerializeField] private FloatVariable _screenHeight;

		private Vector3 _initialPosition;

		private void Awake() => _initialPosition = transform.position;

		private void Start()
		{
			_screenWidth.Subscribe(_ => Resize());
			_screenHeight.Subscribe(_ => Resize());
		}

		private void Resize()
		{
			if (_anchor.HasFlag(Anchor.Top))
				StickTop();
			else if (_anchor.HasFlag(Anchor.Bottom))
				StickBottom();
			if (_anchor.HasFlag(Anchor.Left))
				StickLeft();
			else if (_anchor.HasFlag(Anchor.Right))
				StickRight();
		}

		private void StickTop()
		{
			var tr = transform;
			var pos = tr.position;
				pos.y = _screenHeight / 2 + (_initialPosition.y - _screenHeight.InitialValue / 2);
			tr.position = pos;
		}

		private void StickBottom()
		{
			var tr = transform;
			var pos = tr.position;
				pos.y = -_screenHeight / 2 +
			        (_initialPosition.y - (-_screenHeight.InitialValue / 2));
			tr.position = pos;
		}

		private void StickLeft()
		{
			var tr = transform;
			var pos = tr.position;
				pos.x = -_screenWidth / 2 +
			        (_initialPosition.x - (-_screenWidth.InitialValue / 2));
			tr.position = pos;
		}

		private void StickRight()
		{
			var tr = transform;
			var pos = tr.position;
				pos.x = _screenWidth / 2 + (_initialPosition.x - _screenWidth.InitialValue / 2);
			tr.position = pos;
		}
	}
}
