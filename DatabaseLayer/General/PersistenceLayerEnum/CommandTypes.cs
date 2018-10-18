using System;

namespace DatabaseLayer
{

    /// <summary>
    /// operation type(insert、get、update、delete、persistent、query)
    /// </summary>
    internal enum CommandTypes
    {
        InsertCommand,

        SelectCommand,

        UpdateCommand,

        DeleteCommand,

        PesistentCriteria,

        NoCommand,

        RetrieveObject,

        InsertObject,

        UpdateObject,

        DeleteObject,

        UpdateCriteria,

        DeleteCriteria,

        QueryCommand
    }

}



