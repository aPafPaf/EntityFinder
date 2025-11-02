using ExileCore;
using ExileCore.PoEMemory.MemoryObjects;
using SharpDX;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EntityFinder;

public partial class EntityFinder : BaseSettingsPlugin<EntityFinderSettings>
{
    public override bool Initialise()
    {
        configDir = Path.Combine(AppContext.BaseDirectory, CONFIG_LOCAL_DIR);

        Reset();
        UpdatePresetList();

        LoadButton(Settings.CurrentPreset);

        return true;
    }

    public override void OnLoad()
    {
        _sharpDxColors = typeof(Color)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(Color))
            .ToDictionary(f => f.Name, f => (Color)f.GetValue(null));

        base.OnLoad();
    }

    public override void AreaChange(AreaInstance area)
    {
        Reset();
    }

    public override void EntityAdded(Entity entity)
    {
        if (!Settings.Enable.Value || entity.Type == ExileCore.Shared.Enums.EntityType.Error) return;

        foreach (var data in _entityMetaDataToFind.Where(x => x.Enable))
        {
            if (entity.Metadata != data.MetaData) continue;

            if (entitiesData.Any(x => x.Id == entity.Id)) continue;

            if (Settings.Debug)
            {
                LogMessage($"Found: {data}", 30);
            }

            EntityData entityData = new(entity, data.Name, data.Color);

            entitiesData.Add(entityData);
        }
    }
}