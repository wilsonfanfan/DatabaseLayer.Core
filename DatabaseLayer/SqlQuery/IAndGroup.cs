using System;

namespace DatabaseLayer
{

    public interface IAndGroup : IFilter, IJoinOn, IWhere
    {
        string GetMessage();
    }
}
