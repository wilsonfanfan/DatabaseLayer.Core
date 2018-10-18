using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using DatabaseLayer;
using DatabaseLayer.Entity;

namespace Entity
{

    /// <summary>
    /// Generation Date:2018-09-13
    /// Author:Wilson.Fan
    /// </summary>
    public class r_LogManager : ManagerBase
    {

        #region 基础成员

        /// <summary>
        /// (基础成员)新增实体对象
        /// </summary>
        /// <param name="obj">实体对象</param>
        public void InsEntityObject(r_Log obj)
        {
            try
            {
                obj.Insert();
            }
            catch (PersistenceLayerException exp)
            {
                ErrorLog.Write(exp, exp.ErrorMessage);
                throw new Exception(exp.ErrorMessage);
            }
        }

        /// <summary>
        /// (基础成员)新增实体对象(事务中)
        /// </summary>
        /// <param name="obj">实体对象</param>
        /// <param name="ts">事务</param>
        public void InsEntityObject(r_Log obj, Transaction ts)
        {
            try
            {
                obj.Insert(ts);
            }
            catch (PersistenceLayerException exp)
            {
                throw new Exception(exp.ErrorMessage);
            }
        }

        /// <summary>
        /// (基础成员)修改实体对象
        /// </summary>
        /// <param name="obj">实体对象</param>
        public void UpdEntityObject(r_Log obj)
        {
            try
            {
                obj.Update();
            }
            catch (PersistenceLayerException exp)
            {
                ErrorLog.Write(exp, exp.ErrorMessage);
                throw new Exception(exp.ErrorMessage);
            }
        }

        /// <summary>
        /// (基础成员)修改实体对象(事务中)
        /// </summary>
        /// <param name="obj">实体对象</param>
        /// <param name="ts">事务</param>
        public void UpdEntityObject(r_Log obj, Transaction ts)
        {
            try
            {
                obj.Update(ts);
            }
            catch (PersistenceLayerException exp)
            {
                throw new Exception(exp.ErrorMessage);
            }
        }

        /// <summary>
        /// (基础成员)删除实体对象
        /// </summary>
        /// <param name="logid"></param>
        public void DelEntityObject(System.Int32 logid)
        {
            r_Log obj = new r_Log();
            obj.LogID = logid;
            try
            {
                obj.Delete();
            }
            catch (PersistenceLayerException exp)
            {
                ErrorLog.Write(exp, exp.ErrorMessage);
                throw new Exception(exp.ErrorMessage);
            }
        }

        /// <summary>
        /// (基础成员)删除实体对象(事务中)
        /// </summary>
        /// <param name="logid"></param>
        /// <param name="ts">事务</param>
        public void DelEntityObject(System.Int32 logid, Transaction ts)
        {
            r_Log obj = new r_Log();
            obj.LogID = logid;
            try
            {
                obj.Delete(ts);
            }
            catch (PersistenceLayerException exp)
            {
                throw new Exception(exp.ErrorMessage);
            }
        }

        /// <summary>
        /// (基础成员)删除实体集合
        /// </summary>
        /// <returns></returns>
        public int DelEntityObjects()
        {
            try
            {
                DeleteCriteria dc = GetDeleteCriteria();
                return dc.Execute();
            }
            catch (PersistenceLayerException exp)
            {
                ErrorLog.Write(exp, exp.ErrorMessage);
                throw new Exception(exp.ErrorMessage);
            }
        }

        /// <summary>
        /// (基础成员)删除实体集合
        /// </summary>
        /// <param name='dic'></param>
        /// <returns></returns>
        public int DelEntityObjects(Dictionary<string, object> dic)
        {
            try
            {
                DeleteCriteria dc = GetDeleteCriteria(dic);
                return dc.Execute();
            }
            catch (PersistenceLayerException exp)
            {
                ErrorLog.Write(exp, exp.ErrorMessage);
                throw new Exception(exp.ErrorMessage);
            }
        }

        /// <summary>
        /// (基础成员)删除实体集合(事务中)
        /// </summary>
        /// <param name="ts">事务</param>
        /// <returns></returns>
        public int DelEntityObjects(Transaction ts)
        {
            try
            {
                DeleteCriteria dc = GetDeleteCriteria();
                return dc.Execute(ts);
            }
            catch (PersistenceLayerException exp)
            {
                throw new Exception(exp.ErrorMessage);
            }
        }

