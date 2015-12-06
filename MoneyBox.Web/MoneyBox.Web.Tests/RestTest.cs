using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MoneyBox.DataModel;
using MoneyBox.Web.Core;
using NUnit.Framework;
using MoneyBox.Web.ServiceInterface;
using MoneyBox.Web.ServiceModel;
using MoneyBox.Web.ServiceModel.Actions;
using MoneyBox.Web.ServiceModel.DTO;
using NHibernate.Tool.hbm2ddl;
using ServiceStack.Testing;
using ServiceStack;

namespace MoneyBox.Web.Tests
{
    [TestFixture]
    public class RestTest

    {
        private readonly ServiceStackHost appHost;
        private readonly string baseUrl= "http://localhost:49222/";

        public RestTest()
        {
            SchemaExport exporter=new SchemaExport(SessionFactoryManager.Config.BuildConfiguration());
            exporter.Create(true,true);
            appHost = new AppHostSmartPool()
            {
                //ConfigureContainer = container =>
                //{
                //    //Add your IoC dependencies here
                //}
            }
            .Init();
            appHost.Start(baseUrl);
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            
            appHost.Dispose();
        }

        [Test]
        public void _1_Post()
        {
            //var service = appHost.Container.Resolve<TransactionService>();
            var client = new JsonServiceClient(baseUrl);
            var response =client.Post(new UpsertTransaction() {TransactionDto = new TransactionDto()
                {
                    CreatedDate = DateTime.Now,
                    CurrencyCode = "HUF",
                    ModifiedDate = DateTime.Now,
                    TransactionAmount = 100,
                    TransactionDate = DateTime.Now,
                    TransactionId = 1
                }});

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var response2 = client.Post(new UpsertTransaction()
            {
                TransactionDto = new TransactionDto()
                {
                    CreatedDate = DateTime.Now,
                    CurrencyCode = "GBP",
                    ModifiedDate = DateTime.Now,
                    TransactionAmount = 100,
                    TransactionDate = DateTime.Now,
                    TransactionId = 2
                }
            });
            Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
        [Test]
        public void _2_Get()
        {
            var client = new JsonServiceClient(baseUrl);

            var response = client.Get<List<TransactionDto>>(new GetOrDeleteTransactions());

            

            Assert.That(response.Count, Is.EqualTo(2));
        }

        [Test]
        public void _3_GetOne()
        {
            var client = new JsonServiceClient(baseUrl);

            var response = client.Get<List<TransactionDto>>(new GetOrDeleteTransactions());
            var firstId = response.First().TransactionId;
            var get = client.Get<TransactionDto>(new TransactionIdRequest() {Id = firstId});

            Assert.That(get.TransactionId, Is.EqualTo(firstId));
        }
     [Test]   
        public void _4_Put()
        {
            var client = new JsonServiceClient(baseUrl);

            var response = client.Get<List<TransactionDto>>(new GetOrDeleteTransactions());
            var firstId = response.First().TransactionId;
            var first = response.First();
            first.ModifiedDate=DateTime.Now;
            first.CurrencyCode = "USD";
            var put = client.Put(new UpsertTransaction() {TransactionDto = first});

            Assert.That(put.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

            var get = client.Get<TransactionDto>(new TransactionIdRequest() { Id = firstId });

            Assert.That(get.TransactionId, Is.EqualTo(firstId));
            Assert.That(get.CurrencyCode,Is.EqualTo("USD"));
        }

        [Test]
        public void _5_DeleteOne()
        {
            var client = new JsonServiceClient(baseUrl);

            var response = client.Get<List<TransactionDto>>(new GetOrDeleteTransactions());
            var firstId = response.First().TransactionId;
            
            var get = client.Delete<TransactionIdRequest>(new TransactionIdRequest(){Id= firstId});

            response = client.Get<List<TransactionDto>>(new GetOrDeleteTransactions());
            Assert.That(response.Count, Is.EqualTo(1));
            
        }

        [Test]
        public void _6_DeleteAll()
        {
            var client = new JsonServiceClient(baseUrl);

            var response = client.Delete(new GetOrDeleteTransactions());
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var get = client.Get<List<TransactionDto>>(new GetOrDeleteTransactions());
            Assert.That(get.Count, Is.EqualTo(0));
        }


    }
}
