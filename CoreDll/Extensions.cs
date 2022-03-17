using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing;

namespace CoreDll
{
    public static class Extensions
    {
        public static void AddIfNotContains<T>(this BindingList<T> list, T value)
        {
            if (list != null)
            {
                if (!list.Contains(value))
                {
                    list.Add(value);
                }
            }
        }

        public static void AddRange<T>(this BindingList<T> list, IEnumerable<T> collection)
        {
            if (collection != null)
            {
                foreach (T value in collection)
                {
                    list.Add(value);
                }
            }
        }

        public static void AddRange<T>(this BindingList<T> list, ICollection<T> collection)
        {
            if (collection != null)
            {
                foreach (T value in collection)
                {
                    list.Add(value);
                }
            }
        }

        public static T ToEnum<T>(this string enumString)
        {
            return (T)Enum.Parse(typeof(T), enumString);
        }

        public static T ToEnum<T>(this object enumObject)
        {
            return (T)Enum.Parse(typeof(T), enumObject.ToString());
        }

        public static List<T> ToList<T>(this System.Collections.ICollection collection)
        {
            List<T> newList = new List<T>();

            foreach (object item in collection)
            {
                T convertedItem = (T)item;
                newList.Add(convertedItem);
            }

            return newList;
        }

        public static string JoinWith<T>(this List<T> lista, string separator = "", Func<T, string> forEachItem = null)
        {
            StringBuilder builder = new StringBuilder("");

            if (forEachItem == null)
            {
                forEachItem = (T item) => { return item.ToString(); };
            }

            if (lista.Count > 0)
            {
                builder.Append(forEachItem(lista[0]));
            }

            for (int idx = 1; idx < lista.Count; idx++)
            {
                builder.Append(separator + forEachItem(lista[idx]));
            }

            return builder.ToString();
        }

        public static string[] Split(this string texto, string separator, StringSplitOptions options = StringSplitOptions.None)
        {
            string[] separators = { separator };
            return texto.Split(separators, options);
        }

        /// <summary>
        /// Obtém uma quantidade de caracteres à esquerda.
        /// </summary>
        /// <param name="IntCorte">Quantidade de caracteres.</param>
        /// <returns>Retorna sequência de caracteres indicada.</returns>
        public static string TakeLeft(this string valor, int corte)
        {
            if (valor == null)
                return string.Empty;

            int idx = 0;
            StringBuilder leftPart = new StringBuilder("");

            for (; idx < valor.Count() && idx < corte; idx++)
            {
                leftPart.Append(valor[idx]);
            }

            return leftPart.ToString();
        }

        /// <summary>
        /// Obtém uma quantidade de caracteres à direita.
        /// </summary>
        /// <param name="IntCorte">Quantidade de caracteres.</param>
        /// <returns>Retorna sequência de caracteres indicada.</returns>
        public static string TakeRight(this string valor, int corte)
        {
            if (valor == null)
                return string.Empty;

            int idx = valor.Count() - corte;
            idx = (idx < 0 ? 0 : idx);

            StringBuilder rightPart = new StringBuilder("");

            for (; idx < valor.Count(); idx++)
            {
                rightPart.Append(valor[idx]);
            }

            return rightPart.ToString();
        }

        /// <summary>
        /// Ignora uma quantidade de caracteres à direita.
        /// </summary>
        /// <param name="IntCorte">Quantidade de caracteres.</param>
        public static string SkipRight(this string valor, int corte)
        {
            if (valor == null)
                return string.Empty;

            StringBuilder builder = new StringBuilder("");

            for (int idx = 0; idx < valor.Length - corte; idx++)
            {
                builder.Append(valor[idx]);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Ignora uma quantidade de caracteres à esquerda.
        /// </summary>
        /// <param name="IntCorte">Quantidade de caracteres.</param>
        public static string SkipLeft(this string valor, int corte)
        {
            if (valor == null)
                return string.Empty;

            StringBuilder builder = new StringBuilder("");

            for (int idx = corte; idx < valor.Length; idx++)
            {
                builder.Append(valor[idx]);
            }

            return builder.ToString();
        }

        public static string FormatTo(this string valor, params object[] args)
        {
            string resultado = valor;

            for (int idx = 0; idx < args.Length; idx++)
            {
                resultado = resultado.Replace("{" + idx + "}", args[idx].ToString());
            }

            return resultado;
        }
    }
}