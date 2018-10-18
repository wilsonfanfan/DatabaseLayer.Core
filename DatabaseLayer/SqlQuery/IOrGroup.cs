using System;

namespace DatabaseLayer
{

    public interface IOrGroup : IFilter, IJoinOn, IWhere
    {
        string GetMessage();
    }
}
