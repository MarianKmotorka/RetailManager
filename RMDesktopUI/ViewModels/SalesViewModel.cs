using Caliburn.Micro;
using System.ComponentModel;

namespace RMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private BindingList<string> _products;
        private BindingList<string> _cart;
        private int _quantity;

        public BindingList<string> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }
        public BindingList<string> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);

            }
        }
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                NotifyOfPropertyChange(() => Quantity);
            }
        }
        public string Total
        {
            get { return "$0.00"; }
        }
        public string SubTotal
        {
            get { return "$0.00"; }
        }
        public string Tax
        {
            get { return "$0.00"; }
        }
        public bool CanAddToCart
        {
            get
            {
                return false;
            }
        }
        public bool CanRemoveFromCart
        {
            get
            {
                return false;
            }
        }
        public bool CanCheckout
        {
            get
            {
                return false;
            }
        }

        public void AddToCart() { }

        public void RemoveFromCart() { }

        public void Checkout() { }
    }
}
