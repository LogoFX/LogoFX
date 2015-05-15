// Partial Copyright (c) LogoUI Software Solutions LTD
// Author: LogoUI Team
// This source file is the part of LogoFX Framework http://logofx.codeplex.com
// See accompanying licenses and credits.

using System.Collections.Generic;
using System.Linq;
using System.Text;

// ReSharper disable CheckNamespace
namespace System.Collections
// ReSharper restore CheckNamespace
{
    public static class Extensions
    {
        /// <summary>
        /// Beautifies the string.
        /// </summary>
        /// <param name="originalName">original string.</param>
        /// <returns></returns>
        public static string Beautify(this IEnumerable<char> originalName)
        {
            StringBuilder word = new StringBuilder();
            List<string> words = new List<string>();
            foreach (char c in originalName)
            {
                // ignore punctuations and blanks
                if (Char.IsLetterOrDigit(c))
                {
                    if (Char.IsUpper(c))
                    {
                        if (word.Length > 0 && Char.IsLower(word[word.Length - 1]))
                        {
                            words.Add(word.ToString());
                            word = new StringBuilder();
                        }
                    }
                    word.Append(c);
                }
            }
            if (word.Length > 0)
                words.Add(word.ToString());
            String beautifiedName = String.Join(" ", words.ToArray());
            return beautifiedName;
        }

        /// <summary>
        /// Returns the first element of the sequence, or a default value if
        /// the sequence contains mo elements.
        /// </summary>
        /// <param name="source"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static object FirstOrDefault(this IEnumerable source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            IEnumerator enumerator = source.GetEnumerator();
            enumerator.Reset();
            return enumerator.MoveNext() ? enumerator.Current : null;
        }

        /// <summary>
        /// Same as apply
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ie">The ie.</param>
        /// <param name="action">The action.</param>
        public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
        {
            using (IEnumerator<T> enumerator = ie.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    action(enumerator.Current);
                }
            }

            
        }

        /// <summary>
        /// Performs an action on each item and increases an index of the enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ie"></param>
        /// <param name="action"></param>
        public static void For<T>(this IEnumerable<T> ie, Action<T, int> action)
        {
            int index = 0;
            using (IEnumerator<T> enumerator = ie.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    action(enumerator.Current, index++);
                }
            }
            
        }

        /// <summary>
        /// Performs an action on each item and increases an index of the enumerable.
        /// </summary>
        /// <param name="ie"></param>
        /// <param name="action"></param>
        public static void For(this IEnumerable ie, Action<object, int> action)
        {
            int index = 0;
            IEnumerator enumerator = ie.GetEnumerator();
            while (enumerator.MoveNext())
            {
                action(enumerator.Current, index++);
            }
        }

        /// <summary>
        /// NOT TESTED
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ie"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="action"></param>
        public static void For<T>(this IEnumerable<T> ie, int start, int end, Action<T, int> action)
        {
            int index = 0;
            ie.Skip(start + 1).ForEach(a =>
                                           {
                                               action(a, index++);
                                               if (index == end)
                                                   return;
                                           });
        }


        ///// <summary>
        ///// <see cref="IEnumerable"/> same as Apply
        ///// </summary>
        ///// <param name="ie"></param>
        ///// <param name="action"></param>
        //public static void ForEach(this IEnumerable ie, Action<object> action)
        //{
        //    foreach (object e in ie)
        //    {
        //        action(e);
        //    }
        //}

        /// <summary>
        /// Gets the number of items.
        /// </summary>
        /// <param name="ie"></param>
        /// <returns>The number of items.</returns>
        public static int Count(this IEnumerable ie)
        {
            int counter = 0;
            ie.Apply(item => counter++);
            return counter;
        }

        /// <summary>
        ///  Performs the specified action on each element of the <see cref="IEnumerable{T}"/> 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ie"></param>
        /// <param name="action">The <see cref="System.Action()"/> delegate to perform on each element of the <see cref="IEnumerable{T}"/>.</param>
        /// <exception cref="System.ArgumentNullException">action is <see langword="null"/>.</exception>
        public static void Apply<T>(this IEnumerable<T> ie, Action<T> action)
        {
            using (IEnumerator<T> enumerator = ie.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    action(enumerator.Current);
                }
            }
        }

        /// <summary>
        /// <see cref="IEnumerable"/> implementation of Apply
        /// </summary>
        /// <param name="ie"></param>
        /// <param name="action"></param>
        public static void Apply(this IEnumerable ie, Action<object> action)
        {
            IEnumerator enumerator = ie.GetEnumerator();

            while (enumerator.MoveNext())
            {
                action(enumerator.Current);
            }

        }

       //todo:be carefull, stack overflow
        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> fnRecurse)
        {
           //IEnumerable<T> result =
           // source.Concat(source.SelectMany(fnRecurse).Traverse(fnRecurse));
           //return result;
           foreach (T item in source)
           {
              yield return item;

              IEnumerable<T> seqRecurse = fnRecurse(item);

              if (seqRecurse != null)
              {
                 foreach (T itemRecurse in Traverse(seqRecurse, fnRecurse))
                 {
                    yield return itemRecurse;
                 }
              }
           }
        }

