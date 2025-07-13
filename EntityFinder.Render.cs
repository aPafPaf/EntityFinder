using ExileCore.Shared.Helpers;
using ImGuiNET;
using SharpDX;
using System.Globalization;

namespace EntityFinder;

public partial class EntityFinder
{
    public override void Render()
    {
        int i = 0;
        foreach (var entityd in entitiesData)
        {
            Graphics.DrawCircleInWorld(entityd.WorldPosition.ToVector3Num(), Settings.Radius.Value, Color.Red with { A = (byte)Settings.Transparency }, Settings.Radius.Value / 10);
            Graphics.DrawFilledCircleInWorld(entityd.WorldPosition.ToVector3Num(), Settings.Radius.Value, Color.Red with { A = (byte)(Settings.Transparency / 2) });
            //Graphics.DrawText(entityd.MetaData, new(300, 300 + i));
            i += 20;
        }

        DrawWindow();
    }

    private void DrawWindow()
    {
        ImGui.SetNextWindowSize(new System.Numerics.Vector2());
        ImGui.SetNextWindowBgAlpha(0.6f);
        ImGui.Begin("Find Window", ImGuiWindowFlags.NoDecoration);

        if (ImGui.BeginTable("Find Table", 2, ImGuiTableFlags.RowBg | ImGuiTableFlags.BordersOuter | ImGuiTableFlags.BordersV))
        {
            ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.WidthFixed, 48);
            ImGui.TableSetupColumn("Meta");

            foreach (var entity in entitiesData)
            {
                ImGui.TableNextRow();

                ImGui.TableNextColumn();

                ImGui.Text(entity.Name);
                ImGui.TableNextColumn();
                ImGui.Text(entity.MetaData);
            }
            ImGui.EndTable();
        }

        ImGui.End();
    }
}
