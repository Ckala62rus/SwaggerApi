using Architecture.Core.Services.Files;
using Architecture.Core.Services.Members;
using Architecture.Core.Services.Telegram;
using Architecture.Core.Services.Users;
using Architecture.DAL.Repository.File;
using Architecture.DAL.Repository.Members;
using Architecture.DAL.Repository.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Architecture
{
    public class DependencyInjection
    {
        public DependencyInjection(IServiceCollection services)
        {
            services.AddScoped<IMembersService, MembersService>();
            services.AddScoped<IMembersRepository, MembersRepository>();

            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IFileRepository, FileRepository>();

            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IUsersRepository, UsersRepository>();

            services.AddScoped<ITelegramService, TelegramService>();
        }
    }
}
