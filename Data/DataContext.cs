using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PracticaViamatica.Model;

namespace PracticaViamatica.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public  DbSet<Producto> producto { get; set; }
        public DbSet<Persona> persona { get; set; }
        public DbSet<Credenciales> credenciales { get; set; }


        public int DoExecSP(string nombre_sp, object[] parametros)
        {
            return  this.Database.ExecuteSqlRaw(nombre_sp, parametros);
        }

        public int DoExecSP(string nombre_sp, object[] parametros, ref string mensaje)
        {
            int filasAfectadas = this.Database.ExecuteSqlRaw(nombre_sp, parametros.ToArray());
            SqlParameter nuevo = (SqlParameter)parametros[7];
            mensaje = nuevo.Value.ToString();
            return filasAfectadas;
        }


    }
}
