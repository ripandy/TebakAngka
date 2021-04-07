/* ResolutionManager.cs
 * Author 		: Ripandy Adha (ripandy.adha@gmail.com) (modified)
 *				  Dominikus D. Putranto (www.rollingglory.com) (original)
 * Usage 		: Note that this version is default to bound to MainCamera
 *				  and only works on Ortographics Camera.
 * Requirement	: - Pyra namespace(s).
 **/

using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Kassets.VariableSystem;
using UnityEngine;

namespace Pyra.ScreenManagement {
	/// <summary>
	/// A Manager class to handle screen size.
	/// </summary>
	public class ResolutionManager : MonoBehaviour
	{
		[SerializeField] private FloatVariable _screenWidth;
		[SerializeField] private FloatVariable _screenHeight;

		[SerializeField] private CameraVariable _screenCamera;
		
		private bool _isLandscape => _screenWidth.InitialValue >= _screenHeight.InitialValue;

		private void Start ()
		{
			var token = this.GetCancellationTokenOnDestroy();
			
			_screenCamera.Subscribe(_ => ForceCalculateResolution()).AddTo(token);
			
#if UNITY_EDITOR
			UniTaskAsyncEnumerable.EveryUpdate().Subscribe(unit => CalculateResolution()).AddTo(token);
#endif

			CalculateResolution();
		}
		
		/// <summary>
		/// Calculate the current screen resolution and adjust them to the Screen Camera.
		/// Only works on orthographic camera.
		/// </summary>
		/// <param name="forced">force the resolution calculation</param>
		private void CalculateResolution(bool forced = false)
		{
			if (!_screenCamera.Value.orthographic) return;

			// letterbox
			if (_screenCamera.Value.aspect >= _screenWidth.InitialValue / _screenHeight.InitialValue)
			{
				// fit height
				_screenCamera.Value.orthographicSize = _isLandscape ? _screenHeight.InitialValue / _screenCamera.Value.aspect * 0.5f : _screenHeight.InitialValue * 0.5f;
			}
			else
			{
				// fit width
				_screenCamera.Value.orthographicSize = _isLandscape ? _screenWidth.InitialValue * 0.5f : _screenWidth.InitialValue / _screenCamera.Value.aspect * 0.5f;
			}

			var newStageWidth = GetScreenBoundHorizontal() * 2;
			var newStageHeight = GetScreenBoundVertical() * 2;

			if (Mathf.Abs (_screenWidth - newStageWidth) > Mathf.Epsilon
			    || Mathf.Abs (_screenHeight - newStageHeight) > Mathf.Epsilon
			    || forced) 
			{
				_screenWidth.Value = newStageWidth;
				_screenHeight.Value = newStageHeight;
			}
		}

		private float GetScreenBoundHorizontal() {
			return _screenCamera.Value.orthographicSize * _screenCamera.Value.aspect;
		}

		private float GetScreenBoundVertical() {
			return _screenCamera.Value.orthographicSize;
		}

		/// <summary>
		/// Force the Calculation of the screen resolution and adjust them to the Screen Camera.
		/// </summary>
		private void ForceCalculateResolution() {
			CalculateResolution (true);
		}
	}
}
