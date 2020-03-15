using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Persistence.Data;
using Persistence.Entities;

namespace Services
{
    public interface IPayForDrinkService
    {
        Payment Pay(Drink drink, IEnumerable<MachineCoin> coins, Machine machine, IdentityUser user);
        Task<Payment> PayAsync(Drink drink, IEnumerable<MachineCoin> coins, Machine machine, IdentityUser user, CancellationToken cancellationToken = default);
    }

    public class PayForDrinkService : IPayForDrinkService
    {
        private readonly ApplicationDbContext _db;

        public PayForDrinkService(ApplicationDbContext db)
        {
            _db = db;
        }

        public Payment Pay(Drink drink, IEnumerable<MachineCoin> coins, Machine machine, IdentityUser user)
        {
            var paymentModel = new Payment
            {
                Machine = machine,
                PaymentDateUtc = DateTime.Now,
                MachineCoins = coins.ToList()
            };

            _db.Payments.Add(paymentModel);
            _db.SaveChanges();
            return paymentModel;
        }

        public async Task<Payment> PayAsync(Drink drink, IEnumerable<MachineCoin> coins, Machine machine, IdentityUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var paymentModel = new Payment
            {
                Machine = machine,
                PaymentDateUtc = DateTime.Now,
                MachineCoins = coins.ToList()
            };

            await _db.Payments.AddAsync(paymentModel, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return paymentModel;
        }
    }
}
