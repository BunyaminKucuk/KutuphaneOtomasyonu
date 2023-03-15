using Autofac;
using DataAccess.Abstract;
using DataAccess.Repositories.EntityFramework;

namespace DataAccess.Extensions
{
    public class AutofacResolver : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EfBookRepository>().As<IBookRepository>();
            builder.RegisterType<EfUserRepository>().As<IUserRepository>();
            builder.RegisterType<EfUnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<EfTakeOfBookRepository>().As<ITakeOfBook>();
        }
    }
}
