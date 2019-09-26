using Caliburn.Micro;
using RM.WPF.Library.Api;
using RM.WPF.Library.Helpers;
using RM.WPF.Library.Models;
using System.ComponentModel;
using System.Linq;

namespace RMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private IProductEndpoint _productEndpoint;
        private IConfigHelper _configHelper;
        private BindingList<ProductModel> _products;
        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();
        private ProductModel _selectedProduct;
        private int _productQuantity = 1;
        private CartItemModel _selectedCartItem;

        public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper)
        {
            _productEndpoint = productEndpoint;
            _configHelper = configHelper;
        }

        public CartItemModel SelectedCartItem
        {
            get { return _selectedCartItem; }
            set
            {
                _selectedCartItem = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
            }
        }
        public ProductModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
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
        public BindingList<CartItemModel> Cart
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
                NotifyOfPropertyChange(() => CanAddToCart);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
            }
        }
        public string Total
            => (CalculateSubtotal() + CalculateTax()).ToString("C");
        public string SubTotal
            => CalculateSubtotal().ToString("C");
        public string Tax
            => CalculateTax().ToString("C");
        public bool CanAddToCart
        {
            get
            {
                var output = false;

                if (SelectedProduct?.QuantityInStock >= ProductQuantity && ProductQuantity > 0)
                    output = true;

                return output;
            }
        }
        public bool CanRemoveFromCart
        {
            get
            {
                var output = false;

                if (ProductQuantity > 0 && ProductQuantity <= SelectedCartItem?.QuantityInCart)
                    return true;

                return output;
            }
        }
        public bool CanCheckout 
        {
            get
            {
                return Cart.Any(x => x.QuantityInCart > 0);
            }
        }

        public void AddToCart()
        {
            var existingItem = Cart.SingleOrDefault(x => x.Product == SelectedProduct);

            if (existingItem != null)
            {
                existingItem.QuantityInCart += ProductQuantity;
                Cart.ResetBindings();
            }
            else
            {
                var cartItem = new CartItemModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ProductQuantity
                };

                Cart.Add(cartItem);
            }

            SelectedProduct.QuantityInStock -= ProductQuantity;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => CanCheckout);
            Products.ResetBindings();
            ProductQuantity = 1;
        }
        public void RemoveFromCart()
        {
            SelectedCartItem.QuantityInCart -= ProductQuantity;

            var product = Products.Single(x => x == SelectedCartItem.Product);
            product.QuantityInStock += ProductQuantity;

            ProductQuantity = 1;
            Products.ResetBindings();
            Cart.ResetBindings();
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => CanCheckout);

            if (SelectedCartItem.QuantityInCart == 0)
                Cart.Remove(SelectedCartItem);
        }
        public void Checkout() { }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            Products = new BindingList<ProductModel>(await _productEndpoint.GetAll());
        }

        private decimal CalculateSubtotal()
        {
            return Cart.Sum(x => x.Product.RetailPrice * x.QuantityInCart);
        }
        private decimal CalculateTax()
        {
            var taxRate = _configHelper.TaxRate;
            return Cart.Sum(x => x.Product.RetailPrice * x.QuantityInCart * (x.Product.IsTaxable ? taxRate : 0));
        }
    }
}
