using System;
using DatabaseLayer.Core;
//using DatabaseLayer.PersistenceConfig;

namespace DatabaseLayer.SqlQuery
{

    /// <summary>
    /// AndGroup
    /// </summary>
    internal class AndGroup : Filter, IAndGroup
    {
        public AndGroup() { }        

        public AndGroup(ClassMap clsMap)
            : base(clsMap) => m_BlnOperator = LogicOperator.AND;

        public string GetMessage()=> "AND GROUP";

    }
}
