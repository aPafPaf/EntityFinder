using ExileCore;
using ExileCore.PoEMemory.MemoryObjects;
using System.Collections.Generic;
using System.Linq;

namespace EntityFinder;

public partial class EntityFinder : BaseSettingsPlugin<EntityFinderSettings>
{
    private List<(string name, string meta)> _entityMetaDataToFind = [];

    private List<EntityData> entitiesData = [];

    public override bool Initialise()
    {
        Reset();
        LoadButton();

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
            if (entity.Metadata != data.meta) continue;

            if (entitiesData.Any(x => x.Id == entity.Id)) continue;

            LogMessage($"Found: {data}", 30);
            entitiesData.Add(new(entity, data.name));
        }
    }

    public void Reset()
    {
        entitiesData = [];
    }
}