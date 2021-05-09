using System;
using System.Collections.Generic;
using System.Linq;

namespace Apollon.Mud.Server.Model.ModelExtensions
{
    public static class CollectionExtensions
    {
        public static void RemoveAll<T>(this ICollection<T> collection, Func<T, bool> predicate = null)
        {
            predicate ??= _ => true;
            if (collection is List<T> list) list.RemoveAll(new Predicate<T>(predicate));
            else
            {
                var itemsToDelete = collection
                    .Where(predicate);

                foreach (var item in itemsToDelete)
                {
                    collection.Remove(item);
                }
            }
        }
    }
}