using UniOwl.Components;

namespace UniOwl.Celestials
{
    public abstract class PlanetComponent : ScriptableComponentWithPrefab
    {
        public PlanetObject Planet => List as PlanetObject;
    }
}