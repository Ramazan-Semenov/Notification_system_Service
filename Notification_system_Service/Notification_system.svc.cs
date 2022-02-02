using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Notification_system_Service
{
    public delegate void ListOfNames(List<string> names, object sender);

    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы Service1.svc или Service1.svc.cs в обозревателе решений и начните отладку.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Notification_system : INotification_system
    {

        Dictionary<string, IReceiveChatService> names = new Dictionary<string, IReceiveChatService>();

        public static event ListOfNames ChatListOfNames;

        IReceiveChatService callback = null;

        public Notification_system() { }

        public void Close()
        {
            callback = null;
            names.Clear();
        }

        public void Start(string Name)
        {

            if (!names.ContainsKey(Name))
            {
                callback = OperationContext.Current.GetCallbackChannel<IReceiveChatService>();
                AddUser(Name, callback);
                SendNamesToAll();
            }
        }

        public void Stop(string Name)
        {
           
                if (names.ContainsKey(Name))
                {
                    names.Remove(Name);
                    SendNamesToAll();
                }
           
        }

       private void SendNamesToAll()
        {
            foreach (KeyValuePair<string, IReceiveChatService> name in names)
            {
                IReceiveChatService proxy = name.Value;
                proxy.SendNames(names.Keys.ToList());
            }

            if (ChatListOfNames != null)
            {
                ChatListOfNames(names.Keys.ToList(), this);
            }
        }



        private void AddUser(string name, IReceiveChatService callback)
        {
            names.Add(name, callback);
            if (ChatListOfNames != null)
                ChatListOfNames(names.Keys.ToList(), this);

        }

        void INotification_system.SendMessage(string msg, string sender, string receiver)
        {
            if (names.ContainsKey(receiver))
            {
                callback = names[receiver];
                callback.ReceiveMessage(msg, sender);
            }
            if (receiver=="all")
            {
                foreach (var item in names)
                {
                    callback = item.Value;
                    callback.ReceiveMessage(msg, sender);
                }
            }
        }

        void INotification_system.Start(string Name)
        {
           
                if (!names.ContainsKey(Name))
                {
                    callback = OperationContext.Current.GetCallbackChannel<IReceiveChatService>();
                    AddUser(Name, callback);
                    SendNamesToAll();
                }
           
        }

        void INotification_system.Stop(string Name)
        {
              if (names.ContainsKey(Name))
                {
                    names.Remove(Name);
                    SendNamesToAll();
                }
            
        }
    } 
}
