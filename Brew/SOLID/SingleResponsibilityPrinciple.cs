using System.Collections.Generic;
using System.Linq;

namespace Brew.SOLID;

public class SingleResponsibilityPrinciple : IBrew
{
    #region Shared

    public class OrderItem
    {
        public string ProductName { get; set; }
        public int ProductCode { get; set; }
        public decimal Price { get; set; }
    }

    public class Payment
    {
        public string Email { get; set; }
        public Dictionary<string, string> Details { get; set; }
    }

    #endregion

    #region Before

    public void Before()
    {
        OrderBefore before = new OrderBefore();
        before.AddItem(new OrderItem() { Price = 2_0, ProductCode = 1, ProductName = "Bread " });
        before.AddItem(new OrderItem() { Price = 2_0, ProductCode = 1, ProductName = "Bread " });

        before.Checkout(new Payment()
        {
            Details = new Dictionary<string, string>()
                {
                    { "number" , "03391203193110" },
                    { "CSV" , "XXX" },
                    { "Expire" , "2024" },
                    { "Address" , "Billing Address" }
                }
        });
    }

    public class OrderBefore
    {
        private List<OrderItem> Items { get; } = new();

        public void AddItem(OrderItem item)
        {
            // add an item to the order
            DisplayCart();
        }

        public void RemoveItem(OrderItem item)
        {
            // find and remove an item from the order
            DisplayCart();
        }

        public void DisplayCart()
        {
            // Product Name x Quantity = Sum of Price
            // Display to the user
        }

        public void Checkout(Payment details)
        {
            DisplayCart();

            if (Payment(details))
                SendInvoice(details);
        }

        bool Payment(Payment details)
        {
            // charge card
            // ensure success
            return default;
        }

        void SendInvoice(Payment details)
        {
            // setup invoice object
            // setup smtp client
            // create template
            // configure email body, subject, to, from
            // send email
        }
    }

    #endregion

    #region After

    public void After()
    {
        var after = new OrderAfter(new Checkout(new Emailer("smtp://companyemailserver"), new PaymentProcessor()));

        after.AddItem(new OrderItem() { Price = 2_0, ProductCode = 1, ProductName = "Bread" });
        after.AddItem(new OrderItem() { Price = 2_50, ProductCode = 1, ProductName = "Milk" });

        after.Checkout(new Payment()
        {
            Details = new Dictionary<string, string>()
                {
                    { "number" , "03391203193110" },
                    { "CSV" , "XXX" },
                    { "Expire" , "2024" },
                    { "Address" , "Billing Address" }
                }
        });
    }

    public class OrderAfter
    {
        private Checkout checkout;

        public List<OrderItem> Items { get; } = new();

        public OrderAfter(Checkout checkout)
        {
            this.checkout = checkout;
        }

        public void AddItem(OrderItem item)
        {
            Items.Add(item);
        }

        public void RemoveItem(OrderItem item)
        {
            Items.Remove(item);
        }

        public void Checkout(Payment payment)
        {
            checkout.Process(this, payment);
        }
    }

    public class Emailer
    {
        private string smtpAddress;
        private string fromAddress;

        public Emailer(string smtpAddress)
        {
            this.smtpAddress = smtpAddress;
            fromAddress = "sales@example.com";
        }

        public void SendEmail(string emailTo, OrderAfter order)
        {
            // configure smtp details
            // build email subject from ORDER ID
            // email body contains order items and sum of payment
        }
    }

    public class Checkout
    {
        private Emailer emailer;
        private PaymentProcessor paymentProcessor;

        public Checkout(Emailer emailer, PaymentProcessor paymentProcessor)
        {
            this.emailer = emailer;
            this.paymentProcessor = paymentProcessor;
        }

        public void Process(OrderAfter order, Payment payment)
        {
            // checkout handles the price calculation

            decimal price = order.Items.Sum(x => x.Price);

            // payment handles the payment process
            if (paymentProcessor.Process(price, payment))
            {
                // emailer handles building the email from the order
                emailer.SendEmail(payment.Email, order);
            }
        }
    }

    public class PaymentProcessor
    {
        // payment integration service provider 

        public bool Process(decimal amount, Payment payment)
        {
            return true;
        }
    }

    #endregion
}