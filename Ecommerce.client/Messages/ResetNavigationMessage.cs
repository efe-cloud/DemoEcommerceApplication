using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Ecommerce.client.Messages
{
    public class ResetNavigationMessage : ValueChangedMessage<bool>
    {
        public ResetNavigationMessage(bool value) : base(value)
        {
        }
    }
}
