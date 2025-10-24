using System;
using System.Collections.Generic;
using System.Linq;
using FUMiniHotelSystem.Data.Database;
using FUMiniHotelSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace FUMiniHotelSystem.Data.Repositories
{
    public class CustomerRepository
    {
        public IEnumerable<Customer> GetAll()
        {
            using var context = DbContextFactory.CreateDbContext();
            return context.Customers
                .Where(c => c.CustomerStatus == 1)
                .AsNoTracking()
                .ToList();
        }

        public Customer? GetById(int id)
        {
            using var context = DbContextFactory.CreateDbContext();
            return context.Customers
                .AsNoTracking()
                .FirstOrDefault(c => c.CustomerID == id);
        }

        public void Add(Customer customer)
        {
            using var context = DbContextFactory.CreateDbContext();
            context.Customers.Add(customer);
            context.SaveChanges();
        }

        public void Update(Customer customer)
        {
            using var context = DbContextFactory.CreateDbContext();
            var existing = context.Customers.FirstOrDefault(c => c.CustomerID == customer.CustomerID);
            if (existing is null)
                throw new Exception("Customer not found");

            existing.CustomerFullName = customer.CustomerFullName;
            existing.EmailAddress = customer.EmailAddress;
            existing.Telephone = customer.Telephone;
            existing.CustomerBirthday = customer.CustomerBirthday;
            existing.Password = customer.Password;
            existing.CustomerStatus = customer.CustomerStatus;

            context.SaveChanges();
        }

        public void Delete(int id)
        {
            using var context = DbContextFactory.CreateDbContext();
            var existing = context.Customers.FirstOrDefault(c => c.CustomerID == id);
            if (existing != null)
            {
                existing.CustomerStatus = 2; // soft delete
                context.SaveChanges();
            }
        }
    }
}
