namespace TH1.Patterns.FactoryMethod
{
    public abstract class PaymentServiceFactory
    {
        public abstract IPaymentService CreatePaymentService();
    }

    public class CashPaymentFactory : PaymentServiceFactory
    {
        public override IPaymentService CreatePaymentService() => new CashPaymentService();
    }

    public class PaypalPaymentFactory : PaymentServiceFactory
    {
        public override IPaymentService CreatePaymentService() => new PaypalPaymentService();
    }

    public class VNPayPaymentFactory : PaymentServiceFactory
    {
        public override IPaymentService CreatePaymentService() => new VNPayPaymentService();
    }
}
