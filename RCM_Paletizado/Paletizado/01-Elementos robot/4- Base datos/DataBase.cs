using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;
using Idpsa.Properties;
using System.Linq;
using System.Data.Linq;
using System.Reflection;
using Idpsa.Control.Subsystem;


namespace Idpsa
{
    public enum DBAction
    {
        Select,
        Insert,
        Udpade,
        Delete,
    }

    [Serializable]
    public class DataBase: IManagerRunnable
    {
        
        private class AsyncOutCome
        {
            public bool Ok { get; set; }
            public string Message { get; set; }
            public DBAction Action { get; set; }
            public DBEntity Entity { get; set; }
            public AsyncOutCome() {Ok = true; Message = String.Empty;}
            public AsyncOutCome(bool Ok, string Message, DBAction Action, DBEntity Data)
            {
                this.Ok = Ok; this.Message = Message; this.Action = Action; this.Entity = Data;
            }
        }

        private string _connectionString;

        public delegate void DataBaseChangedHandler();
        public event DataBaseChangedHandler DataBaseChanged;              
        private bool _taskInproccess;

        private delegate AsyncOutCome ActionDelegate(DBEntity item);  
        private Dictionary<KeyValuePair<EntityTypes,DBAction>, List<DBEntity>> _entities;       
        private Dictionary<EntityTypes, List<DBEntity>> _entitiesInsertedLog;

        public DataBase()
        {
            _connectionString = @"Data Source=PORTATIL\SQLEXPRESS;Initial Catalog=DBPasaportes;Integrated Security=True";//"Data Source=IDPSA-516AF581F\\PASAPORTES;Initial Catalog=TrazabilidadPasaportes;Persist Security Info=True;User ID=sa;Password = master";;"Data Source=alberto1;Initial Catalog=DBPasaportes;User ID=sa;Password=master";

            _entities = new Dictionary<KeyValuePair<EntityTypes,DBAction>, List<DBEntity>>();  
            _entitiesInsertedLog = new Dictionary<EntityTypes, List<DBEntity>>();
            
            foreach (var entityAction in GetEntityActionsEnumerator())
                _entities.Add(entityAction, new List<DBEntity>());
           
            foreach (var entity in GetEntityEnumerator())
                _entitiesInsertedLog.Add(entity, new List<DBEntity>());

        }              

        public void DBManagerASync()
        {
            if (!_taskInproccess)
                foreach (var entityAction in GetEntityActionsEnumerator())
                    if (_entities[entityAction].Count > 0)
                    {
                        InvokeAction(GetActionDelegate(entityAction.Value), _entities[entityAction][0]);
                        break;
                    }    
        }

        public void ProcessElement(IDBMappeable element)
        {
            DBEntity value = element.GetDBMapper();
            var index= new KeyValuePair<EntityTypes, DBAction>(value.EntityType, element.DBAction);
            _entities[index].Add(value);          
        }

        private ActionDelegate GetActionDelegate(DBAction action)
        {            
            return (ActionDelegate)Delegate
                .CreateDelegate(typeof(ActionDelegate),this, action.ToString());
        }

        public void DeleteAll()
        {
            AsyncOutCome result= new AsyncOutCome();
            using (var dataContext = GetDataContext())
            {                
                try
                {
                    dataContext.ExecuteCommand("DELETE FROM Catalogos");
                }
                catch (Exception ex)
                {
                    result.Ok = false;
                    result.Message = ex.Message;
                }
            }
        }       

        public DataContext GetDataContext()
        {
            return new DataContext(_connectionString);
        }

        private void InvokeAction(ActionDelegate function,DBEntity item)
        {
            new ActionDelegate(function).BeginInvoke(item,ActionCallBack, null);
            _taskInproccess = true;
        }   

        private AsyncOutCome Insert(DBEntity item) 
        {
            AsyncOutCome result = new AsyncOutCome
            {
                Action =  DBAction.Insert ,Entity = item
            };
            try
            {
                DataContext dataContext = GetDataContext();
                var table = dataContext.GetTable(item.GetType());
                table.InsertOnSubmit(item);                
                dataContext.SubmitChanges();                
            }
            catch (Exception ex)
            {
                result.Ok = false;
                result.Message = ex.Message;
            }
            return result;
        }

