using System;

namespace DatabaseLayer
{

    /// <summary>
    /// operation type(insert、get、update、delete、persistent、query)
    /// </summary>
    internal enum ActionType
    {
        NoAction,

        RetrieveObject,

        InsertObject,

        UpdateObject,

        DeleteObject,

        RetrieveCriteria,

        UpdateCriteria,

        DeleteCriteria,









        ProcessCriteria,



        /// <summary>
        /// 新增
        /// </summary>
        InsertCommand,

        /// <summary>
        /// 获取
        /// </summary>
        SelectCommand,

        /// <summary>
        /// 修改
        /// </summary>
        UpdateCommand,

        /// <summary>
        /// 删除
        /// </summary>
        DeleteCommand,

        /// <summary>
        /// 持久化
        /// </summary>
        PesistentCriteria,







        /// <summary>
        /// 查询
        /// </summary>
        QueryCommand
    }

}



