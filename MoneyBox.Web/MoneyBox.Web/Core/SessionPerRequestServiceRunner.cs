using System;
using System.Data;
using ServiceStack;
using ServiceStack.Host;
using ServiceStack.Web;
using Container = Funq.Container;
using ISession = NHibernate.ISession;
using ITransaction = NHibernate.ITransaction;

namespace MoneyBox.Web.Core
{
    public class BaseServiceRunner<TRequest> : ServiceRunner<TRequest>
    {
        /// <summary>
        /// Contains the current NHibernate session;
        /// </summary>
        protected ISession CurrentSession { get; private set; }

        private readonly Container _container;
        public BaseServiceRunner(ServiceStackHost appHost, ActionContext actionContext)
            : base(appHost, actionContext)
        {
            
            _container = appHost.Container;
        }

        public override void OnBeforeExecute(IRequest requestContext, TRequest request)
        {
            if (request==null)
            {
                base.OnBeforeExecute(requestContext, request);
            }
            

            var factory = _container.TryResolve<NHibernate.ISessionFactory>();
            var session = factory.OpenSession();
            var trans = session.BeginTransaction(IsolationLevel.ReadCommitted);
            CurrentSession = session;
            _container.Register<ISession>(c =>session).ReusedWithin(Funq.ReuseScope.Request);
            _container.Register<ITransaction>(c => trans).ReusedWithin(Funq.ReuseScope.Request);
          
        }

        public override object OnAfterExecute(IRequest requestContext, object response)
        {
            var trans = _container.TryResolve<ITransaction>();
            if (trans != null && trans.IsActive)
                trans.Commit();

            var session = _container.TryResolve<ISession>();
            if (session != null)
            {
                session.Flush();
                session.Close();
            }

            return base.OnAfterExecute(requestContext, response);
        }

        public override object HandleException(IRequest requestContext, TRequest request, Exception ex)
        {

            if (request != null)
            {
                var trans = requestContext.GetItem("transaction") as ITransaction;
                if (trans != null && trans.IsActive)
                    trans.Rollback();

                var session = requestContext.GetItem("session") as ISession;
                if (session != null)
                {
                    session.Flush();
                    session.Close();
                }
            }
            return base.HandleException(requestContext, request, ex);
        }
    }
}