using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.IO;
using AFTCPClient;
using Google.Protobuf;
using AFMsg;
using AFCoreEx;

namespace PlayerNetClient
{
public class PlayerReciver
{
    public PlayerNet mxPlayerNet ;
    public PlayerReciver(PlayerNet xPlayerNet)
    {
        mxPlayerNet = xPlayerNet;
    }

    ~PlayerReciver()
    {
    }

    static public AFCoreEx.AFIDENTID PBToAF(AFMsg.PBGUID xID)
    {
        AFCoreEx.AFIDENTID xIdent = new AFCoreEx.AFIDENTID();
        xIdent.nHead64 = xID.High;
        xIdent.nData64 = xID.Low;

        return xIdent;
    }

    static public AFCoreEx.AFIDataList.Var_Data PBPropertyToData(AFMsg.PBNodeData xDataNode)
    {
        AFCoreEx.AFIDataList.Var_Data xData = new AFCoreEx.AFIDataList.Var_Data();
        xData.nType  = (AFIDataList.VARIANT_TYPE)xDataNode.DataType;
        switch(xData.nType)
        {
        case AFIDataList.VARIANT_TYPE.VTYPE_BOOLEAN:
            {
                xData.mData = xDataNode.VariantData.BoolValue;
            }
            break;
        case AFIDataList.VARIANT_TYPE.VTYPE_INT:
            {
                xData.mData = xDataNode.VariantData.IntValue;
            }
            break;

        case AFIDataList.VARIANT_TYPE.VTYPE_INT64:
            {
                xData.mData = xDataNode.VariantData.Int64Value;
            }
            break;

        case AFIDataList.VARIANT_TYPE.VTYPE_FLOAT:
            {
                xData.mData = xDataNode.VariantData.FloatValue;
            }
            break;

        case AFIDataList.VARIANT_TYPE.VTYPE_DOUBLE:
            {
                xData.mData = xDataNode.VariantData.DoubleValue;
            }
            break;

        case AFIDataList.VARIANT_TYPE.VTYPE_STRING:
            {
                xData.mData = xDataNode.VariantData.StrValue;
            }
            break;

        case AFIDataList.VARIANT_TYPE.VTYPE_OBJECT:
            {

                xData.mData = PBToAF(xDataNode.VariantData.GuidValue);
            }
            break;
        default:
            break;
        }
        return xData;
    }

    static public AFCoreEx.AFIDataList.Var_Data PBRecordToData(AFMsg.PBCellData xTableCellData, ref int nRow, ref int col)
    {
        AFCoreEx.AFIDataList.Var_Data xData = new AFCoreEx.AFIDataList.Var_Data();
        nRow = xTableCellData.Row;
        col = xTableCellData.Col;
        switch((AFIDataList.VARIANT_TYPE)xTableCellData.DataType)
        {
        case AFIDataList.VARIANT_TYPE.VTYPE_BOOLEAN:
            {
                xData.mData = xTableCellData.VariantData.BoolValue;
            }
            break;
        case AFIDataList.VARIANT_TYPE.VTYPE_INT:
            {
                xData.mData = xTableCellData.VariantData.IntValue;
            }
            break;
        case AFIDataList.VARIANT_TYPE.VTYPE_INT64:
            {
                xData.mData = xTableCellData.VariantData.Int64Value;
            }
            break;
        case AFIDataList.VARIANT_TYPE.VTYPE_FLOAT:
            {
                xData.mData = xTableCellData.VariantData.FloatValue;
            }
            break;

        case AFIDataList.VARIANT_TYPE.VTYPE_DOUBLE:
            {
                xData.mData = xTableCellData.VariantData.DoubleValue;
            }
            break;
        case AFIDataList.VARIANT_TYPE.VTYPE_STRING:
            {
                xData.mData = xTableCellData.VariantData.StrValue;
            }
            break;
        case AFIDataList.VARIANT_TYPE.VTYPE_OBJECT:
            {
                xData.mData = PBToAF(xTableCellData.VariantData.GuidValue);
            }
            break;
        }
        return xData;
    }

