using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace MoneyBox.DataModel
{
    public class SessionFactoryManager
    {
        public static FluentConfiguration Config { get; private set; }

        static SessionFactoryManager()
        {
            Config = Fluently.Configure()
                 .Database(
                     SQLiteConfiguration.Standard.ConnectionString("Data Source=" + Environment.CurrentDirectory +
                                                                   "\\moneybox.db;Version=3;New=True;")
                         .AdoNetBatchSize(50))
                 .Mappings(x => x.FluentMappings.AddFromAssembly(typeof(Transaction).Assembly))
                 .ExposeConfiguration(x =>
                 {
                     x.SetProperty("generate_statistics", "true");
                     new SchemaUpdate(x).Execute(false, true);

                 });
        }

        public static ISessionFactory CreateSessionFactory()
        {
           
            return Config.BuildSessionFactory();
        }
    }
}