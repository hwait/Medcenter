using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Medcenter.Service.Model.Interfaces;
using Medcenter.Service.Model.Messaging;
using ServiceStack.Model;

namespace Medcenter.Service.Model.Types
{
    [DataContract]
    public class Payment : INotifyPropertyChanged, IItem
    {
        
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public int Cost { get; set; }

        [DataMember]
        public int FinalCost
        {
            get
            {
                _finalCost = (Discount != null) ? _finalCost = Cost - Cost/100*Discount.Value : 0;
                return _finalCost;
            }
            set
            {
                _finalCost = value;
            }
        }

        public int OldCost { get; set; }
        public string UnderOverPaid
        {
            get
            {
                var diff = OldCost - FinalCost;
                return diff > 0 ? " выдать из кассы " + diff + " тенге" :
                       diff < 0 ? " принять от клиента " + diff * -1 + " тенге" : "";
            }
        }
        [DataMember]
        public int ReceptionId { get; set; }

        [DataMember]
        public int DiscountId
        {
            get
            {
                return _discountId; 
                
            }
            set
            {
                _discountId = value;
                Discount = (Discounts == null) ? new Discount() : Discounts.Single(p => p.Id == _discountId);
            }
        }
        
        public List<Discount> Discounts { get; set; }

        private Discount _discount;
        private int _discountId;
        private int _finalCost;

        public Discount Discount
        {
            get { return _discount; }
            set
            {
                _discount = value;
                Calc();
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("FinalCost"));
                    PropertyChanged(this, new PropertyChangedEventArgs("Discount"));
                    PropertyChanged(this, new PropertyChangedEventArgs("UnderOverPaid"));
                }
            }
        }
        public Payment()
        {
            DiscountId = 1;
        }
        public Payment(List<Discount> discounts, int toPay)
        {
            Date=DateTime.Now;
            Cost = toPay;
            Discounts = discounts;
            DiscountId = 1;
            
            
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Cost"));
                PropertyChanged(this, new PropertyChangedEventArgs("Discounts"));
                
            }
        }

        public List<ResultMessage> Validate()
        {
            List<ResultMessage> em = new List<ResultMessage>();
            //if (Cost == 0) em.Add(new ResultMessage(2, "Сумма:", OperationErrors.ZeroNumber));
            return em;
        }
        public void Calc()
        {
            if (Discount != null)
            {
                FinalCost = Cost - Cost / 100 * Discount.Value;
            }
        }
        public void ActuateProperties()
        {
            Discount = (Discounts == null) ? new Discount() : Discounts.SingleOrDefault(p => p.Id == _discountId);
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Discount"));
            }

        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