//#if WinRT
//        public static PropertyInfo GetProperty(this Type type, String propertyName)
//        {
//            return type.GetTypeInfo().GetDeclaredProperty(propertyName);
//        }

//        public static ConstructorInfo GetConstructor(this Type type,Type[] parTypes)
//        {
//            return (ConstructorInfo) type.GetTypeInfo().DeclaredConstructors.FirstOrDefault(a => a.GetParameters().Select(b=>b.ParameterType).ToArray().TheSameAs(parTypes));
//        }

//        public static bool TheSameAs<T>(this IList<T> that, IList<T> other)where T:class
//        {
//            if (other == null)
//                return false;
//            if (that.Count() != other.Count())
//                return false;
//            for (int i = 0; i < that.Count(); i++)
//            {
//                if (that[i] != other[i])
//                    return false;
//            }
//            return true;
//        }

//        public static MethodInfo GetMethod(this Type type, String methodName)
//        {
//            return type.GetTypeInfo().GetDeclaredMethod(methodName);
//        }

//        public static bool IsSubclassOf(this Type type, Type parentType)
//        {
//            return type.GetTypeInfo().IsSubclassOf(parentType);
//        }

//        public static bool IsAssignableFrom(this Type type, Type parentType)
//        {
//            return type.GetTypeInfo().IsAssignableFrom(parentType.GetTypeInfo());
//        }

//        public static bool IsEnum(this Type type)
//        {
//            return type.GetTypeInfo().IsEnum;
//        }

//        public static IEnumerable<Attribute> GetCustomAttributes(this Type type,Type at, bool inherits)
//        {
//            return type.GetTypeInfo().GetCustomAttributes(at, inherits);
//        }

//        public static bool IsValueType(this Type type)
//        {
//            return type.GetTypeInfo().IsValueType;
//        }

//        public static bool IsPrimitive(this Type type)
//        {
//            return type.GetTypeInfo().IsPrimitive;
//        }

//        public static Type GetBaseType(this Type type)
//        {
//            return type.GetTypeInfo().BaseType;
//        }

//        public static bool IsGenericType(this Type type)
//        {
//            return type.GetTypeInfo().IsGenericType;
//        }

//        public static Type[] GetGenericArguments(this Type type)
//        {
//            return type.GetTypeInfo().GenericTypeArguments;
//        }

//        public static IEnumerable<FieldInfo> GetFields(this Type type)
//        {
//            return type.GetTypeInfo().DeclaredFields;
//        }

//        public static object GetPropertyValue(this Object instance, string propertyValue)
//        {
//            return instance.GetType().GetTypeInfo().GetDeclaredProperty(propertyValue).GetValue(instance);
//        }

//        //public static TypeInfo GetTypeInfo(this Type type)
//        //{
//        //    IReflectableType reflectableType = (IReflectableType)type;
//        //    return reflectableType.GetTypeInfo();
//        //}
//#endif
    }
}
