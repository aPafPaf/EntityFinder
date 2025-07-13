using ExileCore.Shared.Helpers;
using SharpDX;

namespace EntityFinder;

public partial class EntityFinder
{

    public override void Render()
    {
        int i = 0;
        foreach (var entityd in entityDatas)
        {
            Graphics.DrawCircleInWorld(entityd.WorldPosition.ToVector3Num(), Settings.Radius.Value, Color.Red with { A = (byte)Settings.Transparency });
            Graphics.DrawFilledCircleInWorld(entityd.WorldPosition.ToVector3Num(), Settings.Radius.Value, Color.Red with { A = (byte)(Settings.Transparency / 2) }, Settings.Radius.Value / 3);
            Graphics.DrawText(entityd.MetaData, new(300, 300 + i));
            i += 20;
        }
    }
}