        /// <summary>
        /// (基础成员)删除实体集合(事务中)
        /// </summary>
        /// <param name='dic'></param>
        /// <param name="ts">事务</param>
        /// <returns></returns>
        public int DelEntityObjects(Dictionary<string, object> dic, Transaction ts)
        {
            try
            {
                DeleteCriteria dc = GetDeleteCriteria(dic);
                return dc.Execute(ts);
            }
            catch (PersistenceLayerException exp)
            {
                throw new Exception(exp.ErrorMessage);
            }
        }

        /// <summary>
        /// (基础成员)取回实体对象
        /// </summary>
        /// <param name="logid"></param>
        /// <returns></returns>
        public r_Log GetEntityObject(System.Int32 logid)
        {
            r_Log obj = new r_Log();
            obj.LogID = logid;
            obj.Retrieve();
            return obj;
        }

        /// <summary>
        /// (基础成员)取回实体对象(事务中)
        /// </summary>
        /// <param name="logid"></param>
        /// <param name="ts">事务</param>
        /// <returns></returns>
        public r_Log GetEntityObject(System.Int32 logid, Transaction ts)
        {
            r_Log obj = new r_Log();
            obj.LogID = logid;
            obj.Retrieve(ts);
            return obj;
        }

        /// <summary>
        /// (基础成员)取回实体集合
        /// </summary>
        /// <returns>IList</returns>
        public IList GetEntityObjects()
        {
            RetrieveCriteria rc = GetRetrieveCriteria();
            return rc.GetCollection();
        }

        /// <summary>
        /// (基础成员)取回实体集合
        /// </summary>
        /// <param name='dic'></param>
        /// <returns>IList</returns>
        public IList GetEntityObjects(Dictionary<string, object> dic)
        {
            RetrieveCriteria rc = GetRetrieveCriteria(dic);
            return rc.GetCollection();
        }

        /// <summary>
        /// (基础成员)取回实体集合
        /// </summary>
        /// <param name="ts">事务</param>
        /// <returns>IList</returns>
        public IList GetEntityObjects(Transaction ts)
        {
            RetrieveCriteria rc = GetRetrieveCriteria();
            return rc.GetCollection(ts);
        }

        /// <summary>
        /// (基础成员)取回实体集合
        /// </summary>
        /// <param name='dic'></param>
        /// <param name="ts">事务</param>
        /// <returns>IList</returns>
        public IList GetEntityObjects(Dictionary<string, object> dic, Transaction ts)
        {
            RetrieveCriteria rc = GetRetrieveCriteria(dic);
            return rc.GetCollection(ts);
        }

        /// <summary>
        /// (基础成员)取回实体数据
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable()
        {
            RetrieveCriteria rc = GetRetrieveCriteria();
            return rc.GetDataTable();
        }

        /// <summary>
        /// (基础成员)取回实体数据
        /// </summary>
        /// <param name='dic'></param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(Dictionary<string, object> dic)
        {
            RetrieveCriteria rc = GetRetrieveCriteria(dic);
            return rc.GetDataTable();
        }

        /// <summary>
        /// (基础成员)取回实体数据
        /// </summary>
        /// <param name="ts">事务</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(Transaction ts)
        {
            RetrieveCriteria rc = GetRetrieveCriteria();
            return rc.GetDataTable(ts);
        }

        /// <summary>
        /// (基础成员)取回实体数据
        /// </summary>
        /// <param name='dic'></param>
        /// <param name="ts">事务</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(Dictionary<string, object> dic, Transaction ts)
        {
            RetrieveCriteria rc = GetRetrieveCriteria(dic);
            return rc.GetDataTable(ts);
        }

        #endregion


        #region 私有方法


        /// <summary>
        /// (私有方法)字段查询
        /// </summary>
        /// <returns></returns>
        private RetrieveCriteria GetRetrieveCriteria()
        {
            return new RetrieveCriteria(typeof(r_Log));
        }

