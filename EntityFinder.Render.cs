using ExileCore.Shared.Helpers;
using ImGuiNET;

namespace EntityFinder;

public partial class EntityFinder
{
    public override void Render()
    {
        int i = 0;
        foreach (var entityd in entitiesData)
        {
            if (!WorldPositionOnScreenBool(entityd.WorldPosition)) continue;

            Graphics.DrawCircleInWorld(entityd.WorldPosition.ToVector3Num(), Settings.Radius.Value, entityd.Color with { A = (byte)Settings.Transparency }, Settings.Radius.Value / 10);
            Graphics.DrawFilledCircleInWorld(entityd.WorldPosition.ToVector3Num(), Settings.Radius.Value, entityd.Color with { A = (byte)(Settings.Transparency / 2) });
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

        if (ImGui.BeginTable("Find Table", 3, ImGuiTableFlags.RowBg | ImGuiTableFlags.BordersOuter | ImGuiTableFlags.BordersV))
        {
            ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.WidthFixed, 150);
            ImGui.TableSetupColumn("MetaData");
            ImGui.TableSetupColumn("Color", ImGuiTableColumnFlags.WidthFixed, 20);

            foreach (var entity in entitiesData)
            {
                ImGui.TableNextRow();

                ImGui.TableNextColumn();

                ImGui.Text(entity.Name);
                ImGui.TableNextColumn();
                ImGui.Text(entity.MetaData);
                ImGui.TableNextColumn();
                ImGui.ColorButton($"##Color{entity.Id}", ToImGuiColor(entity.Color),
                    ImGuiColorEditFlags.NoAlpha | ImGuiColorEditFlags.NoTooltip,
                    new System.Numerics.Vector2(15, 15));
            }
            ImGui.EndTable();
        }

        ImGui.End();
    }
}
