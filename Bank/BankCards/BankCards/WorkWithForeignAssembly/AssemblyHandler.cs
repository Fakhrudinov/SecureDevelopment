using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace BankCards.WorkWithForeignAssembly
{
    public static class AssemblyHandler
    {
        public static string LoadAssembly()
        {
            string current = Directory.GetCurrentDirectory();//E:\\Downloads\\Обучение\\C#\\DZ_Repozitoriy\\SecureDevelopment\\SecureDevelopment\\Bank\\BankCards\\BankCards

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
            string result = (string)greetMethod.Invoke(instance, null);
            // выводим результат метода на консоль
            Console.WriteLine(result);

            //// смотрим, какие сборки у нас загружены
            //foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            //    Console.WriteLine(asm.GetName().Name);

            // выгружаем контекст
            context.Unload();



            return result;
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

