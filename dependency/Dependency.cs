
namespace ConfigManagePrak2.dependency
{
    public class Dependency
    {
        public string Name { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public int LoadPriority { get; set; } = 0;
        public List<Dependency> Dependencies { get; set; } = [];

        public override string ToString()
        {
            return $"{Name} {Version}";
        }
    }
}
