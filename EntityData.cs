﻿using ExileCore.PoEMemory.MemoryObjects;
using SharpDX;

namespace EntityFinder;

public class EntityData
{
    public uint Id { get; set; }
    public string MetaData { get; set; }
    public string Name { get; set; }
    public Vector3 WorldPosition { get; set; }
    public Color Color { get; set; } = Color.Red;

    public EntityData(uint id, string metaData, Vector3 worldPosition, Color color)
    {
        Id = id;
        MetaData = metaData;
        WorldPosition = worldPosition;
        Color = color;
    }

    public EntityData(Entity entity, string name, Color color)
    {
        Id = entity.Id;
        MetaData = entity.Metadata;
        Name = name;
        WorldPosition = entity.Pos;
        Color = color;
    }
}

public class EntityInfo
{
    public string Name { get; set; }
    public string MetaData { get; set; }
    public Color Color { get; set; }

    public EntityInfo()
    {
        Name = string.Empty;
        MetaData = string.Empty;
        Color = Color.Red;
    }

    public EntityInfo(string name, string metaData, Color color)
    {
        Name = name;
        MetaData = metaData;
        Color = color;
    }
}
