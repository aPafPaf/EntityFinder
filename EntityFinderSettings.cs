using ExileCore.Shared.Attributes;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;

namespace EntityFinder;

public class EntityFinderSettings : ISettings
{
    //Mandatory setting to allow enabling/disabling your plugin
    public ToggleNode Enable { get; set; } = new ToggleNode(false);

    [Menu("TransparencySpawnMonsters")]
    public RangeNode<int> Transparency { get; set; } = new RangeNode<int>(255, 0, 255);

    [Menu("RadiusSpawnMonsters")]
    public RangeNode<int> Radius { get; set; } = new RangeNode<int>(30, 0, 255);
}