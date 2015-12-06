using System.Diagnostics.Contracts;
using Funq;
using MoneyBox.DataModel;
using MoneyBox.Web.ServiceInterface;
using MoneyBox.Web.ServiceModel.DTO;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using ServiceStack;
using ServiceStack.Host;
using ServiceStack.ProtoBuf;
using ServiceStack.Validation;
using ServiceStack.Web;

namespace MoneyBox.Web.Core
{
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// Default constructor.
        /// Base constructor requires a name and assembly to locate web service classes. 
        /// </summary>
        public AppHost()
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
            Plugins.Add(new ProtoBufFormat());
            container.Register<ISessionFactory>(s=>SessionFactoryManager.CreateSessionFactory()).ReusedWithin(ReuseScope.Request);
            container.RegisterValidators(typeof(TransactionDto).Assembly);
            
        }

        
    }
}