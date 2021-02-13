using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KhdoumWeb.Models;
using KhdoumWeb.Data;
using Microsoft.EntityFrameworkCore;
using KhdoumWeb.Models.ViewModels;
using AutoMapper;
using KhdoumWeb.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Globalization;

namespace KhdoumWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        //private bool IsLogin { get; set; }
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            this._context = context;
        }

        public IActionResult Index(int LoadItemsCount = 9)
        {
            return View(HomeModels(LoadItemsCount));
        }

        public IActionResult ShowMoreItems(int LoadItemsCount = 9)
        {
            return PartialView("ItemsList", ShowMoreItemsFun(LoadItemsCount));
        }

        public IActionResult Search(string q)
        {
            try
            {
                var items = (from i in _context.Items.Include(i => i.Member).Include(i => i.City)
                             from c in _context.Categories
                             from sc in _context.SubCategories
                             from ic in _context.ItemsCategories
                             from isc in _context.ItemsSupCategories
                             where ic.ItemId == i.Id && ic.CategoryId == c.Id && isc.ItemId == i.Id && isc.SubCategoryId == sc.Id
                             where i.Title.Contains(q) || i.Description.Contains(q) || i.Address.Contains(q) || c.Name.Contains(q) || sc.Name.Contains(q) || i.Member.FullName.Contains(q)
                             where i.State
                             select i).ToList();
                if (q == null || q == "")
                {
                    q = "غير معروف";
                }
                ViewBag.q = q;
                return View(items);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }

        }

        public IActionResult SubCategories(int id, int SubId = 0)
        {
            if (id == 0)
            {
                return NotFound();
            }
            return View(SubCategoryModels(id, SubId));
        }

        public IActionResult SubCategoriesItems(int SubId = 0)
        {
            if (SubId == 0)
            {
                return NotFound();
            }
            return PartialView("ItemsList", SubCategoriesItemsFun(SubId));
        }

        public IActionResult Item(string id, int FromAction = 0)
        {
            if (id == null)
            {
                return BadRequest();
            }
            try
            {
                var item = (from i in _context.Items.Include(i => i.Member)
                            from c in _context.Categories
                            from sc in _context.SubCategories
                            from ic in _context.ItemsCategories
                            from isc in _context.ItemsSupCategories
                            where ic.ItemId == i.Id && ic.CategoryId == c.Id && isc.ItemId == i.Id && isc.SubCategoryId == sc.Id && i.ShortLink == id
                            select new ItemDetailsViewModel
                            {
                                Id = i.Id
                                 ,
                                Title = i.Title
                                 ,
                                Date = i.Date
                                 ,
                                ImgUrl = i.ImgUrl
                                 ,
                                Description = i.Description
                                 ,
                                Address = i.Address
                                 ,
                                ShortLink = i.ShortLink
                                 ,
                                ViewsCount = i.ViewsCount
                                 ,
                                RatingValues = i.RatingValues
                                 ,
                                RatingCount = i.RatingCount
                                 ,
                                Price = i.Price
                                 ,
                                IsPaid = i.IsPaid
                                 ,
                                Phone = i.Phone
                                 ,
                                CategoryName = c.Name
                                 ,
                                SubCategoryName = sc.Name
                                 ,
                                CategoryId = c.Id
                                 ,
                                SubCategoryId = sc.Id
                                 ,
                                Member = i.Member
                                ,
                                LocationMap = i.LocationMap

                            }).FirstOrDefault();
                if (item == null)
                {
                    return NotFound();
                }

                if (TempData["IsWatchIt"] as string == "true")
                {
                    TempData.Keep("IsWatchIt");
                }


                if (FromAction == 0)
                {

                    if (TempData["IsWatchIt"] as string != "true")
                    {
                        var ItemForViewCount = _context.Items.FirstOrDefault(i => i.ShortLink == id);
                        if (ItemForViewCount.ViewsCount == null)
                        {
                            ItemForViewCount.ViewsCount = 0;
                        }

                        ItemForViewCount.ViewsCount++;
                        _context.Update(ItemForViewCount);
                        _context.SaveChanges();
                        TempData["IsWatchIt"] = "true";



                    }


                }


                return View(item);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }

        }

        public IActionResult Order(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            try
            {
                var Item = _context.Items.FirstOrDefault(i => i.Id == id);

                if (Item == null)
                {
                    return NotFound();
                }
                if (Item.ClicksCount == null)
                {
                    Item.ClicksCount = 0;
                }

                Item.ClicksCount++;
                _context.Update(Item);
                _context.SaveChanges();

                return RedirectToAction(nameof(Item), new { id = Item.ShortLink, FromAction = 1 });
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }

        }
        public IActionResult OrderNow(string UserPhone="",string Password = "")
        {
            if(UserPhone == "" || Password == "")
            {
                return RedirectToAction(nameof(ClientLogin));
            }

            if (IsClient(UserPhone, Password))
            {
                var Client = _context.Members.FirstOrDefault(c => c.Phone == UserPhone);
                var CartItemsTotal = _context.Carts.Where(c => c.MemberId == Client.Id).Select(c=>c.Value).Sum();
                var model = new OrderNowViewModel()
                {
                    Total = CartItemsTotal,
                    UserPhone = Client.Phone,
                    Cities = _context.Cities.ToList()
                };
                return View(model);

            }

            return RedirectToAction(nameof(ClientLogin));

        }
        [HttpPost]
        public IActionResult OrderConfirm(OrderNowViewModel model)
        {
            if(ModelState.IsValid)
            {
                if (model.LoginedUserPhone == "" || model.Password == "")
                {
                    return RedirectToAction(nameof(ClientLogin));
                }

                if (IsClient(model.LoginedUserPhone, model.Password))
                {
                    var Client = _context.Members.FirstOrDefault(c => c.Phone == model.LoginedUserPhone);
                    var CartItems = _context.Carts.Where(c => c.MemberId == Client.Id).ToList();
                    var city = _context.Cities.FirstOrDefault(c => c.Id == model.CityId);
                   if(CartItems.Count == 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                   if(city == null)
                    {
                        return View(model);
                    }
                    var TotalAmount = CartItems.Select(c => c.Value).Sum();
                    var TotalAmountWithDelivery = TotalAmount + city.DeliveryService;
                    var date = DateTime.UtcNow.ToString(new CultureInfo("ar-EG"));
                    
                    var order = new Order()
                    {
                        Name = model.UserName,
                        Date = date,
                        Address = model.Address,
                        Total = TotalAmountWithDelivery,
                        Notes = model.Notes,
                        Phone = model.UserPhone,
                        LoginedPhone = model.LoginedUserPhone,
                        State = "waiting",
                        DeliveryService = city.DeliveryService,
                        CityId = city.Id,
                        DeliveryDate = model.DeliveryDate
                    };

                    _context.Orders.Add(order);
                    _context.SaveChanges();

                    var ThisOrder = _context.Orders.ToList().LastOrDefault(o => o.LoginedPhone == model.LoginedUserPhone); 
                    if(ThisOrder != null)
                    {
                        var OrderDetails = from c in CartItems
                                           select new OrderDetails()
                                           {
                                               Value = c.Value,
                                               Quantity = c.Quantity,
                                               Price = c.Price,
                                               UnitId = c.UnitId,
                                               ItemId = c.ItemId,
                                               MemberId = c.MemberId,
                                               OrderId = ThisOrder.Id
                                           };
                        _context.OrderDetails.AddRange(OrderDetails);
                        _context.SaveChanges();
                        _context.Carts.RemoveRange(CartItems);
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }

                }

            }

            return View(model);
        }
        public IActionResult Login(string Url)
        {
            return View(new LoginViewModel { Url = Url });
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _context.Members.FirstOrDefault(m => m.Phone == login.Phone || m.Email == login.Phone && m.Password == login.Password);
                    if (user == null)
                    {
                        return NotFound();
                    }

                    if (user.State == false)
                    {
                        return NotFound();
                    }

                    TempData["IsLogin"] = "true";

                    if (!string.IsNullOrEmpty(login.Url))
                    {
                        return Redirect(login.Url);
                    }
                    else
                    {
                        return RedirectToAction(nameof(Dashboard), new { id = user.Id });
                    }
                }
                return View(login);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }

        }

        public IActionResult Dashboard(int id = 0)
        {
            try
            {
                if (TempData["IsLogin"] as string != "true")
                {
                    return RedirectToAction(nameof(Login), new { Url = Request.Path });
                }

                TempData.Keep("IsLogin");

                if (id == 0)
                {
                    return NotFound();
                }

                var member = _context.Members.Include(m => m.Items).FirstOrDefault(m => m.Id == id);
                return View(member);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }

        }
        public IActionResult ItemStatistics(int Id = 0)
        {
            try
            {
                if (TempData["IsLogin"] as string != "true")
                {
                    return RedirectToAction(nameof(Login), new { Url = Request.Path });
                }

                TempData.Keep("IsLogin");

                if (Id == 0)
                {
                    return BadRequest();
                }

                var Item = _context.Items.FirstOrDefault(i => i.Id == Id);
                if (Item == null)
                {
                    return NotFound();
                }

                return View(Item);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }

        }

        public IActionResult Favorites(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest();
                }

                string[] items = id.Split(",");


                var itemsList = new List<Item>();
                for (var i = 0; i < items.Length; i++)
                {
                    int iid = int.Parse(items[i]);
                    var item = _context.Items.Include(i => i.City).Where(i => i.State).FirstOrDefault(i => i.Id == iid);
                    if (item != null)
                    {
                        var Itm = itemsList.FirstOrDefault(im => im.Id == item.Id);
                        if (Itm == null)
                        {
                            itemsList.Add(item);
                        }
                    }
                }

                return View(itemsList);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }

        }

        public IActionResult OrderList(string UserPhone="",string Password = "")
        {
            if (UserPhone == "" || Password == "")
            {
                return RedirectToAction(nameof(ClientLogin));
            }

            if (IsClient(UserPhone, Password))
            {
                var Client = _context.Members.FirstOrDefault(c=>c.Phone == UserPhone);
                var CartItems = _context.Carts.Include(c=>c.Member).Include(c=>c.Unit).Include(c=>c.Item).Where(i => i.MemberId == Client.Id).ToList();
                return View(CartItems);
            }
            
            return RedirectToAction(nameof(ClientLogin));

        }

        public IActionResult RemoveItemFromCart(string UserPhone = "", string Password = "",int ItemId=0)
        {
            if (UserPhone == "" || Password == "")
            {
                return Json(new { state = -1 ,num=-1});
            }

            

            if (IsClient(UserPhone, Password))
            {
                if (ItemId == 0)
                {
                    return Json(new { state = -1 ,num=-1});
                }

                var Client = _context.Members.FirstOrDefault(c => c.Phone == UserPhone);

                var CartItem = _context.Carts.FirstOrDefault(i=>i.ItemId == ItemId && i.MemberId == Client.Id);
                if(CartItem != null)
                {
                    _context.Carts.Remove(CartItem);
                    _context.SaveChanges();
                    var CartItemsCount = _context.Carts.Where(c => c.MemberId == Client.Id).ToList().Count;
                    return Json(new { state = 1 ,num = CartItemsCount });
                }
            }

            return Json(new { state = -1,num=-1 });

        }

        public IActionResult ClientRegister()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", 0);
            return View();
        }

        [HttpPost]
        public IActionResult ClientRegister(ClientRegisterViewModel client)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Client = (from c in _context.Members
                                 from r in _context.Roless
                                 from cr in _context.MemberRoles
                                 where cr.RoleId == r.Id && cr.MemberId == c.Id && r.Name == "Customer" && c.Phone == client.UserPhone
                                 select c).FirstOrDefault();

                    if (Client != null)
                    {
                        ModelState.AddModelError("UserPhone", "تم تسجيل هذا الحساب مسبقا");
                        return View(client);
                    }

                    var Member = new Member();
                    Member.FullName = client.UserName;
                    Member.Phone = client.UserPhone;
                    Member.Password = RandomURL.GetURL();
                    Member.CityId = client.CityId;
                    Member.State = true;
                   

                    _context.Add(Member);
                    var created = _context.SaveChanges();
                    var Customer = new Member(); 
                    if (created > 0)
                    {
                        //add Customer to role
                         Customer = _context.Members.ToList().FirstOrDefault(c=>c.Phone == client.UserPhone);
                        var role = _context.Roless.FirstOrDefault(r => r.Name == "Customer");
                        if (role != null && Customer != null)
                        {
                            MemberRole CustomerRole = new MemberRole();
                            CustomerRole.MemberId = Customer.Id;
                            CustomerRole.RoleId = role.Id;
                            _context.MemberRoles.Add(CustomerRole);
                            _context.SaveChanges();
                        }
                    }


                    if (IsClient(Customer.Phone, Customer.Password))
                    {
                        var data = new ClientLoginViewModel { UserPhone = Customer.Phone, Password = Customer.Password, UserName = Customer.FullName };
                        TempData["LoginData"] =JsonConvert.SerializeObject(data);
                        return RedirectToAction(nameof(Index));
                    }
                    //else
                    //{
                    //    ModelState.AddModelError("UserPhone", "رقم الهاتف أو كلمة المرور غير صحيحة");
                    //    return View();
                    //}

                    return RedirectToAction(nameof(Index));
                }

                ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", client.CityId);
                return View(client);
            }
            catch(Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult ClientLogin()
        {
            return View();
        }

        [HttpPost]
         public IActionResult ClientLogin(ClientLoginViewModel client)
        {
            if(ModelState.IsValid)
            {
                if (IsClient(client.UserPhone, client.Password))
                {
                    var Customer = _context.Members.FirstOrDefault(c=>c.Phone == client.UserPhone);
                    var data = new ClientLoginViewModel { UserPhone = Customer.Phone, Password = Customer.Password, UserName = Customer.FullName };
                    TempData["LoginData"] = JsonConvert.SerializeObject(data);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("UserPhone", "رقم الهاتف أو كلمة المرور غير صحيحة");
                    return View();
                }
            }
            return View();
           
        }

        public bool IsClient(string UserPhone,string Password)
        {
            var Client = (from c in _context.Members
                          from r in _context.Roless
                          from cr in _context.MemberRoles
                          where cr.RoleId == r.Id
                          && cr.MemberId == c.Id
                          && r.Name == "Customer"
                          && c.Phone == UserPhone
                          && c.Password == Password
                          && c.State
                          select c).FirstOrDefault();

            if (Client == null)
            {
                return false;
            }
            else
            {
                if(Client.Phone == UserPhone && Client.Password == Password)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public IActionResult AddToCart(CartViewModel model)
        {
            if(model.UserPhone == "" || model.Password == "")
            {
                return Json(new { num = -1 });
            }

            if(IsClient(model.UserPhone,model.Password))
            {
                var Client = _context.Members.FirstOrDefault(c=>c.Phone == model.UserPhone);
                if(model.ItemId == 0)
                {
                    return Json(new { num = -1 });
                }
                else
                {
                    var item = _context.Items.Include(i=>i.Unit).FirstOrDefault(i=>i.Id == model.ItemId);
                    if(model.Quantity > 0)
                    {
                        var CartItem = _context.Carts.FirstOrDefault(c=>c.ItemId == model.ItemId && c.MemberId == Client.Id);
                       
                        decimal value = model.Quantity * Convert.ToDecimal(item.Price);

                        if (CartItem != null)
                        {
                            CartItem.Quantity = model.Quantity;
                            CartItem.Value = value;
                            CartItem.Price = Convert.ToDecimal(item.Price);
                            _context.Entry(CartItem).State = EntityState.Modified;
                        }
                        else
                        {
                            CartItem = new Cart
                            {
                                Quantity = model.Quantity,
                                Value = value,
                                ItemId = model.ItemId,
                                MemberId = Client.Id,
                                UnitId = item.Unit.Id,
                                Price = Convert.ToDecimal(item.Price)
                            };
                            _context.Carts.Add(CartItem);
                        }
                        _context.SaveChanges();
                        var num = _context.Carts.Where(c=>c.MemberId == Client.Id).Count();
                        return Json(new { num = num });

                    }
                    else
                    {
                        return Json(new { num = -1 });
                    }
                }
            }

            return Json(new { num = -1 }); 

        }

        public IActionResult GetCartItemsCount(string UserPhone = "",string Password = "")
        {
            if (UserPhone == "" || Password == "")
            {
                return Json(new { num = -1 }); ;
            }

            if (IsClient(UserPhone, Password))
            {
                var Client = _context.Members.FirstOrDefault(c => c.Phone == UserPhone);
                var num = _context.Carts.Where(c => c.MemberId == Client.Id).Count();
                return Json(new { num = num });
            }
            return Json(new { num = -1 });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public HomeModelView HomeModels(int LoadItemsCount)
        {
            try
            {
                HomeModelView homeModel = new HomeModelView();

                var Categories = _context.Categories.Include(c => c.ItemsCategories).Where(c => c.State).ToList();
                homeModel.Categories = Categories;
                homeModel.Items = ShowMoreItemsFun(LoadItemsCount);

                return homeModel;
            }
            catch
            {
                return new HomeModelView();
            }



        }

        public List<Item> ShowMoreItemsFun(int LoadItemsCount)
        {

            try
            {
                var Items = (from i in _context.Items.Include(i => i.City)
                             from c in _context.Categories
                             from sc in _context.SubCategories
                             from ic in _context.ItemsCategories
                             from isc in _context.ItemsSupCategories
                             where ic.ItemId == i.Id && ic.CategoryId == c.Id && isc.ItemId == i.Id && isc.SubCategoryId == sc.Id
                             where i.State

                             select i).ToList().TakeLast(LoadItemsCount).ToList().OrderByDescending(i => i.Id).ToList();

                return Items;
            }
            catch(Exception ex)
            {
                return new List<Item>();
            }



        }

        public HomeModelView SubCategoryModels(int id, int SubId)
        {
            try
            {
                HomeModelView SubCategoryModels = new HomeModelView();

                var SubCategories = (from i in _context.Items.Include(i => i.City)
                                     from c in _context.Categories
                                     from sc in _context.SubCategories
                                     from ic in _context.ItemsCategories
                                     from isc in _context.ItemsSupCategories
                                     where ic.ItemId == i.Id && ic.CategoryId == c.Id && isc.ItemId == i.Id && isc.SubCategoryId == sc.Id && c.Id == id
                                     select sc).ToList();





                var Items = new List<Item>();

                if (SubId != 0)
                {
                    SubCategoriesItemsFun(SubId);
                }
                else
                {
                    Items = (from i in _context.Items.Include(i => i.City)
                             from c in _context.Categories
                             from ic in _context.ItemsCategories
                             where ic.ItemId == i.Id && ic.CategoryId == c.Id && c.Id == id
                             where i.State
                             select i).ToList();
                }


                SubCategoryModels.SubCategories = SubCategories;
                SubCategoryModels.Items = Items;
                SubCategoryModels.CategoryId = id;
                return SubCategoryModels;
            }
            catch
            {
                return new HomeModelView();
            }

        }

        public List<Item> SubCategoriesItemsFun(int SubId)
        {
            try
            {
                var Items = (from i in _context.Items.Include(i => i.City)
                             from c in _context.SubCategories
                             from isc in _context.ItemsSupCategories
                             where isc.ItemId == i.Id && isc.SubCategoryId == c.Id && c.Id == SubId
                             where i.State
                             select i).ToList();
                return Items;
            }
            catch
            {
                return new List<Item>();
            }

        }

        public IActionResult About()
        {
            return View();
        }

    }
}
