using MetalcutWeb.Domain.Entity;

namespace MetalcutWeb.ViewModels
{
    public class ChatMessageViewModel
    {
        public ChatEntity ChatEntity { get; set; }
        public MessageEntity messageEntity { get; set; }
        public IEnumerable<MessageEntity> Messages { get; set; }
    }
}
