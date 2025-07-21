using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFinder;

public partial class EntityFinder
{
    private static Dictionary<string, Color> _sharpDxColors;

    private List<EntityInfo> _entityMetaDataToFind = [];
    private List<EntityData> entitiesData = [];

    private string presetName = string.Empty;

    private string configDir = string.Empty;
    private int selectedPresetIndex = 0;
    private string[] presetFiles = Array.Empty<string>();

    private const string CONFIG_LOCAL_DIR = "config/EntityFinder";
    private const int SEGMENTS_CIRCLE = 20;
}
