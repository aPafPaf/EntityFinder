using SharpDX;

namespace EntityFinder;

public partial class EntityFinder
{
    public void Reset()
    {
        entitiesData = [];
    }

    private bool WorldPositionOnScreenBool(System.Numerics.Vector3 worldPos, int edgeBounds = 70)
    {
        var windowRect = GameController.Window.GetWindowRectangle();
        var screenPos = GameController.IngameState.Camera.WorldToScreen(worldPos);

        windowRect.X -= windowRect.Location.X;
        windowRect.Y -= windowRect.Location.Y;

        var result = GameController.Window.ScreenToClient((int)screenPos.X, (int)screenPos.Y) + GameController.Window.GetWindowRectangle().Location;

        var rectBounds = new SharpDX.RectangleF(
            x: windowRect.X + edgeBounds,
            y: windowRect.Y + edgeBounds,
            width: windowRect.Width - (edgeBounds * 2),
            height: windowRect.Height - (edgeBounds * 2));

        return rectBounds.Contains(result);
    }
}
