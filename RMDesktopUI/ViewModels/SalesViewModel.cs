using Caliburn.Micro;
using RM.WPF.Library.Api;
using RM.WPF.Library.Models;
using System.ComponentModel;

namespace RMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private IProductEndpoint _productEndpoint;
        private BindingList<ProductModel> _products;
        private BindingList<ProductModel> _cart;
        private int _productQuantity;

        public SalesViewModel(IProductEndpoint productEndpoint)
        {
            _productEndpoint = productEndpoint;
        }

        public BindingList<ProductModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }
        public BindingList<ProductModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);

            }
        }
        public int ProductQuantity
        {
            get { return _productQuantity; }
            set
            {
                _productQuantity = value;
                NotifyOfPropertyChange(() => ProductQuantity);
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

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            Products = new BindingList<ProductModel>(await _productEndpoint.GetAll());
        }
    }
}
