using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.ExtensionMethods
{
    /// <summary>
    /// Class containing custom extensions
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Adds the specified value to a <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of Object in the <see cref="IEnumerable{T}"/> to which you are adding.</typeparam>
        /// <param name="e">The <see cref="IEnumerable{T}"/> to which you are adding.</param>
        /// <param name="value">The object of T type to add to the <see cref="IEnumerable{T}"/>parameter.</param>
        /// <returns></returns>
        public static IEnumerable<T> Add<T>(this IEnumerable<T> e, T value) {
            foreach ( var cur in e) {
                yield return cur;
            }
            yield return value;
        }
    }
}
