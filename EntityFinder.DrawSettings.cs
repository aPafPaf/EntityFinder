using ImGuiNET;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace EntityFinder;

public partial class EntityFinder
{
    public override void DrawSettings()
    {
        base.DrawSettings();

        for (int i = 0; i < _entityMetaDataToFind.Count; i++)
        {
            var metaData = _entityMetaDataToFind[i];

            if (ImGui.InputTextWithHint($"##EntityMeta-{i}", "Enter MetaData...", ref metaData, 128))
            {
                _entityMetaDataToFind[i] = metaData;
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
            _entityMetaDataToFind.Add("");
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
            _entityMetaDataToFind = new List<string>
            {
                "Metadata/NPC/League/Azmeri/UniqueDealerMaps",
                "Metadata/Terrain/Leagues/Settlers/Objects/VerisiumBossSubAreaEntrance",
            };
        }
    }

    public void SaveButton()
    {
        // Ensure the /config directory exists in the application's root
        string configDir = Path.Combine(AppContext.BaseDirectory, "config");
        Directory.CreateDirectory(configDir);

        // Path to the JSON file
        string filePath = Path.Combine(configDir, "entityMetaData.json");

        // Serialize the _entityMetaDataToFind list to JSON and save using Newtonsoft.Json
        var json = JsonConvert.SerializeObject(_entityMetaDataToFind, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    public void LoadButton()
    {
        string configDir = Path.Combine(AppContext.BaseDirectory, "config");
        string filePath = Path.Combine(configDir, "entityMetaData.json");

        if (File.Exists(filePath))
        {
            // Read and deserialize the JSON file into the list using Newtonsoft.Json
            var json = File.ReadAllText(filePath);
            var loadedList = JsonConvert.DeserializeObject<List<string>>(json);

            if (loadedList != null)
                _entityMetaDataToFind = loadedList;
        }
    }
}
