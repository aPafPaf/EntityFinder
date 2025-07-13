using ExileCore;
using ExileCore.PoEMemory.MemoryObjects;
using SharpDX;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EntityFinder;

public partial class EntityFinder : BaseSettingsPlugin<EntityFinderSettings>
{
    private static Dictionary<string, Color> _sharpDxColors;

    private List<EntityInfo> _entityMetaDataToFind = [];

    private List<EntityData> entitiesData = [];

    public override bool Initialise()
    {
        Reset();
        LoadButton();

        _sharpDxColors = typeof(Color)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(Color))
            .ToDictionary(f => f.Name, f => (Color)f.GetValue(null));

        return true;
    }

    public override void AreaChange(AreaInstance area)
    {
        Reset();
    }

    public override Job Tick()
    {
        return null;
    }

    public override void EntityAdded(Entity entity)
    {
        if (!Settings.Enable.Value || entity.Type == ExileCore.Shared.Enums.EntityType.Error) return;

        foreach (var data in _entityMetaDataToFind)
        {
            if (entity.Metadata != data.MetaData) continue;

            if (entitiesData.Any(x => x.Id == entity.Id)) continue;

            LogMessage($"Found: {data}", 30);
            entitiesData.Add(new(entity, data.Name, data.Color));
        }
    }

    public void Reset()
    {
        entitiesData = [];
    }
}