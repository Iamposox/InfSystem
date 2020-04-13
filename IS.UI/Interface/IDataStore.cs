using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IS.UI.Interface
{
    public interface IDataStore<T>
    {
        /// <summary>
        /// Add item or Update if it is already stored
        /// </summary>
        /// <param name="_item"></param>
        /// <returns></returns>
        Task<bool> AddOrUpdateItemAsync(T _item);

        /// <summary>
        /// Add item to storage
        /// </summary>
        Task<bool> AddItemAsync(T _item);

        /// <summary>
        /// Currently not implemented due lack of need
        /// </summary>
        Task<bool> UpdateItemAsync(T _item);

        /// <summary>
        /// Remove item from the storage
        /// </summary>
        Task<bool> DeleteItemAsync(int _id);

        /// <summary>
        /// Returns item with specific id
        /// </summary>
        Task<T> GetItemAsync(int _id);

        /// <summary>
        /// Get all items from the storage
        /// </summary>
        /// <param name="forceRefresh">Ensure to refresh data before return</param>
        Task<IEnumerable<T>> GetItemsAsync(bool _forceRefresh = false);
    }
}
