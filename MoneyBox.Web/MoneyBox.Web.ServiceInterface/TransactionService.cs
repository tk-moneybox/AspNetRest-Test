using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Funq;
using MoneyBox.DataModel;
using ServiceStack;
using MoneyBox.Web.ServiceModel;
using MoneyBox.Web.ServiceModel.Actions;
using MoneyBox.Web.ServiceModel.DTO;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Tool.hbm2ddl;

namespace MoneyBox.Web.ServiceInterface
{
    public class TransactionService : Service
    {
        private ISession _session;

        public TransactionService()
        {

        }
        /// <summary>
        /// Get a Transaction by Id
        /// </summary>
        /// <param name="idRequest"></param>
        /// <returns></returns>
        public TransactionDto Get(TransactionIdRequest idRequest)
        {
            _session = this.TryResolve<ISession>();
            return _session.Get<Transaction>(idRequest.Id).ConvertTo<TransactionDto>();
        }

        public List<TransactionDto> Get(GetOrDeleteTransactions request)
        {
            _session = this.TryResolve<ISession>();

            return _session.Query<Transaction>().ToList().ConvertAll(x => x.ConvertTo<TransactionDto>());
        }

        public HttpResult Delete(TransactionIdRequest delete)
        {
            _session = this.TryResolve<ISession>();
            var result=_session.Delete("from Transaction t where t.TransactionId = ? ", delete.Id, NHibernateUtil.Int64);
            return new HttpResult() { StatusDescription = $"Deleted {result} objects",StatusCode = HttpStatusCode.OK};
        }

        public HttpResult Delete(GetOrDeleteTransactions all)
        {
            _session = this.TryResolve<ISession>();
            var result=_session.Delete("from Transaction t ");
            return new HttpResult() {StatusDescription = $"Deleted {result} objects", StatusCode = HttpStatusCode.OK };
            
        }

        public HttpResult Put(UpsertTransaction transaction)
        {
            _session = this.TryResolve<ISession>();
            _session.Update(transaction.TransactionDto.ConvertTo<Transaction>());
            return new HttpResult() {  StatusCode = HttpStatusCode.NoContent };
        }

        public HttpResult Post(UpsertTransaction transaction)
        {
            _session = this.TryResolve<ISession>();
            var created=_session.Save(transaction.TransactionDto.ConvertTo<Transaction>());
            return new HttpResult() { StatusDescription = $"Object Created with {created} id ", StatusCode = HttpStatusCode.OK };
            
        }

        public void Get(DropAndCreateDb dropAndCreate)
        {
            
            SchemaExport exporter = new SchemaExport(SessionFactoryManager.Config.BuildConfiguration());
            exporter.Drop(true,true);
            exporter.Create(true, true);
        }
    }
}