        private AsyncOutCome Update(DBEntity item) 
        {
            AsyncOutCome result = new AsyncOutCome
            {
                Action = DBAction.Udpade ,Entity = item
            };
            using (DataContext dataContext = GetDataContext())
            {
                var table = dataContext.GetTable(item.GetType());
                try
                {
                    var originalItem = FindInsertedEntity(item);
                    System.Reflection.PropertyInfo FechaInicial;

                    if ((FechaInicial = originalItem.GetType().GetProperty("FechaInicial")) != null)
                        item.GetType().GetProperty("FechaInicial").SetValue(item, FechaInicial.GetValue(originalItem, null), null);

                    table.Attach(item, originalItem);
                    dataContext.SubmitChanges();
                }
                catch (Exception ex)
                {
                    result.Ok = false;
                    result.Message = ex.Message;
                }
            }
            return result;
        }

        private AsyncOutCome Delete(DBEntity item)
        {
            AsyncOutCome result = new AsyncOutCome
            {
                Action = DBAction.Delete,Entity = item
            };
            try
            {
                DataContext dataContext = GetDataContext();
                var table = dataContext.GetTable(item.GetType());               
                var originalItem = FindInsertedEntity(item);
                if (originalItem != null)
                {
                    table.Attach(originalItem);
                    table.DeleteOnSubmit(originalItem);
                    dataContext.SubmitChanges();
                }                
            }
            catch (Exception ex)
            {
                result.Ok = false;
                result.Message = ex.Message;
            }
            return result;
        }

        private void ActionCallBack(IAsyncResult result)
        {
            var BdDelegate = (ActionDelegate)((AsyncResult)result).AsyncDelegate;
            AsyncOutCome asynResult = BdDelegate.EndInvoke(result);
            ProccessWork(asynResult);
            _taskInproccess = false;
        }

        private void ProccessWork(AsyncOutCome result)
        {
            var index = new KeyValuePair<EntityTypes,DBAction>(result.Entity.EntityType, result.Action);
            if (result.Ok)
            {
                ManageInsertedEntityLog(result.Entity, result.Action);
                _entities[index].RemoveAt(0);
                if (_entities[index].Count == 0)
                    OnDataBaseChaged();
            }            
        }

        private void ManageInsertedEntityLog(DBEntity entity,DBAction action){
            switch (action)
            {
                case DBAction.Insert:
                    _entitiesInsertedLog[entity.EntityType].Add(entity);
                    break;
                case DBAction.Udpade:                    
                case DBAction.Delete:
                    RemoveEntityInInsertedLog(entity,action);
                    break;
            }
        }

        private void RemoveEntityInInsertedLog(DBEntity entity,DBAction action)
        {            
            DBEntity toRemove = FindInsertedEntity(
                _entities[new KeyValuePair<EntityTypes,DBAction>(entity.EntityType,action)].ElementAt(0));
            if (toRemove != null)
                _entitiesInsertedLog[entity.EntityType].Remove(toRemove);
        }

        private DBEntity FindInsertedEntity(DBEntity entity)
        {
            return _entitiesInsertedLog[entity.EntityType].Find(item => entity.EqualEntity(item));
        }      

        private void OnDataBaseChaged()
        {
            if (DataBaseChanged != null)
            {
                DataBaseChanged();
            }
        }

        private EntityTypes[] ArrayEntitys = new EntityTypes[]{EntityTypes.Catalog,
            EntityTypes.Box, EntityTypes.Group, EntityTypes.Passport};
        private DBAction[] ArrayActions = new DBAction[] {DBAction.Insert,DBAction.Udpade,
            DBAction.Select,DBAction.Delete};

        private IEnumerable<EntityTypes> GetEntityEnumerator()
        {
            foreach (EntityTypes entity in ArrayEntitys)
                yield return entity;
        }

        private IEnumerable<DBAction> GetBDActionEnumerator()
        {
            foreach (DBAction action in ArrayActions)
                yield return action;
        }

        private IEnumerable<KeyValuePair<EntityTypes,DBAction>> GetEntityActionsEnumerator()
        {
            foreach (var entity in GetEntityEnumerator())
                foreach (var action in GetBDActionEnumerator())
                    yield return new KeyValuePair<EntityTypes, DBAction>(entity, action);
        }



        #region Miembros de IDelegateTasksOwner

        public IEnumerable<Action> GetManagers()
        {
            return new Action[] { DBManagerASync };
        }

        #endregion
    }

}