    public  void OnDisConnect()
    {

    }

    public  void OnConnect()
    {

    }
    public void Init()
    {

        mxPlayerNet.mxNet.RegisteredConnectDelegation(OnConnect);
        mxPlayerNet.mxNet.RegisteredDisConnectDelegation(OnDisConnect);

        mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EgmiAckLogin, EGMI_ACK_LOGIN);
        mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EgmiAckWorldList, EGMI_ACK_WORLD_LIST);
        mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EgmiEventResult, EGMI_EVENT_RESULT);
        mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EgmiAckRoleList, EGMI_ACK_ROLE_LIST);
        mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EgmiAckConnectWorld, EGMI_ACK_CONNECT_WORLD);
        mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EgmiAckConnectKey, EGMI_ACK_CONNECT_KEY);
        mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EgmiAckSelectServer, EGMI_ACK_SELECT_SERVER);
        mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EgmiAckSwapScene, EGMI_ACK_SWAP_SCENE);

        mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EgmiAckEntityEnter, EGMI_ACK_OBJECT_ENTRY);
        mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EgmiAckEntityLeave, EGMI_ACK_OBJECT_LEAVE);
        mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EgmiAckMove, EGMI_ACK_MOVE);
        mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EgmiAckMoveImmune, EGMI_ACK_MOVE_IMMUNE);

        mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EgmiAckNodeData, EGMI_ACK_PROPERTY_DATA);

        mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EgmiAckTableData, EGMI_ACK_RECORD_DATA);
        mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EgmiAckSwapRow, EGMI_ACK_SWAP_ROW);
        mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EgmiAckAddRow, EGMI_ACK_ADD_ROW);
        mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EgmiAckRemoveRow, EGMI_ACK_REMOVE_ROW);

        mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EgmiAckEntityDataTableEnter, EGMI_ACK_OBJECT_RECORD_ENTRY);
        mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EgmiAckEntityDataNodeEnter, EGMI_ACK_OBJECT_PROPERTY_ENTRY);


        mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EgmiAckSkillObjectx, EGMI_ACK_SKILL_OBJECTX);
        mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EgmiAckChat, EGMI_ACK_CHAT);
    }

        //private void ReceiveMsg<T>(MemoryStream stream, ref T xData) where T : MessageParser
        //{
        //    xData.Parser.ParseFrom(stream);
        //}

        private IMessage ReceiveMsg<TMessage>(MemoryStream stream, TMessage xData) where TMessage : MessageParser
        {
            return xData.ParseFrom(stream);
        }
        //private IMessage ReceiveMsg2<TParser, TMessage>(MemoryStream stream, TParser xData) 
        //    where TMessage : IMessage<TMessage>
        //    where TParser : MessageParser
        //{
        //    TMessage msg = xData.ParseFrom(stream) as TMessage;
        //    return msg;
        //}
        
        private void EGMI_EVENT_RESULT(MsgHead head, MemoryStream stream)
        {
            //OnResultMsg
            AFMsg.AckEventResult xResultCode = new AFMsg.AckEventResult();
            xResultCode = AFMsg.AckEventResult.Parser.ParseFrom(stream);
            //ReceiveMsg(stream, ref xResultCode);
            AFMsg.EGameEventCode eEvent = xResultCode.EventCode;

            mxPlayerNet.mxNet.DoResultCodeDelegation((int)eEvent);
        }

        private void EGMI_ACK_LOGIN(MsgHead head, MemoryStream stream)
        {
            AFMsg.AckEventResult xData = new AFMsg.AckEventResult();
            xData = AFMsg.AckEventResult.Parser.ParseFrom(stream);
            //ReceiveMsg(stream, ref xData);

            if (EGameEventCode.EgecAccountSuccess == xData.EventCode)
            {
                mxPlayerNet.ChangePlayerState(PlayerNet.PLAYER_STATE.E_PLAYER_LOGIN_SUCCESSFUL);
            }
        }

        private void EGMI_ACK_WORLD_LIST(MsgHead head, MemoryStream stream)
        {
            AFMsg.AckServerList xData = new AFMsg.AckServerList();
            xData = AFMsg.AckServerList.Parser.ParseFrom(stream);
            //ReceiveMsg(stream, ref xData);

            if (ReqServerListType.RsltWorldServer == xData.Type)
            {
                for(int i = 0; i < xData.Info.Count; ++i)
                {
                    ServerInfo info = xData.Info[i];
                    mxPlayerNet.aWorldList.Add(info);
                }
                mxPlayerNet.ChangePlayerState(PlayerNet.PLAYER_STATE.E_PLAYER_WORLD_LIST_SUCCESSFUL_WAITING_SELECT_WORLD);
            }
            else if(ReqServerListType.RsltGamesErver == xData.Type)
            {
                for(int i = 0; i < xData.Info.Count; ++i)
                {
                    ServerInfo info = xData.Info[i];
                    mxPlayerNet.aServerList.Add(info);
                }
            }
        }

        private void EGMI_ACK_CONNECT_WORLD(MsgHead head, MemoryStream stream)
        {
            mxPlayerNet.mxNet.Disconnect();

            AFMsg.AckConnectWorldResult xData = new AFMsg.AckConnectWorldResult();
            xData = AFMsg.AckConnectWorldResult.Parser.ParseFrom(stream);
            //ReceiveMsg(stream, ref xData);

            mxPlayerNet.strKey = xData.WorldKey;
            mxPlayerNet.strWorldIP = xData.WorldIp;
            mxPlayerNet.nWorldPort = xData.WorldPort;
            mxPlayerNet.ChangePlayerState(PlayerNet.PLAYER_STATE.E_PLAYER_GET_WORLD_KEY_SUCCESSFUL);
        }

        private void EGMI_ACK_CONNECT_KEY(MsgHead head, MemoryStream stream)
        {
            AFMsg.AckEventResult xData = new AFMsg.AckEventResult();
            xData = AFMsg.AckEventResult.Parser.ParseFrom(stream);
            //ReceiveMsg(stream, ref xData);

            if (xData.EventCode == EGameEventCode.EgecVerifyKeySuccess)
            {
                //验证成功
                mxPlayerNet.ChangePlayerState(PlayerNet.PLAYER_STATE.E_VERIFY_KEY_SUCCESS_FULL);
                mxPlayerNet.nMainRoleID = PBToAF(xData.EventObject);

                //申请世界内的服务器列表
                PlayerSender sender = mxPlayerNet.mxSender;
                if(null != sender)
                {
                    sender.RequireServerList();
                }
            }
        }

        private void EGMI_ACK_SELECT_SERVER(MsgHead head, MemoryStream stream)
        {
            AFMsg.AckEventResult xData = new AFMsg.AckEventResult();
            xData = AFMsg.AckEventResult.Parser.ParseFrom(stream);
            //ReceiveMsg(stream, ref xData);

            if (xData.EventCode == EGameEventCode.EgecSelectserverSuccess)
            {
                PlayerSender sender = mxPlayerNet.mxSender;
                if(null != sender)
                {
                    sender.RequireRoleList(mxPlayerNet.strAccount, mxPlayerNet.nServerID);
                }
            }
        }

        private void EGMI_ACK_ROLE_LIST(MsgHead head, MemoryStream stream)
        {
            AFMsg.AckRoleLiteInfoList xData = new AFMsg.AckRoleLiteInfoList();
            xData = AFMsg.AckRoleLiteInfoList.Parser.ParseFrom(stream);
            //ReceiveMsg(stream, ref xData);

            mxPlayerNet.aCharList.Clear();
            for(int i = 0; i < xData.CharData.Count; ++i)
            {
                AFMsg.RoleLiteInfo info = xData.CharData[i];
                mxPlayerNet.aCharList.Add(info);
            }

            if(PlayerNet.PLAYER_STATE.E_WAIT_SELECT_ROLE != mxPlayerNet.GetPlayerState())
            {

                AFCDataList varList = new AFCDataList();
                varList.AddString("SelectScene");
                AFCLogicEvent.Instance.DoEvent((int)ClientEventDefine.EventDefine_LoadSelectRole, varList);
            }

            mxPlayerNet.ChangePlayerState(PlayerNet.PLAYER_STATE.E_GETROLELIST_SUCCESSFUL);
        }

        private void EGMI_ACK_SWAP_SCENE(MsgHead head, MemoryStream stream)
        {
            mxPlayerNet.ChangePlayerState(PlayerNet.PLAYER_STATE.E_PLAYER_GAMEING);

            AFMsg.ReqAckSwapScene xData = new AFMsg.ReqAckSwapScene();
            xData = AFMsg.ReqAckSwapScene.Parser.ParseFrom(stream);
            //ReceiveMsg(stream, ref xData);

            //AFCRenderInterface.Instance.LoadScene(xData.scene_id, xData.x, xData.y, xData.z);

            AFCDataList varList = new AFCDataList();
            varList.AddInt64(xData.SceneId);
            varList.AddFloat(xData.X);
            varList.AddFloat(xData.Y);
            varList.AddFloat(xData.Z);

            AFCLogicEvent.Instance.DoEvent((int)ClientEventDefine.EventDefine_Swap_Scene, varList);
        }

        private void EGMI_ACK_OBJECT_ENTRY(MsgHead head, MemoryStream stream)
        {
            AFMsg.AckEntityEnterList xData = new AFMsg.AckEntityEnterList();
            xData = AFMsg.AckEntityEnterList.Parser.ParseFrom(stream);
            //ReceiveMsg(stream, ref xData);

            for (int i = 0; i < xData.EntityList.Count; ++i)
            {
                AFMsg.EntityEnterInfo xInfo = xData.EntityList[i];

                AFIDataList var = new AFCDataList();
                var.AddString("X");
                var.AddFloat(xInfo.Pos.X);
                var.AddString("Y");
                var.AddFloat(xInfo.Pos.Y);
                var.AddString("Z");
                var.AddFloat(xInfo.Pos.Z);
                AFIObject xGO = AFCKernel.Instance.CreateObject(PBToAF(xInfo.ObjectGuid), xInfo.SceneId, 0, xInfo.ClassId, xInfo.ConfigId, var);
                if(null == xGO)
                {
                    continue;
                }
            }
        }

        private void EGMI_ACK_OBJECT_LEAVE(MsgHead head, MemoryStream stream)
        {
            AFMsg.AckEntityLeaveList xData = new AFMsg.AckEntityLeaveList();
            xData = AFMsg.AckEntityLeaveList.Parser.ParseFrom(stream);
            //ReceiveMsg(stream, ref xData);

            for (int i = 0; i < xData.EntityList.Count; ++i)
            {
                AFCKernel.Instance.DestroyObject(PBToAF(xData.EntityList[i]));
            }
        }

        private void EGMI_ACK_MOVE(MsgHead head, MemoryStream stream)
        {
            AFMsg.ReqAckPlayerMove xData = new AFMsg.ReqAckPlayerMove();
            xData = AFMsg.ReqAckPlayerMove.Parser.ParseFrom(stream);
            //ReceiveMsg(stream, ref xData);
            if (xData.TargetPos.Count <= 0)
            {
                return;
            }
            float fSpeed = AFCKernel.Instance.QueryPropertyInt(PBToAF(xData.Mover), "MOVE_SPEED") / 10000.0f;

            AFCDataList varList = new AFCDataList();
            varList.AddObject(PBToAF(xData.Mover));
            varList.AddFloat(xData.TargetPos[0].X);
            varList.AddFloat(xData.TargetPos[0].Y);
            varList.AddFloat(xData.TargetPos[0].Z);
            varList.AddFloat(fSpeed);

            AFCLogicEvent.Instance.DoEvent((int)ClientEventDefine.EventDefine_MoveTo, varList);

            //AFCRenderInterface.Instance.MoveTo(PBToAF(xData.mover), new Vector3(xData.target_pos[0].x, xData.target_pos[0].y, xData.target_pos[0].z), fSpeed, true);
        }

        private void EGMI_ACK_MOVE_IMMUNE(MsgHead head, MemoryStream stream)
        {
            AFMsg.ReqAckPlayerMove xData = ReceiveMsg(stream, ReqAckPlayerMove.Parser) as ReqAckPlayerMove;

            if (xData.TargetPos.Count <= 0)
            {
                return;
            }

            //其实就是jump
            float fSpeed = AFCKernel.Instance.QueryPropertyInt(PBToAF(xData.Mover), "MOVE_SPEED") / 10000.0f;
            fSpeed *= 1.5f;

            AFCDataList varList = new AFCDataList();
            varList.AddObject(PBToAF(xData.Mover));
            varList.AddFloat(xData.TargetPos[0].X);
            varList.AddFloat(xData.TargetPos[0].Y);
            varList.AddFloat(xData.TargetPos[0].Z);
            varList.AddFloat(fSpeed);

            AFCLogicEvent.Instance.DoEvent((int)ClientEventDefine.EVENTDEFINE_MOVE_IMMUNE, varList);

            //AFCRenderInterface.Instance.MoveImmuneBySpeed(PBToAF(xData.mover), new Vector3(xData.target_pos[0].x, xData.target_pos[0].y, xData.target_pos[0].z), fSpeed, true);

        }
    /////////////////////////////////////////////////////////////////////
    private void EGMI_ACK_PROPERTY_DATA(MsgHead head, MemoryStream stream)
    {
        AFMsg.EntityDataNode propertyData = ReceiveMsg(stream, EntityDataNode.Parser) as EntityDataNode;


        AFIObject go = AFCKernel.Instance.GetObject(PBToAF(propertyData.EntityId));
        AFIPropertyManager propertyManager = go.GetPropertyManager();

        for(int i = 0; i < propertyData.DataNodeList.Count; i++)
        {
            AFCoreEx.AFIDataList.Var_Data xData = PBPropertyToData(propertyData.DataNodeList[i]);
            AFIProperty property = propertyManager.GetProperty(propertyData.DataNodeList[i].NodeName);
            if(null == property)
            {

                AFIDataList varList = new AFCDataList();
                varList.AddDataObject(ref xData);

                property = propertyManager.AddProperty(propertyData.DataNodeList[i].NodeName, varList);
            }

            property.SetDataObject(ref xData);
        }
    }

    private void EGMI_ACK_RECORD_DATA(MsgHead head, MemoryStream stream)
    {
        AFMsg.EntityDataTable recordData = ReceiveMsg(stream, EntityDataTable.Parser) as EntityDataTable;

        AFIObject go = AFCKernel.Instance.GetObject(PBToAF(recordData.EntityId));
        AFIRecordManager recordManager = go.GetRecordManager();
        AFIRecord record = recordManager.GetRecord(recordData.TableName);

        for(int i = 0; i < recordData.TableCellList.Count; i++)
        {
            int nRow = -1;
            int nCol = -1;
            AFCoreEx.AFIDataList.Var_Data xRowData = PBRecordToData(recordData.TableCellList[i], ref nRow, ref nCol);
            record.SetDataObject(nRow, nCol, xRowData);
        }
    }

    private void EGMI_ACK_SWAP_ROW(MsgHead head, MemoryStream stream)
    {
        AFMsg.EntityDataTableSwap recordData =  ReceiveMsg(stream, EntityDataTableSwap.Parser) as EntityDataTableSwap;

        AFIObject go = AFCKernel.Instance.GetObject(PBToAF(recordData.EntityId));
        AFIRecordManager recordManager = go.GetRecordManager();
        AFIRecord record = recordManager.GetRecord(recordData.OriginTableName);


        //目前认为在同一张表中交换吧
        record.SwapRow(recordData.RowOrigin, recordData.RowTarget);
    }

    private void ADD_ROW(AFCoreEx.AFIDENTID self, string strRecordName, AFMsg.DataTableAddRow xAddStruct)
    {
        AFIObject go = AFCKernel.Instance.GetObject(self);
        AFIRecordManager xRecordManager = go.GetRecordManager();


        Hashtable recordVecDesc = new Hashtable();
        Hashtable recordVecData = new Hashtable();

        AFCoreEx.AFCDataList RowList = new AFCDataList();
        AFCoreEx.AFIDataList varListDesc = new AFCDataList();
        for(int k = 0; k < xAddStruct.CellList.Count; ++k)
        {
            AFMsg.PBCellData addStruct = (AFMsg.PBCellData)xAddStruct.CellList[k];
            if(addStruct.Col >= 0)
            {
                int nRow = -1;
                int nCol = -1;
                AFCoreEx.AFIDataList.Var_Data xRowData = PBRecordToData(addStruct, ref nRow, ref nCol);
                RowList.AddDataObject(ref xRowData);
                varListDesc.AddDataObject(ref xRowData);
            }
        }

        AFIRecord xRecord = xRecordManager.GetRecord(strRecordName);
        if(null == xRecord)
        {
            xRecord = xRecordManager.AddRecord(strRecordName, 512, varListDesc);
        }

        xRecord.AddRow(xAddStruct.Row, RowList);
    }

    private void EGMI_ACK_ADD_ROW(MsgHead head, MemoryStream stream)
    {
        AFMsg.EntityDataTableAddRow recordData =  ReceiveMsg(stream, EntityDataTableAddRow.Parser) as EntityDataTableAddRow;

        AFIObject go = AFCKernel.Instance.GetObject(PBToAF(recordData.EntityId));
        AFIRecordManager recordManager = go.GetRecordManager();

        for(int i = 0; i < recordData.RowData.Count; i++)
        {
            ADD_ROW(PBToAF(recordData.EntityId), recordData.TableName, recordData.RowData[i]);
        }
    }

    private void EGMI_ACK_REMOVE_ROW(MsgHead head, MemoryStream stream)
    {
        AFMsg.EntityDataTableRemove recordData = EntityDataTableRemove.Parser.ParseFrom(stream); //new AFMsg.ObjectRecordRemove();
        // ReceiveMsg(stream, ref recordData);

        AFIObject go = AFCKernel.Instance.GetObject(PBToAF(recordData.EntityId));
        AFIRecordManager recordManager = go.GetRecordManager();
        AFIRecord record = recordManager.GetRecord(recordData.TableName);

        for(int i = 0; i < recordData.RemoveRow.Count; i++)
        {
            record.Remove(recordData.RemoveRow[i]);
        }
    }

    private void EGMI_ACK_OBJECT_RECORD_ENTRY(MsgHead head, MemoryStream stream)
    {
        AFMsg.MultiEntityDataTableList xMultiObjectRecordData = MultiEntityDataTableList.Parser.ParseFrom(stream); // new AFMsg.MultiObjectRecordList();
       //ReceiveMsg(stream, ref xMultiObjectRecordData);

        for(int i = 0; i < xMultiObjectRecordData.MultiEntityDataTableList_.Count; i++)
        {
            AFMsg.EntityDataTableList xObjectRecordList = xMultiObjectRecordData.MultiEntityDataTableList_[i];
            for(int j = 0; j < xObjectRecordList.DataTableList.Count; j++)
            {
                AFMsg.EntityDataTableBase xObjectRecordBase = xObjectRecordList.DataTableList[j];
                for(int k = 0; k < xObjectRecordBase.Row.Count; ++k)
                {
                    AFMsg.DataTableAddRow xAddRow = xObjectRecordBase.Row[i];

                    ADD_ROW(PBToAF(xObjectRecordList.EntityId), xObjectRecordBase.TableName, xAddRow);
                }
            }
        }
    }

    private void EGMI_ACK_OBJECT_PROPERTY_ENTRY(MsgHead head, MemoryStream stream)
    {
        AFMsg.MultiEntityDataNodeList xMultiObjectPropertyList = MultiEntityDataNodeList.Parser.ParseFrom(stream);
       // ReceiveMsg(stream, ref xMultiObjectPropertyList);

        for(int i = 0; i < xMultiObjectPropertyList.MultiEntityDataNodeList_.Count; i++)
        {
            AFMsg.EntityDataNodeList xPropertyData = xMultiObjectPropertyList.MultiEntityDataNodeList_[i];
            AFIObject go = AFCKernel.Instance.GetObject(PBToAF(xPropertyData.EntityId));
            AFIPropertyManager xPropertyManager = go.GetPropertyManager();

            for(int j = 0; j < xPropertyData.DataNodeList.Count; j++)
            {
                string strPropertyName = xPropertyData.DataNodeList[j].NodeName;

                AFCoreEx.AFIDataList.Var_Data xPropertyValue  = PBPropertyToData(xPropertyData.DataNodeList[j]);
                AFIProperty xProperty = xPropertyManager.GetProperty(strPropertyName);
                if(null == xProperty)
                {
                    AFIDataList varList = new AFCDataList();
                    varList.AddDataObject(ref xPropertyValue);

                    xProperty = xPropertyManager.AddProperty(strPropertyName, varList);
                }

                xProperty.SetDataObject(ref xPropertyValue);
            }
        }
    }

    //////////////////////////////////
    private void EGMI_ACK_SKILL_OBJECTX(MsgHead head, MemoryStream stream)
    {
            AFMsg.ReqAckUseSkill xReqAckUseSkill = ReqAckUseSkill.Parser.ParseFrom(stream);// new AFMsg.ReqAckUseSkill();
       // ReceiveMsg(stream, ref xReqAckUseSkill);
        AFMsg.Position xNowPos = xReqAckUseSkill.NowPos;
        AFMsg.Position xTarPos = xReqAckUseSkill.TarPos;

        AFIDataList xObjectList = new AFCDataList();
        AFIDataList xRtlList = new AFCDataList();
        AFIDataList xValueList = new AFCDataList();

        if(xReqAckUseSkill.EffectData.Count <= 0)
        {
            return;
        }

        for(int i = 0; i < xReqAckUseSkill.EffectData.Count; ++i)
        {
            xObjectList.AddObject(PBToAF(xReqAckUseSkill.EffectData[i].EffectIdent));
            xRtlList.AddInt64((int)xReqAckUseSkill.EffectData[i].EffectRlt);
            xValueList.AddInt64((int)xReqAckUseSkill.EffectData[i].EffectValue);
        }


            string strSkillName = xReqAckUseSkill.SkillId;//System.Text.Encoding.Default.GetString();
        //Debug.Log("AckUseSkill:" + strSkillName);

        AFCDataList varList = new AFCDataList();
        varList.AddObject(PBToAF(xReqAckUseSkill.User));
        varList.AddFloat(xNowPos.X);
        varList.AddFloat(xNowPos.Z);
        varList.AddFloat(xTarPos.X);
        varList.AddFloat(xTarPos.Z);

        if(xObjectList.Count() != xRtlList.Count() || xObjectList.Count() != xValueList.Count())
        {
            return;
        }

        varList.AddInt64(xObjectList.Count());
        for(int i = 0; i < xObjectList.Count(); ++i)
        {
            varList.AddObject(xObjectList.ObjectVal(i));
        }

        for(int i = 0; i < xRtlList.Count(); ++i)
        {
            varList.AddInt64(xRtlList.Int64Val(i));
        }

        for(int i = 0; i < xValueList.Count(); ++i)
        {
            varList.AddInt64(xValueList.Int64Val(i));
        }


        AFCLogicEvent.Instance.DoEvent((int)ClientEventDefine.EVENTDEFINE_USESKILL, varList);

        //AFCRenderInterface.Instance.UseSkill(, strSkillName, xNowPos.x, xNowPos.z, xTarPos.x, xTarPos.z, xObjectList, xRtlList, xValueList);
    }

    private void EGMI_ACK_CHAT(MsgHead head, MemoryStream stream)
    {
            AFMsg.ReqAckPlayerChat xReqAckChat = ReqAckPlayerChat.Parser.ParseFrom(stream);// new AFMsg.ReqAckPlayerChat();
        //ReceiveMsg(stream, ref xReqAckChat);

        mxPlayerNet.aChatMsgList.Add(PBToAF(xReqAckChat.ChatId).ToString() + ":" + xReqAckChat.ChatInfo);
    }
}

}