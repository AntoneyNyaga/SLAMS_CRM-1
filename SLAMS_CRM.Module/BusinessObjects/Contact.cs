﻿using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Filtering;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base.General;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.ObjectModel;

namespace SLAMS_CRM.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("SLAMS CRM")]
    [Persistent("Contact")]
    [ImageName("NewCustomer")]


    [ObjectCaptionFormat("{0:FullName}")]
    [DefaultProperty(nameof(FullName))]
    public class Contact : BaseObject
    { 
        public Contact(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        Address address;
        string jobTitle;
        Communication inbox;
        string notes;
        string company;
        private const string V = "{FirstName} {LastName}";
        string phoneNumber;
        string emailAddress;
        string lastName;
        string firstName;
        string leadSource;

        [RuleRequiredField("RuleRequiredField for Contact.FirstName", DefaultContexts.Save)]
        public string FirstName
        {
            get => firstName;
            set => SetPropertyValue(nameof(FirstName), ref firstName, value);
        }

        [RuleRequiredField("RuleRequiredField for Contact.LastName", DefaultContexts.Save)]
        public string LastName
        {
            get => lastName;
            set => SetPropertyValue(nameof(LastName), ref lastName, value);
        }

        [Size(50)]
        public string JobTitle
        {
            get => jobTitle;
            set => SetPropertyValue(nameof(JobTitle), ref jobTitle, value);
        }


        [Size(100)]
        //[RuleRequiredField("RuleRequiredField for Contact.Company", DefaultContexts.Save)]
        public string Company
        {
            get => company;
            set => SetPropertyValue(nameof(Company), ref company, value);
        }


        [RuleRegularExpression("RuleRegularExpression for Contact.PhoneNumber", DefaultContexts.Save, @"^(\+)?\d+(\s*\-\s*\d+)*$")]
        [RuleRequiredField("RuleRequiredField for Contact.PhoneNumber", DefaultContexts.Save)]
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { SetPropertyValue(nameof(PhoneNumber), ref phoneNumber, value); }
        }
        [RuleRequiredField("RuleRequiredField for Contact.Address", DefaultContexts.Save)]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [DevExpress.Xpo.Aggregated]
        
        public Address Address
        {
            get => address;
            set => SetPropertyValue(nameof(Address), ref address, value);
        }

        [RuleRegularExpression("RuleRegularExpression for Contact.EmailAddress", DefaultContexts.Save, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$")]
        [RuleRequiredField("RuleRequiredField for Contact.EmailAddress", DefaultContexts.Save)]
        public string EmailAddress
        {
            get => emailAddress;
            set => SetPropertyValue(nameof(EmailAddress), ref emailAddress, value);
        }

        [Size(4096)]
        public string Notes
        {
            get => notes;
            set => SetPropertyValue(nameof(Notes), ref notes, value);
        }
 
        [DevExpress.Xpo.Association("Communication-Contacts")]
        public Communication Inbox
        {
            get => inbox;
            set => SetPropertyValue(nameof(Inbox), ref inbox, value);
        }

        [Browsable(false)]
        public int LeadSource
        {
            get => leadSource == null ? 0 : (int)Enum.Parse(typeof(LeadSource), leadSource);
            set
            {
                SetPropertyValue(nameof(LeadSource), ref leadSource, Enum.GetName(typeof(LeadSource), value));
            }
        }

        [NotMapped]
        public SourceType SourceType
        {
            get => (SourceType)LeadSource;
            set => LeadSource = (int)value;
        }

        [Browsable(false)]
        public IList <Quote> Quote { get; set; } = new ObservableCollection<Quote>();


        [Browsable(false)]
        [ReadOnly(true)]
        [SearchMemberOptions(SearchMemberMode.Exclude)]
        public String FullName
        {
            get
            {
                return ObjectFormatter.Format(FullNameFormat, this, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty);
            }
        }

        [Browsable(false)]
        [ReadOnly(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public String DisplayName
        {
            get
            {
                return FullName;
            }
        }

     
        private static String FullNameFormat = V;


    }
}