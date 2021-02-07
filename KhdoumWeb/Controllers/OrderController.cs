﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KhdoumWeb.Data;
using KhdoumWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KhdoumWeb.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        public class OrderDetailsModelView
        {
            public Order Order = new Order();
            public List<OrderDetails> OrderDetails = new List<OrderDetails>();
        }
            

        public OrderController(ApplicationDbContext context)
        {
            this._context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id = 0)
        {
            if(id == 0)
            {
                return BadRequest();
            }

            var order = _context.Orders.Include(o=>o.City).FirstOrDefault(o=>o.Id == id);
            var orderDetails = _context.OrderDetails.Include(od=>od.Item).Include(od=>od.Member).Include(od=>od.Unit).Where(od => od.OrderId == id).ToList();
            var OrderDetailsModel = new OrderDetailsModelView()
            {
                Order = order,
                OrderDetails = orderDetails
            };
            if(order == null)
            {
                return NotFound();
            }

            return View(OrderDetailsModel);
        }


        public IActionResult OrdersList(int Filter = 2)
        {
            return PartialView(AllOrders(Filter));
        }


        public List<Order> AllOrders(int Filter = 0)
        {
            var orders = new List<Order>();
            if(Filter != 0)
            {
                if(Filter == 1)
                {
                    var date = DateTime.Now.ToShortDateString();
                    orders = _context.Orders.Where(o => o.Date == date).ToList();
                }
                else if(Filter == 2)
                {
                    orders = _context.Orders.ToList();
                }
            }
            return orders;
        }
    }
}
