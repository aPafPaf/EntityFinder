using ExileCore.PoEMemory.MemoryObjects;
using SharpDX;

namespace EntityFinder;

public class EntityData
{
    public uint Id { get; set; }
    public string MetaData { get; set; }
    public string Name { get; set; }

    public System.Numerics.Vector3 WorldPosition { get; set; }
    public System.Numerics.Vector2 GridPosition { get; set; }

    public Color Color { get; set; } = Color.Red;
    public string RenderName { get; set; } = string.Empty;

    public EntityData(Entity entity, string name, Color color)
    {
        Id = entity.Id;
        MetaData = entity.Metadata;
        Name = name;
        WorldPosition = entity.PosNum;
        GridPosition = entity.GridPosNum;
        Color = color;
        RenderName = entity.RenderName;
    }
}

public class EntityInfo
{
    public string Name { get; set; }
    public bool Enable { get;set; }
    public string MetaData { get; set; }
    public Color Color { get; set; }

    public EntityInfo()
    {
        Name = string.Empty;
        MetaData = string.Empty;
        Color = Color.Red;
        Enable = true;
    }

    public EntityInfo(string name, string metaData, Color color)
    {
        Name = name;
        MetaData = metaData;
        Color = color;
        Enable = true;
    }

    public EntityInfo(string name, string metaData, Color color,bool enable)
    {
        Name = name;
        MetaData = metaData;
        Color = color;
        Enable = enable;
    }
}
