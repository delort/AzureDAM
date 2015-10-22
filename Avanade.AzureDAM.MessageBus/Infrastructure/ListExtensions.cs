using System;
using System.Collections.Generic;

namespace Avanade.AzureDAM.MessageBus.Infrastructure
{
    public static class ListExtensions
    {
        public static void ForEach<TItem>(this IEnumerable<TItem> list, Action<TItem> action)
        {
            foreach (var item in list)
                action(item);
        }
    }
}