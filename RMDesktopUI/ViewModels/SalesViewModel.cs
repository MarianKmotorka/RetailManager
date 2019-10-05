using AutoMapper;
using Caliburn.Micro;
using RM.WPF.Library.Api;
using RM.WPF.Library.Helpers;
using RM.WPF.Library.Models;
using RMDesktopUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private IProductEndpoint _productEndpoint;
        private ISaleEndpoint _saleEndpoint;
        private IConfigHelper _configHelper;
        private IMapper _mapper;
        private BindingList<ProductDisplayModel> _products;
        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();
        private ProductDisplayModel _selectedProduct;
        private CartItemDisplayModel _selectedCartItem;
        private int _productQuantity = 1;

        public SalesViewModel(IProductEndpoint productEndpoint,
            ISaleEndpoint saleEndpoint,
            IConfigHelper configHelper,
            IMapper mapper)
        {
            _productEndpoint = productEndpoint;
            _saleEndpoint = saleEndpoint;
            _configHelper = configHelper;
            _mapper = mapper;
        }

        public CartItemDisplayModel SelectedCartItem
        {
            get { return _selectedCartItem; }
            set
            {
                _selectedCartItem = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
            }
        }
        public ProductDisplayModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }
        public BindingList<ProductDisplayModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }
        public BindingList<CartItemDisplayModel> Cart
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
            }
            else
            {
                var cartItem = new CartItemDisplayModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ProductQuantity
                };

                Cart.Add(cartItem);
            }

            SelectedProduct.QuantityInStock -= ProductQuantity;
            ProductQuantity = 1;

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => CanCheckout);
        }
        public void RemoveFromCart()
        {
            SelectedCartItem.QuantityInCart -= ProductQuantity;

            var product = Products.Single(x => x == SelectedCartItem.Product);
            product.QuantityInStock += ProductQuantity;
            ProductQuantity = 1;

            if (SelectedCartItem.QuantityInCart == 0)
                Cart.Remove(SelectedCartItem);

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => CanCheckout);
        }
        public async Task Checkout() 
        {
            var sale = new SaleModel();

            foreach (var item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                });
            }

            await _saleEndpoint.Post(sale);
            await ResetSalesViewModel();

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => CanCheckout);
        }

        private async Task ResetSalesViewModel()
        {
            Cart = new BindingList<CartItemDisplayModel>();
            await LoadProducts();
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            var products = _mapper.Map<List<ProductDisplayModel>>(await _productEndpoint.GetAll());
            Products = new BindingList<ProductDisplayModel>(products);
        }

        private decimal CalculateSubtotal()
        {
            return Cart.Sum(x => x.Product.RetailPrice * x.QuantityInCart);
        }
        private decimal CalculateTax()
        {
            var taxRate = _configHelper.TaxRate;
            return Cart
                .Where(x => x.Product.IsTaxable)
                .Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);
        }
    }
}
