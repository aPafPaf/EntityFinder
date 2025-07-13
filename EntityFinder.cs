using ExileCore;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Helpers;
using SharpDX;
using System.Collections.Generic;
using System.Linq;

namespace EntityFinder;

public partial class EntityFinder : BaseSettingsPlugin<EntityFinderSettings>
{
    private List<string> _entityMetaDataToFind = [];

    private List<EntityData> entityDatas = [];

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

        foreach (var targetName in _entityMetaDataToFind)
        {
            if (entity.Metadata != targetName) continue;

            if (entityDatas.Any(x => x.Id == entity.Id)) continue;

            LogMessage($"Found: {targetName}", 30);
            entityDatas.Add(new(entity));
        }
    }

    public void Reset()
    {
        entityDatas = [];
    }
}