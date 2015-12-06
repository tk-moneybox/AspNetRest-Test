using System;
using FluentNHibernate.Mapping;

namespace MoneyBox.DataModel
{
    
    public class Transaction
    {
        
        public virtual long TransactionId { get; set; }
        public virtual DateTime TransactionDate { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal TransactionAmount { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual string CurrencyCode { get; set; }
        public virtual string Merchant { get; set; }
    }

    public class TransationMap : ClassMap<Transaction>
    {
        public TransationMap()
        {
            Id(t => t.TransactionId).Unique().GeneratedBy.Assigned();
            Map(t => t.CreatedDate).Not.Nullable();
            Map(t => t.CurrencyCode).Not.Nullable();
            Map(t => t.Description);
            Map(t => t.Merchant);
            Map(t => t.ModifiedDate).Not.Nullable();
            Map(t => t.TransactionAmount).Not.Nullable();
            Map(t => t.TransactionDate).Not.Nullable();

        }
    }
}