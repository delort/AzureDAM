using Avanade.AzureDAM.Messages;

namespace Avanade.AzureDAM.MessageHandlers
{
    public interface IHandle<in T> where T: Message
    {
        void Handle(T message);
    }
}