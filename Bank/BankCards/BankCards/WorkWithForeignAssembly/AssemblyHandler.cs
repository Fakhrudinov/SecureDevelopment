using DataAbstraction.AuthModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace BankCards.WorkWithForeignAssembly
{
    public static class AssemblyHandler
    {
        public static NewUser LoadAssembly()
        {
            string current = Directory.GetCurrentDirectory();

            var context = new CustomAssemblyLoadContext();
            // установка обработчика выгрузки
            context.Unloading += Context_Unloading;
            // получаем путь к сборке MyApp
            var assemblyPath = Path.Combine(Directory.GetCurrentDirectory(), "WorkWithForeignAssembly\\ForeignDLL\\StrongNameSignedLogonGenerator.dll");
            // загружаем сборку
            Assembly assembly = context.LoadFromAssemblyPath(assemblyPath);
            // получаем тип
            var type = assembly.GetType("StrongNameSignedLogonGenerator.DataGenerator");
            // получаем его метод 
            var greetMethod = type.GetMethod("GenerateSrting");

            // вызываем метод
            var instance = Activator.CreateInstance(type);
            //int result = (int)greetMethod.Invoke(instance, new object[] { number });
            object result = greetMethod.Invoke(instance, null);

            //foreach (PropertyInfo prop in props)
            //{
            //    object propValue = prop.GetValue(result, null);
            //    Console.WriteLine();
            //    // Do something with propValue
            //}

            //foreach(var qqq in result.GetType()
            //                         .GetProperties())
            //{
            //    string val = qqq.ToString();
            //    Console.WriteLine(val.);
            //}
            //var user = (NewUser)result;

            //// смотрим, какие сборки у нас загружены
            //foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            //    Console.WriteLine(asm.GetName().Name);

            IList<PropertyInfo> props = new List<PropertyInfo>(result.GetType().GetProperties());
            NewUser user = new NewUser { 
                Login= props[0].GetValue(result, null).ToString(),
                Password= props[1].GetValue(result, null).ToString()
            };

            // выгружаем контекст
            context.Unload();

            return user;
        }
        // обработчик выгрузки контекста
        private static void Context_Unloading(AssemblyLoadContext obj)
        {
            Console.WriteLine("Библиотека MyApp выгружена \n");
        }
    }
    public class CustomAssemblyLoadContext : AssemblyLoadContext
    {
        public CustomAssemblyLoadContext() : base(isCollectible: true) { }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            return null;
        }
    }
}

