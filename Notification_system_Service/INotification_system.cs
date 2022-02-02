using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Notification_system_Service
{
    //это наш простой контракт на обслуживание
    [ServiceContract(CallbackContract = typeof(IReceiveChatService))]
    public interface INotification_system
    {
        [OperationContract(IsOneWay = true)]
        void SendMessage(string msg, string sender, string receiver);
        [OperationContract(IsOneWay = true)]
        void Start(string Name);
        [OperationContract(IsOneWay = true)]
        void Stop(string Name);


        // TODO: Добавьте здесь операции служб
    }
    //этот интерфейс канала предоставляет адаптер множественного наследования для нашей фабрики каналов
    //который объединяет два интерфейса, необходимых для создания канала
    public interface IChatChannel : INotification_system, IClientChannel
    {
    }
    public interface IReceiveChatService
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveMessage(string msg, string receiver);
        [OperationContract(IsOneWay = true)]
        void SendNames(List<string> names);
    }
    // Используйте контракт данных, как показано в примере ниже, чтобы добавить составные типы к операциям служб.

}
