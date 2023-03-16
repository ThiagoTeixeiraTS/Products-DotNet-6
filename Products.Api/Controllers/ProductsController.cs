﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Products.Api.Data;
using Products.Api.Models;

namespace Products.Api.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]

    public class ProductsController : ControllerBase
    {
        private readonly ProductsContext _context;
        public ProductsController(ProductsContext context)
        {
            _context= context;
        }
        [HttpGet]
        [Route("/products")]
        public async Task<ActionResult> GetProducts()
        {
            return Ok(await _context.Products.ToListAsync());
        }

        [HttpPost]
        [Route("/products")]
        public async Task<ActionResult> CreateProduct(Product product)
        {
             await _context.Products.AddAsync(product); 
             await _context.SaveChangesAsync();

            return Ok(product);
        }

        [HttpPut]
        [Route("/products")]
        public async Task<ActionResult> UpdateProduct(Product product) 
        {
            var dbProduct = await _context.Products.FindAsync(product.Id);
            if(dbProduct == null)
                return NotFound();

            dbProduct.Name= product.Name;
            dbProduct.Price= product.Price;
            dbProduct.Category = product.Category;

            await _context.SaveChangesAsync();
            
            return Ok(product);

        }
        [HttpDelete]
        [Route("/products")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            var dbProduct = await _context.Products.FindAsync(id);
            if (dbProduct == null)
                return NotFound();

            _context.Remove(dbProduct);

            await _context.SaveChangesAsync();

            return NoContent( );

        }

        [HttpGet]
        [Route("/productsById")]
        public async Task<ActionResult> GetProductById(Guid id)
        {
            var dbProduct = await _context.Products.FindAsync(id);
            if (dbProduct == null)
                return NotFound();

           

            return Ok(dbProduct);

        }
    }
}
