﻿using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineAppleShoppingStore.Web.Models
{
    public partial class ShoppingCart
    {
        OnlineAppleShoppingStoreEntities db = new OnlineAppleShoppingStoreEntities();

        
        private readonly ICartsRepository repository_carts;
        private readonly IProductsOrderedRepository repository_po;

        public ShoppingCart()
        {
        }

        public ShoppingCart( ICartsRepository objIrepository_carts, IProductsOrderedRepository objIrepository_po)
        {
            repository_po = objIrepository_po;
            repository_carts = objIrepository_carts;
        }
   

        public string ShoppingCartId { get; set; }

        public const string CartSessionKey = "cartId";

        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();

            cart.ShoppingCartId = cart.GetCartId(context);

            return cart;
        }

        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public void AddToCart(Product product)
        {
            var cartItem = db.Carts.SingleOrDefault(c => c.CartId == ShoppingCartId && c.ProductId == product.Id);

            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    ProductId = product.Id,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                db.Carts.Add(cartItem);
            }
            else
            {
                cartItem.Count++;
            }

            db.SaveChanges();
        }

        public int RemoveFromCart(int id)
        {
            var cartItem = db.Carts.SingleOrDefault(cart => cart.CartId == ShoppingCartId && cart.ProductId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    db.Carts.Remove(cartItem);
                }

                db.SaveChanges();
            }
            return itemCount;
        }

        public int UpdateQuantity(int id, int cartCount)
        {
            var cartItem = db.Carts.Single(
                cart => cart.CartId == ShoppingCartId
                && cart.ProductId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartCount > 0)
                {
                    cartItem.Count = cartCount;
                    itemCount = cartItem.Count;
                }
                else
                {
                    db.Carts.Remove(cartItem);
                }
                db.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = db.Carts.Where(cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                db.Carts.Remove(cartItem);
            }
            db.SaveChanges();
        }

        public List<Cart> GetCartItems()
        {
            return db.Carts.Where(cart => cart.CartId == ShoppingCartId).ToList();
        }

        public int GetCount()
        {
            int? count = (from cartItems in db.Carts where cartItems.CartId == ShoppingCartId select (int?)cartItems.Count).Sum();

            return count ?? 0;
        }

        public decimal GetTotal()
        {
            decimal? total = (from cartItems in db.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count * cartItems.Product.Price).Sum();

            return total ?? decimal.Zero;
        }

        public Order CreateOrder(Order order)
        {
            decimal orderTotal = 0;
            order.ProductsOrdereds = new List<ProductsOrdered>();

            var cartItems = GetCartItems();

            foreach (var item in cartItems)
            {
                var orderedProduct = new ProductsOrdered
                {
                    ProductId = item.ProductId,
                    OrderId = order.Id,
                    Quantity = item.Count
                };

                orderTotal += (item.Count * item.Product.Price);
                order.ProductsOrdereds.Add(orderedProduct);
                db.ProductsOrdereds.Add(orderedProduct);
            }

            order.Amount = orderTotal;

            db.SaveChanges();

            EmptyCart();

            return order;
        }

        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }

                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }

            return context.Session[CartSessionKey].ToString();
        }

        public void MigrateCart(string userName)
        {
            var shoppingCart = db.Carts.Where(c => c.CartId == ShoppingCartId);
            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
            }

            db.SaveChanges();
        }

    }
}