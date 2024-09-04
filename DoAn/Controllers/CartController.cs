using DoAn.Models;
using DoAn.Models.ViewModel;
using DoAn.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.Controllers
{
	public class CartController : Controller
	{
		private readonly DataContext _dataContext;
		public CartController(DataContext context)
		{
			_dataContext = context;
		}
		public IActionResult Index()
		{
			List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();


			CartItemViewModel cartVN = new()
			{
				CartItems = cartItems,
				GrandTotal = cartItems.Sum(x => x.Quantily * x.Price)

			};
			return View(cartVN);
		}
		public ActionResult Checkout()
		{
			return View("~/Views/Checkout/Index.cshtml");
		}
		public async Task<IActionResult> Add(int Id)
		{
			// Tìm sản phẩm theo Id trong cơ sở dữ liệu
			ProductModel product = await _dataContext.Products.FindAsync(Id);

			// Kiểm tra nếu product là null (sản phẩm không tồn tại)
			if (product == null)
			{
				// Trả về thông báo lỗi nếu sản phẩm không tồn tại
				return NotFound("Sản phẩm không tồn tại.");
			}

			// Lấy giỏ hàng từ session (hoặc tạo mới nếu giỏ hàng rỗng)
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

			// Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng hay chưa
			CartItemModel cartItems = cart.Where(c => c.ProductId == Id).FirstOrDefault();

			if (cartItems == null)
			{
				// Nếu sản phẩm chưa có trong giỏ hàng, thêm sản phẩm vào giỏ hàng
				cart.Add(new CartItemModel(product));
			}
			else
			{
				// Nếu sản phẩm đã tồn tại trong giỏ hàng, tăng số lượng lên 1
				cartItems.Quantily += 1;
			}

			// Lưu giỏ hàng vào session
			HttpContext.Session.SetJson("Cart", cart);
			TempData["success"] = "Add Item to cart successfuly";

			// Chuyển hướng trở lại trang trước đó
			return Redirect(Request.Headers["Referer"].ToString());
		}
		public async Task<IActionResult> Decrease(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();
			if (cartItem.Quantily > 1)
			{
				--cartItem.Quantily;
			}
			else
			{
				cart.RemoveAll(p => p.ProductId == Id);
			}
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}
            TempData["success"] = "Decrease Item quantity to cart successfuly";

            return RedirectToAction("Index");
		}
		public async Task<IActionResult> Increase(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();
			if (cartItem.Quantily >= 1)
			{
				++cartItem.Quantily;
			}
			else
			{
				cart.RemoveAll(p => p.ProductId == Id);
			}
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}
            TempData["success"] = "Increase Item quantity to cart successfuly";

            return RedirectToAction("Index");
		}
		public async Task<IActionResult> Remove(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");

			cart.RemoveAll(p=>p.ProductId == Id);
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");

			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);

			}
            TempData["success"] = "Remove Item  off cart successfuly";

            return RedirectToAction("Index");

		}
		public async Task<IActionResult> Clear()
		{
			HttpContext.Session.Remove("Cart");
            TempData["success"] = "Clear All Item off cart successfuly";

            return RedirectToAction("Index");

		}
	}

}
