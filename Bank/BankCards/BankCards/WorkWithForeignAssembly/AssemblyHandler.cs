using System;
using System.Reflection;
using System.Runtime.Loader;
using StrongNameSignedLogonGenerator;

namespace BankCards.WorkWithForeignAssembly
{
    public static class AssemblyHandler
    {
        public static string LoadAssembly()
        {
            DataGenerator dg = new DataGenerator();
            string str = dg.GenerateSrting();

            var context = new CustomAssemblyLoadContext();
            // установка обработчика выгрузки
            context.Unloading += Context_Unloading;

            // выгружаем контекст
            context.Unload();

            return str;
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

