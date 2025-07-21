using ExileCore.Shared.Attributes;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;

namespace EntityFinder;

public class EntityFinderSettings : ISettings
{
    public ToggleNode Enable { get; set; } = new(false);

    [Menu("Drawing in the game")]
    public ToggleNode DrawInGame { get; set; } = new(true);

    [Menu("Game Settings")]
    public GameSettings GameSettings { get; set; } = new();

    [Menu("Draw on the map")]
    public ToggleNode DrawOnMap { get; set; } = new(true);

    [Menu("Map Settings")]
    public MapSettings MapSettings { get; set; } = new();

    [Menu("Debug")]
    public ToggleNode Debug { get; set; } = new(false);

    //[Menu("Current Preset")]
    public string CurrentPreset { get; set; } 
}

[Submenu(CollapsedByDefault = true)]
public class MapSettings
{
    [Menu("Draw point")]
    public ToggleNode DrawPoint { get; set; } = new ToggleNode(true);

    [Menu("Draw text")]
    public ToggleNode DrawText { get; set; } = new ToggleNode(true);

    [Menu("Transparency ")]
    public RangeNode<int> Transparency { get; set; } = new RangeNode<int>(255, 0, 255);

    [Menu("Radius")]
    public RangeNode<int> Radius { get; set; } = new RangeNode<int>(30, 0, 255);

    [Menu("Thickness")]
    public RangeNode<int> Thickness { get; set; } = new RangeNode<int>(5, 0, 30);

    [Menu("Offset Text X")]
    public RangeNode<int> OffsetX { get; set; } = new RangeNode<int>(0, -100, 100);

    [Menu("Offset Text Y")]
    public RangeNode<int> OffsetY { get; set; } = new RangeNode<int>(0, -100, 100);
}

[Submenu(CollapsedByDefault = true)]
public class GameSettings
{
    [Menu("Transparency")]
    public RangeNode<int> Transparency { get; set; } = new RangeNode<int>(255, 0, 255);

    [Menu("Radius")]
    public RangeNode<int> Radius { get; set; } = new RangeNode<int>(10, 0, 255);

    [Menu("Thickness")]
    public RangeNode<int> Thickness { get; set; } = new RangeNode<int>(5, 0, 30);

    [Menu("Transparency Radius")]
    public RangeNode<int> TransparencyRadius { get; set; } = new RangeNode<int>(255, 0, 255);
}