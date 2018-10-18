using System;
using DatabaseLayer.Core;

namespace DatabaseLayer.SqlQuery
{

    /// <summary>
    /// OrGroup
    /// </summary>
    internal class OrGroup : Filter, IOrGroup
    {

        public OrGroup() { }

        public OrGroup(ClassMap clsMap)
            : base(clsMap) => m_BlnOperator = LogicOperator.OR;

        public string GetMessage() => "OR GROUP";


    }
}
