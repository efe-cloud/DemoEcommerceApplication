using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Ecommerce.client.Messages
{
    /// <summary>
    /// Message indicating that a new order has been created.
    /// Carries the ID of the newly created order.
    /// </summary>
    public class NewOrderCreatedMessage : ValueChangedMessage<int>
    {
        public NewOrderCreatedMessage(int orderId) : base(orderId)
        {
        }
    }
}
