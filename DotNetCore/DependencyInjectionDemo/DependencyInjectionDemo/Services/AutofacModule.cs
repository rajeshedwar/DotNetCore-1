using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using DependencyInjectionDemo.Services;


namespace DependencyInjectionDemo.Services
{
    public class AutofacModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MongoDataManager>().As<IDataManager>();
            //builder.RegisterType<SqlDataManager>().As<IDataManager>().PropertiesAutowried();
        }
    }
}
