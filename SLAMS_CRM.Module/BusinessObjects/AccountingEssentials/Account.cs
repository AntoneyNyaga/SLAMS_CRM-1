﻿using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using SLAMS_CRM.Module.BusinessObjects.ActivityStreamManagement;
using SLAMS_CRM.Module.BusinessObjects.CustomerService;
using SLAMS_CRM.Module.BusinessObjects.OrderManagement;
using SLAMS_CRM.Module.BusinessObjects.PipelineManagement;
using System.Collections.ObjectModel;
using AssociationAttribute = DevExpress.Xpo.AssociationAttribute;

namespace SLAMS_CRM.Module.BusinessObjects.AccountingEssentials
{
    [DefaultClassOptions]
    [NavigationItem("Accounting")]
    [ImageName("AccountingNumberFormat")]

    public class Account : BaseObject
    {
        public Account(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            createdOn = DateTime.Now;
            isAccountCreated = 3;
        }


        int isAccountCreated;
        //Invoice invoice;
        string associatedWith;
        Address shippingAddress;
        DateTime modifiedOn;
        DateTime createdOn;
        decimal annualRevenue;
        string description;
        //PhoneNumber officePhone;
        string emailAddress;
        string website;
        string name;
        string acType;
        string indType;

        [RuleRequiredField("RuleRequiredField for Account.Name", DefaultContexts.Save)]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }


        public string Website { get => website; set => SetPropertyValue(nameof(Website), ref website, value); }

        [RuleRequiredField("RuleRequiredField for Account.EmailAddress", DefaultContexts.Save)]
        public string EmailAddress
        {
            get => emailAddress;
            set => SetPropertyValue(nameof(EmailAddress), ref emailAddress, value);
        }

        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [Aggregated]
        public PhoneNumber OfficePhone
        {
            get { return GetPropertyValue<PhoneNumber>(nameof(OfficePhone)); }
            set { SetPropertyValue(nameof(OfficePhone), value); }
        }

        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [Aggregated]
        [RuleRequiredField("RuleRequiredField for Account.ShippingAddress", DefaultContexts.Save)]

        public Address ShippingAddress
        {
            get => shippingAddress;
            set => SetPropertyValue(nameof(ShippingAddress), ref shippingAddress, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Description
        {
            get => description;
            set => SetPropertyValue(nameof(Description), ref description, value);
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public int AcType
        {
            get => acType == null ? 0 : (int)Enum.Parse(typeof(AccountType), acType);
            set => SetPropertyValue(nameof(AcType), ref acType, Enum.GetName(typeof(AccountType), value));
        }

        public AccountType? TypeOfAccount { get; set; }


        public decimal AnnualRevenue
        {
            get => annualRevenue;
            set => SetPropertyValue(nameof(AnnualRevenue), ref annualRevenue, value);
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public int IndType
        {
            get => indType == null ? 0 : (int)Enum.Parse(typeof(IndustryType), indType);
            set => SetPropertyValue(nameof(IndType), ref indType, Enum.GetName(typeof(IndustryType), value));
        }


        public IndustryType? Industry { get; set; }

        [ModelDefault("AllowEdit", "false")]
        public string AssociatedWith
        {
            get
            {
                if (IsAccountCreated == 1)
                {
                    return "Lead";
                }
                else if (IsAccountCreated == 2)
                {
                    return "Contact";
                }
                else
                {
                    return "Not Related To any Contact or Lead";
                }
            }

            set => SetPropertyValue(nameof(AssociatedWith), ref associatedWith, value);
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public int IsAccountCreated
        {
            get => isAccountCreated;
            set => SetPropertyValue(nameof(IsAccountCreated), ref isAccountCreated, value);
        }



        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime CreatedOn
        {
            get => createdOn;
            set => SetPropertyValue(nameof(CreatedOn), ref createdOn, value);
        }


        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime ModifiedOn
        {
            get => modifiedOn;
            set => SetPropertyValue(nameof(ModifiedOn), ref modifiedOn, value);
        }

        public IList<Opportunity> Opportunities { get; set; } = new ObservableCollection<Opportunity>();

        protected override void OnSaving()
        {
            if (Session.IsNewObject(this))
            {
                CreatedOn = DateTime.Now;
                AddActivityStreamEntry("created", SecuritySystem.CurrentUser as ApplicationUser);
            }
            else
            {
                AddActivityStreamEntry("modified", SecuritySystem.CurrentUser as ApplicationUser);
            }
            ModifiedOn = DateTime.Now;
            base.OnSaving();
        }

        private void AddActivityStreamEntry(string action, ApplicationUser applicationUser)
        {
            var activityStreamEntry = new MyActivityStream(Session)
            {
                AccountName = Name,
                Action = action,
                Date = DateTime.Now,
                CreatedBy = applicationUser?.UserName
            };
            activityStreamEntry.Save();
        }

        //public IList<Quote> Quote { get; set; } = new ObservableCollection<Quote>();
        [Association("Account-Quotes")]
        public XPCollection<Quote> Quotes
        {
            get
            {
                return GetCollection<Quote>(nameof(Quotes));
            }
        }


        [DevExpress.Xpo.Association("Account-PurchaseOrders")]
        public XPCollection<PurchaseOrder> PurchaseOrders
        {
            get
            {
                return GetCollection<PurchaseOrder>(nameof(PurchaseOrders));
            }
        }

        [Association("Account-Payments")]
        public XPCollection<Payment> Payments
        {
            get
            {
                return GetCollection<Payment>(nameof(Payments));
            }
        }

        [Association("Account-Bills")]
        public XPCollection<Bills> Bills
        {
            get
            {
                return GetCollection<Bills>(nameof(Bills));
            }
        }

        [Association("Account-Cases")]
        public XPCollection<Cases> Cases
        {
            get
            {
                return GetCollection<Cases>(nameof(Cases));
            }
        }

        [DevExpress.Xpo.Association("Account-Invoices")]
        public XPCollection<Invoice> Invoices { get { return GetCollection<Invoice>(nameof(Invoices)); } }
    }

    public enum AccountType
    {
        Analyst,
        Competitor,
        Customer,
        Integrator,
        Investor,
        partner,
        Press,
        Prospect,
        Reseller,
        Other
    }

    public enum IndustryType
    {
        Agriculture,
        Automotive,
        BankingAndFinance,
        Biotechnology,
        Chemicals,
        Construction,
        ConsumerGoods,
        Education,
        EnergyAndUtilities,
        EntertainmentAndMedia,
        HealthCare,
        HospitalityAndTourism,
        InformationTechnology,
        Insurance,
        Manufacturing,
        Mining,
        Pharmaceuticals,
        RealEstate,
        Retail,
        Telecommunications,
        TransportationAndLogistics
    }
}