        /// <summary>
        /// (私有方法)字段查询
        /// </summary>
        /// <param name='dic'></param>
        /// <returns></returns>
        private RetrieveCriteria GetRetrieveCriteria(Dictionary<string, object> dic)
        {
            RetrieveCriteria rc = new RetrieveCriteria(typeof(r_Log));

            if (dic != null && dic.Count > 0)
            {
                IFilter filter = rc.GetFilter();
                object dicObject;
                if (dic.TryGetValue(r_Log.F_LOGID, out dicObject)) { filter.AddEqualTo(r_Log.F_LOGID, dicObject.ToString()); }
                if (dic.TryGetValue(r_Log.F_INSERTDATETIME, out dicObject)) { filter.AddEqualTo(r_Log.F_INSERTDATETIME, dicObject.ToString()); }
                if (dic.TryGetValue(r_Log.F_DATACOUNT, out dicObject)) { filter.AddEqualTo(r_Log.F_DATACOUNT, dicObject.ToString()); }
                if (dic.TryGetValue(r_Log.F_QUERYTIMESTAMP, out dicObject)) { filter.AddEqualTo(r_Log.F_QUERYTIMESTAMP, dicObject.ToString()); }
                if (dic.TryGetValue(r_Log.F_INGESTTIMESTAMP, out dicObject)) { filter.AddEqualTo(r_Log.F_INGESTTIMESTAMP, dicObject.ToString()); }
                if (dic.TryGetValue(r_Log.F_USERID, out dicObject)) { filter.AddEqualTo(r_Log.F_USERID, dicObject.ToString()); }
                if (dic.TryGetValue(r_Log.F_STATUS, out dicObject)) { filter.AddEqualTo(r_Log.F_STATUS, dicObject.ToString()); }
                if (dic.TryGetValue(r_Log.F_REMARK, out dicObject)) { filter.AddEqualTo(r_Log.F_REMARK, dicObject.ToString()); }
                if (dic.TryGetValue(r_Log.F_ACTIVITYDATE, out dicObject)) { filter.AddEqualTo(r_Log.F_ACTIVITYDATE, dicObject.ToString()); }
            }
            return rc;
        }

        /// <summary>
        /// (私有方法)字段删除
        /// </summary>
        /// <returns></returns>
        private DeleteCriteria GetDeleteCriteria()
        {
            return new DeleteCriteria(typeof(r_Log));
        }

        /// <summary>
        /// (私有方法)字段删除
        /// </summary>
        /// <param name='dic'></param>
        /// <returns></returns>
        private DeleteCriteria GetDeleteCriteria(Dictionary<string, object> dic)
        {
            DeleteCriteria dc = new DeleteCriteria(typeof(r_Log));

            if (dic != null && dic.Count > 0)
            {
                IFilter filter = dc.GetFilter();
                object dicObject;
                if (dic.TryGetValue(r_Log.F_LOGID, out dicObject)) { filter.AddEqualTo(r_Log.F_LOGID, dicObject.ToString()); }
                if (dic.TryGetValue(r_Log.F_INSERTDATETIME, out dicObject)) { filter.AddEqualTo(r_Log.F_INSERTDATETIME, dicObject.ToString()); }
                if (dic.TryGetValue(r_Log.F_DATACOUNT, out dicObject)) { filter.AddEqualTo(r_Log.F_DATACOUNT, dicObject.ToString()); }
                if (dic.TryGetValue(r_Log.F_QUERYTIMESTAMP, out dicObject)) { filter.AddEqualTo(r_Log.F_QUERYTIMESTAMP, dicObject.ToString()); }
                if (dic.TryGetValue(r_Log.F_INGESTTIMESTAMP, out dicObject)) { filter.AddEqualTo(r_Log.F_INGESTTIMESTAMP, dicObject.ToString()); }
                if (dic.TryGetValue(r_Log.F_USERID, out dicObject)) { filter.AddEqualTo(r_Log.F_USERID, dicObject.ToString()); }
                if (dic.TryGetValue(r_Log.F_STATUS, out dicObject)) { filter.AddEqualTo(r_Log.F_STATUS, dicObject.ToString()); }
                if (dic.TryGetValue(r_Log.F_REMARK, out dicObject)) { filter.AddEqualTo(r_Log.F_REMARK, dicObject.ToString()); }
                if (dic.TryGetValue(r_Log.F_ACTIVITYDATE, out dicObject)) { filter.AddEqualTo(r_Log.F_ACTIVITYDATE, dicObject.ToString()); }
            }
            return dc;
        }

        #endregion

    }
}
