using System;

namespace CWTest.Core.DataManipulation
{
  public class GuidAbstraction : ITypedIdAbstraction<Guid>
  {
    private Guid _id;
    public Guid Value => _id;

    public string ValueAsString => _id.ToString();

    public GuidAbstraction(Guid id)
    {
      _id = id;
    }
  }
}
