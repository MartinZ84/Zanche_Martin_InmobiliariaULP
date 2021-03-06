namespace Zanche_Martin_InmobiliariaULP
{
	public abstract class RepositorioBase
	{
        protected readonly IConfiguration configuration;
        protected readonly string connectionString;

        protected RepositorioBase(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }
    }
}