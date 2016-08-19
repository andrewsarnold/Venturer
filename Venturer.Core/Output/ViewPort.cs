using Venturer.Core.Input;

namespace Venturer.Core.Output
{
	internal abstract class ViewPort
	{
		/// <summary>
		/// The output screen width.
		/// </summary>
		protected readonly int Width;

		/// <summary>
		/// The output screen height.
		/// </summary>
		protected readonly int Height;

		protected ViewPort(int width, int height)
		{
			Width = width;
			Height = height;
		}

		/// <summary>
		///	Handles key input from the console window.
		/// </summary>
		/// <param name="command"></param>
		/// <returns>Returns True if input should continue to bubble down through the screen stack.</returns>
		internal abstract bool HandleInput(Command command);

		/// <summary>
		/// Writes a graphical representation of this viewport to a Screen object.
		/// </summary>
		/// <param name="width">The output screen width.</param>
		/// <param name="height">The output screen height.</param>
		/// <param name="xOffset">The amount by which the screen will be shifted left in the console.</param>
		/// <param name="yOffset">The amount by which the screen will be shifted down in the console.</param>
		/// <returns></returns>
		internal abstract Screen ToScreen(int width, int height, int xOffset = 0, int yOffset = 0);

		/// <summary>
		/// Returns null, or a new viewport that should be displayed on top to be handled immediately.
		/// </summary>
		internal abstract ViewPort GetAndClearNewViewPort();

		/// <summary>
		/// Returns whether this screen should be removed from the stack after this input process loop is over.
		/// </summary>
		internal abstract bool ShouldDestroy { get; }

		/// <summary>
		/// Returns the input context for this viewport.
		/// </summary>
		internal abstract InputContext InputContext { get; }
	}
}
