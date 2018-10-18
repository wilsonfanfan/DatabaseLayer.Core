using System;

namespace DatabaseLayer
{

    public interface IPersistenceCursor
    {

        #region method

        PersistentObject PreviousObject();

        PersistentObject NextObject();

        PersistentObject MoveObject(int point);

        void MoveFirst();

        void MoveLast();

        #endregion

        #region property

        bool HasObject();

        int Count { get; }

        #endregion

    }
}
