using ExileCore.PoEMemory.MemoryObjects;
using SharpDX;

namespace EntityFinder;

public class EntityData
{
    public uint Id { get; set; }
    public string MetaData { get; set; }
    public Vector3 WorldPosition { get; set; }

    public EntityData(uint id, string metaData, Vector3 worldPosition)
    {
        Id = id;
        MetaData = metaData;
        WorldPosition = worldPosition;
    }

    public EntityData(Entity entity)
    {
        Id = entity.Id;
        MetaData = entity.Metadata;
        WorldPosition = entity.Pos;
    }
}
