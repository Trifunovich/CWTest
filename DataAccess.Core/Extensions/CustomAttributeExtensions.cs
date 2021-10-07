using System.Linq;
using System.Reflection;

namespace DataAccess.Core.Extensions
{
  public static class CustomAttributeExtensions
  {
    public static T[] GetCustomAttributesFromClass<T>(this MemberInfo element, bool inherit)
    {
      var attTypes = element.GetCustomAttributes(true).Where(x => x.GetType() == typeof(T)).Select(x => (T)x).ToArray();
      return attTypes;
    }
  }
}