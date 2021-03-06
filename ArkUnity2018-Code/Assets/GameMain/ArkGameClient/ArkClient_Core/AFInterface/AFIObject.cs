using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFCoreEx
{
    public abstract class AFIObject
    {
        public enum CLASS_EVENT_TYPE
        {
			OBJECT_DESTROY,
            OBJECT_CREATE,
			OBJECT_LOADDATA,
			OBJECT_CREATE_FINISH,
        }

        public delegate void ClassEventHandler(AFIDENTID self, int nContainerID, int nGroupID, CLASS_EVENT_TYPE eType, string strClassName, string strConfigIndex);

        public abstract bool Init();
        public abstract bool Shut();

        public abstract bool UpData(float fLastTime, float fAllTime);

        ///////////////////////////////////////////////////////////////////////
        public abstract AFIDENTID Self();
		public abstract int ContainerID();
		public abstract int GroupID();
        public abstract string ClassName();
        public abstract string ConfigIndex();
        // public abstract bool AddHeartBeat(  string strHeartBeatName, HEART_BEAT_FUNC cb,  AFIDataList& var,  float fTime,  int nCount );

        public abstract bool FindHeartBeat(string strHeartBeatName);

        public abstract bool RemoveHeartBeat(string strHeartBeatName);

        //////////////////////////////////////////////////
        // 
        //     public abstract bool AddRecordCallBack(  string strRecordName,  RECORD_EVENT_FUNC cb );
        // 
        //     public abstract bool AddPropertyCallBack(  string strCriticalName,  PROPERTY_EVENT_FUNC cb );

        /////////////////////////////////////////////////////////////////

        public abstract bool FindProperty(string strPropertyName);

        public abstract bool SetPropertyInt(string strPropertyName, Int64 nValue);
        public abstract bool SetPropertyFloat(string strPropertyName, float fValue);
        public abstract bool SetPropertyDouble(string strPropertyName, double dwValue);
        public abstract bool SetPropertyString(string strPropertyName, string strValue);
        public abstract bool SetPropertyObject(string strPropertyName, AFIDENTID obj);

        public abstract Int64 QueryPropertyInt(string strPropertyName);
        public abstract float QueryPropertyFloat(string strPropertyName);
        public abstract double QueryPropertyDouble(string strPropertyName);
        public abstract string QueryPropertyString(string strPropertyName);
        public abstract AFIDENTID QueryPropertyObject(string strPropertyName);

        public abstract bool FindRecord(string strRecordName);

        public abstract bool SetRecordInt(string strRecordName, int nRow, int nCol, Int64 nValue);
        public abstract bool SetRecordFloat(string strRecordName, int nRow, int nCol, float fValue);
        public abstract bool SetRecordDouble(string strRecordName, int nRow, int nCol, double dwValue);
        public abstract bool SetRecordString(string strRecordName, int nRow, int nCol, string strValue);
        public abstract bool SetRecordObject(string strRecordName, int nRow, int nCol, AFIDENTID obj);

        public abstract Int64 QueryRecordInt(string strRecordName, int nRow, int nCol);
        public abstract float QueryRecordFloat(string strRecordName, int nRow, int nCol);
        public abstract double QueryRecordDouble(string strRecordName, int nRow, int nCol);
        public abstract string QueryRecordString(string strRecordName, int nRow, int nCol);
        public abstract AFIDENTID QueryRecordObject(string strRecordName, int nRow, int nCol);

        public abstract AFIRecordManager GetRecordManager();
        public abstract AFIHeartBeatManager GetHeartBeatManager();
        public abstract AFIPropertyManager GetPropertyManager();
		public abstract AFIEventManager GetEventManager();
    }
}