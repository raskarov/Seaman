namespace Seaman.Core
{
    public interface IEntity : IEntity<int>
    {


    }


    public interface IEntity<out TKey>
    {
        TKey Id { get; }
    }
}