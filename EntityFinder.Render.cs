using ExileCore;
using ExileCore.PoEMemory.Elements;
using ImGuiNET;
using System.Numerics;

namespace EntityFinder;

public partial class EntityFinder
{
    public override void Render()
    {
        var ui = GameController.IngameState.IngameUi;

        foreach (var entityData in entitiesData)
        {
            if (Settings.DrawOnMap && ui.Map.LargeMap.IsVisible)
            {
                DrawLargeMap(entityData);
            }

            if (!Settings.DrawInGame || !WorldPositionOnScreenBool(entityData.WorldPosition)) continue;

            Graphics.DrawCircleInWorld(entityData.WorldPosition, Settings.GameSettings.Radius.Value, entityData.Color with { A = (byte)Settings.GameSettings.TransparencyRadius }, Settings.GameSettings.Thickness);
            Graphics.DrawFilledCircleInWorld(entityData.WorldPosition, Settings.GameSettings.Radius.Value - Settings.GameSettings.Thickness, entityData.Color with { A = (byte)(Settings.GameSettings.Transparency) });

            if (Settings.GameSettings.DrawText.Value)
            {
                var screenPosition = GameController.IngameState.Data.GetGridScreenPosition(entityData.GridPosition);
                var resultPosition = screenPosition + new Vector2(Settings.GameSettings.OffsetX, Settings.GameSettings.OffsetY);

                var text = $"[{entityData.Id}] {entityData.Name}-{entityData.RenderName}";
                Graphics.DrawTextWithBackground(text, resultPosition, entityData.Color, SharpDX.Color.Black);
            }
        }

        DrawWindow();
    }

    private void DrawLargeMap(EntityData entityData)
    {
        Vector2 gridPos = entityData.GridPosition;
        SharpDX.Color color = entityData.Color;

        var map = GameController.Game.IngameState.IngameUi.Map;
        var largeMap = map.LargeMap.AsObject<SubMap>();

        if (!largeMap.IsVisible)
            return;

        var mapCenter = largeMap.MapCenter;
        var mapScale = largeMap.MapScale;

        var player = GameController.Game.IngameState.Data.LocalPlayer;
        var playerPos = player.GetComponent<ExileCore.PoEMemory.Components.Positioned>()?.GridPosNum ?? default;

        var delta = gridPos - new Vector2(playerPos.X, playerPos.Y);

        const double cameraAngle = 38.7 * System.Math.PI / 180;
        float cos = (float)System.Math.Cos(cameraAngle);
        float sin = (float)System.Math.Sin(cameraAngle);

        var mapOffset = mapScale * new Vector2((delta.X - delta.Y) * cos, -(delta.X + delta.Y) * sin);
        var finalPos = mapCenter + mapOffset;

        if (Settings.MapSettings.DrawPoint)
        {
            var finalPositon = new Vector2(finalPos.X, finalPos.Y);
            Graphics.DrawCircle(
                finalPositon,
                Settings.MapSettings.Radius.Value * (mapScale / 2),
                color with { A = (byte)Settings.MapSettings.Transparency },
                Settings.MapSettings.Thickness.Value, SEGMENTS_CIRCLE);
        }

        if (Settings.MapSettings.DrawText)
        {
            var finalPosition = new Vector2(finalPos.X + Settings.MapSettings.OffsetX, finalPos.Y + Settings.MapSettings.OffsetY);
            var text = $"[{entityData.Id}] {entityData.Name} - {entityData.RenderName}";
            Graphics.DrawTextWithBackground(text, finalPosition, entityData.Color, SharpDX.Color.Black);
        }
    }

    private void DrawWindow()
    {
        ImGui.SetNextWindowSize(new System.Numerics.Vector2());
        ImGui.SetNextWindowBgAlpha(0.6f);
        ImGui.Begin("Find Window", ImGuiWindowFlags.NoDecoration);

        if (ImGui.BeginTable("Find Table", 4, ImGuiTableFlags.RowBg | ImGuiTableFlags.BordersOuter | ImGuiTableFlags.BordersV))
        {
            ImGui.TableSetupColumn("Id", ImGuiTableColumnFlags.WidthFixed, 40);
            ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.WidthFixed, 150);
            ImGui.TableSetupColumn("MetaData");
            ImGui.TableSetupColumn("Color", ImGuiTableColumnFlags.WidthFixed, 20);

            foreach (var entity in entitiesData)
            {
                ImGui.TableNextRow();

                ImGui.TableNextColumn();

                ImGui.Text($"[{entity.Id}]");
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
