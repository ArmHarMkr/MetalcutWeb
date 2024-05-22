namespace MetalcutWeb.DAL.Repository
{
    public interface IUnitOfWork
    {
        IEmployeeRepository Employee { get; }
        IProductRepository Product { get; }
        IDeliveryRepository Delivery { get; }
        IDepartmentRepository Department { get; }
        ICommentRepository Comment { get; }
        IChatRepository Chat { get; }
        IMessageRepository Message { get; }
        IStorageProductRepository StorageProduct { get; }
        IOrderRepository Order { get; }
        Task Save();
    }
}
