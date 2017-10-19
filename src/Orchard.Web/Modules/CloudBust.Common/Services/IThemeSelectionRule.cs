using Orchard;

namespace CloudBust.Common.Services {
    public interface IThemeSelectionRule : IDependency {
        bool Matches(string name, string criterion);
    }
}