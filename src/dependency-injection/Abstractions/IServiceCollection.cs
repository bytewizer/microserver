using System.Collections;

namespace Bytewizer.TinyCLR.DependencyInjection
{
    /// <summary>
    /// Specifies the contract for a collection of service descriptors.
    /// </summary>
    public interface IServiceCollection
    {
        ServiceDescriptor this[int index] { get; set; }

        int Count { get; }
        bool IsReadOnly { get; }

        int Add(ServiceDescriptor item);
        void Clear();
        bool Contains(ServiceDescriptor item);
        void CopyTo(ServiceDescriptor[] array, int arrayIndex);
        IEnumerator GetEnumerator();
        int IndexOf(ServiceDescriptor item);
        void Insert(int index, ServiceDescriptor item);
        void Remove(ServiceDescriptor item);
        void RemoveAt(int index);
    }
}