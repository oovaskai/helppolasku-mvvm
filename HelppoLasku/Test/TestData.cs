using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.Models;
using HelppoLasku.DataAccess;

namespace HelppoLasku.Test
{
    public static class TestData
    {
        public static IDGenerator IdGen;

        public static List<DataModel> Companies;
        public static List<DataModel> Customers;
        public static List<DataModel> Groups;
        public static List<DataModel> Products;
        public static List<DataModel> Invoices;

        public static void Init()
        {
            IdGen = new IDGenerator(10);
            IdGen.AddID("OZwYHSa1Sv");

            Companies = new List<DataModel>
            {
                new Company
                {
                    ID = "OZwYHSa1Sv",
                    Name = "Test Company",
                    CompanyID = "1234567-8",
                    Phone = "040-1234567",
                    Email = "opovaskainen@gmail.com",
                    WebPage = "https://www.github.com/tookue",
                    Address = "Osoite 1",
                    PostalCode = "28100",
                    City = "Pori",
                    Country = "Finland",
                    BIC = "BANKBIC",
                    IBAN = "FI 12345 60000 7890",
                    CompanyExpire = 10,
                    PersonExpire = 14,
                    CompanyInterest = 11.5,
                    PersonInterest = 11.5,
                    ReferenceBase = "0002",
                    InvoiceID = 1002,
                    Tax = -24,
                    CompanyAnnotation = 7,
                    PersonAnnotation = 7
                }
            };

            DataModel customer1 = new Customer
            {
                ID = IdGen.Next(),
                Name = "CustomerCompany",
                CompanyID = "3453451-9",
                ContactPerson = "Jane Doe",
                Address = "Company St. 124",
                PostalCode = "23470",
                City = "Dallas",
                Country = "USA",
                Phone = "12-76-09876",
                Email = "jane.doe@customercompany.fi",
                WebPage = "https://www.customercompany.com"
            };

            DataModel customer2 = new Customer
            {
                ID = IdGen.Next(),
                Name = "John Doe",
                Address = "Customer street 4",
                PostalCode = "99990",
                City = "Stockholm",
                Country = "Sweden",
                Phone = "012-5673452"
            };

            Customers = new List<DataModel> { customer1, customer2 };

            DataModel services = new ProductGroup
            {
                ID = IdGen.Next(),
                Index = 1,
                Name = "Services"
            };
            DataModel hardware = new ProductGroup
            {
                ID = IdGen.Next(),
                Index = 2,
                Name = "Hardware"
            };

            Groups = new List<DataModel> { services, hardware };

            Products = new List<DataModel>
            {
                new Product
                {
                    ID = IdGen.Next(),
                    Name = "IT-Consulting",
                    Unit = "h",
                    Price = 79,
                    Tax = -24,
                    Group = services as ProductGroup
                },
                new Product
                {
                    ID = IdGen.Next(),
                    Name = "Power cable",
                    Unit = "m",
                    Price = 1.5,
                    Tax = 24,
                    Group = hardware as ProductGroup
                },
                new Product
                {
                    ID = IdGen.Next(),
                    Name = "Network adapter",
                    Unit = "pcs",
                    Price = 99,
                    Tax = -24,
                    Group = hardware as ProductGroup
                }
            };

            Invoice invoice1 = new Invoice
            {
                ID = IdGen.Next(),
                InvoiceID = 1001,
                Customer = customer1 as Customer,
                Date = new DateTime(2020, 1, 3),
                Paid = true,
                DueDate = new DateTime(2020, 1, 17),
                PayDate = new DateTime(2020, 1, 15),
                Interest = 11.5,
                Reference = "10003",
                AnnotationTime = 7,
                Titles = new List<InvoiceTitle>()
            };
            InvoiceTitle title1 = new InvoiceTitle
            {
                ID = IdGen.Next(),
                Header = "Consulting customer company 20.12.2019 - 2.1.2020",
                Invoice = invoice1,
                Items = new List<InvoiceItem>()
            };
            invoice1.Titles.Add(title1);

            InvoiceItem item1 = new InvoiceItem
            {
                ID = IdGen.Next(),
                Title = title1,
                Content = "IT-Consulting",
                Count = 24,
                Unit = "h",
                Price = 79,
                Tax = -24
            };
            title1.Items.Add(item1);

            Invoice invoice2 = new Invoice
            {
                ID = IdGen.Next(),
                InvoiceID = 1002,
                Customer = customer2 as Customer,
                Date = new DateTime(2020, 1, 27),
                Paid = false,
                Interest = 11.5,
                Reference = "10016",
                AnnotationTime = 7,
                DueDate = new DateTime(2020, 2, 10),
                Titles = new List<InvoiceTitle>()
            };
            InvoiceTitle title2 = new InvoiceTitle
            {
                ID = IdGen.Next(),
                Invoice = invoice2,
                Items = new List<InvoiceItem>()
            };
            invoice2.Titles.Add(title2);

            InvoiceItem item3 = new InvoiceItem
            {
                ID = IdGen.Next(),
                Title = title2,
                Content = "Network adapter",
                Count = 1,
                Unit = "pcs",
                Price = 99,
                Tax = -24
            };
            InvoiceItem item4 = new InvoiceItem
            {
                ID = IdGen.Next(),
                Title = title2,
                Content = "Power cable",
                Count = 8.5,
                Unit = "m",
                Price = 1.5,
                Tax = 24
            };
            title2.Items.Add(item3);
            title2.Items.Add(item4);



            Invoice invoice3 = new Invoice
            {
                ID = IdGen.Next(),
                Customer = customer1 as Customer,
                Titles = new List<InvoiceTitle>(),
            };
            InvoiceTitle title3 = new InvoiceTitle
            {
                ID = IdGen.Next(),
                Invoice = invoice3,
                Items = new List<InvoiceItem>()
            };
            invoice3.Titles.Add(title3);
            InvoiceItem item5 = new InvoiceItem
            {
                ID = IdGen.Next(),
                Title = title3,
                Content = "IT-Consulting",
                Count = 8,
                Unit = "h",
                Price = 79,
                Tax = -24
            };
            title3.Items.Add(item1);

           

            Invoices = new List<DataModel> { invoice1, invoice2, invoice3 };
        }
    }
}
