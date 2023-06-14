using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Restaurant.Service.APIEmail.Data;
using Restaurant.Service.APIEmail.Messages;
using Restaurant.Service.APIEmail.Models;
using Restaurant.Service.APIEmail.Models.Dto;
using Restaurant.Service.APIEmail.Services.IServices;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Service.APIEmail.Services
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<ApplicationDBContext> _dbOptions;

        public EmailService(DbContextOptions<ApplicationDBContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task EmailCartAndLog(CartDto cartDto)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("<br/>Cart Email Requested ");
            message.AppendLine("<br/>Total " + cartDto.CartHeader.CartTotal);
            message.Append("<br/>");
            message.Append("<ul>");
            foreach (var item in cartDto.CartDetails)
            {
                message.Append("<li>");
                message.Append(item.Product.Name + " x " + item.Count);
                message.Append("</li>");
            }
            message.Append("</ul>");

            await LogAndEmail(message.ToString(), cartDto.CartHeader.Email);
        }

        public async Task LogOrderPlaced(RewardsMessage rewardsDto)
        {
            string message = "Đơn hàng đã được thanh toán  ! <br/> ;" + rewardsDto.OrderId;
            await LogAndEmail(message, "hoangtuanboom42@gmail.com");
        }

        public async Task RegisterUserEmailAndLog(string email)
        {
            string message = "User Registeration Successful. <br/> Email : " + email;
            await LogAndEmail(message, "hoangtuanboom42@gmail.com");
        }

        private async Task<bool> LogAndEmail(string message, string email)
        {
            try
            {
                EmailLogger emailLog = new()
                {
                    Email = email,
                    EmailSent = DateTime.Now,
                    Message = message
                };
                await using var _db = new ApplicationDBContext(_dbOptions);
                await _db.EmailLoggers.AddAsync(emailLog);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
