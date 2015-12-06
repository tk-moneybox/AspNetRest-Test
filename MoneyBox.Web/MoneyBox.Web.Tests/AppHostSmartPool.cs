using System.Diagnostics.Contracts;
using Funq;
using MoneyBox.DataModel;
using MoneyBox.Web.Core;
using MoneyBox.Web.ServiceInterface;
using MoneyBox.Web.ServiceModel.DTO;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using ServiceStack;
using ServiceStack.Host;
using ServiceStack.Validation;
using ServiceStack.Web;

namespace MoneyBox.Web.Tests
{
    public class AppHostSmartPool : AppHostHttpListenerSmartPoolBase
    {
        /// <summary>
        /// Default constructor.
        /// Base constructor requires a name and assembly to locate web service classes. 
        /// </summary>
        public AppHostSmartPool()
            : base("MoneyBox.Web", typeof(TransactionService).Assembly)
        {

        }

        /// <summary>
        /// Specific ServiceRunner implementaion for the Session/Request strategy
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        public override IServiceRunner<TRequest> CreateServiceRunner<TRequest>(ActionContext actionContext)
        {
            return new BaseServiceRunner<TRequest>(this, actionContext);
        }

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        /// <param name="container"></param>
        public override void Configure(Container container)
        {
            
            container.Register<ISessionFactory>(s=>SessionFactoryManager.CreateSessionFactory()).ReusedWithin(ReuseScope.Request);
            container.RegisterValidators(typeof(TransactionDto).Assembly);
            
        }

        public override void Dispose()
        {
            SchemaExport exporter=new SchemaExport(SessionFactoryManager.Config.BuildConfiguration());
            exporter.Drop(false,true);
            base.Dispose();
        }
    }
}