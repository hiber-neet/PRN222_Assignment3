using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Services
{
    public class CartService
    {
        public List<CartItem> Items { get; private set; } = new List<CartItem>();
        public event Action OnChange;

        public void AddToCart(Product product, int quantity = 1)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductId == product.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                Items.Add(new CartItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
            
                    UnitPrice = product.UnitPrice,
                    Quantity = quantity
                });
            }

            OnChange?.Invoke();
        }

        public void RemoveFromCart(int productId)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                Items.Remove(item);
                OnChange?.Invoke();
            }
        }

        public void UpdateQuantity(int productId, int quantity)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                if (quantity <= 0)
                {
                    RemoveFromCart(productId);
                }
                else
                {
                    item.Quantity = quantity;
                    OnChange?.Invoke();
                }
            }
        }

        public void ClearCart()
        {
            Items.Clear();
            OnChange?.Invoke();
        }

        public decimal GetTotal()
        {
            return Items.Sum(item => item.UnitPrice * item.Quantity);
        }

        public int GetItemCount()
        {
            return Items.Sum(item => item.Quantity);
        }
    }
}
