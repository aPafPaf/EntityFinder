using ImGuiNET;
using Newtonsoft.Json;
using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EntityFinder;

public partial class EntityFinder
{

    public override void DrawSettings()
    {
        base.DrawSettings();

        for (int i = 0; i < _entityMetaDataToFind.Count; i++)
        {
            var data = _entityMetaDataToFind[i];
            var name = data.Name;
            var meta = data.MetaData;
            var color = data.Color;
            var colorName = GetColorName(color);

            ImGui.PushItemWidth(200);
            if (ImGui.InputTextWithHint($"##EntityName-{i}", "Enter Name...", ref name, 20))
            {
                data.Name = name;
                _entityMetaDataToFind[i] = data;
            }

            ImGui.SameLine();
            ImGui.PushItemWidth(400);
            if (ImGui.InputTextWithHint($"##EntityMeta-{i}", "Enter MetaData...", ref meta, 120))
            {
                data.MetaData = meta;
                _entityMetaDataToFind[i] = data;
            }

            ImGui.PushItemWidth(20);
            ImGui.SameLine();
            var _tempColorVec = ToImGuiColor(color);

            // Изменяем логику открытия комбо-бокса
            bool colorButtonClicked = ImGui.ColorButton($"##Current{GetColorName(color)}", _tempColorVec,
                ImGuiColorEditFlags.NoAlpha | ImGuiColorEditFlags.NoTooltip,
                new System.Numerics.Vector2(40, ImGui.GetFrameHeight()));

            ImGui.SameLine();
            if (ImGui.BeginCombo($"##SharpDXColorCombo{i}", colorName, ImGuiComboFlags.PopupAlignLeft))
            {
                foreach (var colorPair in _sharpDxColors)
                {
                    bool isSelected = (colorName == colorPair.Key);

                    var colorVec = ToImGuiColor(colorPair.Value);

                    ImGui.ColorButton($"##{colorPair.Key}", colorVec,
                        ImGuiColorEditFlags.NoAlpha | ImGuiColorEditFlags.NoTooltip,
                        new System.Numerics.Vector2(20, 20));

                    ImGui.SameLine();

                    if (ImGui.Selectable(colorPair.Key, isSelected))
                    {
                        data.Color = colorPair.Value;
                        _entityMetaDataToFind[i] = data;
                    }

                    if (isSelected)
                        ImGui.SetItemDefaultFocus();
                }
                ImGui.EndCombo();
            }

            ImGui.SameLine();
            ImGui.PushItemWidth(10);
            if (ImGui.Button($"x##RemoveLine-{i}-"))
            {
                _entityMetaDataToFind.RemoveAt(i);
                i--;
            }

            ImGui.PopItemWidth();
        }

        if (ImGui.Button("AddLine"))
        {
            _entityMetaDataToFind.Add(new("", "", SharpDX.Color.Red));
        }

        ImGui.Spacing();
        if (ImGui.Button("Save"))
        {
            SaveButton();
        }

        ImGui.Spacing();
        if (ImGui.Button("Load"))
        {
            LoadButton();
        }

        ImGui.Spacing();
        if (ImGui.Button("Reset"))
        {
            Reset();
        }

        ImGui.Spacing();
        if (ImGui.Button("Default"))
        {
            _entityMetaDataToFind = new List<EntityInfo>
            {
                new("Nameless Seer", "Metadata/NPC/League/Azmeri/UniqueDealerMaps",SharpDX.Color.Red ),
                new("Verisium Boss", "Metadata/Terrain/Leagues/Settlers/Objects/VerisiumBossSubAreaEntrance",SharpDX.Color.Blue),
                new("Stash", "Metadata/MiscellaneousObjects/Stash",SharpDX.Color.LimeGreen),
            };
        }
    }

    public void SaveButton()
    {
        string configDir = Path.Combine(AppContext.BaseDirectory, "config");
        Directory.CreateDirectory(configDir);

        string filePath = Path.Combine(configDir, "entityMetaData.json");

        var json = JsonConvert.SerializeObject(_entityMetaDataToFind, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    public void LoadButton()
    {
        string configDir = Path.Combine(AppContext.BaseDirectory, "config");
        string filePath = Path.Combine(configDir, "entityMetaData.json");

        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            var loadedList = JsonConvert.DeserializeObject<List<EntityInfo>>(json);

            if (loadedList != null)
                _entityMetaDataToFind = loadedList;
        }
    }

    private System.Numerics.Vector4 ToImGuiColor(Color color)
    {
        return new System.Numerics.Vector4(
            color.R / 255f,
            color.G / 255f,
            color.B / 255f,
            color.A / 255f);
    }

    public string GetColorName(Color color)
    {
        var matchingPair = _sharpDxColors.FirstOrDefault(pair =>
            pair.Value.R == color.R &&
            pair.Value.G == color.G &&
            pair.Value.B == color.B);

        if (!string.IsNullOrEmpty(matchingPair.Key))
            return matchingPair.Key;

        return $"R:{color.R} G:{color.G} B:{color.B}";
    }
}
