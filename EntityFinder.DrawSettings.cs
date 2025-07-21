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

        if (presetFiles.Length == 0 && Directory.Exists(configDir))
        {
            UpdatePresetList();
        }

        if (ImGui.BeginCombo("Select preset", Settings.CurrentPreset))
        {
            for (int i = 0; i < presetFiles.Length; i++)
            {
                bool isSelected = (selectedPresetIndex == i);

                if (ImGui.Selectable(presetFiles[i], isSelected))
                {
                    selectedPresetIndex = i;
                    LoadButton(presetFiles[i]);
                }

                if (isSelected)
                    ImGui.SetItemDefaultFocus();
            }

            ImGui.EndCombo();
        }

        if (ImGui.Button("Update Preset List"))
        {
            UpdatePresetList();
        }

        for (int i = 0; i < _entityMetaDataToFind.Count; i++)
        {
            var data = _entityMetaDataToFind[i];
            var name = data.Name;
            var enable = data.Enable;
            var meta = data.MetaData;
            var color = data.Color;
            var colorName = GetColorName(color);

            ImGui.Checkbox($"##Enable-{i}", ref enable);
            {
                data.Enable = enable;
                _entityMetaDataToFind[i] = data;
            }

            ImGui.SameLine();
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

        ImGui.Spacing();
        if (ImGui.Button("Add New Line"))
        {
            _entityMetaDataToFind.Add(new("", "", SharpDX.Color.Red));
        }

        ImGui.Spacing();
        ImGui.PushItemWidth(500);
        if (ImGui.InputTextWithHint($"##PresetName", "Preset name...", ref presetName, 500))
        {

        }

        ImGui.SameLine();
        if (ImGui.Button("Save Preset"))
        {
            SaveButton(presetName);
        }

        ImGui.Spacing();
        if (ImGui.Button("Clearing Found Entity"))
        {
            Reset();
        }

        ImGui.Spacing();
        if (ImGui.Button("Default"))
        {
            DefaultButton();
        }
    }

    public void UpdatePresetList()
    {
        presetFiles = Directory.GetFiles(configDir, "*.json")
                               .Select(Path.GetFileNameWithoutExtension)
                               .ToArray();
    }

    public void DefaultButton()
    {
        _entityMetaDataToFind = new List<EntityInfo>
            {
                new("Nameless Seer", "Metadata/NPC/League/Azmeri/UniqueDealerMaps",SharpDX.Color.Red ),
                new("Verisium Boss", "Metadata/Terrain/Leagues/Settlers/Objects/VerisiumBossSubAreaEntrance",SharpDX.Color.Blue),
                new("Stash", "Metadata/MiscellaneousObjects/Stash",SharpDX.Color.LimeGreen),
            };

        Settings.CurrentPreset = "Default";
    }

    public void SaveButton(string presetName)
    {
        Directory.CreateDirectory(configDir);

        string filePath = Path.Combine(configDir, $"{presetName}.json");

        var json = JsonConvert.SerializeObject(_entityMetaDataToFind, Formatting.Indented);
        File.WriteAllText(filePath, json);

        Settings.CurrentPreset = presetName;
    }

    public void LoadButton(string presetName)
    {
        try
        {
            if (presetName == string.Empty || presetFiles.Length == 0) DefaultButton();

            string filePath = Path.Combine(configDir, $"{presetName}.json");

            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var loadedList = JsonConvert.DeserializeObject<List<EntityInfo>>(json);

                if (loadedList != null)
                {
                    _entityMetaDataToFind = loadedList;
                    Settings.CurrentPreset = presetName;
                }
            }
        }
        catch (Exception)
        {
            DefaultButton();
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
